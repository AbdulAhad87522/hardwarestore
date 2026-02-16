using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HardWareStore.Models;
using HardWareStore.DL;

namespace HardWareStore.UI
{
    public partial class SupplierBillDetailsForm : Form
    {
        private readonly int _supplierId;
        private readonly string _supplierName;
        private readonly PurchaseBatchDL _batchDL = new PurchaseBatchDL();
        private readonly SupplierPaymentDL _paymentDL = new SupplierPaymentDL();

        public SupplierBillDetailsForm(int supplierId, string supplierName)
        {
            InitializeComponent();
            _supplierId = supplierId;
            _supplierName = supplierName;
            UIHelper.CustomizeGrid(dgvBatches);
            UIHelper.CustomizeGrid(dgvPayments);
        }

        private void SupplierBillDetailsForm_Load(object sender, EventArgs e)
        {
            LoadSupplierSummary();
            LoadSupplierBatches();
            LoadPaymentRecords();
        }

        private async void LoadSupplierSummary()
        {
            try
            {
                var summary = await GetSupplierSummary(_supplierId);

                lblSupplierName.Text = $"Supplier: {_supplierName}";
                lblTotalAmount.Text = $"Total: Rs. {summary.total_price:N2}";
                lblPaidAmount.Text = $"Paid: Rs. {summary.paid:N2}";
                lblPendingAmount.Text = $"Pending: Rs. {summary.remaining:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplier summary: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadSupplierBatches()
        {
            try
            {
                var batches = await GetSupplierBatches(_supplierId);
                dgvBatches.DataSource = batches;


                // Hide columns
                if (dgvBatches.Columns.Contains("batch_id"))
                    dgvBatches.Columns["batch_id"].Visible = false;
                if (dgvBatches.Columns.Contains("supplier_id"))
                    dgvBatches.Columns["supplier_id"].Visible = false;

                // Rename headers
                if (dgvBatches.Columns.Contains("BatchName"))
                    dgvBatches.Columns["BatchName"].HeaderText = "Batch Name";
                if (dgvBatches.Columns.Contains("purchase_date"))
                    dgvBatches.Columns["purchase_date"].HeaderText = "Date";
                if (dgvBatches.Columns.Contains("total_price"))
                    dgvBatches.Columns["total_price"].HeaderText = "Total";
                if (dgvBatches.Columns.Contains("paid"))
                    dgvBatches.Columns["paid"].HeaderText = "Paid";
                if (dgvBatches.Columns.Contains("remaining"))
                    dgvBatches.Columns["remaining"].HeaderText = "Remaining";
                if (dgvBatches.Columns.Contains("status"))
                    dgvBatches.Columns["status"].HeaderText = "Status";

                UIHelper.AddButtonColumn(dgvBatches, "ViewItems", "View Items", "ViewItems");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading batches: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadPaymentRecords()
        {
            try
            {
                var payments = await _paymentDL.GetPaymentRecordsBySupplier(_supplierId);
                dgvPayments.DataSource = payments;

                dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Hide columns
                if (dgvPayments.Columns.Contains("payment_id"))
                    dgvPayments.Columns["payment_id"].Visible = false;
                if (dgvPayments.Columns.Contains("supplier_id"))
                    dgvPayments.Columns["supplier_id"].Visible = false;
                if (dgvPayments.Columns.Contains("supplier_name"))
                    dgvPayments.Columns["supplier_name"].Visible = false;
                if (dgvPayments.Columns.Contains("created_at"))
                    dgvPayments.Columns["created_at"].Visible = false;

                // Rename headers
                if (dgvPayments.Columns.Contains("batch_name"))
                    dgvPayments.Columns["batch_name"].HeaderText = "Batch";
                if (dgvPayments.Columns.Contains("payment_date"))
                    dgvPayments.Columns["payment_date"].HeaderText = "Date";
                if (dgvPayments.Columns.Contains("payment_amount"))
                    dgvPayments.Columns["payment_amount"].HeaderText = "Payment";
                if (dgvPayments.Columns.Contains("remarks"))
                    dgvPayments.Columns["remarks"].HeaderText = "Remarks";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment records: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task<SupplierSummary> GetSupplierSummary(int supplierId)
        {
            string query = @"
                SELECT 
                    SUM(pb.total_price) as total_price,
                    SUM(pb.paid) as paid,
                    SUM(pb.total_price - pb.paid) as remaining
                FROM purchase_batches pb
                WHERE pb.supplier_id = @supplier_id;";

            var parameters = new MySql.Data.MySqlClient.MySqlParameter[]
            {
                new MySql.Data.MySqlClient.MySqlParameter("@supplier_id", supplierId)
            };

            return await System.Threading.Tasks.Task.Run(() =>
            {
                using (var reader = DatabaseHelper.Instance.ExecuteReader(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new SupplierSummary
                        {
                            total_price = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0),
                            paid = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                            remaining = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        };
                    }
                }
                return new SupplierSummary();
            });
        }

        private async System.Threading.Tasks.Task<List<BatchSummary>> GetSupplierBatches(int supplierId)
        {
            string query = @"
                SELECT 
                    pb.batch_id,
                    pb.supplier_id,
                    pb.BatchName,
                    pb.total_price,
                    pb.paid,
                    (pb.total_price - pb.paid) as remaining,
                    pb.status,
                    DATE_FORMAT(pb.created_at, '%Y-%m-%d') as purchase_date
                FROM purchase_batches pb
                WHERE pb.supplier_id = @supplier_id
                ORDER BY pb.created_at DESC;";

            var parameters = new MySql.Data.MySqlClient.MySqlParameter[]
            {
                new MySql.Data.MySqlClient.MySqlParameter("@supplier_id", supplierId)
            };

            return await System.Threading.Tasks.Task.Run(() =>
            {
                var batches = new List<BatchSummary>();
                using (var reader = DatabaseHelper.Instance.ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        batches.Add(new BatchSummary
                        {
                            batch_id = reader.GetInt32("batch_id"),
                            supplier_id = reader.GetInt32("supplier_id"),
                            BatchName = reader.GetString("BatchName"),
                            total_price = reader.GetDecimal("total_price"),
                            paid = reader.GetDecimal("paid"),
                            remaining = reader.GetDecimal("remaining"),
                            status = reader.GetString("status"),
                            purchase_date = reader.GetString("purchase_date")
                        });
                    }
                }
                return batches;
            });
        }

        private async void dgvBatches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvBatches.Columns[e.ColumnIndex].Name;

            if (columnName == "ViewItems")
            {
                int batchId = Convert.ToInt32(dgvBatches.Rows[e.RowIndex].Cells["batch_id"].Value);
                string batchName = dgvBatches.Rows[e.RowIndex].Cells["BatchName"].Value.ToString();

                // Open batch items form
                var itemsForm = new BatchItemsForm(batchId, batchName);
                itemsForm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class SupplierSummary
        {
            public decimal total_price { get; set; }
            public decimal paid { get; set; }
            public decimal remaining { get; set; }
        }

        public class BatchSummary
        {
            public int batch_id { get; set; }
            public int supplier_id { get; set; }
            public string BatchName { get; set; }
            public decimal total_price { get; set; }
            public decimal paid { get; set; }
            public decimal remaining { get; set; }
            public string status { get; set; }
            public string purchase_date { get; set; }
        }
    }
}