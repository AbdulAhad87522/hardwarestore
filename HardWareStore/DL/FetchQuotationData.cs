using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardWareStore.Models;
using MySql.Data.MySqlClient;

namespace HardWareStore.DL
{
    internal class FetchQuotationDataclass
    {
        private QuotationData FetchQuotationData(string searchValue, bool isNumericId)
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
    }
}
