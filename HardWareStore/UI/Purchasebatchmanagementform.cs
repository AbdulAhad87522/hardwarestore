using HardWareStore.DL;
using HardWareStore.Models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class PurchaseBatchManagementForm : Form
    {
        private readonly PurchaseBatchDL _batchDL = new PurchaseBatchDL();
        private readonly ProductsDL _productsDL = new ProductsDL();
        private int _selectedBatchId = -1;

        public PurchaseBatchManagementForm()
        {
            InitializeComponent();
            panelEdit.Visible = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && panelEdit.Visible)
            {
                panelEdit.Visible = false;
                return true;
            }

            if (keyData == (Keys.Control | Keys.Enter) && panelEdit.Visible)
            {
                btnSaveEdit.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PurchaseBatchManagementForm_Load(object sender, EventArgs e)
        {
            LoadBatches();
        }

        private async void LoadBatches()
        {
            try
            {
                var batches = await _batchDL.GetAllBatches();
                dgvBatches.DataSource = batches;

                // Hide columns
                dgvBatches.Columns["supplier_id"].Visible = false;
                dgvBatches.Columns["CreatedAt"].Visible = false;
                dgvBatches.Columns["batch_id"].Visible=false;

                UIHelper.CustomizeGrid(dgvBatches);

                // Add buttons
                UIHelper.AddButtonColumn(dgvBatches, "Edit", "Edit", "Edit");
                UIHelper.AddButtonColumn(dgvBatches, "AddDetails", "Add Details", "Add Details");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading batches: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddNewBatch_Click(object sender, EventArgs e)
        {
            var form = new AddPurchaseBatchForm(_productsDL, _batchDL);
            form.ShowDialog();
            LoadBatches();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    LoadBatches();
                    return;
                }

                var batches = await _batchDL.SearchBatches(searchTerm);
                dgvBatches.DataSource = batches;

                dgvBatches.Columns["supplier_id"].Visible = false;
                dgvBatches.Columns["created_at"].Visible = false;

                UIHelper.CustomizeGrid(dgvBatches);
                UIHelper.AddButtonColumn(dgvBatches, "Edit", "Edit", "Edit");
                UIHelper.AddButtonColumn(dgvBatches, "AddDetails", "Add Details", "Add Details");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dgvBatches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = dgvBatches.Rows[e.RowIndex];
            _selectedBatchId = Convert.ToInt32(row.Cells["batch_id"].Value);
            string columnName = dgvBatches.Columns[e.ColumnIndex].Name;

            if (columnName == "Edit")
            {
                // Show edit panel with batch info
                txtSupplierName.Text = row.Cells["supplier_name"].Value.ToString();
                txtBatchName.Text = row.Cells["BatchName"].Value.ToString();
                txtTotalPrice.Text = row.Cells["total_price"].Value.ToString();
                txtPaid.Text = row.Cells["paid"].Value.ToString();

                panelEdit.Visible = true;
                UIHelper.RoundPanelCorners(panelEdit, 20);
            }
            else if (columnName == "AddDetails")
            {
                // Open batch details form
                OpenAddDetailsForm(row);
            }
        }

        private void OpenAddDetailsForm(DataGridViewRow row)
        {
            try
            {
                string batchName = row.Cells["BatchName"].Value.ToString();
                int batchId = Convert.ToInt32(row.Cells["batch_id"].Value);

                var form = new AddPurchaseBatchForm(_productsDL, _batchDL);
                form.LoadExistingBatch(batchId, batchName);
                form.ShowDialog();

                LoadBatches();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening batch details: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string batchName = txtBatchName.Text.Trim();
                string supplierName = txtSupplierName.Text.Trim();
                decimal totalPrice = decimal.TryParse(txtTotalPrice.Text.Trim(), out var tp) ? tp : 0;
                decimal paid = decimal.TryParse(txtPaid.Text.Trim(), out var p) ? p : 0;

                if (string.IsNullOrWhiteSpace(batchName) || string.IsNullOrWhiteSpace(supplierName))
                {
                    MessageBox.Show("Batch name and Supplier name cannot be empty.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (totalPrice < 0 || paid < 0)
                {
                    MessageBox.Show("Price values cannot be negative.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var batch = await _batchDL.GetBatchById(_selectedBatchId);
                if (batch == null)
                {
                    MessageBox.Show("Batch not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                batch.BatchName = batchName;
                batch.total_price = totalPrice;
                batch.paid = paid;

                bool success = await _batchDL.UpdateBatch(batch);
                if (success)
                {
                    MessageBox.Show("Batch updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelEdit.Visible = false;
                    LoadBatches();
                }
                else
                {
                    MessageBox.Show("Failed to update batch.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            panelEdit.Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBatches();
            txtSearch.Clear();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}