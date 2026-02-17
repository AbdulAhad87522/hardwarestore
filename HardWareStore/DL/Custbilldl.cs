using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardWareStore.Interfaces;
using HardWareStore.Models;
using MySql.Data.MySqlClient;

namespace HardWareStore.DL
{
    internal class Custbilldl : Icustomerbilldl 
    {
        public List<custbill> GetCustomerBills(string text)
        {
            List<custbill> customerBills = new List<custbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT b.customer_id, 
                                            c.full_name, 
                                            SUM(b.total_amount) AS total_amount, 
                                            SUM(b.amount_paid) AS paid, 
                                            (SUM(b.total_amount) - SUM(b.amount_paid)) AS remaining
                                     FROM bills b
                                     JOIN customers c ON b.customer_id = c.customer_id
                                     WHERE c.full_name LIKE @search OR b.customer_id LIKE @search
                                     GROUP BY b.customer_id, c.full_name";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{text}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custbill bill = new custbill
                                {
                                    customer_id = reader.GetInt32("customer_id"),
                                    full_name = reader.GetString("full_name"),
                                    total_amount = reader.GetDecimal("total_amount"),
                                    paid = reader.GetDecimal("paid"),
                                    remaining = reader.GetDecimal("remaining")
                                };
                                customerBills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company bills", ex);
            }
            return customerBills;
        }

        public List<custbill> GetCustomerBills(int companyid)
        {
            List<custbill> companyBills = new List<custbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT b.customer_id, 
                                            c.full_name, 
                                            SUM(b.total_amount) AS total_amount, 
                                            SUM(b.amount_paid) AS paid, 
                                            (SUM(b.total_amount) - SUM(b.amount_paid)) AS remaining
                                     FROM bills b
                                     JOIN customers c ON b.customer_id = c.customer_id
                                     WHERE b.customer_id = @search
                                     GROUP BY b.customer_id, c.full_name";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", companyid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custbill bill = new custbill
                                {
                                    customer_id = reader.GetInt32("customer_id"),
                                    full_name = reader.GetString("full_name"),
                                    total_amount = reader.GetDecimal("total_amount"),
                                    paid = reader.GetDecimal("paid"),
                                    remaining = reader.GetDecimal("remaining")
                                };
                                companyBills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company bills", ex);
            }
            return companyBills;
        }

        public bool AddCustomerPayment(int customerid, decimal paymentAmount)
        {
            MySqlTransaction tran = null;
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    // 1. Fetch unpaid bills
                    string selectQuery = @"SELECT bill_id, total_amount, amount_paid
                               FROM bills
                               WHERE customer_id = @customerid 
                               AND (total_amount - amount_paid) > 0
                               ORDER BY bill_date ASC, bill_id ASC";

                    var sales = new List<(int id, decimal total, decimal paid)>();
                    using (var cmd = new MySqlCommand(selectQuery, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@customerid", customerid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sales.Add((
                                    reader.GetInt32("bill_id"),      // ✅ Bug 1 fixed
                                    reader.GetDecimal("total_amount"),
                                    reader.GetDecimal("amount_paid")
                                ));
                            }
                        }
                    }

                    decimal remainingPayment = paymentAmount;

                    // 2. Distribute payment across bills
                    foreach (var sale in sales)
                    {
                        if (remainingPayment <= 0) break;

                        decimal saleRemaining = sale.total - sale.paid;
                        decimal toPay = Math.Min(remainingPayment, saleRemaining);

                        if (toPay > 0)
                        {
                            // Update bill paid amount
                            string updateQuery = @"UPDATE bills 
                                   SET amount_paid = amount_paid + @toPay  
                                   WHERE bill_id = @sale_id";          // ✅ Bug 2 fixed
                            using (var updateCmd = new MySqlCommand(updateQuery, conn, tran))
                            {
                                updateCmd.Parameters.AddWithValue("@toPay", toPay);
                                updateCmd.Parameters.AddWithValue("@sale_id", sale.id);
                                updateCmd.ExecuteNonQuery();
                            }

                            // Record payment
                            string insertPayment = @"INSERT INTO customerpricerecord 
                                   (customer_id, bill_id, date, payment, remarks)  
                                   VALUES (@customerid, @saleid, @date, @amount, @remarks)"; // ✅ Bug 4 fixed
                            using (var cmdInsert = new MySqlCommand(insertPayment, conn, tran))
                            {
                                cmdInsert.Parameters.AddWithValue("@customerid", customerid);
                                cmdInsert.Parameters.AddWithValue("@saleid", sale.id);
                                cmdInsert.Parameters.AddWithValue("@date", DateTime.Now);
                                cmdInsert.Parameters.AddWithValue("@amount", toPay);
                                cmdInsert.Parameters.AddWithValue("@remarks", $"Payment applied to bill #{sale.id}");
                                cmdInsert.ExecuteNonQuery();
                            }

                            remainingPayment -= toPay;
                        }
                    }

                    // 3. Handle overpayment as credit
                    if (remainingPayment > 0)
                    {
                        string overpaymentQuery = @"INSERT INTO customerpricerecord 
                                  (customer_id, bill_id, date, payment, remarks) 
                                  VALUES (@customerid, @billid, @date, @amount, @remarks)";
                        using (var overCmd = new MySqlCommand(overpaymentQuery, conn, tran))
                        {
                            overCmd.Parameters.AddWithValue("@customerid", customerid);
                            overCmd.Parameters.AddWithValue("@billid", DBNull.Value); // ✅ Bug 3 fixed
                            overCmd.Parameters.AddWithValue("@date", DateTime.Now);
                            overCmd.Parameters.AddWithValue("@amount", remainingPayment);
                            overCmd.Parameters.AddWithValue("@remarks", "Credit balance");
                            overCmd.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                tran?.Rollback();
                throw new Exception("Error adding customer payment", ex);
            }
        }
        public List<custPaymentRecord> GetCustomerPaymentRecords(int companyId)
        {
            var records = new List<custPaymentRecord>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    string query = @"
                        SELECT 
                            p.record_id,
                            p.customer_id,
                            p.date,
                            p.payment,
                            c.full_name
                        FROM customerpricerecord p
                        JOIN customers c ON p.customer_id = c.customer_id
                        WHERE p.customer_id = @companyId
                        ORDER BY p.date DESC;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@companyId", companyId);
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var record = new custPaymentRecord
                                {
                                    PaymentId = reader.GetInt32("record_id"),
                                    customerId = reader.GetInt32("customer_id"),
                                    Date = reader.GetDateTime("date"),
                                    Amount = reader.GetDecimal("payment"),
                                    CustomerName = reader.GetString("full_name")
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching payment records", ex);
            }

            return records;
        }

        public List<custPaymentRecord> GetcustPaymentRecords(int companyId)
        {
            var records = new List<custPaymentRecord>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            pr.record_id,
                            pr.customer_id,
                            pr.date,
                            pr.payment,
                            pb.total_amount,
                            pb.paid_amount,
                            (pb.total_amount - pb.paid_amount) AS remaining_balance
                        FROM customerpricerecord pr
                        JOIN sales pb ON pr.customer_id = pb.customer_id
                        WHERE pr.customer_id = @CompanyId
                        ORDER BY pr.date DESC;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var record = new custPaymentRecord
                                {
                                    PaymentId = reader.GetInt32("record_id"),
                                    customerId = reader.GetInt32("customer_id"),
                                    Date = reader.GetDateTime("date"),
                                    Amount = reader.GetDecimal("payment"),
                                    TotalPrice = reader.GetDecimal("total_amount"),
                                    Paid = reader.GetDecimal("paid_amount"),
                                    RemainingBalance = reader.GetDecimal("remaining_balance")
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching payment records: " + ex.Message);
            }

            return records;
        }
        public static List<CustomerSale> GetCustomerSales(int customerId)
        {
            var sales = new List<CustomerSale>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT DISTINCT
                    s.sale_id,
                    s.sale_date,
                    s.total_amount,
                    s.paid_amount,
                    (s.total_amount - s.paid_amount) as remaining_amount,
                    CASE 
                        WHEN s.paid_amount >= s.total_amount THEN 'Paid'
                        WHEN s.paid_amount > 0 THEN 'Partial'
                        ELSE 'Unpaid'
                    END as status,
                    c.full_name as customer_name
                FROM sales s
                JOIN customers c ON s.customer_id = c.customer_id
                WHERE s.customer_id = @CustomerId
                ORDER BY s.sale_date DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sales.Add(new CustomerSale
                                {
                                    SaleId = reader.GetInt32("sale_id"),
                                    SaleDate = reader.GetDateTime("sale_date"),
                                    TotalAmount = reader.GetDecimal("total_amount"),
                                    PaidAmount = reader.GetDecimal("paid_amount"),
                                    RemainingAmount = reader.GetDecimal("remaining_amount"),
                                    Status = reader.GetString("status"),
                                    CustomerName = reader.GetString("customer_name")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading customer sales: " + ex.Message);
            }

            return sales;
        }

        public static List<CustomerSale> SearchCustomerSales(int customerId, string searchText)
        {
            var sales = new List<CustomerSale>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT DISTINCT
                    s.sale_id,
                    s.sale_date,
                    s.total_amount,
                    s.paid_amount,
                    (s.total_amount - s.paid_amount) as remaining_amount,
                    CASE 
                        WHEN s.paid_amount >= s.total_amount THEN 'Paid'
                        WHEN s.paid_amount > 0 THEN 'Partial'
                        ELSE 'Unpaid'
                    END as status,
                    c.full_name as customer_name
                FROM sales s
                JOIN customers c ON s.customer_id = c.customer_id
                WHERE s.customer_id = @CustomerId 
                  AND (s.sale_id LIKE @search OR s.total_amount LIKE @search OR c.full_name LIKE @search)
                ORDER BY s.sale_date DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@search", $"%{searchText}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sales.Add(new CustomerSale
                                {
                                    SaleId = reader.GetInt32("sale_id"),
                                    SaleDate = reader.GetDateTime("sale_date"),
                                    TotalAmount = reader.GetDecimal("total_amount"),
                                    PaidAmount = reader.GetDecimal("paid_amount"),
                                    RemainingAmount = reader.GetDecimal("remaining_amount"),
                                    Status = reader.GetString("status"),
                                    CustomerName = reader.GetString("customer_name")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching customer sales: " + ex.Message);
            }

            return sales;
        }

        // Helper class for customer sales
        public class CustomerSale
        {
            public int SaleId { get; set; }
            public DateTime SaleDate { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal RemainingAmount { get; set; }
            public string Status { get; set; }
            public string CustomerName { get; set; }
        }
    }
}