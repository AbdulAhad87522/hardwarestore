using System;
using System.Windows.Forms;
using HardWareStore.DL;

namespace HardWareStore.UI
{
    public partial class BatchItemsForm : Form
    {
        private readonly int _batchId;
        private readonly string _batchName;
        private readonly PurchaseBatchDL _batchDL = new PurchaseBatchDL();

        public BatchItemsForm(int batchId, string batchName)
        {
            InitializeComponent();
            _batchId = batchId;
            _batchName = batchName;
            this.Text = $"Batch Items - {batchName}";
            UIHelper.CustomizeGrid(dgvItems);
        }

        private async void BatchItemsForm_Load(object sender, EventArgs e)
        {
            try
            {
                var items = await _batchDL.GetBatchItems(_batchId);
                dgvItems.DataSource = items;

                // Hide columns
                if (dgvItems.Columns.Contains("variant_id"))
                    dgvItems.Columns["variant_id"].Visible = false;
                if (dgvItems.Columns.Contains("purchase_batch_id"))
                    dgvItems.Columns["purchase_batch_id"].Visible = false;
                if (dgvItems.Columns.Contains("purchase_batch_item_id"))
                    dgvItems.Columns["purchase_batch_item_id"].Visible = false;
                if (dgvItems.Columns.Contains("CreatedAt"))
                    dgvItems.Columns["CreatedAt"].Visible = false;

                // Rename headers
                if (dgvItems.Columns.Contains("product_name"))
                    dgvItems.Columns["product_name"].HeaderText = "Product";
                if (dgvItems.Columns.Contains("size"))
                    dgvItems.Columns["size"].HeaderText = "Size";
                if (dgvItems.Columns.Contains("class_type"))
                    dgvItems.Columns["class_type"].HeaderText = "Class";
                if (dgvItems.Columns.Contains("quantity_received"))
                    dgvItems.Columns["quantity_received"].HeaderText = "Quantity";
                if (dgvItems.Columns.Contains("cost_price"))
                    dgvItems.Columns["cost_price"].HeaderText = "Cost Price";
                if (dgvItems.Columns.Contains("sale_price"))
                    dgvItems.Columns["sale_price"].HeaderText = "Sale Price";
                if (dgvItems.Columns.Contains("line_total"))
                    dgvItems.Columns["line_total"].HeaderText = "Total";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading batch items: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}