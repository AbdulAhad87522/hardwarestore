using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HardWareStore.Models;
using MySql.Data.MySqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
                                infoCol.Item().Text($"quotation #: INV-{DateTime.Now:yyMMddHHmm}");
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

    }
}
