using HardWareStore.Interfaces;
using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class ProductsDL : IProductsDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;


        // CREATE - Add new product
        public async Task<bool> AddProduct(Products product)
        {
            try
            {
                string query = @"
                    INSERT INTO products (name, category_id, description, supplier_id, has_variants, is_active)
                    VALUES (@name, @category_id, @description, @supplier_id, @has_variants, @is_active);";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@name", product.Name),
                    new MySqlParameter("@category_id", product.category_id),
                    new MySqlParameter("@description", product.description ?? (object)DBNull.Value),
                    new MySqlParameter("@supplier_id", product.supplier_id > 0 ? (object)product.supplier_id : DBNull.Value),
                    new MySqlParameter("@has_variants", product.has_variants),
                    new MySqlParameter("@is_active", product.is_active)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                throw;
            }
        }

        public async Task<Products> GetProductById(int productId)
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    WHERE p.product_id = @product_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId)
                };

                return await Task.Run(() =>
                {
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        if (reader.Read())
                        {
                            return MapProductFromReader(reader);
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product: {ex.Message}");
                throw;
            }
        }

        // READ - Get all products
        public async Task<List<Products>> GetAllProducts()
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name, l.value as category_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    LEFT JOIN lookup l ON p.category_id = l.lookup_id
                    ORDER BY p.name;";

                return await Task.Run(() =>
                {
                    var products = new List<Products>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all products: {ex.Message}");
                throw;
            }
        }

        // READ - Get all active products
        public async Task<List<Products>> GetActiveProducts()
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name, l.value as category_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    LEFT JOIN lookup l ON p.category_id = l.lookup_id
                    WHERE p.is_active = TRUE
                    ORDER BY p.name;";

                return await Task.Run(() =>
                {
                    var products = new List<Products>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active products: {ex.Message}");
                throw;
            }
        }

        // UPDATE - Update existing product
        public async Task<bool> UpdateProduct(Products product)
        {
            try
            {
                string query = @"
                    UPDATE products 
                    SET name = @name,
                        category_id = @category_id,
                        description = @description,
                        supplier_id = @supplier_id,
                        has_variants = @has_variants,
                        is_active = @is_active
                    WHERE product_id = @product_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", product.product_id),
                    new MySqlParameter("@name", product.Name),
                    new MySqlParameter("@category_id", product.category_id),
                    new MySqlParameter("@description", product.description ?? (object)DBNull.Value),
                    new MySqlParameter("@supplier_id", product.supplier_id > 0 ? (object)product.supplier_id : DBNull.Value),
                    new MySqlParameter("@has_variants", product.has_variants),
                    new MySqlParameter("@is_active", product.is_active)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                throw;
            }
        }

        // DELETE - Soft delete product (set is_active to false)
        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                string query = @"
                    UPDATE products 
                    SET is_active = FALSE 
                    WHERE product_id = @product_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                throw;
            }
        }

        // DELETE - Hard delete product (permanent delete)
        public async Task<bool> HardDeleteProduct(int productId)
        {
            try
            {
                string query = @"DELETE FROM products WHERE product_id = @product_id;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId)
                };

                int result = await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hard deleting product: {ex.Message}");
                throw;
            }
        }

        // SEARCH - Search products by keyword
        public async Task<List<Products>> SearchProducts(string keyword)
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name, l.value as category_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    LEFT JOIN lookup l ON p.category_id = l.lookup_id
                    WHERE p.name LIKE @keyword 
                    OR p.description LIKE @keyword
                    OR s.name LIKE @keyword
                    ORDER BY p.name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var products = new List<Products>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching products: {ex.Message}");
                throw;
            }
        }

        // Get products by category
        public async Task<List<Products>> GetProductsByCategory(int categoryId)
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name, l.value as category_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    LEFT JOIN lookup l ON p.category_id = l.lookup_id
                    WHERE p.category_id = @category_id AND p.is_active = TRUE
                    ORDER BY p.name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@category_id", categoryId)
                };

                return await Task.Run(() =>
                {
                    var products = new List<Products>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products by category: {ex.Message}");
                throw;
            }
        }

        // Get products by supplier
        public async Task<List<Products>> GetProductsBySupplier(int supplierId)
        {
            try
            {
                string query = @"
                    SELECT p.*, s.name as supplier_name, l.value as category_name
                    FROM products p
                    LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                    LEFT JOIN lookup l ON p.category_id = l.lookup_id
                    WHERE p.supplier_id = @supplier_id AND p.is_active = TRUE
                    ORDER BY p.name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@supplier_id", supplierId)
                };

                return await Task.Run(() =>
                {
                    var products = new List<Products>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products by supplier: {ex.Message}");
                throw;
            }
        }

        // Helper method to map reader to Product object
        private Products MapProductFromReader(MySqlDataReader reader)
        {
            return new Products
            {
                product_id = reader.GetInt32("product_id"),
                Name = reader.GetString("name"),
                category_id = reader.GetInt32("category_id"),
                description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                supplier_id = reader.IsDBNull(reader.GetOrdinal("supplier_id")) ? 0 : reader.GetInt32("supplier_id"),
                supplier_name = reader.IsDBNull(reader.GetOrdinal("supplier_name")) ? null : reader.GetString("supplier_name"),
                has_variants = reader.GetBoolean("has_variants"),
                is_active = reader.GetBoolean("is_active"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            };
        }


        #region Searchable ComboBox Methods

        // Get product names for searchable combobox
        public async Task<List<string>> GetProductNames(string keyword = "")
        {
            try
            {
                string query = @"
                    SELECT name 
                    FROM products 
                    WHERE is_active = TRUE 
                    AND name LIKE @keyword
                    ORDER BY name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var names = new List<string>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString("name"));
                        }
                    }
                    return names;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product names: {ex.Message}");
                throw;
            }
        }

        // Get product ID by name
        public async Task<int> GetProductIdByName(string productName)
        {
            try
            {
                string query = @"
                    SELECT product_id 
                    FROM products 
                    WHERE name = @name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@name", productName)
                };

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(parameters);
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : -1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product ID: {ex.Message}");
                throw;
            }
        }

        // Get category names for searchable combobox
        public async Task<List<string>> GetCategoryNames(string keyword = "")
        {
            try
            {
                string query = @"
                    SELECT value 
                    FROM lookup 
                    WHERE type = 'category' 
                    AND is_active = TRUE
                    AND value LIKE @keyword
                    ORDER BY display_order, value;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var names = new List<string>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString("value"));
                        }
                    }
                    return names;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting category names: {ex.Message}");
                throw;
            }
        }

        // Get category ID by name
        public async Task<int> GetCategoryIdByName(string categoryName)
        {
            try
            {
                string query = @"
                    SELECT lookup_id 
                    FROM lookup 
                    WHERE type = 'category' AND value = @value;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@value", categoryName)
                };

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(parameters);
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : -1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting category ID: {ex.Message}");
                throw;
            }
        }

        // Get supplier names for searchable combobox
        public async Task<List<string>> GetSupplierNames(string keyword = "")
        {
            try
            {
                string query = @"
                    SELECT name 
                    FROM supplier 
                    WHERE is_active = TRUE 
                    AND name LIKE @keyword
                    ORDER BY name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@keyword", $"%{keyword}%")
                };

                return await Task.Run(() =>
                {
                    var names = new List<string>();
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString("name"));
                        }
                    }
                    return names;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting supplier names: {ex.Message}");
                throw;
            }
        }

        // Get supplier ID by name
        public async Task<int> GetSupplierIdByName(string supplierName)
        {
            try
            {
                string query = @"
                    SELECT supplier_id 
                    FROM supplier 
                    WHERE name = @name;";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@name", supplierName)
                };

                return await Task.Run(() =>
                {
                    using (var conn = _db.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(parameters);
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : -1;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting supplier ID: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}