using HardWareStore.DL;
using HardWareStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class SupplierBillsForm : Form
    {
        private readonly PurchaseBatchDL _batchDL = new PurchaseBatchDL();
        private readonly SupplierPaymentDL _paymentDL = new SupplierPaymentDL();
        private readonly ProductsDL _productsDL = new ProductsDL();
        private readonly DatabaseHelper _db=DatabaseHelper.Instance;

        public SupplierBillsForm()
        {
            InitializeComponent();
            panelPayment.Visible = false;
            txtSupplierId.Visible=false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && panelPayment.Visible)
            {
                panelPayment.Visible = false;
                return true;
            }

            if (keyData == (Keys.Control | Keys.Enter) && panelPayment.Visible)
            {
                btnSavePayment.PerformClick();
                return true;
            }

            if (keyData == Keys.Enter && panelPayment.Visible)
            {
                btnSavePayment.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SupplierBillsForm_Load(object sender, EventArgs e)
        {
            LoadBills();
            dgvBills.Focus();
        }

        private async void LoadBills()
        {
            try
            {
                // Get aggregated supplier bills (total across all batches)
                var suppliers = await GetAggregatedSupplierBills();

                dgvBills.DataSource = suppliers;
                CustomizeGrid();

                UIHelper.AddButtonColumn(dgvBills, "Details", "View Details", "Details");
                UIHelper.AddButtonColumn(dgvBills, "Payment", "Add Payment", "Payment");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bills: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CustomizeGrid()
        {
            var grid = dgvBills;
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


            if (dgvBills.Columns.Contains("supplier_id"))
                dgvBills.Columns["supplier_id"].Visible = false;

            if (dgvBills.Columns.Contains("supplier_name"))
                dgvBills.Columns["supplier_name"].HeaderText = "Supplier";
            if (dgvBills.Columns.Contains("total_price"))
                dgvBills.Columns["total_price"].HeaderText = "Total";
            if (dgvBills.Columns.Contains("paid"))
                dgvBills.Columns["paid"].HeaderText = "Paid";
            if (dgvBills.Columns.Contains("remaining"))
                dgvBills.Columns["remaining"].HeaderText = "Pending";
            if (dgvBills.Columns.Contains("status"))
                dgvBills.Columns["status"].HeaderText = "Status";
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadBills();
                    return;
                }

                var batches = await _batchDL.SearchBatches(keyword);
                var filtered = batches.Where(b => b.total_price != 0).ToList();

                dgvBills.DataSource = filtered;
                CustomizeGrid();

                UIHelper.AddButtonColumn(dgvBills, "Details", "View Details", "Details");
                UIHelper.AddButtonColumn(dgvBills, "Payment", "Add Payment", "Payment");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBills();
            txtSearch.Clear();
        }

        private void dgvBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvBills.Columns[e.ColumnIndex].Name;
            var row = dgvBills.Rows[e.RowIndex];

            if (columnName == "Details")
            {
                int supplierId = Convert.ToInt32(row.Cells["supplier_id"].Value);
                string supplierName = row.Cells["supplier_name"].Value.ToString();

                var detailsForm = new SupplierBillDetailsForm(supplierId, supplierName);
                detailsForm.ShowDialog();

                LoadBills(); // Refresh after viewing details
            }
            else if (columnName == "Payment")
            {
                // Populate payment panel
                txtSupplierId.Text = row.Cells["supplier_id"].Value.ToString(); // Add this textbox to designer
                txtSupplierName.Text = row.Cells["supplier_name"].Value.ToString();
                txtPendingAmount.Text = row.Cells["remaining"].Value.ToString();
                dtpPaymentDate.Value = DateTime.Now;
                txtPaymentAmount.Clear();
                txtRemarks.Clear();

                // Show payment panel
                UIHelper.RoundPanelCorners(panelPayment, 20);
                panelPayment.Visible = true;
                panelPayment.BringToFront();
                txtPaymentAmount.Focus();
            }
        }
        private async void btnSavePayment_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text))
                {
                    MessageBox.Show("Please enter payment amount.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal paymentAmount = decimal.Parse(txtPaymentAmount.Text);
                if (paymentAmount <= 0)
                {
                    MessageBox.Show("Payment amount must be greater than zero.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int supplierId = Convert.ToInt32(txtSupplierId.Text);

                // Get the oldest unpaid/partial batch for this supplier
                int batchId = await GetOldestUnpaidBatch(supplierId);

                if (batchId == 0)
                {
                    MessageBox.Show("No unpaid batches found for this supplier.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var payment = new SupplierPaymentRecord
                {
                    supplier_id = supplierId,
                    batch_id = batchId,
                    payment_amount = paymentAmount,
                    payment_date = dtpPaymentDate.Value,
                    remarks = txtRemarks.Text.Trim()
                };

                bool success = await _paymentDL.AddPaymentRecord(payment);

                if (success)
                {
                    MessageBox.Show("Payment saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelPayment.Visible = false;
                    LoadBills();
                }
                else
                {
                    MessageBox.Show("Failed to save payment.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payment: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add this helper method to get the oldest unpaid batch
        private async Task<int> GetOldestUnpaidBatch(int supplierId)
        {
            try
            {
                string query = @"
            SELECT batch_id 
            FROM purchase_batches 
            WHERE supplier_id = @supplier_id 
            AND paid < total_price
            ORDER BY created_at ASC
            LIMIT 1;";

                var parameters = new MySql.Data.MySqlClient.MySqlParameter[]
                {
            new MySql.Data.MySqlClient.MySqlParameter("@supplier_id", supplierId)
                };

                return await Task.Run(() =>
                {
                    using (var reader = _db.ExecuteReader(query, parameters))
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32("batch_id");
                        }
                    }
                    return 0;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting oldest unpaid batch: {ex.Message}");
                return 0;
            }
        }

        private async System.Threading.Tasks.Task<int> GetSupplierIdFromBatch(int batchId)
        {
            var batch = await _batchDL.GetBatchById(batchId);
            return batch?.supplier_id ?? 0;
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            panelPayment.Visible = false;
        }
        private async Task<List<SupplierBillSummary>> GetAggregatedSupplierBills()
        {
            try
            {
                string query = @"
            SELECT 
                s.supplier_id,
                s.name as supplier_name,
                SUM(pb.total_price) as total_price,
                SUM(pb.paid) as paid,
                SUM(pb.total_price - pb.paid) as remaining,
                CASE 
                    WHEN SUM(pb.paid) >= SUM(pb.total_price) THEN 'Completed'
                    WHEN SUM(pb.paid) > 0 THEN 'Partial'
                    ELSE 'Pending'
                END as status
            FROM supplier s
            LEFT JOIN purchase_batches pb ON s.supplier_id = pb.supplier_id
            GROUP BY s.supplier_id, s.name
            HAVING SUM(pb.total_price) > 0
            ORDER BY s.name;";

                return await Task.Run(() =>
                {
                    var suppliers = new List<SupplierBillSummary>();
                    using (var reader = _db.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new SupplierBillSummary
                            {
                                supplier_id = reader.GetInt32("supplier_id"),
                                supplier_name = reader.GetString("supplier_name"),
                                total_price = reader.GetDecimal("total_price"),
                                paid = reader.GetDecimal("paid"),
                                remaining = reader.GetDecimal("remaining"),
                                status = reader.GetString("status")
                            });
                        }
                    }
                    return suppliers;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting aggregated bills: {ex.Message}");
                throw;
            }
        }
    }
    public class SupplierBillSummary
    {
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public decimal total_price { get; set; }
        public decimal paid { get; set; }
        public decimal remaining { get; set; }
        public string status { get; set; }
    }
}