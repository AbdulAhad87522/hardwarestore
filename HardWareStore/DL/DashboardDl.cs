using HardWareStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardWareStore.DL
{
    public class DashboardDL
    {
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;

        public async Task<DashboardStats> GetDashboardStats()
        {
            var stats = new DashboardStats();

            await Task.Run(() =>
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    // Today's Revenue
                    string todayQuery = @"
                        SELECT COALESCE(SUM(total_amount), 0) as revenue, COUNT(*) as count
                        FROM bills 
                        WHERE DATE(bill_date) = CURDATE();";
                    using (var cmd = new MySqlCommand(todayQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TodayRevenue = reader.GetDecimal("revenue");
                            stats.TodayBills = reader.GetInt32("count");
                        }
                    }

                    // Week Revenue
                    string weekQuery = @"
                        SELECT COALESCE(SUM(total_amount), 0) as revenue, COUNT(*) as count
                        FROM bills 
                        WHERE YEARWEEK(bill_date, 1) = YEARWEEK(CURDATE(), 1);";
                    using (var cmd = new MySqlCommand(weekQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.WeekRevenue = reader.GetDecimal("revenue");
                            stats.WeekBills = reader.GetInt32("count");
                        }
                    }

                    // Month Revenue
                    string monthQuery = @"
                        SELECT COALESCE(SUM(total_amount), 0) as revenue, COUNT(*) as count
                        FROM bills 
                        WHERE MONTH(bill_date) = MONTH(CURDATE()) 
                        AND YEAR(bill_date) = YEAR(CURDATE());";
                    using (var cmd = new MySqlCommand(monthQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.MonthRevenue = reader.GetDecimal("revenue");
                            stats.MonthBills = reader.GetInt32("count");
                        }
                    }

                    // Total Revenue & Bills
                    string totalQuery = @"
                        SELECT COALESCE(SUM(total_amount), 0) as revenue, COUNT(*) as count 
                        FROM bills;";
                    using (var cmd = new MySqlCommand(totalQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalRevenue = reader.GetDecimal("revenue");
                            stats.TotalBills = reader.GetInt32("count");
                        }
                    }

                    // Pending Bills
                    // ✅ lookup values are 'Pending' and 'Partial' (capital P)
                    string pendingQuery = @"
                        SELECT COUNT(*) as count 
                        FROM bills b
                        JOIN lookup l ON b.payment_status_id = l.lookup_id
                        WHERE l.value IN ('Pending', 'Partial');";
                    using (var cmd = new MySqlCommand(pendingQuery, conn))
                    {
                        stats.PendingBills = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Customers
                    // ✅ column is 'full_name' not 'name', no 'walkin' type
                    string customersQuery = @"
                        SELECT 
                            COUNT(*) as total,
                            SUM(CASE WHEN is_active = 1 THEN 1 ELSE 0 END) as active,
                            COALESCE(SUM(current_balance), 0) as outstanding
                        FROM customers
                        WHERE customer_type != 'walkin';";
                    using (var cmd = new MySqlCommand(customersQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalCustomers = reader.GetInt32("total");
                            stats.ActiveCustomers = reader.GetInt32("active");
                            stats.TotalOutstanding = reader.GetDecimal("outstanding");
                        }
                    }

                    // Products
                    string productsQuery = @"
                        SELECT COUNT(DISTINCT p.product_id) as total_products
                        FROM products p WHERE p.is_active = 1;";
                    using (var cmd = new MySqlCommand(productsQuery, conn))
                    {
                        stats.TotalProducts = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Stock
                    string stockQuery = @"
                        SELECT 
                            SUM(CASE WHEN quantity_in_stock <= reorder_level 
                                     AND quantity_in_stock > 0 THEN 1 ELSE 0 END) as low_stock,
                            SUM(CASE WHEN quantity_in_stock = 0 THEN 1 ELSE 0 END) as out_of_stock,
                            COALESCE(SUM(quantity_in_stock * price_per_unit), 0) as stock_value
                        FROM product_variants WHERE is_active = 1;";
                    using (var cmd = new MySqlCommand(stockQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.LowStockProducts = reader.GetInt32("low_stock");
                            stats.OutOfStockProducts = reader.GetInt32("out_of_stock");
                            stats.TotalStockValue = reader.GetDecimal("stock_value");
                        }
                    }

                    // Suppliers
                    // ✅ purchase_batches columns: total_price, paid, supplier_id
                    string suppliersQuery = @"
                        SELECT 
                            COUNT(DISTINCT s.supplier_id) as total,
                            COALESCE(SUM(pb.total_price - pb.paid), 0) as pending,
                            COALESCE(SUM(pb.paid), 0) as paid
                        FROM supplier s
                        LEFT JOIN purchase_batches pb ON s.supplier_id = pb.supplier_id
                        WHERE s.is_active = 1;";
                    using (var cmd = new MySqlCommand(suppliersQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalSuppliers = reader.GetInt32("total");
                            stats.SuppliersPending = reader.GetDecimal("pending");
                            stats.SuppliersPaid = reader.GetDecimal("paid");
                        }
                    }

                    // Quotations
                    // ✅ quotation_status values: Draft, Sent, Accepted, Rejected, Converted, Expired
                    // 'Draft' is the equivalent of pending (newly created)
                    string quotationsQuery = @"
                        SELECT 
                            COUNT(*) as total,
                            SUM(CASE WHEN l.value IN ('Draft', 'Sent') THEN 1 ELSE 0 END) as pending,
                            COALESCE(SUM(q.total_amount), 0) as value
                        FROM quotations q
                        JOIN lookup l ON q.status_id = l.lookup_id;";
                    using (var cmd = new MySqlCommand(quotationsQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalQuotations = reader.GetInt32("total");
                            stats.PendingQuotations = reader.GetInt32("pending");
                            stats.QuotationsValue = reader.GetDecimal("value");
                        }
                    }

                    // Estimated Profit
                    string profitQuery = @"
                        SELECT 
                            (SELECT COALESCE(SUM(total_amount), 0) FROM bills) -
                            (SELECT COALESCE(SUM(total_price), 0) FROM purchase_batches) as profit;";
                    using (var cmd = new MySqlCommand(profitQuery, conn))
                    {
                        var profit = cmd.ExecuteScalar();
                        if (profit != DBNull.Value)
                        {
                            stats.EstimatedProfit = Convert.ToDecimal(profit);
                            if (stats.TotalRevenue > 0)
                                stats.ProfitMargin = (stats.EstimatedProfit / stats.TotalRevenue) * 100;
                        }
                    }
                }
            });

            return stats;
        }

        public async Task<List<TopProduct>> GetTopProducts(int limit = 10)
        {
            var products = new List<TopProduct>();

            string query = @"
                SELECT 
                    p.name as ProductName,
                    pv.size as Size,
                    COUNT(bi.bill_item_id) as TotalSales,
                    SUM(bi.quantity) as QuantitySold,
                    SUM(bi.line_total) as Revenue
                FROM bill_items bi
                JOIN products p ON bi.product_id = p.product_id
                JOIN product_variants pv ON bi.variant_id = pv.variant_id
                GROUP BY p.product_id, pv.variant_id, p.name, pv.size
                ORDER BY Revenue DESC
                LIMIT @limit;";

            await Task.Run(() =>
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@limit", limit)
                };

                using (var reader = _db.ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        products.Add(new TopProduct
                        {
                            ProductName = reader.GetString("ProductName"),
                            Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? "Standard" : reader.GetString("Size"),
                            TotalSales = Convert.ToInt32(reader["TotalSales"]),
                            QuantitySold = Convert.ToInt32(reader["QuantitySold"]),
                            Revenue = reader.GetDecimal("Revenue")
                        });
                    }
                }
            });

            return products;
        }

        public async Task<List<RecentBill>> GetRecentBills(int limit = 10)
        {
            var bills = new List<RecentBill>();

            // ✅ customers column is 'full_name' not 'name'
            string query = @"
                SELECT 
                    b.bill_number as BillNumber,
                    COALESCE(c.full_name, 'Walk-in') as CustomerName,
                    b.bill_date as BillDate,
                    b.total_amount as TotalAmount,
                    l.value as PaymentStatus,
                    b.amount_due as AmountDue
                FROM bills b
                LEFT JOIN customers c ON b.customer_id = c.customer_id
                JOIN lookup l ON b.payment_status_id = l.lookup_id
                ORDER BY b.bill_date DESC
                LIMIT @limit;";

            await Task.Run(() =>
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@limit", limit)
                };

                using (var reader = _db.ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        bills.Add(new RecentBill
                        {
                            BillNumber = reader.GetString("BillNumber"),
                            CustomerName = reader.GetString("CustomerName"),
                            BillDate = reader.GetDateTime("BillDate"),
                            TotalAmount = reader.GetDecimal("TotalAmount"),
                            PaymentStatus = reader.GetString("PaymentStatus"),
                            AmountDue = reader.GetDecimal("AmountDue")
                        });
                    }
                }
            });

            return bills;
        }

        public async Task<List<LowStockItem>> GetLowStockItems(int limit = 10)
        {
            var items = new List<LowStockItem>();

            string query = @"
                SELECT 
                    p.name as ProductName,
                    pv.size as Size,
                    pv.quantity_in_stock as CurrentStock,
                    pv.reorder_level as ReorderLevel,
                    COALESCE(s.name, 'No Supplier') as SupplierName
                FROM product_variants pv
                JOIN products p ON pv.product_id = p.product_id
                LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                WHERE pv.quantity_in_stock <= pv.reorder_level
                AND pv.is_active = 1 AND p.is_active = 1
                ORDER BY (pv.reorder_level - pv.quantity_in_stock) DESC
                LIMIT @limit;";

            await Task.Run(() =>
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@limit", limit)
                };

                using (var reader = _db.ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        items.Add(new LowStockItem
                        {
                            ProductName = reader.GetString("ProductName"),
                            Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? "Standard" : reader.GetString("Size"),
                            CurrentStock = reader.GetDecimal("CurrentStock"),
                            ReorderLevel = reader.GetDecimal("ReorderLevel"),
                            SupplierName = reader.GetString("SupplierName")
                        });
                    }
                }
            });

            return items;
        }

        public async Task<List<SalesChartData>> GetSalesChartData(int days = 30)
        {
            var data = new List<SalesChartData>();

            string query = @"
                SELECT 
                    DATE(bill_date) as Period,
                    COALESCE(SUM(total_amount), 0) as Sales,
                    COUNT(*) as BillCount
                FROM bills
                WHERE bill_date >= DATE_SUB(CURDATE(), INTERVAL @days DAY)
                GROUP BY DATE(bill_date)
                ORDER BY Period;";

            await Task.Run(() =>
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@days", days)
                };

                using (var reader = _db.ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        data.Add(new SalesChartData
                        {
                            Period = reader.GetDateTime("Period").ToString("MMM dd"),
                            Sales = reader.GetDecimal("Sales"),
                            BillCount = reader.GetInt32("BillCount")
                        });
                    }
                }
            });

            return data;
        }

        public async Task<List<CategorySales>> GetCategorySales()
        {
            var categories = new List<CategorySales>();

            string query = @"
                SELECT 
                    l.value as CategoryName,
                    COALESCE(SUM(bi.line_total), 0) as TotalSales,
                    COUNT(DISTINCT bi.bill_item_id) as ItemCount
                FROM lookup l
                JOIN products p ON l.lookup_id = p.category_id
                JOIN bill_items bi ON p.product_id = bi.product_id
                WHERE l.type = 'category'
                GROUP BY l.lookup_id, l.value
                ORDER BY TotalSales DESC;";

            decimal totalSales = 0;

            await Task.Run(() =>
            {
                using (var reader = _db.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var sales = reader.GetDecimal("TotalSales");
                        totalSales += sales;

                        categories.Add(new CategorySales
                        {
                            CategoryName = reader.GetString("CategoryName"),
                            TotalSales = sales,
                            ItemCount = reader.GetInt32("ItemCount"),
                            Percentage = 0
                        });
                    }
                }
            });

            if (totalSales > 0)
                foreach (var cat in categories)
                    cat.Percentage = (double)(cat.TotalSales / totalSales * 100);

            return categories;
        }

        // ✅ REPLACED GetPaymentMethodStats - bills table no longer has payment_method column
        // Now groups by payment status instead
        public async Task<List<PaymentMethodStats>> GetPaymentMethodStats()
        {
            var methods = new List<PaymentMethodStats>();

            string query = @"
                SELECT 
                    l.value as PaymentMethod,
                    COUNT(*) as Count,
                    COALESCE(SUM(b.total_amount), 0) as Amount
                FROM bills b
                JOIN lookup l ON b.payment_status_id = l.lookup_id
                GROUP BY l.lookup_id, l.value
                ORDER BY Amount DESC;";

            decimal totalAmount = 0;

            await Task.Run(() =>
            {
                using (var reader = _db.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var amount = reader.GetDecimal("Amount");
                        totalAmount += amount;

                        methods.Add(new PaymentMethodStats
                        {
                            PaymentMethod = reader.GetString("PaymentMethod"),
                            Count = reader.GetInt32("Count"),
                            Amount = amount,
                            Percentage = 0
                        });
                    }
                }
            });

            if (totalAmount > 0)
                foreach (var method in methods)
                    method.Percentage = (double)(method.Amount / totalAmount * 100);

            return methods;
        }
    }
}