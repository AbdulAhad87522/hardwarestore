using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Xml.Linq;
using QuestPDF.Infrastructure;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HardWareStore.Models;
using System.ComponentModel;

namespace HardWareStore.DL
{

    internal class Customersaledl
    {
        // Updated GetProductThings - Shows aggregated product info (no batch_item_id)
        public DataTable GetProductThings(string text)
        {
            DataTable dt = new DataTable();
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = @"SELECT 
                                    p.product_id,
                                    p.name AS product_name,
                                    pv.size,
                                    p.description,
                                    pv.unit_of_measure,
                                    pv.quantity_in_stock,
                                    pv.price_per_unit as sale_price,
                                    s.name AS supplier_name,
                                    l.value AS category_type,
                                    pv.variant_id
                                FROM products p
                                INNER JOIN product_variants pv ON p.product_id = pv.product_id
                                LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                                LEFT JOIN lookup l ON p.category_id = l.lookup_id
                                WHERE 
                                    p.is_active = TRUE 
                                    AND pv.is_active = TRUE
                                    AND l.type = 'category'
                                    AND (
                                        p.name LIKE @text
                                        -- OR pv.size LIKE '1'
                                --         OR p.description LIKE 'a'
                                --         OR s.name LIKE 'a'
                                --         OR l.value LIKE 'a'
                                    )
                                ORDER BY p.name, pv.size;";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int getcustomerid(string text)
        {
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "select customer_id from customers where full_name = @text";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@text", text);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to recieve customer_id " + ex);
            }
        }

        public DataTable getallcustomer(string text)
        {
            DataTable dt = new DataTable();
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = "SELECT  full_name, address, phone FROM customers WHERE full_name LIKE @text";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@text", "%" + text + "%");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // Updated SaveDataToDatabase - Uses product_id and implements FIFO deduction
        //public bool SaveDataToDatabase(int? customerId, DateTime? date, decimal? total_amount, decimal? paid_amount, DataGridView d, int staffId = 1)
        //{
        //    using (var con = DatabaseHelper.Instance.GetConnection())
        //    {
        //        con.Open();
        //        using (var tran = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Validate input data first
        //                if (d.Rows.Count == 0 || (d.Rows.Count == 1 && d.Rows[0].IsNewRow))
        //                {
        //                    throw new Exception("No products selected for sale");
        //                }

        //                // Generate bill number
        //                string billNumber = $"INV-{DateTime.Now:yyyy}-{DateTime.Now:MMddHHmmss}";

        //                // Get payment status lookup_id (assuming 'paid' or 'partial' status exists)
        //                int paymentStatusId;
        //                string statusQuery = "SELECT lookup_id FROM lookup WHERE type = 'payment_status' AND value = @status";
        //                using (MySqlCommand statusCmd = new MySqlCommand(statusQuery, con, tran))
        //                {
        //                    string status = (paid_amount >= total_amount) ? "paid" : "partial";
        //                    statusCmd.Parameters.AddWithValue("@status", status);
        //                    object statusResult = statusCmd.ExecuteScalar();
        //                    if (statusResult == null)
        //                    {
        //                        throw new Exception("Payment status not found in lookup table");
        //                    }
        //                    paymentStatusId = Convert.ToInt32(statusResult);
        //                }

        //                // Insert into bills table
        //                decimal amountDue = (total_amount ?? 0) - (paid_amount ?? 0);
        //                string billQuery = @"INSERT INTO bills 
        //            (bill_number, bill_date, customer_id, staff_id, subtotal, discount_amount, 
        //             total_amount, amount_paid, amount_due, payment_status_id) 
        //            VALUES 
        //            (@bill_number, @bill_date, @customer_id, @staff_id, @subtotal, @discount_amount,
        //             @total_amount, @amount_paid, @amount_due, @payment_status_id);
        //            SELECT LAST_INSERT_ID();";

        //                int billId;
        //                using (MySqlCommand cmd = new MySqlCommand(billQuery, con, tran))
        //                {
        //                    cmd.Parameters.AddWithValue("@bill_number", billNumber);
        //                    cmd.Parameters.AddWithValue("@bill_date", date ?? DateTime.Now);
        //                    cmd.Parameters.AddWithValue("@customer_id", customerId ?? (object)DBNull.Value);
        //                    cmd.Parameters.AddWithValue("@staff_id", staffId);
        //                    cmd.Parameters.AddWithValue("@subtotal", total_amount ?? 0);
        //                    cmd.Parameters.AddWithValue("@discount_amount", 0); // Adjust if you have total discount
        //                    cmd.Parameters.AddWithValue("@total_amount", total_amount ?? 0);
        //                    cmd.Parameters.AddWithValue("@amount_paid", paid_amount ?? 0);
        //                    cmd.Parameters.AddWithValue("@amount_due", amountDue);
        //                    cmd.Parameters.AddWithValue("@payment_status_id", paymentStatusId);
        //                    //cmd.Parameters.AddWithValue("@payment_method", "cash"); // Adjust as needed

        //                    object result = cmd.ExecuteScalar();
        //                    if (result == null)
        //                    {
        //                        throw new Exception("Failed to get bill ID");
        //                    }
        //                    billId = Convert.ToInt32(result);
        //                }

        //                // Process each product in the DataGridView
        //                foreach (DataGridViewRow row in d.Rows)
        //                {
        //                    if (row.IsNewRow) continue;

        //                    string productName = row.Cells["product_name"]?.Value?.ToString()?.Trim();
        //                    string size = row.Cells["size"]?.Value?.ToString()?.Trim();

        //                    if (string.IsNullOrEmpty(productName))
        //                    {
        //                        throw new Exception("Product name is missing");
        //                    }

        //                    // Parse and validate quantity
        //                    if (!decimal.TryParse(row.Cells["quantity"]?.Value?.ToString(), out decimal quantity) || quantity <= 0)
        //                    {
        //                        throw new Exception($"Invalid quantity for product: {productName}");
        //                    }

        //                    // Get product and variant details
        //                    int productId;
        //                    int variantId;
        //                    decimal salePrice;
        //                    decimal currentStock;
        //                    string unitOfMeasure;

        //                    string productQuery = @"SELECT 
        //                                    p.product_id, 
        //                                    pv.variant_id,
        //                                    pv.price_per_unit,
        //                                    pv.quantity_in_stock,
        //                                    pv.unit_of_measure
        //                                FROM products p
        //                                INNER JOIN product_variants pv ON p.product_id = pv.product_id
        //                                WHERE p.name = @ProductName 
        //                                AND pv.size = @size
        //                                AND p.is_active = TRUE 
        //                                AND pv.is_active = TRUE";

        //                    using (MySqlCommand productCmd = new MySqlCommand(productQuery, con, tran))
        //                    {
        //                        productCmd.Parameters.AddWithValue("@ProductName", productName);
        //                        productCmd.Parameters.AddWithValue("@size", string.IsNullOrEmpty(size) ? "" : size);

        //                        using (var reader = productCmd.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                productId = reader.GetInt32("product_id");
        //                                variantId = reader.GetInt32("variant_id");
        //                                salePrice = reader.GetDecimal("price_per_unit");
        //                                currentStock = reader.GetDecimal("quantity_in_stock");
        //                                unitOfMeasure = reader.GetString("unit_of_measure");
        //                                reader.Close();

        //                                // Check if sufficient stock is available
        //                                if (currentStock < quantity)
        //                                {
        //                                    throw new Exception($"Insufficient stock for {productName} ({size}). Available: {currentStock}, Requested: {quantity}");
        //                                }

        //                                // Calculate line total (price may be overridden in grid)
        //                                decimal unitPrice = salePrice;
        //                                if (row.Cells["sale_price"] != null &&
        //                                    decimal.TryParse(row.Cells["sale_price"]?.Value?.ToString(), out decimal gridPrice) &&
        //                                    gridPrice > 0)
        //                                {
        //                                    unitPrice = gridPrice;
        //                                }

        //                                decimal lineTotal = unitPrice * quantity;

        //                                // Insert bill item
        //                                string billItemQuery = @"INSERT INTO bill_items 
        //                            (bill_id, product_id, variant_id, quantity, unit_of_measure, unit_price, line_total, notes) 
        //                            VALUES 
        //                            (@bill_id, @product_id, @variant_id, @quantity, @unit_of_measure, @unit_price, @line_total, @notes)";

        //                                using (MySqlCommand billItemCmd = new MySqlCommand(billItemQuery, con, tran))
        //                                {
        //                                    billItemCmd.Parameters.AddWithValue("@bill_id", billId);
        //                                    billItemCmd.Parameters.AddWithValue("@product_id", productId);
        //                                    billItemCmd.Parameters.AddWithValue("@variant_id", variantId);
        //                                    billItemCmd.Parameters.AddWithValue("@quantity", quantity);
        //                                    billItemCmd.Parameters.AddWithValue("@unit_of_measure", unitOfMeasure);
        //                                    billItemCmd.Parameters.AddWithValue("@unit_price", unitPrice);
        //                                    billItemCmd.Parameters.AddWithValue("@line_total", lineTotal);
        //                                    billItemCmd.Parameters.AddWithValue("@notes", DBNull.Value);

        //                                    int rowsInserted = billItemCmd.ExecuteNonQuery();
        //                                    if (rowsInserted == 0)
        //                                    {
        //                                        throw new Exception($"Failed to insert bill item for: {productName}");
        //                                    }
        //                                }

        //                                // Note: Stock reduction is handled by trigger 'trg_reduce_stock_on_bill'
        //                                // The trigger automatically reduces product_variants.quantity_in_stock
        //                                // when a bill_item is inserted

        //                                // Verify stock was reduced (optional verification)
        //                                string verifyStockQuery = "SELECT quantity_in_stock FROM product_variants WHERE variant_id = @variant_id";
        //                                using (MySqlCommand verifyCmd = new MySqlCommand(verifyStockQuery, con, tran))
        //                                {
        //                                    verifyCmd.Parameters.AddWithValue("@variant_id", variantId);
        //                                    object newStock = verifyCmd.ExecuteScalar();
        //                                    if (newStock == null)
        //                                    {
        //                                        throw new Exception($"Failed to verify stock reduction for: {productName}");
        //                                    }

        //                                    decimal verifiedStock = Convert.ToDecimal(newStock);
        //                                    decimal expectedStock = currentStock - quantity;
        //                                    if (verifiedStock != expectedStock)
        //                                    {
        //                                        throw new Exception($"Stock mismatch for {productName}. Expected: {expectedStock}, Got: {verifiedStock}");
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw new Exception($"Product not found: {productName} ({size})");
        //                            }
        //                        }
        //                    }
        //                }

        //                // Update customer balance if credit customer
        //                if (customerId.HasValue && amountDue > 0)
        //                {
        //                    string updateBalanceQuery = @"UPDATE customer 
        //                                          SET current_balance = current_balance + @amount_due 
        //                                          WHERE customer_id = @customer_id";
        //                    using (MySqlCommand balanceCmd = new MySqlCommand(updateBalanceQuery, con, tran))
        //                    {
        //                        balanceCmd.Parameters.AddWithValue("@amount_due", amountDue);
        //                        balanceCmd.Parameters.AddWithValue("@customer_id", customerId.Value);
        //                        balanceCmd.ExecuteNonQuery();
        //                    }
        //                }

        //                tran.Commit();
        //                return true;
        //            }
        //            catch (Exception e)
        //            {
        //                try
        //                {
        //                    tran.Rollback();
        //                }
        //                catch (Exception rollbackEx)
        //                {
        //                    System.Diagnostics.Debug.WriteLine("Rollback failed: " + rollbackEx.Message);
        //                }

        //                System.Diagnostics.Debug.WriteLine("Sale save error: " + e.ToString());
        //                throw new Exception("Failed to save sale: " + e.Message, e);
        //            }
        //        }
        //    }
        //}

        public bool SaveDataToDatabase(int? customerId, DateTime? date, decimal? total_amount, decimal? paid_amount, DataGridView d, int staffId = 1)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // Validate input data first
                        if (d.Rows.Count == 0 || (d.Rows.Count == 1 && d.Rows[0].IsNewRow))
                        {
                            throw new Exception("No products selected for sale");
                        }

                        // Generate bill number
                        string billNumber = $"INV-{DateTime.Now:yyyy}-{DateTime.Now:MMddHHmmss}";

                        // Get payment status lookup_id
                        int paymentStatusId;
                        string statusQuery = "SELECT lookup_id FROM lookup WHERE type = 'payment_status' AND value = @status";
                        using (MySqlCommand statusCmd = new MySqlCommand(statusQuery, con, tran))
                        {
                            string status = (paid_amount >= total_amount) ? "paid" : "partial";
                            statusCmd.Parameters.AddWithValue("@status", status);
                            object statusResult = statusCmd.ExecuteScalar();
                            if (statusResult == null)
                            {
                                throw new Exception("Payment status not found in lookup table");
                            }
                            paymentStatusId = Convert.ToInt32(statusResult);
                        }

                        // Insert into bills table
                        decimal amountDue = (total_amount ?? 0) - (paid_amount ?? 0);
                        string billQuery = @"INSERT INTO bills 
                    (bill_number, bill_date, customer_id, staff_id, subtotal, discount_amount, 
                     total_amount, amount_paid, amount_due, payment_status_id) 
                    VALUES 
                    (@bill_number, @bill_date, @customer_id, @staff_id, @subtotal, @discount_amount,
                     @total_amount, @amount_paid, @amount_due, @payment_status_id);
                    SELECT LAST_INSERT_ID();";

                        int billId;
                        using (MySqlCommand cmd = new MySqlCommand(billQuery, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@bill_number", billNumber);
                            cmd.Parameters.AddWithValue("@bill_date", date ?? DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", customerId ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@staff_id", staffId);
                            cmd.Parameters.AddWithValue("@subtotal", total_amount ?? 0);
                            cmd.Parameters.AddWithValue("@discount_amount", 0);
                            cmd.Parameters.AddWithValue("@total_amount", total_amount ?? 0);
                            cmd.Parameters.AddWithValue("@amount_paid", paid_amount ?? 0);
                            cmd.Parameters.AddWithValue("@amount_due", amountDue);
                            cmd.Parameters.AddWithValue("@payment_status_id", paymentStatusId);

                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("Failed to get bill ID");
                            }
                            billId = Convert.ToInt32(result);
                        }

                        // Process each product in the DataGridView
                        // Column indices based on your grid:
                        // 0 = product_name/name
                        // 1 = size
                        // 2 = unit_of_measure
                        // 3 = category_type
                        // 4 = sale_price/Unit Price
                        // 5 = quantity
                        // 6 = discount
                        // 7 = total
                        // 8 = final

                        foreach (DataGridViewRow row in d.Rows)
                        {
                            if (row.IsNewRow) continue;

                            // Use INDEX instead of column names
                            string productName = row.Cells[0]?.Value?.ToString()?.Trim();
                            string size = row.Cells[1]?.Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(productName))
                            {
                                throw new Exception("Product name is missing");
                            }

                            // Parse and validate quantity (column 5)
                            if (!decimal.TryParse(row.Cells[5]?.Value?.ToString(), out decimal quantity) || quantity <= 0)
                            {
                                throw new Exception($"Invalid quantity for product: {productName}");
                            }

                            // Get product and variant details
                            int productId;
                            int variantId;
                            decimal salePrice;
                            decimal currentStock;
                            string unitOfMeasure;

                            string productQuery = @"SELECT 
                                        p.product_id, 
                                        pv.variant_id,
                                        pv.price_per_unit,
                                        pv.quantity_in_stock,
                                        pv.unit_of_measure
                                    FROM products p
                                    INNER JOIN product_variants pv ON p.product_id = pv.product_id
                                    WHERE p.name = @ProductName 
                                    AND pv.size = @size
                                    AND p.is_active = TRUE 
                                    AND pv.is_active = TRUE";

                            using (MySqlCommand productCmd = new MySqlCommand(productQuery, con, tran))
                            {
                                productCmd.Parameters.AddWithValue("@ProductName", productName);
                                productCmd.Parameters.AddWithValue("@size", string.IsNullOrEmpty(size) ? "" : size);

                                using (var reader = productCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        productId = reader.GetInt32("product_id");
                                        variantId = reader.GetInt32("variant_id");
                                        salePrice = reader.GetDecimal("price_per_unit");
                                        currentStock = reader.GetDecimal("quantity_in_stock");
                                        unitOfMeasure = reader.GetString("unit_of_measure");
                                        reader.Close();

                                        // Check if sufficient stock is available
                                        if (currentStock < quantity)
                                        {
                                            throw new Exception($"Insufficient stock for {productName} ({size}). Available: {currentStock}, Requested: {quantity}");
                                        }

                                        // Calculate line total (price may be overridden in grid - column 4)
                                        decimal unitPrice = salePrice;
                                        if (row.Cells[4] != null &&
                                            decimal.TryParse(row.Cells[4]?.Value?.ToString(), out decimal gridPrice) &&
                                            gridPrice > 0)
                                        {
                                            unitPrice = gridPrice;
                                        }

                                        decimal lineTotal = unitPrice * quantity;

                                        // Insert bill item
                                        string billItemQuery = @"INSERT INTO bill_items 
                                    (bill_id, product_id, variant_id, quantity, unit_of_measure, unit_price, line_total, notes) 
                                    VALUES 
                                    (@bill_id, @product_id, @variant_id, @quantity, @unit_of_measure, @unit_price, @line_total, @notes)";

                                        using (MySqlCommand billItemCmd = new MySqlCommand(billItemQuery, con, tran))
                                        {
                                            billItemCmd.Parameters.AddWithValue("@bill_id", billId);
                                            billItemCmd.Parameters.AddWithValue("@product_id", productId);
                                            billItemCmd.Parameters.AddWithValue("@variant_id", variantId);
                                            billItemCmd.Parameters.AddWithValue("@quantity", quantity);
                                            billItemCmd.Parameters.AddWithValue("@unit_of_measure", unitOfMeasure);
                                            billItemCmd.Parameters.AddWithValue("@unit_price", unitPrice);
                                            billItemCmd.Parameters.AddWithValue("@line_total", lineTotal);
                                            billItemCmd.Parameters.AddWithValue("@notes", DBNull.Value);

                                            int rowsInserted = billItemCmd.ExecuteNonQuery();
                                            if (rowsInserted == 0)
                                            {
                                                throw new Exception($"Failed to insert bill item for: {productName}");
                                            }
                                        }

                                        // Verify stock was reduced
                                        string verifyStockQuery = "SELECT quantity_in_stock FROM product_variants WHERE variant_id = @variant_id";
                                        using (MySqlCommand verifyCmd = new MySqlCommand(verifyStockQuery, con, tran))
                                        {
                                            verifyCmd.Parameters.AddWithValue("@variant_id", variantId);
                                            object newStock = verifyCmd.ExecuteScalar();
                                            if (newStock == null)
                                            {
                                                throw new Exception($"Failed to verify stock reduction for: {productName}");
                                            }

                                            decimal verifiedStock = Convert.ToDecimal(newStock);
                                            decimal expectedStock = currentStock - quantity;
                                            if (verifiedStock != expectedStock)
                                            {
                                                throw new Exception($"Stock mismatch for {productName}. Expected: {expectedStock}, Got: {verifiedStock}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Product not found: {productName} ({size})");
                                    }
                                }
                            }
                        }

                        // Update customer balance if credit customer
                        if (customerId.HasValue && amountDue > 0)
                        {
                            string updateBalanceQuery = @"UPDATE customers
                                              SET current_balance = current_balance + @amount_due 
                                              WHERE customer_id = @customer_id";
                            using (MySqlCommand balanceCmd = new MySqlCommand(updateBalanceQuery, con, tran))
                            {
                                balanceCmd.Parameters.AddWithValue("@amount_due", amountDue);
                                balanceCmd.Parameters.AddWithValue("@customer_id", customerId.Value);
                                balanceCmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            System.Diagnostics.Debug.WriteLine("Rollback failed: " + rollbackEx.Message);
                        }

                        System.Diagnostics.Debug.WriteLine("Sale save error: " + e.ToString());
                        throw new Exception("Failed to save sale: " + e.Message, e);
                    }
                }
            }
        }
        public static QuotationData FetchQuotationData(string searchValue, bool isNumericId)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                try
                {
                    QuotationData quotationData = new QuotationData();

                    // Query to get quotation header
                    string quotationQuery = @"SELECT 
                q.quotation_id,
                q.quotation_number,
                q.quotation_date,
                q.customer_id,
                c.name AS customer_name,
                c.contact AS customer_contact,
                q.staff_id,
                s.name AS staff_name,
                q.subtotal,
                q.discount_amount,
                q.total_amount,
                q.status_id,
                l.value AS status,
                q.valid_until,
                q.notes
            FROM quotations q
            LEFT JOIN customer c ON q.customer_id = c.customer_id
            LEFT JOIN staff s ON q.staff_id = s.staff_id
            LEFT JOIN lookup l ON q.status_id = l.lookup_id
            WHERE " + (isNumericId ? "q.quotation_id = @search" : "q.quotation_number = @search");

                    using (MySqlCommand cmd = new MySqlCommand(quotationQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@search", searchValue);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                quotationData.QuotationId = reader.GetInt32("quotation_id");
                                quotationData.QuotationNumber = reader.GetString("quotation_number");
                                quotationData.QuotationDate = reader.GetDateTime("quotation_date");
                                quotationData.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id"))
                                    ? (int?)null
                                    : reader.GetInt32("customer_id");
                                quotationData.CustomerName = reader.IsDBNull(reader.GetOrdinal("customer_name"))
                                    ? "Walk-in Customer"
                                    : reader.GetString("customer_name");
                                quotationData.CustomerContact = reader.IsDBNull(reader.GetOrdinal("customer_contact"))
                                    ? ""
                                    : reader.GetString("customer_contact");
                                quotationData.StaffId = reader.GetInt32("staff_id");
                                quotationData.StaffName = reader.GetString("staff_name");
                                quotationData.Subtotal = reader.GetDecimal("subtotal");
                                quotationData.DiscountAmount = reader.GetDecimal("discount_amount");
                                quotationData.TotalAmount = reader.GetDecimal("total_amount");
                                quotationData.Status = reader.GetString("status");
                                quotationData.ValidUntil = reader.GetDateTime("valid_until");
                                quotationData.Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                                    ? ""
                                    : reader.GetString("notes");
                            }
                            else
                            {
                                return null; // Quotation not found
                            }
                        }
                    }

                    // Query to get quotation items
                    string itemsQuery = @"SELECT 
                qi.quotation_item_id,
                qi.product_id,
                p.name AS product_name,
                qi.variant_id,
                pv.size,
                pv.class_type,
                qi.quantity,
                qi.unit_of_measure,
                qi.unit_price,
                qi.line_total,
                qi.notes,
                pv.quantity_in_stock AS available_stock,
                sup.name AS supplier_name,
                cat.value AS category
            FROM quotation_items qi
            INNER JOIN products p ON qi.product_id = p.product_id
            INNER JOIN product_variants pv ON qi.variant_id = pv.variant_id
            LEFT JOIN supplier sup ON p.supplier_id = sup.supplier_id
            LEFT JOIN lookup cat ON p.category_id = cat.lookup_id
            WHERE qi.quotation_id = @quotation_id
            ORDER BY qi.quotation_item_id";

                    quotationData.Items = new BindingList<QuotationItemData>();

                    using (MySqlCommand cmd = new MySqlCommand(itemsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@quotation_id", quotationData.QuotationId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new QuotationItemData
                                {
                                    QuotationItemId = reader.GetInt32("quotation_item_id"),
                                    ProductId = reader.GetInt32("product_id"),
                                    ProductName = reader.GetString("product_name"),
                                    VariantId = reader.GetInt32("variant_id"),
                                    Size = reader.GetString("size"),
                                    ClassType = reader.IsDBNull(reader.GetOrdinal("class_type"))
                                        ? ""
                                        : reader.GetString("class_type"),
                                    Quantity = reader.GetDecimal("quantity"),
                                    UnitOfMeasure = reader.GetString("unit_of_measure"),
                                    sale_price = reader.GetDecimal("unit_price"),
                                    final = reader.GetDecimal("line_total"),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                                        ? ""
                                        : reader.GetString("notes"),
                                    AvailableStock = reader.GetDecimal("available_stock"),
                                    SupplierName = reader.IsDBNull(reader.GetOrdinal("supplier_name"))
                                        ? ""
                                        : reader.GetString("supplier_name"),
                                    Category = reader.IsDBNull(reader.GetOrdinal("category"))
                                        ? ""
                                        : reader.GetString("category")
                                };

                                quotationData.Items.Add(item);
                            }
                        }
                    }

                    return quotationData;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in FetchQuotationData: {ex}");
                    throw;
                }
            }
        }


        public bool SaveQuotationToDatabase(int? customerId, DateTime? date, decimal? total_amount, DataGridView d, int staffId = 1)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // Validate input data first
                        if (d.Rows.Count == 0 || (d.Rows.Count == 1 && d.Rows[0].IsNewRow))
                        {
                            throw new Exception("No products selected for quotation");
                        }

                        // Generate quotation number
                        string quotationNumber = $"QUO-{DateTime.Now:yyyy}-{DateTime.Now:MMddHHmmss}";
                        System.Diagnostics.Debug.WriteLine($"Generated quotation number: {quotationNumber}");

                        // Get quotation status lookup_id
                        int statusId;
                        string statusQuery = "SELECT lookup_id FROM lookup WHERE type = 'quotation_status' AND value = 'pending'";
                        using (MySqlCommand statusCmd = new MySqlCommand(statusQuery, con, tran))
                        {
                            object statusResult = statusCmd.ExecuteScalar();
                            if (statusResult == null)
                            {
                                // Try to get any quotation_status if 'pending' doesn't exist
                                statusQuery = "SELECT lookup_id FROM lookup WHERE type = 'quotation_status' LIMIT 1";
                                statusCmd.CommandText = statusQuery;
                                statusResult = statusCmd.ExecuteScalar();

                                if (statusResult == null)
                                {
                                    throw new Exception("No quotation_status found in lookup table. Please add quotation_status entries.");
                                }
                            }
                            statusId = Convert.ToInt32(statusResult);
                            System.Diagnostics.Debug.WriteLine($"Status ID: {statusId}");
                        }

                        // Insert into quotations table
                        string quotationQuery = @"INSERT INTO quotations 
                    (quotation_number, quotation_date, customer_id, staff_id, subtotal, 
                     discount_amount, total_amount, status_id, valid_until) 
                    VALUES 
                    (@quotation_number, @quotation_date, @customer_id, @staff_id, @subtotal, 
                     @discount_amount, @total_amount, @status_id, @valid_until);
                    SELECT LAST_INSERT_ID();";

                        int quotationId;
                        using (MySqlCommand cmd = new MySqlCommand(quotationQuery, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@quotation_number", quotationNumber);
                            cmd.Parameters.AddWithValue("@quotation_date", date ?? DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", customerId ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@staff_id", staffId);
                            cmd.Parameters.AddWithValue("@subtotal", total_amount ?? 0);
                            cmd.Parameters.AddWithValue("@discount_amount", 0);
                            cmd.Parameters.AddWithValue("@total_amount", total_amount ?? 0);
                            cmd.Parameters.AddWithValue("@status_id", statusId);
                            cmd.Parameters.AddWithValue("@valid_until", DateTime.Now.AddDays(30));

                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("Failed to get quotation ID");
                            }
                            quotationId = Convert.ToInt32(result);
                            System.Diagnostics.Debug.WriteLine($"Quotation ID created: {quotationId}");
                        }

                        // Process each product in the DataGridView
                        int itemCount = 0;
                        foreach (DataGridViewRow row in d.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string productName = row.Cells["product_name"]?.Value?.ToString()?.Trim();
                            string size = row.Cells["size"]?.Value?.ToString()?.Trim();

                            System.Diagnostics.Debug.WriteLine($"Processing: {productName} - {size}");

                            if (string.IsNullOrEmpty(productName))
                            {
                                System.Diagnostics.Debug.WriteLine("Skipping row - product name is empty");
                                continue; // Skip instead of throwing exception
                            }

                            // Parse and validate quantity
                            if (!decimal.TryParse(row.Cells["quantity"]?.Value?.ToString(), out decimal quantity) || quantity <= 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"Invalid quantity for {productName}");
                                continue; // Skip instead of throwing exception
                            }

                            // Get product and variant details
                            int productId;
                            int variantId;
                            decimal salePrice;
                            string unitOfMeasure;

                            string productQuery = @"SELECT 
                                        p.product_id, 
                                        pv.variant_id,
                                        pv.price_per_unit,
                                        pv.unit_of_measure
                                    FROM products p
                                    INNER JOIN product_variants pv ON p.product_id = pv.product_id
                                    WHERE p.name = @product_name 
                                    AND pv.size = @size
                                    AND p.is_active = TRUE 
                                    AND pv.is_active = TRUE";

                            using (MySqlCommand productCmd = new MySqlCommand(productQuery, con, tran))
                            {
                                productCmd.Parameters.AddWithValue("@product_name", productName);
                                productCmd.Parameters.AddWithValue("@size", string.IsNullOrEmpty(size) ? "" : size);

                                using (var reader = productCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        productId = reader.GetInt32("product_id");
                                        variantId = reader.GetInt32("variant_id");
                                        salePrice = reader.GetDecimal("price_per_unit");
                                        unitOfMeasure = reader.GetString("unit_of_measure");
                                        reader.Close();

                                        System.Diagnostics.Debug.WriteLine($"Found product - ID: {productId}, Variant: {variantId}");

                                        // Get unit price from grid or use default
                                        decimal unitPrice = salePrice;
                                        if (row.Cells["sale_price"] != null &&
                                            decimal.TryParse(row.Cells["sale_price"]?.Value?.ToString(), out decimal gridPrice) &&
                                            gridPrice > 0)
                                        {
                                            unitPrice = gridPrice;
                                        }

                                        decimal lineTotal = unitPrice * quantity;

                                        System.Diagnostics.Debug.WriteLine($"Inserting item - Qty: {quantity}, Unit Price: {unitPrice}, Total: {lineTotal}");

                                        // Insert quotation item - NO STOCK DEDUCTION
                                        string quotationItemQuery = @"INSERT INTO quotation_items 
                                    (quotation_id, product_id, variant_id, quantity, unit_of_measure, 
                                     unit_price, line_total, notes) 
                                    VALUES 
                                    (@quotation_id, @product_id, @variant_id, @quantity, @unit_of_measure, 
                                     @unit_price, @line_total, @notes)";

                                        using (MySqlCommand quotationItemCmd = new MySqlCommand(quotationItemQuery, con, tran))
                                        {
                                            quotationItemCmd.Parameters.AddWithValue("@quotation_id", quotationId);
                                            quotationItemCmd.Parameters.AddWithValue("@product_id", productId);
                                            quotationItemCmd.Parameters.AddWithValue("@variant_id", variantId);
                                            quotationItemCmd.Parameters.AddWithValue("@quantity", quantity);
                                            quotationItemCmd.Parameters.AddWithValue("@unit_of_measure", unitOfMeasure);
                                            quotationItemCmd.Parameters.AddWithValue("@unit_price", unitPrice);
                                            quotationItemCmd.Parameters.AddWithValue("@line_total", lineTotal);
                                            quotationItemCmd.Parameters.AddWithValue("@notes", DBNull.Value);

                                            int rowsInserted = quotationItemCmd.ExecuteNonQuery();
                                            if (rowsInserted > 0)
                                            {
                                                itemCount++;
                                                System.Diagnostics.Debug.WriteLine($"Successfully inserted quotation item #{itemCount}");
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine($"Failed to insert quotation item for: {productName}");
                                            }
                                        }

                                        // IMPORTANT: NO STOCK REDUCTION FOR QUOTATIONS
                                    }
                                    else
                                    {
                                        reader.Close();
                                        System.Diagnostics.Debug.WriteLine($"Product not found: {productName} ({size})");
                                        // Continue instead of throwing - allows other items to be processed
                                    }
                                }
                            }
                        }

                        if (itemCount == 0)
                        {
                            throw new Exception("No valid products were added to the quotation");
                        }

                        System.Diagnostics.Debug.WriteLine($"Total items inserted: {itemCount}");

                        // Commit the transaction
                        tran.Commit();
                        System.Diagnostics.Debug.WriteLine("Transaction committed successfully");
                        return true;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            tran.Rollback();
                            System.Diagnostics.Debug.WriteLine("Transaction rolled back");
                        }
                        catch (Exception rollbackEx)
                        {
                            System.Diagnostics.Debug.WriteLine("Rollback failed: " + rollbackEx.Message);
                        }

                        System.Diagnostics.Debug.WriteLine("Quotation save error: " + e.ToString());
                        throw new Exception("Failed to save quotation: " + e.Message, e);
                    }
                }
            }
        }

        // PDF Generation methods remain the same...
        //public static void CreateA4ReceiptPdf(DataGridView cart, string filePath, string customerName, decimal total, decimal paid, decimal totaldiscount)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;

        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin(40);
        //            page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11));

        //            page.Content().Column(column =>
        //            {
        //                column.Item().AlignCenter().Text("Hardware store").Bold().FontSize(24);
        //                column.Item().AlignCenter().Text("Main bazar lahore").FontSize(12);
        //                column.Item().AlignCenter().Text("Phone: 03021222005").FontSize(12);
        //                column.Item().PaddingVertical(10).LineHorizontal(1);

        //                column.Item().PaddingBottom(10).Row(row =>
        //                {
        //                    row.RelativeItem().Column(infoCol =>
        //                    {
        //                        infoCol.Item().Text($"Customer: {customerName}").Bold();
        //                        infoCol.Item().Text($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
        //                    });
        //                    row.RelativeItem().AlignRight().Column(dateCol =>
        //                    {
        //                        dateCol.Item().Text($"Date: {DateTime.Now:dd-MMM-yyyy}");
        //                        dateCol.Item().Text($"Time: {DateTime.Now:hh:mm tt}");
        //                    });
        //                });

        //                column.Item().PaddingBottom(15).LineHorizontal(0.5f);

        //                column.Item().PaddingBottom(5).Table(table =>
        //                {
        //                    table.ColumnsDefinition(columns =>
        //                    {
        //                        columns.RelativeColumn(3);
        //                        columns.ConstantColumn(60);
        //                        columns.ConstantColumn(70);
        //                        columns.ConstantColumn(60);
        //                        columns.ConstantColumn(80);
        //                    });

        //                    table.Header(header =>
        //                    {
        //                        header.Cell().Padding(5).Background("#f0f0f0").Text("Product").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("size").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("unit").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Qty").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Price").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Discount").Bold();
        //                        header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Total").Bold();
        //                    });
        //                });

        //                decimal totalDiscount = 0;
        //                decimal subTotal = 0;
        //                int itemCount = 0;

        //                foreach (DataGridViewRow row in cart.Rows)
        //                {
        //                    if (row.IsNewRow) continue;

        //                    string name = row.Cells["name"].Value?.ToString() ?? "";
        //                    string size = row.Cells["size"].Value?.ToString() ?? "";
        //                    string unit = row.Cells["unit"].Value?.ToString() ?? "";
        //                    string qty = row.Cells["quantity"].Value?.ToString() ?? "0";
        //                    decimal price = ConvertToDecimalSafe(row.Cells["sale_price"].Value ?? 0);
        //                    decimal discount = ConvertToDecimalSafe(row.Cells["discount"].Value ?? 0);
        //                    decimal itemTotal = ConvertToDecimalSafe(row.Cells["final"].Value ?? 0);

        //                    totalDiscount += discount * Convert.ToInt32(qty);
        //                    subTotal += itemTotal;
        //                    itemCount++;

        //                    column.Item().Table(table =>
        //                    {
        //                        table.ColumnsDefinition(columns =>
        //                        {
        //                            columns.RelativeColumn(3);
        //                            columns.ConstantColumn(20);
        //                            columns.ConstantColumn(20);
        //                            columns.ConstantColumn(20);
        //                            columns.ConstantColumn(70);
        //                            columns.ConstantColumn(60);
        //                            columns.ConstantColumn(80);
        //                        });

        //                        table.Cell().Padding(5).Text(name);
        //                        table.Cell().Padding(5).AlignRight().Text(size);
        //                        table.Cell().Padding(5).AlignRight().Text(unit);

        //                        table.Cell().Padding(5).AlignRight().Text(qty);
        //                        table.Cell().Padding(5).AlignRight().Text($"Rs. {price:N2}");
        //                        table.Cell().Padding(5).AlignRight().Text($"Rs. {discount:N2}");
        //                        table.Cell().Padding(5).AlignRight().Text($"Rs. {itemTotal:N2}").Bold();
        //                    });

        //                    if (itemCount < cart.Rows.Count - 1)
        //                    {
        //                        column.Item().PaddingHorizontal(10).LineHorizontal(0.2f);
        //                    }
        //                }

        //                column.Item().PaddingTop(20).Table(summaryTable =>
        //                {
        //                    summaryTable.ColumnsDefinition(columns =>
        //                    {
        //                        columns.RelativeColumn();
        //                        columns.ConstantColumn(150);
        //                    });

        //                    summaryTable.Cell().Padding(3).AlignRight().Text("Subtotal:");
        //                    summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {(subTotal + totalDiscount):N2}");

        //                    summaryTable.Cell().Padding(3).AlignRight().Text("Total Discount:");
        //                    summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {totaldiscount:N2}");

        //                    summaryTable.Cell().Padding(5).Background("#e8f4fd").AlignRight().Text("TOTAL:").Bold();
        //                    summaryTable.Cell().Padding(5).Background("#e8f4fd").AlignRight().Text($"Rs. {total:N2}").Bold().FontSize(12);

        //                    summaryTable.Cell().Padding(3).AlignRight().Text("Amount Paid:");
        //                    summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {paid:N2}");

        //                    summaryTable.Cell().Padding(5).Background("#fff8dc").AlignRight().Text("BALANCE:").Bold();
        //                    summaryTable.Cell().Padding(5).Background("#fff8dc").AlignRight().Text($"Rs. {(total - paid):N2}").Bold();
        //                });

        //                column.Item().PaddingVertical(15).LineHorizontal(1);

        //                column.Item().AlignCenter().Text("Thank you for your shopping here!").Bold().FontSize(14);
        //                column.Item().PaddingVertical(5).AlignCenter().Text("بل کے بغیر واپسی نہیں ہوگی");
        //                //column.Item().AlignCenter().Text("");
        //                column.Item().AlignCenter().Text("آپ کے اعتماد کا شکریہ");

        //                column.Item().PaddingVertical(15).AlignCenter().Text("Terms & Conditions:").SemiBold();
        //                column.Item().AlignCenter().Text("• Goods once sold cannot be returned or exchanged");
        //                //column.Item().AlignCenter().Text("• Medicines cannot be returned after use");
        //                column.Item().AlignCenter().Text("• Please check items at the time of purchase");

        //                column.Item().PaddingVertical(20).LineHorizontal(0.5f);

        //                column.Item().AlignCenter().Text("Developed By: devinfantary.com | 03477048001").FontSize(9);
        //                column.Item().AlignCenter().Text($"Printed on: {DateTime.Now:dd-MMM-yyyy hh:mm tt}").FontSize(9);
        //            });
        //        });
        //    }).GeneratePdf(filePath);
        //}

        public static void CreateA4ReceiptPdf(DataGridView cart, string filePath, string customerName, decimal total, decimal paid, decimal totaldiscount)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11));

                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Hardware store").Bold().FontSize(24);
                        column.Item().AlignCenter().Text("Main bazar lahore").FontSize(12);
                        column.Item().AlignCenter().Text("Phone: 03021222005").FontSize(12);
                        column.Item().PaddingVertical(10).LineHorizontal(1);

                        column.Item().PaddingBottom(10).Row(row =>
                        {
                            row.RelativeItem().Column(infoCol =>
                            {
                                infoCol.Item().Text($"Customer: {customerName}").Bold();
                                infoCol.Item().Text($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
                            });
                            row.RelativeItem().AlignRight().Column(dateCol =>
                            {
                                dateCol.Item().Text($"Date: {DateTime.Now:dd-MMM-yyyy}");
                                dateCol.Item().Text($"Time: {DateTime.Now:hh:mm tt}");
                            });
                        });

                        column.Item().PaddingBottom(15).LineHorizontal(0.5f);

                        column.Item().PaddingBottom(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Padding(5).Background("#f0f0f0").Text("Product").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Size").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Unit").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Qty").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Price").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Discount").Bold();
                                header.Cell().Padding(5).Background("#f0f0f0").AlignRight().Text("Total").Bold();
                            });
                        });

                        decimal totalDiscount = 0;
                        decimal subTotal = 0;
                        int itemCount = 0;

                        foreach (DataGridViewRow row in cart.Rows)
                        {
                            if (row.IsNewRow) continue;

                            // USE INDICES INSTEAD OF COLUMN NAMES
                            string name = row.Cells[0]?.Value?.ToString() ?? ""; // product_name
                            string size = row.Cells[1]?.Value?.ToString() ?? ""; // size
                            string unit = row.Cells[2]?.Value?.ToString() ?? ""; // unit_of_measure
                            string qty = row.Cells[5]?.Value?.ToString() ?? "0"; // quantity
                            decimal price = ConvertToDecimalSafe(row.Cells[4]?.Value ?? 0); // sale_price
                            decimal discount = ConvertToDecimalSafe(row.Cells[6]?.Value ?? 0); // discount
                            decimal itemTotal = ConvertToDecimalSafe(row.Cells[8]?.Value ?? 0); // final

                            totalDiscount += discount * Convert.ToDecimal(qty);
                            subTotal += itemTotal;
                            itemCount++;

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(70);
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(70);
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(80);
                                });

                                table.Cell().Padding(5).Text(name);
                                table.Cell().Padding(5).AlignRight().Text(size);
                                table.Cell().Padding(5).AlignRight().Text(unit);
                                table.Cell().Padding(5).AlignRight().Text(qty);
                                table.Cell().Padding(5).AlignRight().Text($"Rs. {price:N2}");
                                table.Cell().Padding(5).AlignRight().Text($"Rs. {discount:N2}");
                                table.Cell().Padding(5).AlignRight().Text($"Rs. {itemTotal:N2}").Bold();
                            });

                            if (itemCount < cart.Rows.Count - 1)
                            {
                                column.Item().PaddingHorizontal(10).LineHorizontal(0.2f);
                            }
                        }

                        column.Item().PaddingTop(20).Table(summaryTable =>
                        {
                            summaryTable.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(150);
                            });

                            summaryTable.Cell().Padding(3).AlignRight().Text("Subtotal:");
                            summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {(subTotal + totalDiscount):N2}");

                            summaryTable.Cell().Padding(3).AlignRight().Text("Total Discount:");
                            summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {totaldiscount:N2}");

                            summaryTable.Cell().Padding(5).Background("#e8f4fd").AlignRight().Text("TOTAL:").Bold();
                            summaryTable.Cell().Padding(5).Background("#e8f4fd").AlignRight().Text($"Rs. {total:N2}").Bold().FontSize(12);

                            summaryTable.Cell().Padding(3).AlignRight().Text("Amount Paid:");
                            summaryTable.Cell().Padding(3).AlignRight().Text($"Rs. {paid:N2}");

                            summaryTable.Cell().Padding(5).Background("#fff8dc").AlignRight().Text("BALANCE:").Bold();
                            summaryTable.Cell().Padding(5).Background("#fff8dc").AlignRight().Text($"Rs. {(total - paid):N2}").Bold();
                        });

                        column.Item().PaddingVertical(15).LineHorizontal(1);

                        column.Item().AlignCenter().Text("Thank you for your shopping here!").Bold().FontSize(14);
                        column.Item().PaddingVertical(5).AlignCenter().Text("بل کے بغیر واپسی نہیں ہوگی");
                        column.Item().AlignCenter().Text("آپ کے اعتماد کا شکریہ");

                        column.Item().PaddingVertical(15).AlignCenter().Text("Terms & Conditions:").SemiBold();
                        column.Item().AlignCenter().Text("• Goods once sold cannot be returned or exchanged");
                        column.Item().AlignCenter().Text("• Please check items at the time of purchase");

                        column.Item().PaddingVertical(20).LineHorizontal(0.5f);

                        column.Item().AlignCenter().Text("Developed By: devinfantary.com | 03477048001").FontSize(9);
                        column.Item().AlignCenter().Text($"Printed on: {DateTime.Now:dd-MMM-yyyy hh:mm tt}").FontSize(9);
                    });
                });
            }).GeneratePdf(filePath);
        }

        public static decimal ConvertToDecimalSafe(object value, decimal defaultValue = 0)
        {
            if (value == null) return defaultValue;
            if (decimal.TryParse(value.ToString(), out decimal result))
                return result;
            return defaultValue;
        }

        public static void PrintA4ReceiptDirectly(DataGridView cart, string customerName, decimal total, decimal paid, decimal totaldiscount)
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"Receipt_{DateTime.Now:yyyyMMddHHmmss}.pdf");

                CreateA4ReceiptPdf(cart, tempFilePath, customerName, total, paid, totaldiscount);
                PrintPdfToPrinter(tempFilePath);

                MessageBox.Show("Receipt sent to printer successfully!", "Print Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Printing failed: {ex.Message}", "Print Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private static void PrintPdfToPrinter(string filePath)
        //{
        //    using (PrintDialog printDialog = new PrintDialog())
        //    {
        //        printDialog.AllowSomePages = false;
        //        printDialog.AllowSelection = false;

        //        if (printDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            using (Process process = new Process())
        //            {
        //                ProcessStartInfo startInfo = new ProcessStartInfo
        //                {
        //                    Verb = "printto",
        //                    FileName = filePath,
        //                    Arguments = $"\"{printDialog.PrinterSettings.PrinterName}\"",
        //                    WindowStyle = ProcessWindowStyle.Hidden,
        //                    CreateNoWindow = true,
        //                    UseShellExecute = true
        //                };

        //                process.StartInfo = startInfo;
        //                process.Start();
        //                process.WaitForInputIdle();
        //                Thread.Sleep(3000);

        //                if (!process.HasExited)
        //                {
        //                    process.CloseMainWindow();
        //                    process.Close();
        //                }
        //            }
        //        }
        //    }
        //}

        private static void PrintPdfToPrinter(string filePath)
        {
            try
            {
                // First, let the user select a printer
                using (PrintDialog printDialog = new PrintDialog())
                {
                    printDialog.AllowSomePages = false;
                    printDialog.AllowSelection = false;

                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Method 1: Try using Adobe Reader if installed
                        string acrobatPath = GetAcrobatReaderPath();
                        if (!string.IsNullOrEmpty(acrobatPath))
                        {
                            PrintWithAcrobat(filePath, printDialog.PrinterSettings.PrinterName, acrobatPath);
                        }
                        else
                        {
                            // Method 2: Fallback to system print dialog
                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = filePath,
                                Verb = "Print",
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            };

                            Process.Start(psi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If all else fails, open the PDF and let user print manually
                Process.Start(filePath);
                throw new Exception($"Could not print automatically. PDF opened for manual printing. Error: {ex.Message}");
            }
        }

        private static void PrintWithAcrobat(string filePath, string printerName, string acrobatPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = acrobatPath,
                Arguments = $"/t \"{filePath}\" \"{printerName}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit(10000); // Wait up to 10 seconds
            }
        }

        private static string GetAcrobatReaderPath()
        {
            // Common paths for Adobe Reader/Acrobat
            string[] possiblePaths =
            {
        @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
        @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe",
        @"C:\Program Files (x86)\Adobe\Acrobat Reader\Reader\AcroRd32.exe",
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Adobe", "Acrobat Reader DC", "Reader", "AcroRd32.exe")
    };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }



        public static void PrintThermalReceipt(DataGridView cart, string customerName, decimal total, decimal paid, decimal totaldiscount)
        {
            PrintDocument printDocument = new PrintDocument();

            // Set up for 80mm thermal printer
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 0);
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            Font receiptFont = new Font("Consolas", 8, FontStyle.Regular);
            Font boldFont = new Font("Consolas", 8, FontStyle.Bold);
            Font headerFont = new Font("Consolas", 12, FontStyle.Bold); // Bigger font for header
            int currentY = 5;

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics g = e.Graphics;
                int width = e.PageBounds.Width;

                // Helper functions
                void DrawText(string text, Font font = null, int offsetX = 0)
                {
                    font = font ?? receiptFont;
                    g.DrawString(text, font, Brushes.Black, offsetX, currentY);
                    currentY += (int)g.MeasureString(text, font).Height + 1;
                }

                void DrawCenteredText(string text, Font font = null)
                {
                    font = font ?? receiptFont;
                    SizeF textSize = g.MeasureString(text, font);
                    float x = (width - textSize.Width) / 2;
                    g.DrawString(text, font, Brushes.Black, x, currentY);
                    currentY += (int)textSize.Height + 1;
                }

                void DrawLine()
                {
                    g.DrawLine(Pens.Black, 10, currentY, width - 10, currentY);
                    currentY += 5;
                }

                void DrawLeftRight(string left, string right)
                {
                    g.DrawString(left, receiptFont, Brushes.Black, 10, currentY);
                    SizeF rightSize = g.MeasureString(right, receiptFont);
                    g.DrawString(right, receiptFont, Brushes.Black, width - rightSize.Width - 10, currentY);
                    currentY += (int)rightSize.Height + 1;
                }

                // --- Header with Double Line Big Font ---
                DrawCenteredText("ALI VETERINARY", headerFont);
                DrawCenteredText("CLINIC", headerFont);
                DrawCenteredText("main jalsai bazar, Tehsil Lahor district Swabi");
                DrawCenteredText("Phone: 0300-6634245");
                DrawLine();

                // --- Customer Info ---
                DrawLeftRight($"Customer: {customerName}", $"{DateTime.Now:dd-MMM-yyyy hh:mm tt}");
                DrawLine();

                // --- Table Header ---
                DrawCenteredText("----------------------------------------");
                DrawText("MEDIC         QTY PRICE DISC TOTAL");
                DrawCenteredText("----------------------------------------");

                // --- Cart Items ---
                decimal totalDiscount = 0;
                decimal subTotal = 0;

                foreach (DataGridViewRow row in cart.Rows)
                {
                    if (row.IsNewRow) continue;

                    string name = row.Cells["name"].Value?.ToString() ?? "";
                    string qty = row.Cells["quantity"].Value?.ToString()?.PadLeft(2);
                    string price = row.Cells["sale_price"].Value?.ToString()?.PadLeft(5);
                    string discount = row.Cells["discount"].Value?.ToString()?.PadLeft(3);
                    string totalPrice = row.Cells["final"].Value?.ToString()?.PadLeft(6);

                    if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discVal))
                        totalDiscount += discVal * Convert.ToInt32(row.Cells["quantity"].Value);
                    if (decimal.TryParse(row.Cells["final"].Value?.ToString(), out decimal itemTotal))
                        subTotal += itemTotal;

                    // Split name across lines
                    string[] nameParts = name.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    string firstWord = nameParts.Length > 0 ? nameParts[0] : name;
                    string[] remainingWords = nameParts.Skip(1).ToArray();

                    // First line with first word and all data
                    string firstLine = $"{firstWord,-12}{qty} {price} {discount} {totalPrice}";
                    DrawText(firstLine);

                    // Remaining words as new lines
                    foreach (var word in remainingWords)
                    {
                        DrawText($"    {word}"); // 4 spaces indentation
                    }
                }

                // --- Summary ---
                DrawCenteredText("----------------------------------------");
                DrawLeftRight($"SUBTOTAL:", $"Rs. {(subTotal + totalDiscount):N0}");
                DrawLeftRight($"DISCOUNT:", $"Rs. {totaldiscount:N0}");
                DrawLeftRight($"TOTAL:", $"Rs. {total:N0}");
                DrawLeftRight($"PAID:", $"Rs. {paid:N0}");
                DrawLeftRight($"BALANCE:", $"Rs. {(total - paid):N0}");
                DrawCenteredText("----------------------------------------");

                // --- Footer ---
                currentY += 3;
                DrawCenteredText("Thank you for your shopping here!", boldFont);
                DrawLine();
                DrawCenteredText("** SPECIAL OFFERS **", boldFont);
                DrawCenteredText("بل کے بغیر واپسی نہیں ہوگی");
                DrawCenteredText("دوائیں استعمال ہونے کے بعد واپس نہیں ہوں گی");
                DrawCenteredText("آپ کے اعتماد کا شکریہ");
                DrawCenteredText($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
                currentY += 3;
                DrawCenteredText("Developed By:");
                DrawCenteredText("abdulahad18022@gmail.com");
                DrawCenteredText("03477048001");

            };

            // Show print dialog and print
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        //public static void CreateThermalReceiptPdf(DataGridView cart, string filePath, decimal total, decimal paid)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;

        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(226, PageSizes.A4.Height, Unit.Point); // 80mm width
        //            page.Margin(5);
        //            page.DefaultTextStyle(x => x.FontFamily("Consolas").FontSize(9));

        //            page.Content().Column(column =>
        //            {
        //                // --- Logo + Header ---
        //                column.Item().AlignCenter().Image(GetLogoImageStream(), ImageScaling.FitWidth);
        //                column.Item().AlignCenter().Text("MNS Computers").Bold().FontSize(12);
        //                column.Item().AlignCenter().Text("office # 39 & 40, 1st floor Gallery 3, Rex city, Sitiana Road");
        //                column.Item().AlignCenter().Text("Phone: 0300-6634245");
        //                column.Item().PaddingBottom(5).LineHorizontal(0.5f);

        //                // --- Invoice Info ---
        //                column.Item().Row(row =>
        //                {
        //                    row.RelativeItem().AlignRight().Text($"{DateTime.Now:dd-MMM-yyyy hh:mm tt}");
        //                });

        //                column.Item().PaddingBottom(5).LineHorizontal(0.5f);

        //                // --- Table Header ---
        //                column.Item().Text("----------------------------------------");
        //                column.Item().Text("ITEM         QTY PRICE DISC TOTAL");
        //                column.Item().Text("----------------------------------------");

        //                // --- Cart Items ---
        //                decimal totalDiscount = 0;
        //                decimal subTotal = 0;

        //                foreach (DataGridViewRow row in cart.Rows)
        //                {
        //                    if (row.IsNewRow) continue;

        //                    string name = row.Cells["name"].Value?.ToString() ?? "";
        //                    string qty = row.Cells["quantity"].Value?.ToString()?.PadLeft(2);
        //                    string price = row.Cells["total"].Value?.ToString()?.PadLeft(5);
        //                    string discount = row.Cells["discount"].Value?.ToString()?.PadLeft(3);
        //                    string totalPrice = row.Cells["final"].Value?.ToString()?.PadLeft(6);

        //                    if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discVal))
        //                        totalDiscount += discVal * Convert.ToInt32(row.Cells["quantity"].Value);
        //                    if (decimal.TryParse(row.Cells["total"].Value?.ToString(), out decimal itemTotal))
        //                        subTotal += itemTotal;

        //                    // Split name across lines
        //                    string[] nameParts = name.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
        //                    string firstWord = nameParts.Length > 0 ? nameParts[0] : name;
        //                    string[] remainingWords = nameParts.Skip(1).ToArray();

        //                    // First line with first word and all data
        //                    string firstLine = $"{firstWord,-12}{qty} {price} {discount} {totalPrice}";
        //                    column.Item().Text(firstLine);

        //                    // Remaining words as new lines
        //                    foreach (var word in remainingWords)
        //                    {
        //                        column.Item().PaddingLeft(10).Text(word);
        //                    }
        //                }

        //                // --- Summary ---
        //                column.Item().Text("----------------------------------------");
        //                column.Item().Text($"SUBTOTAL:    Rs. {subTotal + totalDiscount:N0}");
        //                column.Item().Text($"DISCOUNT:    Rs. {totalDiscount:N0}");
        //                column.Item().Text($"TOTAL:       Rs. {total:N0}");
        //                column.Item().Text($"PAID:        Rs. {paid:N0}");
        //                column.Item().Text($"BALANCE:     Rs. {(total - paid):N0}");
        //                column.Item().Text("----------------------------------------");

        //                // --- Footer ---
        //                column.Item().AlignCenter().Text("Thank you for your shopping here!").Bold();
        //                column.Item().PaddingTop(5).LineHorizontal(0.5f);
        //                column.Item().AlignCenter().Text("** SPECIAL OFFERS **").Bold();
        //                column.Item().AlignCenter().Text("Free diagnostics with any repair");
        //                column.Item().AlignCenter().Text("10% discount on next purchase");
        //                column.Item().AlignCenter().Text("Ask about our warranty plans!");
        //                column.Item().AlignCenter().Text($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
        //                column.Item().PaddingTop(5).AlignCenter().Text("Developed By:");
        //                column.Item().PaddingTop(5).AlignCenter().Text("abdulahad18022@gmail.com");
        //            });
        //        });
        //    }).GeneratePdf(filePath);
        //}



    }
}
