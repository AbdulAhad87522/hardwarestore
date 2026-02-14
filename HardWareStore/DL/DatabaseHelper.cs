using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace HardWareStore.DL
{
    public sealed class DatabaseHelper
    {
        // Singleton instance
        private static readonly Lazy<DatabaseHelper> _instance = new Lazy<DatabaseHelper>(() => new DatabaseHelper());

        // Private constructor
        private DatabaseHelper() { }

        // Public accessor
        public static DatabaseHelper Instance => _instance.Value;

        // Methods
        public MySqlConnection GetConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            return new MySqlConnection(connStr);
        }

        public MySqlDataReader ExecuteReader(string query, MySqlParameter[] parameters = null)
        {
            var conn = GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int GetLastInsertId()
        {
            string query = "SELECT LAST_INSERT_ID();";
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int ExecuteNonQueryTransaction(string query, MySqlParameter[] parameters, MySqlTransaction transaction)
        {
            using (var cmd = new MySqlCommand(query, transaction.Connection, transaction))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                LogCommand(cmd);
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    // Log the command being executed
                    LogCommand(cmd);

                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected: {result}");
                        return result;
                    }
                    catch (MySqlException ex)
                    {
                        LogMySqlError(ex, cmd);
                        throw; // Re-throw to let caller handle
                    }
                }
            }
        }

        private void LogCommand(MySqlCommand cmd)
        {
            Console.WriteLine("Executing command:");
            Console.WriteLine($"SQL: {cmd.CommandText}");
            foreach (MySqlParameter p in cmd.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} = {p.Value} (Type: {p.MySqlDbType})");
            }
        }

        private void LogMySqlError(MySqlException ex, MySqlCommand cmd)
        {
            Console.WriteLine("MySQL Error occurred:");
            Console.WriteLine($"Error Code: {ex.Number}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Command that failed:");
            Console.WriteLine(cmd.CommandText);
            foreach (MySqlParameter p in cmd.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} = {p.Value}");
            }
        }

        // Get lookup ID by type and value
        public int GetLookupId(string type, string value)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT lookup_id FROM lookup WHERE type = @type AND value = @value;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@value", value);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving lookup ID for type '{type}' and value '{value}': " + ex.Message);
            }
        }

        // Get lookup values by type
        public List<string> GetLookupValues(string type, string keyword = "")
        {
            List<string> values = new List<string>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT value 
                        FROM lookup 
                        WHERE type = @type 
                        AND is_active = TRUE 
                        AND value LIKE @keyword
                        ORDER BY display_order, value;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                values.Add(reader.GetString("value"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving lookup values for type '{type}': " + ex.Message);
            }
            return values;
        }

        // Get category ID by name (backward compatibility)
        public int GetCategoryId(string name)
        {
            return GetLookupId("category", name);
        }

        // Get categories (updated to use lookup table)
        public List<string> GetCategories(string keyword = "")
        {
            return GetLookupValues("category", keyword);
        }

        // Get suppliers for searchable combobox
        public List<string> GetSuppliers(string keyword = "")
        {
            List<string> suppliers = new List<string>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT name 
                        FROM supplier 
                        WHERE is_active = TRUE 
                        AND name LIKE @keyword
                        ORDER BY name;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppliers.Add(reader.GetString("name"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving suppliers: " + ex.Message);
            }
            return suppliers;
        }

        // Get supplier ID by name
        public int GetSupplierId(string name)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT supplier_id FROM supplier WHERE name = @name;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving supplier ID: " + ex.Message);
            }
        }

        // Get customer ID by name
        public int GetCustomerId(string name)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT customer_id FROM customer WHERE name = @name;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name.Trim());
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customer ID: " + ex.Message);
            }
        }

        // Get all customers for searchable combobox
        public List<string> GetCustomers(string keyword = "")
        {
            var customers = new List<string>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT name 
                        FROM customer 
                        WHERE is_active = TRUE 
                        AND name LIKE @keyword
                        ORDER BY name;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                customers.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customers: " + ex.Message);
            }
            return customers;
        }

        // Get products for searchable combobox
        public List<string> GetProducts(string keyword = "")
        {
            List<string> products = new List<string>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT name 
                        FROM products 
                        WHERE is_active = TRUE 
                        AND name LIKE @keyword
                        ORDER BY name;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(reader.GetString("name"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products: " + ex.Message);
            }
            return products;
        }

        // Get product ID by name
        public int GetProductId(string name)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT product_id FROM products WHERE name = @name;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving product ID: " + ex.Message);
            }
        }

        public DataTable ExecuteDataTable(string query, MySqlParameter[] parameters = null)
        {
            var dt = new DataTable();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public object ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}