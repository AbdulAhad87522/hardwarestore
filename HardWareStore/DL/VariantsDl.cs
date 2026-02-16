using HardWareStore.Interfaces;
using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class VariantsDL : IVariantsDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        #region Variant CRUD Operations

        // CREATE - Add new variant
        public async Task<bool> AddVariant(Variants variant)
        {
            try
            {
                string query = @"
                    INSERT INTO product_variants 
                    (product_id, size, class_type, unit_of_measure, price_per_unit, 
                     price_per_length, length_in_feet, quantity_in_stock, reorder_level, 
                     minimum_order_qty, is_active)
                    VALUES 
                    (@product_id, @size, @class_type, @unit_of_measure, @price_per_unit, 
                     @price_per_length, @length_in_feet, @quantity_in_stock, @reorder_level, 
                     @minimum_order_qty, @is_active);";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", variant.product_id),
                    new MySqlParameter("@size", variant.size ?? "Standard"),
                    new MySqlParameter("@class_type", variant.class_type ?? (object)DBNull.Value),
                    new MySqlParameter("@unit_of_measure", variant.unit_of_measure),
                    new MySqlParameter("@price_per_unit", variant.price_per_unit),
                    new MySqlParameter("@price_per_length", variant.price_per_lenght),
                    new MySqlParameter("@length_in_feet", variant.lenght_in_feet),
                    new MySqlParameter("@quantity_in_stock", variant.quantity_in_stock),
                    new MySqlParameter("@reorder_level", variant.reorder_level),
                    new MySqlParameter("@minimum_order_qty", variant.minimum_order_quantity),
                    new MySqlParameter("@is_active", variant.is_active)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding variant: {ex.Message}");
                throw;
            }
        }

        // READ - Get variant by ID
        public async Task<Variants> GetVariantById(int variantId)
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variantId)
                };

                return await Task.Run(() =>
                {
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        if (reader.Read())
                        {
                            return MapVariantFromReader(reader);
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant: {ex.Message}");
                throw;
            }
        }

        // READ - Get all variants
        public async Task<List<Variants>> GetAllVariants()
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    ORDER BY p.name, pv.size;";

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all variants: {ex.Message}");
                throw;
            }
        }

        // READ - Get all active variants
        public async Task<List<Variants>> GetActiveVariants()
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.is_active = TRUE AND p.is_active = TRUE
                    ORDER BY p.name, pv.size;";

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active variants: {ex.Message}");
                throw;
            }
        }

        // READ - Get variants by product ID
        public async Task<List<Variants>> GetVariantsByProductId(int productId)
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.product_id = @product_id
                    ORDER BY pv.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId)
                };

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variants by product: {ex.Message}");
                throw;
            }
        }

        // UPDATE - Update existing variant
        public async Task<bool> UpdateVariant(Variants variant)
        {
            try
            {
                string query = @"
                    UPDATE product_variants 
                    SET product_id = @product_id,
                        size = @size,
                        class_type = @class_type,
                        unit_of_measure = @unit_of_measure,
                        price_per_unit = @price_per_unit,
                        price_per_length = @price_per_length,
                        length_in_feet = @length_in_feet,
                        quantity_in_stock = @quantity_in_stock,
                        reorder_level = @reorder_level,
                        minimum_order_qty = @minimum_order_qty,
                        is_active = @is_active
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variant.variant_id),
                    new MySqlParameter("@product_id", variant.product_id),
                    new MySqlParameter("@size", variant.size ?? "Standard"),
                    new MySqlParameter("@class_type", variant.class_type ?? (object)DBNull.Value),
                    new MySqlParameter("@unit_of_measure", variant.unit_of_measure),
                    new MySqlParameter("@price_per_unit", variant.price_per_unit),
                    new MySqlParameter("@price_per_length", variant.price_per_lenght),
                    new MySqlParameter("@length_in_feet", variant.lenght_in_feet),
                    new MySqlParameter("@quantity_in_stock", variant.quantity_in_stock),
                    new MySqlParameter("@reorder_level", variant.reorder_level),
                    new MySqlParameter("@minimum_order_qty", variant.minimum_order_quantity),
                    new MySqlParameter("@is_active", variant.is_active)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating variant: {ex.Message}");
                throw;
            }
        }

        // UPDATE - Update stock quantity
        public async Task<bool> UpdateStock(int variantId, decimal newQuantity)
        {
            try
            {
                string query = @"
                    UPDATE product_variants 
                    SET quantity_in_stock = @quantity
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variantId),
                    new MySqlParameter("@quantity", newQuantity)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                throw;
            }
        }

        // UPDATE - Adjust stock (add or subtract)
        public async Task<bool> AdjustStock(int variantId, decimal quantityChange)
        {
            try
            {
                string query = @"
                    UPDATE product_variants 
                    SET quantity_in_stock = quantity_in_stock + @quantity_change
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variantId),
                    new MySqlParameter("@quantity_change", quantityChange)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adjusting stock: {ex.Message}");
                throw;
            }
        }

        // DELETE - Soft delete variant (set is_active to false)
        public async Task<bool> DeleteVariant(int variantId)
        {
            try
            {
                string query = @"
                    UPDATE product_variants 
                    SET is_active = FALSE 
                    WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variantId)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting variant: {ex.Message}");
                throw;
            }
        }

        // DELETE - Hard delete variant (permanent delete)
        public async Task<bool> HardDeleteVariant(int variantId)
        {
            try
            {
                string query = @"DELETE FROM product_variants WHERE variant_id = @variant_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@variant_id", variantId)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hard deleting variant: {ex.Message}");
                throw;
            }
        }

        // SEARCH - Search variants by keyword
        public async Task<List<Variants>> SearchVariants(string keyword)
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE p.name LIKE @keyword 
                    OR pv.size LIKE @keyword
                    OR pv.class_type LIKE @keyword
                    ORDER BY p.name, pv.size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching variants: {ex.Message}");
                throw;
            }
        }

        // Get low stock variants
        public async Task<List<Variants>> GetLowStockVariants()
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.quantity_in_stock <= pv.reorder_level
                    AND pv.is_active = TRUE AND p.is_active = TRUE
                    ORDER BY (pv.reorder_level - pv.quantity_in_stock) DESC;";

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting low stock variants: {ex.Message}");
                throw;
            }
        }

        // Get out of stock variants
        public async Task<List<Variants>> GetOutOfStockVariants()
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.quantity_in_stock = 0
                    AND pv.is_active = TRUE AND p.is_active = TRUE
                    ORDER BY p.name, pv.size;";

                return await Task.Run(() =>
                {
                    var variants = new List<Variants>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            variants.Add(MapVariantFromReader(reader));
                        }
                    }
                    return variants;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting out of stock variants: {ex.Message}");
                throw;
            }
        }

        // Helper method to map reader to Variant object
        private Variants MapVariantFromReader(MySqlDataReader reader)
        {
            return new Variants
            {
                variant_id = reader.GetInt32("variant_id"),
                product_id = reader.GetInt32("product_id"),
                product_name = reader.GetString("product_name"),
                size = reader.IsDBNull(reader.GetOrdinal("size")) ? "Standard" : reader.GetString("size"),
                class_type = reader.IsDBNull(reader.GetOrdinal("class_type")) ? null : reader.GetString("class_type"),
                unit_of_measure = reader.GetString("unit_of_measure"),
                price_per_unit = reader.GetDecimal("price_per_unit"),
                price_per_lenght = reader.GetDecimal("price_per_length"),
                lenght_in_feet = reader.GetDecimal("length_in_feet"),
                quantity_in_stock = reader.GetDecimal("quantity_in_stock"),
                reorder_level = reader.GetDecimal("reorder_level"),
                minimum_order_quantity = reader.GetDecimal("minimum_order_qty"),
                is_active = reader.GetBoolean("is_active"),
                CeratedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            };
        }

        #endregion

        #region Searchable ComboBox Methods

        // Get variant display strings for searchable combobox (ProductName - Size - Class)
        public async Task<List<string>> GetVariantDisplayStrings(string keyword = "", int? productId = null)
        {
            try
            {
                string query = @"
                    SELECT CONCAT(p.name, ' - ', pv.size, 
                           CASE WHEN pv.class_type IS NOT NULL 
                                THEN CONCAT(' - ', pv.class_type) 
                                ELSE '' END) as display_string
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.is_active = TRUE AND p.is_active = TRUE
                    AND (CONCAT(p.name, ' - ', pv.size, 
                         CASE WHEN pv.class_type IS NOT NULL 
                              THEN CONCAT(' - ', pv.class_type) 
                              ELSE '' END) LIKE @keyword)";

                if (productId.HasValue)
                {
                    query += " AND pv.product_id = @product_id";
                }

                query += " ORDER BY p.name, pv.size;";

                var paramList = new List<MySqlParameter>
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                if (productId.HasValue)
                {
                    paramList.Add(new MySqlParameter("@product_id", productId.Value));
                }

                return await Task.Run(() =>
                {
                    var displayStrings = new List<string>();
                    using (var reader = _db.ExecuteReader(query, paramList.ToArray()))
                    {
                        while (reader.Read())
                        {
                            displayStrings.Add(reader.GetString("display_string"));
                        }
                    }
                    return displayStrings;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant display strings: {ex.Message}");
                throw;
            }
        }

        // Get variant ID by display string
        public async Task<int> GetVariantIdByDisplayString(string displayString)
        {
            try
            {
                // Parse display string: "ProductName - Size - Class" or "ProductName - Size"
                string[] parts = displayString.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length < 2)
                    return -1;

                string productName = parts[0].Trim();
                string size = parts[1].Trim();
                string classType = parts.Length > 2 ? parts[2].Trim() : null;

                string query = @"
                    SELECT pv.variant_id
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE p.name = @product_name 
                    AND pv.size = @size";

                var paramList = new List<MySqlParameter>
                {
                    new MySqlParameter("@product_name", productName),
                    new MySqlParameter("@size", size)
                };

                if (!string.IsNullOrEmpty(classType))
                {
                    query += " AND pv.class_type = @class_type";
                    paramList.Add(new MySqlParameter("@class_type", classType));
                }
                else
                {
                    query += " AND pv.class_type IS NULL";
                }

                query += ";";

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(paramList.ToArray());
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : -1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant ID by display string: {ex.Message}");
                throw;
            }
        }

        // Get sizes for a specific product
        public async Task<List<string>> GetSizesByProductId(int productId, string keyword = "")
        {
            try
            {
                string query = @"
                    SELECT DISTINCT size
                    FROM product_variants
                    WHERE product_id = @product_id 
                    AND is_active = TRUE
                    AND size LIKE @keyword
                    ORDER BY size;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId),
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var sizes = new List<string>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            sizes.Add(reader.GetString("size"));
                        }
                    }
                    return sizes;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting sizes: {ex.Message}");
                throw;
            }
        }

        // Get variant by product ID and size
        public async Task<Variants> GetVariantByProductAndSize(int productId, string size, string classType = null)
        {
            try
            {
                string query = @"
                    SELECT pv.*, p.name as product_name
                    FROM product_variants pv
                    INNER JOIN products p ON pv.product_id = p.product_id
                    WHERE pv.product_id = @product_id 
                    AND pv.size = @size";

                var paramList = new List<MySqlParameter>
                {
                    new MySqlParameter("@product_id", productId),
                    new MySqlParameter("@size", size)
                };

                if (!string.IsNullOrEmpty(classType))
                {
                    query += " AND pv.class_type = @class_type";
                    paramList.Add(new MySqlParameter("@class_type", classType));
                }
                else
                {
                    query += " AND pv.class_type IS NULL";
                }

                query += ";";

                return await Task.Run(() =>
                {
                    using (var reader = _db.ExecuteReader(query, paramList.ToArray()))
                    {
                        if (reader.Read())
                        {
                            return MapVariantFromReader(reader);
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant by product and size: {ex.Message}");
                throw;
            }
        }

        // Get unit of measures for dropdown
        public List<string> GetUnitOfMeasures()
        {
            return new List<string>
            {
                "FT", "LENGTH", "PCS", "MTR", "PACK",
                "UNIT", "BOTTLE", "BOX", "KG", "LITER"
            };
        }

        #endregion
    }
}