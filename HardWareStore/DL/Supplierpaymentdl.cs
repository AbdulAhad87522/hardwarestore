using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class SupplierPaymentDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        // Add payment record and update batch
        public async Task<bool> AddPaymentRecord(SupplierPaymentRecord payment)
        {
            try
            {
                string query = @"
                    INSERT INTO supplier_payment_records 
                    (supplier_id, batch_id, payment_amount, payment_date, remarks)
                    VALUES 
                    (@supplier_id, @batch_id, @payment_amount, @payment_date, @remarks);";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@supplier_id", payment.supplier_id),
                    new MySqlParameter("@batch_id", payment.batch_id),
                    new MySqlParameter("@payment_amount", payment.payment_amount),
                    new MySqlParameter("@payment_date", payment.payment_date),
                    new MySqlParameter("@remarks", payment.remarks ?? (object)DBNull.Value)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding payment record: {ex.Message}");
                throw;
            }
        }

        // Get all payment records for a batch
        public async Task<List<SupplierPaymentRecord>> GetPaymentRecordsByBatch(int batchId)
        {
            try
            {
                string query = @"
                    SELECT spr.*, 
                           s.name as supplier_name,
                           pb.BatchName as batch_name
                    FROM supplier_payment_records spr
                    INNER JOIN supplier s ON spr.supplier_id = s.supplier_id
                    INNER JOIN purchase_batches pb ON spr.batch_id = pb.batch_id
                    WHERE spr.batch_id = @batch_id
                    ORDER BY spr.payment_date DESC;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@batch_id", batchId)
                };

                return await Task.Run(() =>
                {
                    var records = new List<SupplierPaymentRecord>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            records.Add(MapPaymentFromReader(reader));
                        }
                    }
                    return records;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting payment records by batch: {ex.Message}");
                throw;
            }
        }

        // Get all payment records for a supplier
        public async Task<List<SupplierPaymentRecord>> GetPaymentRecordsBySupplier(int supplierId)
        {
            try
            {
                string query = @"
                    SELECT spr.*, 
                           s.name as supplier_name,
                           pb.BatchName as batch_name
                    FROM supplier_payment_records spr
                    INNER JOIN supplier s ON spr.supplier_id = s.supplier_id
                    INNER JOIN purchase_batches pb ON spr.batch_id = pb.batch_id
                    WHERE spr.supplier_id = @supplier_id
                    ORDER BY spr.payment_date DESC;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@supplier_id", supplierId)
                };

                return await Task.Run(() =>
                {
                    var records = new List<SupplierPaymentRecord>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            records.Add(MapPaymentFromReader(reader));
                        }
                    }
                    return records;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting payment records by supplier: {ex.Message}");
                throw;
            }
        }

        // Get all payment records
        public async Task<List<SupplierPaymentRecord>> GetAllPaymentRecords()
        {
            try
            {
                string query = @"
                    SELECT spr.*, 
                           s.name as supplier_name,
                           pb.BatchName as batch_name
                    FROM supplier_payment_records spr
                    INNER JOIN supplier s ON spr.supplier_id = s.supplier_id
                    INNER JOIN purchase_batches pb ON spr.batch_id = pb.batch_id
                    ORDER BY spr.payment_date DESC;";

                return await Task.Run(() =>
                {
                    var records = new List<SupplierPaymentRecord>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            records.Add(MapPaymentFromReader(reader));
                        }
                    }
                    return records;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all payment records: {ex.Message}");
                throw;
            }
        }

        // Search payment records
        public async Task<List<SupplierPaymentRecord>> SearchPaymentRecords(string keyword)
        {
            try
            {
                string query = @"
                    SELECT spr.*, 
                           s.name as supplier_name,
                           pb.BatchName as batch_name
                    FROM supplier_payment_records spr
                    INNER JOIN supplier s ON spr.supplier_id = s.supplier_id
                    INNER JOIN purchase_batches pb ON spr.batch_id = pb.batch_id
                    WHERE s.name LIKE @keyword 
                    OR pb.BatchName LIKE @keyword
                    OR spr.remarks LIKE @keyword
                    ORDER BY spr.payment_date DESC;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var records = new List<SupplierPaymentRecord>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            records.Add(MapPaymentFromReader(reader));
                        }
                    }
                    return records;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching payment records: {ex.Message}");
                throw;
            }
        }

        // Delete payment record (with trigger to reverse batch updates)
        public async Task<bool> DeletePaymentRecord(int paymentId, int batchId, decimal paymentAmount)
        {
            try
            {
                string query = @"
                    DELETE FROM supplier_payment_records WHERE payment_id = @payment_id;
                    
                    UPDATE purchase_batches
                    SET paid = paid - @payment_amount,
                        status = CASE 
                            WHEN (paid - @payment_amount) <= 0 THEN 'Pending'
                            WHEN (paid - @payment_amount) < total_price THEN 'Partial'
                            ELSE status
                        END
                    WHERE batch_id = @batch_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@payment_id", paymentId),
                    new MySqlParameter("@batch_id", batchId),
                    new MySqlParameter("@payment_amount", paymentAmount)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting payment record: {ex.Message}");
                throw;
            }
        }

        private SupplierPaymentRecord MapPaymentFromReader(MySqlDataReader reader)
        {
            return new SupplierPaymentRecord
            {
                payment_id = reader.GetInt32("payment_id"),
                supplier_id = reader.GetInt32("supplier_id"),
                batch_id = reader.GetInt32("batch_id"),
                payment_amount = reader.GetDecimal("payment_amount"),
                payment_date = reader.GetDateTime("payment_date"),
                remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString("remarks"),
                created_at = reader.GetDateTime("created_at"),
                supplier_name = reader.GetString("supplier_name"),
                batch_name = reader.GetString("batch_name")
            };
        }
    }
}