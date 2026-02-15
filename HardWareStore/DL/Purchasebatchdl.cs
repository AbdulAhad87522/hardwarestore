using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class PurchaseBatchDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        #region Purchase Batch Operations

        // Get next batch ID
        public async Task<int> GetNextBatchId()
        {
            try
            {
                string query = "SELECT COALESCE(MAX(batch_id), 0) + 1 FROM purchase_batches;";

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting next batch ID: {ex.Message}");
                throw;
            }
        }

        // Create new batch
        public async Task<bool> AddBatch(PurchaseBatch batch)
        {
            try
            {
                string query = @"
                    INSERT INTO purchase_batches 
                    (batch_id, supplier_id, BatchName, total_price, paid, status)
                    VALUES 
                    (@batch_id, @supplier_id, @BatchName, @total_price, @paid, @status);";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@batch_id", batch.batch_id),
                    new MySqlParameter("@supplier_id", batch.supplier_id),
                    new MySqlParameter("@BatchName", batch.BatchName),
                    new MySqlParameter("@total_price", batch.total_price),
                    new MySqlParameter("@paid", batch.paid),
                    new MySqlParameter("@status", batch.status)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding batch: {ex.Message}");
                throw;
            }
        }

        // Update batch
        public async Task<bool> UpdateBatch(PurchaseBatch batch)
        {
            try
            {
                string query = @"
                    UPDATE purchase_batches 
                    SET supplier_id = @supplier_id,
                        BatchName = @BatchName,
                        total_price = @total_price,
                        paid = @paid,
                        status = @status
                    WHERE batch_id = @batch_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@batch_id", batch.batch_id),
                    new MySqlParameter("@supplier_id", batch.supplier_id),
                    new MySqlParameter("@BatchName", batch.BatchName),
                    new MySqlParameter("@total_price", batch.total_price),
                    new MySqlParameter("@paid", batch.paid),
                    new MySqlParameter("@status", batch.status)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating batch: {ex.Message}");
                throw;
            }
        }

        // Get batch by ID
        public async Task<PurchaseBatch> GetBatchById(int batchId)
        {
            try
            {
                string query = @"
                    SELECT pb.*, s.name as supplier_name,
                           (pb.total_price - pb.paid) as remaining
                    FROM purchase_batches pb
                    LEFT JOIN supplier s ON pb.supplier_id = s.supplier_id
                    WHERE pb.batch_id = @batch_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@batch_id", batchId)
                };

                return await Task.Run(() =>
                {
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        if (reader.Read())
                        {
                            return MapBatchFromReader(reader);
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting batch: {ex.Message}");
                throw;
            }
        }

        // Get all batches
        public async Task<List<PurchaseBatch>> GetAllBatches()
        {
            try
            {
                string query = @"
                    SELECT pb.*, s.name as supplier_name,
                           (pb.total_price - pb.paid) as remaining
                    FROM purchase_batches pb
                    LEFT JOIN supplier s ON pb.supplier_id = s.supplier_id
                    ORDER BY pb.batch_id DESC;";

                return await Task.Run(() =>
                {
                    var batches = new List<PurchaseBatch>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            batches.Add(MapBatchFromReader(reader));
                        }
                    }
                    return batches;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all batches: {ex.Message}");
                throw;
            }
        }

        // Search batches
        public async Task<List<PurchaseBatch>> SearchBatches(string keyword)
        {
            try
            {
                string query = @"
                    SELECT pb.*, s.name as supplier_name,
                           (pb.total_price - pb.paid) as remaining
                    FROM purchase_batches pb
                    LEFT JOIN supplier s ON pb.supplier_id = s.supplier_id
                    WHERE pb.BatchName LIKE @keyword 
                    OR s.name LIKE @keyword
                    OR pb.status LIKE @keyword
                    ORDER BY pb.batch_id DESC;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var batches = new List<PurchaseBatch>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            batches.Add(MapBatchFromReader(reader));
                        }
                    }
                    return batches;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching batches: {ex.Message}");
                throw;
            }
        }

        // Delete batch
        public async Task<bool> DeleteBatch(int batchId)
        {
            try
            {
                // First delete items
                string deleteItems = "DELETE FROM purchase_batch_items WHERE purchase_batch_id = @batch_id;";
                var itemParams = new MySqlParameter[] { new MySqlParameter("@batch_id", batchId) };
                await Task.Run(() => _db.ExecuteNonQuery(deleteItems, itemParams));

                // Then delete batch
                string deleteBatch = "DELETE FROM purchase_batches WHERE batch_id = @batch_id;";
                var batchParams = new MySqlParameter[] { new MySqlParameter("@batch_id", batchId) };
                int result = await Task.Run(() => _db.ExecuteNonQuery(deleteBatch, batchParams));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting batch: {ex.Message}");
                throw;
            }
        }

        private PurchaseBatch MapBatchFromReader(MySqlDataReader reader)
        {
            return new PurchaseBatch
            {
                batch_id = reader.GetInt32("batch_id"),
                supplier_id = reader.GetInt32("supplier_id"),
                BatchName = reader.GetString("BatchName"),
                total_price = reader.GetDecimal("total_price"),
                paid = reader.GetDecimal("paid"),
                status = reader.GetString("status"),
                supplier_name = reader.IsDBNull(reader.GetOrdinal("supplier_name")) ? "" : reader.GetString("supplier_name"),
                remaining = reader.GetDecimal("remaining")
            };
        }

        #endregion

        #region Purchase Batch Items Operations

        // Get next item ID
        public async Task<int> GetNextItemId()
        {
            try
            {
                string query = "SELECT COALESCE(MAX(purchase_batch_item_id), 0) + 1 FROM purchase_batch_items;";

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting next item ID: {ex.Message}");
                throw;
            }
        }

        // Add batch item and update stock
        public async Task<bool> AddBatchItem(PurchaseBatchItem item)
        {
            try
            {
                string query = @"
                    INSERT INTO purchase_batch_items 
                    (purchase_batch_item_id, purchase_batch_id, variant_id, quantity_recieved, cost_price)
                    VALUES 
                    (@item_id, @batch_id, @variant_id, @quantity, @cost_price);
                    
                    UPDATE product_variants 
                    SET quantity_in_stock = quantity_in_stock + @quantity,
                        updated_at = CURRENT_TIMESTAMP
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@item_id", item.purchase_batch_item_id),
                    new MySqlParameter("@batch_id", item.purchase_batch_id),
                    new MySqlParameter("@variant_id", item.variant_id),
                    new MySqlParameter("@quantity", item.quantity_received),
                    new MySqlParameter("@cost_price", item.cost_price)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding batch item: {ex.Message}");
                throw;
            }
        }

        // Get items for a batch
        public async Task<List<PurchaseBatchItem>> GetBatchItems(int batchId)
        {
            try
            {
                string query = @"
                    SELECT pbi.*, 
                           p.name as product_name,
                           pv.size,
                           pv.class_type,
                           pv.price_per_unit as sale_price,
                           (pbi.quantity_recieved * pbi.cost_price) as line_total
                    FROM purchase_batch_items pbi
                    INNER JOIN product_variants pv ON pbi.variant_id = pv.variant_id
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pbi.purchase_batch_id = @batch_id
                    ORDER BY pbi.purchase_batch_item_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@batch_id", batchId)
                };

                return await Task.Run(() =>
                {
                    var items = new List<PurchaseBatchItem>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapItemFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting batch items: {ex.Message}");
                throw;
            }
        }

        // Delete batch item and restore stock
        public async Task<bool> DeleteBatchItem(int itemId, int variantId, decimal quantity)
        {
            try
            {
                string query = @"
                    DELETE FROM purchase_batch_items WHERE purchase_batch_item_id = @item_id;
                    
                    UPDATE product_variants 
                    SET quantity_in_stock = quantity_in_stock - @quantity,
                        updated_at = CURRENT_TIMESTAMP
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@item_id", itemId),
                    new MySqlParameter("@variant_id", variantId),
                    new MySqlParameter("@quantity", quantity)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting batch item: {ex.Message}");
                throw;
            }
        }

        private PurchaseBatchItem MapItemFromReader(MySqlDataReader reader)
        {
            return new PurchaseBatchItem
            {
                purchase_batch_item_id = reader.GetInt32("purchase_batch_item_id"),
                purchase_batch_id = reader.GetInt32("purchase_batch_id"),
                variant_id = reader.GetInt32("variant_id"),
                quantity_received = reader.GetDecimal("quantity_recieved"),
                cost_price = reader.GetDecimal("cost_price"),
                line_total = reader.GetDecimal("line_total"),
                product_name = reader.GetString("product_name"),
                size = reader.IsDBNull(reader.GetOrdinal("size")) ? "Standard" : reader.GetString("size"),
                class_type = reader.IsDBNull(reader.GetOrdinal("class_type")) ? null : reader.GetString("class_type"),
                sale_price = reader.GetDecimal("sale_price"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }

        #endregion

        #region Helper Methods for Variant Selection

        // Get all variants with product info for selection grid
        public async Task<List<PurchaseBatchItem>> GetVariantsForSelection()
        {
            try
            {
                string query = @"
                    SELECT 
                        pv.variant_id,
                        p.name as product_name,
                        pv.size,
                        pv.class_type,
                        pv.price_per_unit as sale_price,
                        pv.quantity_in_stock,
                        0 as quantity_received,
                        0 as cost_price,
                        0 as line_total,
                        0 as purchase_batch_id,
                        0 as purchase_batch_item_id
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.is_active = TRUE AND p.is_active = TRUE
                    ORDER BY p.name, pv.size;";

                return await Task.Run(() =>
                {
                    var items = new List<PurchaseBatchItem>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            items.Add(new PurchaseBatchItem
                            {
                                variant_id = reader.GetInt32("variant_id"),
                                product_name = reader.GetString("product_name"),
                                size = reader.IsDBNull(reader.GetOrdinal("size")) ? "Standard" : reader.GetString("size"),
                                class_type = reader.IsDBNull(reader.GetOrdinal("class_type")) ? null : reader.GetString("class_type"),
                                sale_price = reader.GetDecimal("sale_price"),
                                quantity_received = 0,
                                cost_price = 0,
                                line_total = 0
                            });
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variants: {ex.Message}");
                throw;
            }
        }

        // Search variants for selection
        public async Task<List<PurchaseBatchItem>> SearchVariantsForSelection(string keyword)
        {
            try
            {
                string query = @"
                    SELECT 
                        pv.variant_id,
                        p.name as product_name,
                        pv.size,
                        pv.class_type,
                        pv.price_per_unit as sale_price,
                        pv.quantity_in_stock,
                        0 as quantity_received,
                        0 as cost_price,
                        0 as line_total,
                        0 as purchase_batch_id,
                        0 as purchase_batch_item_id
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE (p.name LIKE @keyword OR pv.size LIKE @keyword OR pv.class_type LIKE @keyword)
                    AND pv.is_active = TRUE AND p.is_active = TRUE
                    ORDER BY p.name, pv.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var items = new List<PurchaseBatchItem>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            items.Add(new PurchaseBatchItem
                            {
                                variant_id = reader.GetInt32("variant_id"),
                                product_name = reader.GetString("product_name"),
                                size = reader.IsDBNull(reader.GetOrdinal("size")) ? "Standard" : reader.GetString("size"),
                                class_type = reader.IsDBNull(reader.GetOrdinal("class_type")) ? null : reader.GetString("class_type"),
                                sale_price = reader.GetDecimal("sale_price"),
                                quantity_received = 0,
                                cost_price = 0,
                                line_total = 0
                            });
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching variants: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}