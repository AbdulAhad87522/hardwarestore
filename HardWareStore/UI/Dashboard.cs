using HardWareStore.DL;
using HardWareStore.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class Dashboard : Form
    {
        private readonly DashboardDL _dashboardDL = new DashboardDL();

        public Dashboard()
        {
            InitializeComponent();
        }

        private async void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDashboardData()
        {
            mainPanel.Controls.Clear();

            // Calculate panel width (minus scrollbar and padding)
            int panelWidth = mainPanel.ClientSize.Width - 30;
            int yPosition = 0;

            // Load data
            var stats = await _dashboardDL.GetDashboardStats();
            var recentBills = await _dashboardDL.GetRecentBills(10);
            var lowStock = await _dashboardDL.GetLowStockItems(10);
            var salesData = await _dashboardDL.GetSalesChartData(30);
            var categorySales = await _dashboardDL.GetCategorySales();
            var paymentMethods = await _dashboardDL.GetPaymentMethodStats();

            // Header
            var header = CreateHeader(panelWidth);
            header.Location = new Point(0, yPosition);
            mainPanel.Controls.Add(header);
            yPosition += header.Height + 15;

            // Statistics Cards (3 rows x 5 cards = 15 cards)
            var cardsPanel = CreateStatisticsCards(stats, panelWidth);
            cardsPanel.Location = new Point(0, yPosition);
            mainPanel.Controls.Add(cardsPanel);
            yPosition += cardsPanel.Height + 15;

            // Charts Row 1: Sales Trend + Category Sales
            var chartsRow1 = CreateChartsRow1(salesData, categorySales, panelWidth);
            chartsRow1.Location = new Point(0, yPosition);
            mainPanel.Controls.Add(chartsRow1);
            yPosition += chartsRow1.Height + 15;

            // Charts Row 2: Payment Methods
            var chartsRow2 = CreateChartsRow2(paymentMethods, panelWidth);
            chartsRow2.Location = new Point(0, yPosition);
            mainPanel.Controls.Add(chartsRow2);
            yPosition += chartsRow2.Height + 15;

            // Main Grids Row: Recent Bills + Low Stock
            var gridsPanel = CreateGridsSection(recentBills, lowStock, panelWidth);
            gridsPanel.Location = new Point(0, yPosition);
            mainPanel.Controls.Add(gridsPanel);
        }

        private Panel CreateHeader(int width)
        {
            var header = new Panel
            {
                Size = new Size(width, 80),
                BackColor = Color.FromArgb(20, 25, 72)
            };

            var titleLabel = new Label
            {
                Text = "📊 Bismillah Hardware Dashboard",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var dateLabel = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy  •  hh:mm tt"),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(200, 200, 200),
                AutoSize = true,
                Location = new Point(20, 45)
            };

            var refreshBtn = new Button
            {
                Text = "🔄 Refresh",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Cursor = Cursors.Hand,
                Location = new Point(width - 220, 22)
            };
            refreshBtn.FlatAppearance.BorderSize = 0;
            refreshBtn.Click += async (s, e) => await LoadDashboardData();

            var exportBtn = new Button
            {
                Text = "📥 Export",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Cursor = Cursors.Hand,
                Location = new Point(width - 110, 22)
            };
            exportBtn.FlatAppearance.BorderSize = 0;
            exportBtn.Click += ExportDashboard_Click;

            header.Controls.AddRange(new Control[] { titleLabel, dateLabel, refreshBtn, exportBtn });
            return header;
        }

        private Panel CreateStatisticsCards(DashboardStats stats, int panelWidth)
        {
            // FIXED: Dynamic card width based on panel width
            int cardWidth = (panelWidth - 50) / 5;  // 5 cards per row with proper spacing
            int cardHeight = 110;
            int spacing = 10;
            int cardsPerRow = 5;
            int totalRows = 3;

            var panel = new Panel
            {
                Size = new Size(panelWidth, (cardHeight + spacing) * totalRows - spacing),
                BackColor = Color.Transparent
            };


            var cards = new[]
            {
                // Row 1
                new { Title = "Today Revenue", Value = $"Rs. {stats.TodayRevenue:N0}", Icon = "💰", Color = Color.FromArgb(76, 175, 80), SubText = $"{stats.TodayBills} bills today" },
                new { Title = "Week Revenue", Value = $"Rs. {stats.WeekRevenue:N0}", Icon = "📈", Color = Color.FromArgb(33, 150, 243), SubText = $"{stats.WeekBills} bills" },
                new { Title = "Month Revenue", Value = $"Rs. {stats.MonthRevenue:N0}", Icon = "📊", Color = Color.FromArgb(156, 39, 176), SubText = $"{stats.MonthBills} bills" },
                new { Title = "Total Revenue", Value = $"Rs. {stats.TotalRevenue:N0}", Icon = "💵", Color = Color.FromArgb(255, 152, 0), SubText = "All time" },
                new { Title = "Customers", Value = stats.TotalCustomers.ToString(), Icon = "👥", Color = Color.FromArgb(0, 150, 136), SubText = $"{stats.ActiveCustomers} active" },
                
                // Row 2
                new { Title = "Outstanding", Value = $"Rs. {stats.TotalOutstanding:N0}", Icon = "⚠️", Color = Color.FromArgb(244, 67, 54), SubText = "Receivables" },
                new { Title = "Pending Bills", Value = stats.PendingBills.ToString(), Icon = "📋", Color = Color.FromArgb(255, 87, 34), SubText = "Unpaid" },
                new { Title = "Products", Value = stats.TotalProducts.ToString(), Icon = "📦", Color = Color.FromArgb(63, 81, 181), SubText = "In catalog" },
                new { Title = "Low Stock", Value = stats.LowStockProducts.ToString(), Icon = "⚡", Color = Color.FromArgb(255, 193, 7), SubText = "Reorder soon" },
                new { Title = "Out of Stock", Value = stats.OutOfStockProducts.ToString(), Icon = "❌", Color = Color.FromArgb(233, 30, 99), SubText = "Empty items" },
                
                // Row 3
                new { Title = "Stock Value", Value = $"Rs. {stats.TotalStockValue:N0}", Icon = "💎", Color = Color.FromArgb(0, 188, 212), SubText = "Total inventory" },
                new { Title = "Suppliers", Value = stats.TotalSuppliers.ToString(), Icon = "🏢", Color = Color.FromArgb(121, 85, 72), SubText = "Active vendors" },
                new { Title = "Supplier Dues", Value = $"Rs. {stats.SuppliersPending:N0}", Icon = "💳", Color = Color.FromArgb(255, 64, 129), SubText = "To be paid" },
                new { Title = "Quotations", Value = stats.TotalQuotations.ToString(), Icon = "📄", Color = Color.FromArgb(103, 58, 183), SubText = $"{stats.PendingQuotations} pending" },
                new { Title = "Est. Profit", Value = $"Rs. {stats.EstimatedProfit:N0}", Icon = "✨", Color = Color.FromArgb(0, 200, 83), SubText = $"{stats.ProfitMargin:F1}% margin" }
            };

            for (int i = 0; i < cards.Length; i++)
            {
                int row = i / cardsPerRow;
                int col = i % cardsPerRow;

                var card = CreateStatCard(
                    cards[i].Title,
                    cards[i].Value,
                    cards[i].Icon,
                    cards[i].Color,
                    cards[i].SubText,
                    cardWidth,
                    cardHeight
                );

                card.Location = new Point(col * (cardWidth + spacing), row * (cardHeight + spacing));
                panel.Controls.Add(card);
            }

            return panel;
        }

        private Panel CreateStatCard(string title, string value, string icon, Color color, string subText, int width, int height)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Draw border
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Icon - left side, bigger
            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 18),
                AutoSize = true,
                Location = new Point(10, 10)
            };

            // Title - to the right of icon, small gray text
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(120, 120, 120),
                AutoSize = false,
                Size = new Size(width - 60, 20),
                Location = new Point(95, 12),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Value - large colored text below, spanning full width
            var valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = color,
                AutoSize = false,
                Size = new Size(width - 20, 35),
                Location = new Point(10, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Subtext - small gray text at bottom
            var subTextLabel = new Label
            {
                Text = subText,
                Font = new Font("Segoe UI", 6.5F),
                ForeColor = Color.FromArgb(130, 130, 130),
                AutoSize = false,
                Size = new Size(width - 20, 15),
                Location = new Point(10, height - 22),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.AddRange(new Control[] { iconLabel, titleLabel, valueLabel, subTextLabel });
            return card;
        }

        private Panel CreateChartsRow1(System.Collections.Generic.List<SalesChartData> salesData,
            System.Collections.Generic.List<CategorySales> categorySales, int panelWidth)
        {
            int gridWidth = (panelWidth - 15) / 2;
            int gridHeight = 380;

            var panel = new Panel
            {
                Size = new Size(panelWidth, gridHeight),
                BackColor = Color.Transparent
            };

            var salesGrid = CreateDataGrid("📈 Sales Trend (Last 30 Days)", salesData,
                Color.FromArgb(33, 150, 243), gridWidth, gridHeight);
            salesGrid.Location = new Point(0, 0);
            panel.Controls.Add(salesGrid);

            var categoryGrid = CreateDataGrid("📊 Sales by Category", categorySales,
                Color.FromArgb(156, 39, 176), gridWidth, gridHeight);
            categoryGrid.Location = new Point(gridWidth + 15, 0);
            panel.Controls.Add(categoryGrid);

            return panel;
        }

        private Panel CreateChartsRow2(System.Collections.Generic.List<PaymentMethodStats> paymentMethods, int panelWidth)
        {
            int gridWidth = (panelWidth - 15) / 2;
            int gridHeight = 380;

            var panel = new Panel
            {
                Size = new Size(panelWidth, gridHeight),
                BackColor = Color.Transparent
            };

            var paymentGrid = CreateDataGrid("💳 Payment Methods", paymentMethods,
                Color.FromArgb(255, 152, 0), gridWidth, gridHeight);
            paymentGrid.Location = new Point(0, 0);
            panel.Controls.Add(paymentGrid);

            // Placeholder for another chart
            var placeholderPanel = new Panel
            {
                Size = new Size(gridWidth, gridHeight),
                BackColor = Color.White,
                Location = new Point(gridWidth + 15, 0)
            };
            var placeholderLabel = new Label
            {
                Text = "📊 Additional Chart Area\n(Available for future use)",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            placeholderPanel.Controls.Add(placeholderLabel);
            panel.Controls.Add(placeholderPanel);

            return panel;
        }

        private Panel CreateDataGrid(string title, object data, Color headerColor, int width, int height)
        {
            var container = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 25, 72),
                AutoSize = true,
                Location = new Point(12, 12)
            };

            var grid = new DataGridView
            {
                Size = new Size(width - 24, height - 60),
                Location = new Point(12, 45),
                DataSource = data,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                EnableHeadersVisualStyles = false
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grid.ColumnHeadersHeight = 35;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grid.RowTemplate.Height = 28;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Format columns
            if (grid.Columns.Contains("Sales"))
                grid.Columns["Sales"].DefaultCellStyle.Format = "N0";
            if (grid.Columns.Contains("TotalSales"))
                grid.Columns["TotalSales"].DefaultCellStyle.Format = "N0";
            if (grid.Columns.Contains("Amount"))
                grid.Columns["Amount"].DefaultCellStyle.Format = "N0";
            if (grid.Columns.Contains("Percentage"))
                grid.Columns["Percentage"].DefaultCellStyle.Format = "N1";

            container.Controls.AddRange(new Control[] { titleLabel, grid });
            return container;
        }

        private Panel CreateGridsSection(System.Collections.Generic.List<RecentBill> recentBills,
            System.Collections.Generic.List<LowStockItem> lowStock, int panelWidth)
        {
            int gridWidth = (panelWidth - 15) / 2;
            int gridHeight = 420;

            var panel = new Panel
            {
                Size = new Size(panelWidth, gridHeight),
                BackColor = Color.Transparent
            };

            var billsGrid = CreateMainGrid("📋 Recent Bills", recentBills,
                Color.FromArgb(20, 25, 72), gridWidth, gridHeight);
            billsGrid.Location = new Point(0, 0);
            panel.Controls.Add(billsGrid);

            var stockGrid = CreateMainGrid("⚠️ Low Stock Alert", lowStock,
                Color.FromArgb(244, 67, 54), gridWidth, gridHeight);
            stockGrid.Location = new Point(gridWidth + 15, 0);
            panel.Controls.Add(stockGrid);

            return panel;
        }

        private Panel CreateMainGrid(string title, object data, Color headerColor, int width, int height)
        {
            var container = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = title.Contains("Alert") ? Color.FromArgb(244, 67, 54) : Color.FromArgb(20, 25, 72),
                AutoSize = true,
                Location = new Point(12, 12)
            };

            var grid = new DataGridView
            {
                Size = new Size(width - 24, height - 60),
                Location = new Point(12, 45),
                DataSource = data,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                EnableHeadersVisualStyles = false
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grid.ColumnHeadersHeight = 35;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grid.RowTemplate.Height = 30;
            grid.AlternatingRowsDefaultCellStyle.BackColor = title.Contains("Alert") ?
                Color.FromArgb(255, 245, 245) : Color.FromArgb(248, 248, 248);

            // Format columns
            if (grid.Columns.Contains("BillDate"))
                grid.Columns["BillDate"].DefaultCellStyle.Format = "MMM dd, yyyy";
            if (grid.Columns.Contains("TotalAmount"))
                grid.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            if (grid.Columns.Contains("AmountDue"))
                grid.Columns["AmountDue"].DefaultCellStyle.Format = "N0";
            if (grid.Columns.Contains("CurrentStock"))
                grid.Columns["CurrentStock"].DefaultCellStyle.Format = "N1";
            if (grid.Columns.Contains("ReorderLevel"))
                grid.Columns["ReorderLevel"].DefaultCellStyle.Format = "N1";

            container.Controls.AddRange(new Control[] { titleLabel, grid });
            return container;
        }

        private void ExportDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.RestoreDirectory = true;
                    saveDialog.FileName = $"Dashboard_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToCSV(saveDialog.FileName);
                        MessageBox.Show($"Dashboard data exported successfully to:\n{saveDialog.FileName}",
                            "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting dashboard: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportToCSV(string filePath)
        {
            var csv = new System.Text.StringBuilder();

            // Add header
            csv.AppendLine("BISMILLAH HARDWARE - DASHBOARD EXPORT");
            csv.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            csv.AppendLine();

            // Get fresh data
            var stats = await _dashboardDL.GetDashboardStats();
            var salesData = await _dashboardDL.GetSalesChartData(30);
            var categorySales = await _dashboardDL.GetCategorySales();
            var paymentMethods = await _dashboardDL.GetPaymentMethodStats();
            var recentBills = await _dashboardDL.GetRecentBills(10);
            var lowStock = await _dashboardDL.GetLowStockItems(10);

            // Dashboard Statistics
            csv.AppendLine("=== DASHBOARD STATISTICS ===");
            csv.AppendLine("Metric,Value");
            csv.AppendLine($"Today Revenue,{stats.TodayRevenue}");
            csv.AppendLine($"Today Bills,{stats.TodayBills}");
            csv.AppendLine($"Week Revenue,{stats.WeekRevenue}");
            csv.AppendLine($"Week Bills,{stats.WeekBills}");
            csv.AppendLine($"Month Revenue,{stats.MonthRevenue}");
            csv.AppendLine($"Month Bills,{stats.MonthBills}");
            csv.AppendLine($"Total Revenue,{stats.TotalRevenue}");
            csv.AppendLine($"Total Customers,{stats.TotalCustomers}");
            csv.AppendLine($"Active Customers,{stats.ActiveCustomers}");
            csv.AppendLine($"Outstanding Amount,{stats.TotalOutstanding}");
            csv.AppendLine($"Pending Bills,{stats.PendingBills}");
            csv.AppendLine($"Total Products,{stats.TotalProducts}");
            csv.AppendLine($"Low Stock Products,{stats.LowStockProducts}");
            csv.AppendLine($"Out of Stock Products,{stats.OutOfStockProducts}");
            csv.AppendLine($"Total Stock Value,{stats.TotalStockValue}");
            csv.AppendLine($"Total Suppliers,{stats.TotalSuppliers}");
            csv.AppendLine($"Supplier Dues,{stats.SuppliersPending}");
            csv.AppendLine($"Total Quotations,{stats.TotalQuotations}");
            csv.AppendLine($"Pending Quotations,{stats.PendingQuotations}");
            csv.AppendLine($"Estimated Profit,{stats.EstimatedProfit}");
            csv.AppendLine($"Profit Margin %,{stats.ProfitMargin}");
            csv.AppendLine();

            // Sales Trend
            csv.AppendLine("=== SALES TREND (LAST 30 DAYS) ===");
            csv.AppendLine("Period,Sales,Bill Count");
            foreach (var item in salesData)
            {
                csv.AppendLine($"{item.Period},{item.Sales},{item.BillCount}");
            }
            csv.AppendLine();

            // Category Sales
            csv.AppendLine("=== SALES BY CATEGORY ===");
            csv.AppendLine("Category,Total Sales,Item Count,Percentage");
            foreach (var item in categorySales)
            {
                csv.AppendLine($"{item.CategoryName},{item.TotalSales},{item.ItemCount},{item.Percentage}");
            }
            csv.AppendLine();

            // Payment Methods
            csv.AppendLine("=== PAYMENT METHODS ===");
            csv.AppendLine("Payment Method,Amount,Count,Percentage");
            foreach (var item in paymentMethods)
            {
                csv.AppendLine($"{item.PaymentMethod},{item.Amount},{item.Count},{item.Percentage}");
            }
            csv.AppendLine();

            // Recent Bills
            csv.AppendLine("=== RECENT BILLS ===");
            var billProps = typeof(RecentBill).GetProperties();
            csv.AppendLine(string.Join(",", billProps.Select(p => p.Name)));
            foreach (var bill in recentBills)
            {
                var values = billProps.Select(p => p.GetValue(bill)?.ToString() ?? "");
                csv.AppendLine(string.Join(",", values));
            }
            csv.AppendLine();

            // Low Stock Items
            csv.AppendLine("=== LOW STOCK ALERT ===");
            var stockProps = typeof(LowStockItem).GetProperties();
            csv.AppendLine(string.Join(",", stockProps.Select(p => p.Name)));
            foreach (var item in lowStock)
            {
                var values = stockProps.Select(p => p.GetValue(item)?.ToString() ?? "");
                csv.AppendLine(string.Join(",", values));
            }

            // Write to file
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }
    }
}