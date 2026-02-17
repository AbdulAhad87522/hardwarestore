using HardWareStore.Interfaces;
using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HardWareStore.DL
{
    public class ReturnsDL : IReturnsDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        // ─────────────────────────────────────────────────────────────────────
        // ProcessReturn — single atomic transaction using DatabaseHelper pattern
        // ─────────────────────────────────────────────────────────────────────

        public bool ProcessReturn(ReturnRequest request)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // ── 1. Get "Approved" return_status lookup_id ─────────────
                        int approvedStatusId = _db.GetLookupId("return_status", "Approved");
                        if (approvedStatusId == -1)
                            throw new Exception("Could not find 'Approved' return_status in lookup table.");

                        // ── 2. Get customer_id from the bill ──────────────────────
                        int customerId;
                        string getCustomerSql = "SELECT customer_id FROM bills WHERE bill_id = @billId LIMIT 1;";
                        using (var cmd = new MySqlCommand(getCustomerSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@billId", request.BillId);
                            object result = cmd.ExecuteScalar();
                            if (result == null || result == DBNull.Value)
                                throw new Exception($"Bill ID {request.BillId} not found.");
                            customerId = Convert.ToInt32(result);
                        }

                        // ── 3. Insert return header ───────────────────────────────
                        string insertReturnSql = @"
                            INSERT INTO returns
                                (bill_id, customer_id, refund_amount, status_id, reason, notes, return_date)
                            VALUES
                                (@billId, @customerId, @refundAmount, @statusId, @reason, @notes, NOW());";

                        var returnParams = new MySqlParameter[]
                        {
                            new MySqlParameter("@billId",       request.BillId),
                            new MySqlParameter("@customerId",   customerId),
                            new MySqlParameter("@refundAmount", request.RefundAmount),
                            new MySqlParameter("@statusId",     approvedStatusId),
                            new MySqlParameter("@reason",       request.Reason ?? ""),
                            new MySqlParameter("@notes",        (object)request.Notes ?? DBNull.Value)
                        };

                        _db.ExecuteNonQueryTransaction(insertReturnSql, returnParams, tx);
                        int returnId = _db.GetLastInsertId();

                        if (returnId == 0)
                            throw new Exception("Failed to insert return header — no ID returned.");

                        // ── 4. Insert return_items + optionally restore stock ──────
                        foreach (var item in request.Items)
                        {
                            string conditionNote = request.RestoreStock
                                ? "Resalable"
                                : "Damaged - not restocked";

                            string insertItemSql = @"
                                INSERT INTO return_items (return_id, variant_id, quantity, condition_note)
                                VALUES (@returnId, @variantId, @quantity, @conditionNote);";

                            var itemParams = new MySqlParameter[]
                            {
                                new MySqlParameter("@returnId",      returnId),
                                new MySqlParameter("@variantId",     item.VariantId),
                                new MySqlParameter("@quantity",      item.Quantity),
                                new MySqlParameter("@conditionNote", conditionNote)
                            };

                            _db.ExecuteNonQueryTransaction(insertItemSql, itemParams, tx);

                            // Restore stock only if user opted in
                            if (request.RestoreStock)
                            {
                                string restockSql = @"
                                    UPDATE product_variants
                                    SET quantity_in_stock = quantity_in_stock + @qty,
                                        updated_at        = NOW()
                                    WHERE variant_id = @variantId;";

                                var restockParams = new MySqlParameter[]
                                {
                                    new MySqlParameter("@qty",       item.Quantity),
                                    new MySqlParameter("@variantId", item.VariantId)
                                };

                                _db.ExecuteNonQueryTransaction(restockSql, restockParams, tx);
                            }
                        }

                        // ── 5. Reduce customer outstanding balance (credit customers) ──
                        string updateBalanceSql = @"
                            UPDATE customers
                            SET current_balance = GREATEST(0, current_balance - @refundAmount),
                                updated_at      = NOW()
                            WHERE customer_id   = @customerId
                              AND current_balance > 0;";

                        var balanceParams = new MySqlParameter[]
                        {
                            new MySqlParameter("@refundAmount", request.RefundAmount),
                            new MySqlParameter("@customerId",   customerId)
                        };

                        _db.ExecuteNonQueryTransaction(updateBalanceSql, balanceParams, tx);

                        // ── 6. Mark bill as Refunded ──────────────────────────────
                        int refundedStatusId = _db.GetLookupId("payment_status", "Refunded");
                        if (refundedStatusId != -1)
                        {
                            string updateBillSql = @"
                                UPDATE bills
                                SET payment_status_id = @statusId,
                                    updated_at        = NOW()
                                WHERE bill_id = @billId;";

                            var billParams = new MySqlParameter[]
                            {
                                new MySqlParameter("@statusId", refundedStatusId),
                                new MySqlParameter("@billId",   request.BillId)
                            };

                            _db.ExecuteNonQueryTransaction(updateBillSql, billParams, tx);
                        }

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // BillsDL — returns-specific bill lookup queries
    // ─────────────────────────────────────────────────────────────────────────

    public class BillsDL : IBillsDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        public BillSummary GetBillByNumber(string billNumber)
        {
            string sql = @"
                SELECT
                    b.bill_id,
                    b.bill_number,
                    COALESCE(c.full_name, 'Walk-in Customer') AS customer_name,
                    b.bill_date,
                    b.total_amount
                FROM bills b
                LEFT JOIN customers c ON c.customer_id = b.customer_id
                WHERE b.bill_number = @billNumber
                LIMIT 1;";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@billNumber", billNumber)
            };

            var dt = _db.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new BillSummary
            {
                bill_id = Convert.ToInt32(row["bill_id"]),
                bill_number = row["bill_number"].ToString(),
                customer_name = row["customer_name"].ToString(),
                bill_date = Convert.ToDateTime(row["bill_date"]),
                total_amount = Convert.ToDecimal(row["total_amount"])
            };
        }

        public List<BillItemRow> GetBillItems(int billId)
        {
            string sql = @"
                SELECT
                    bi.bill_item_id,
                    bi.bill_id,
                    bi.product_id,
                    bi.variant_id,
                    p.name              AS product_name,
                    pv.size,
                    bi.unit_of_measure,
                    bi.quantity,
                    bi.unit_price,
                    bi.line_total,
                    bi.notes
                FROM bill_items bi
                JOIN products p           ON p.product_id    = bi.product_id
                JOIN product_variants pv  ON pv.variant_id   = bi.variant_id
                WHERE bi.bill_id = @billId
                ORDER BY bi.bill_item_id;";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@billId", billId)
            };

            var dt = _db.ExecuteDataTable(sql, parameters);
            var items = new List<BillItemRow>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                items.Add(new BillItemRow
                {
                    bill_item_id = Convert.ToInt32(row["bill_item_id"]),
                    bill_id = Convert.ToInt32(row["bill_id"]),
                    product_id = Convert.ToInt32(row["product_id"]),
                    variant_id = Convert.ToInt32(row["variant_id"]),
                    product_name = row["product_name"].ToString(),
                    size = row["size"].ToString(),
                    unit_of_measure = row["unit_of_measure"].ToString(),
                    quantity = Convert.ToDecimal(row["quantity"]),
                    unit_price = Convert.ToDecimal(row["unit_price"]),
                    line_total = Convert.ToDecimal(row["line_total"]),
                    notes = row["notes"] == DBNull.Value ? null : row["notes"].ToString()
                });
            }

            return items;
        }
    }
}