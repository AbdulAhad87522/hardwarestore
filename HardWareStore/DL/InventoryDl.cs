using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class InventoryDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        // Get all inventory items (products with their variants)
        public async Task<List<InventoryResponse>> GetAllInventory()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.is_active = TRUE AND v.is_active = TRUE
                    ORDER BY p.name, v.size;";

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all inventory: {ex.Message}");
                throw;
            }
        }

        // Get active inventory items only
        public async Task<List<InventoryResponse>> GetActiveInventory()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.is_active = TRUE AND v.is_active = TRUE
                    ORDER BY p.name, v.size;";

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active inventory: {ex.Message}");
                throw;
            }
        }

        // Get low stock inventory items
        public async Task<List<InventoryResponse>> GetLowStockInventory()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.is_active = TRUE 
                    AND v.is_active = TRUE
                    AND v.quantity_in_stock <= v.reorder_level
                    ORDER BY (v.reorder_level - v.quantity_in_stock) DESC, p.name, v.size;";

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting low stock inventory: {ex.Message}");
                throw;
            }
        }

        // Get out of stock inventory items
        public async Task<List<InventoryResponse>> GetOutOfStockInventory()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.is_active = TRUE 
                    AND v.is_active = TRUE
                    AND v.quantity_in_stock = 0
                    ORDER BY p.name, v.size;";

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting out of stock inventory: {ex.Message}");
                throw;
            }
        }

        // Search inventory by keyword
        public async Task<List<InventoryResponse>> SearchInventory(string keyword)
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE (p.name LIKE @keyword 
                        OR p.description LIKE @keyword
                        OR s.name LIKE @keyword
                        OR v.size LIKE @keyword
                        OR v.class_type LIKE @keyword)
                    AND p.is_active = TRUE AND v.is_active = TRUE
                    ORDER BY p.name, v.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching inventory: {ex.Message}");
                throw;
            }
        }

        // Get inventory by supplier
        public async Task<List<InventoryResponse>> GetInventoryBySupplier(int supplierId)
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.supplier_id = @supplier_id
                    AND p.is_active = TRUE AND v.is_active = TRUE
                    ORDER BY p.name, v.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@supplier_id", supplierId)
                };

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting inventory by supplier: {ex.Message}");
                throw;
            }
        }

        // Get inventory by category
        public async Task<List<InventoryResponse>> GetInventoryByCategory(int categoryId)
        {
            try
            {
                string query = @"
                    SELECT 
                        p.name as product_name,
                        p.description,
                        p.is_active,
                        s.name as supplier_name,
                        v.size,
                        v.class_type,
                        v.unit_of_measure,
                        v.price_per_unit,
                        v.price_per_length,
                        v.length_in_feet,
                        v.quantity_in_stock,
                        v.reorder_level,
                        v.minimum_order_qty
                    FROM products p
                    INNER JOIN product_variants v ON p.product_id = v.product_id
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.category_id = @category_id
                    AND p.is_active = TRUE AND v.is_active = TRUE
                    ORDER BY p.name, v.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@category_id", categoryId)
                };

                return await Task.Run(() =>
                {
                    var items = new List<InventoryResponse>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            items.Add(MapInventoryFromReader(reader));
                        }
                    }
                    return items;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting inventory by category: {ex.Message}");
                throw;
            }
        }

        // Helper method to map reader to InventoryResponse object
        private InventoryResponse MapInventoryFromReader(MySqlDataReader reader)
        {
            return new InventoryResponse
            {
                product_name = reader.GetString("product_name"),
                description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                is_active = reader.GetBoolean("is_active"),
                supplier_name = reader.IsDBNull(reader.GetOrdinal("supplier_name")) ? "N/A" : reader.GetString("supplier_name"),
                size = reader.IsDBNull(reader.GetOrdinal("size")) ? "Standard" : reader.GetString("size"),
                class_type = reader.IsDBNull(reader.GetOrdinal("class_type")) ? null : reader.GetString("class_type"),
                unit_of_measure = reader.GetString("unit_of_measure"),
                price_per_unit = reader.GetDecimal("price_per_unit"),
                price_per_lenght = reader.GetDecimal("price_per_length"),
                lenght_in_feet = reader.GetDecimal("length_in_feet"),
                quantity_in_stock = reader.GetDecimal("quantity_in_stock"),
                reorder_level = reader.GetDecimal("reorder_level"),
                minimum_order_quantity = reader.GetDecimal("minimum_order_qty")
            };
        }
    }
}