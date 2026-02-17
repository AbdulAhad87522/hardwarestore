using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public static class UIHelper
    {
        /// <summary>
        /// Adds a button column to a DataGridView
        /// </summary>
        public static void AddButtonColumn(DataGridView grid, string name, string headerText, string buttonText)
        {
            // Check if column already exists
            if (grid.Columns.Contains(name))
            {
                grid.Columns.Remove(name);
            }

            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = headerText,
                Text = buttonText,
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };

            grid.Columns.Add(btnColumn);
        }

        /// <summary>
        /// Rounds the corners of a panel
        /// </summary>
        public static void RoundPanelCorners(Panel panel, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            panel.Region = new Region(path);
        }

        /// <summary>
        /// Applies a modern look to a form
        /// </summary>
        public static void ApplyModernStyle(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.BackColor = Color.FromArgb(245, 247, 250);

            // Add drop shadow (Windows 10+)
            if (Environment.OSVersion.Version.Major >= 10)
            {
                var v = 2;
                DwmSetWindowAttribute(form.Handle, 2, ref v, 4);
                var margins = new MARGINS() { bottomHeight = 1, leftWidth = 1, rightWidth = 1, topHeight = 1 };
                DwmExtendFrameIntoClientArea(form.Handle, ref margins);
            }
        }

        /// <summary>
        /// Adds hover effects to a button
        /// </summary>
        /// 
        public static void StyleGridView(DataGridView grid)
        {
            if (grid == null) return;

            // General layout & restrictions
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowTemplate.Height = 35;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating row style
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);

            // Cell borders & selection colors
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            grid.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            // Header styling
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);

            // Default cell font
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);


        }

        public static void AddButtonHoverEffect(Button button, Color hoverColor)
        {
            Color originalColor = button.BackColor;

            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = originalColor;
        }

        /// <summary>
        /// Centers a panel on a form
        /// </summary>
        public static void CenterPanel(Panel panel, Form form)
        {
            panel.Location = new Point(
                (form.ClientSize.Width - panel.Width) / 2,
                (form.ClientSize.Height - panel.Height) / 2
            );
        }

        /// <summary>
        /// Animates a panel sliding in
        /// </summary>
        public static void SlideIn(Panel panel, int targetX, int targetY, int speed = 20)
        {
            Timer timer = new Timer { Interval = 10 };

            timer.Tick += (s, e) =>
            {
                int dx = targetX - panel.Location.X;
                int dy = targetY - panel.Location.Y;

                if (Math.Abs(dx) < speed && Math.Abs(dy) < speed)
                {
                    panel.Location = new Point(targetX, targetY);
                    timer.Stop();
                    timer.Dispose();
                }
                else
                {
                    panel.Location = new Point(
                        panel.Location.X + Math.Sign(dx) * Math.Min(speed, Math.Abs(dx)),
                        panel.Location.Y + Math.Sign(dy) * Math.Min(speed, Math.Abs(dy))
                    );
                }
            };

            timer.Start();
        }

        /// <summary>
        /// Formats currency for display
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return $"Rs. {amount:N2}";
        }

        /// <summary>
        /// Validates that a textbox contains a valid decimal number
        /// </summary>
        public static bool ValidateDecimal(TextBox textBox, out decimal result)
        {
            if (decimal.TryParse(textBox.Text, out result))
            {
                textBox.BackColor = Color.White;
                return true;
            }
            else
            {
                textBox.BackColor = Color.LightPink;
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Validates that a textbox contains a valid integer
        /// </summary>
        public static bool ValidateInteger(TextBox textBox, out int result)
        {
            if (int.TryParse(textBox.Text, out result))
            {
                textBox.BackColor = Color.White;
                return true;
            }
            else
            {
                textBox.BackColor = Color.LightPink;
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Shows a success message
        /// </summary>
        public static void ShowSuccess(string message, string title = "Success")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning message
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Confirms an action with the user
        /// </summary>
        public static bool Confirm(string message, string title = "Confirm")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // Windows API for drop shadow
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        public  static  void CustomizeGrid(DataGridView datagridview)
        {
            var grid = datagridview;
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SeaGreen;
            grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            grid.BackgroundColor = System.Drawing.Color.White;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;


        }
    }
}