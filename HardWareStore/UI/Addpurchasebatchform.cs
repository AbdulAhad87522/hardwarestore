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
    public partial class AddPurchaseBatchForm : Form
    {
        private readonly PurchaseBatchDL _batchDL = new PurchaseBatchDL();
        private readonly ProductsDL _productsDL = new ProductsDL();

        private int _currentBatchId = 0;
        private int _selectedVariantId = 0;
        private List<PurchaseBatchItem> _batchItems = new List<PurchaseBatchItem>();
        private bool _isLoadingExistingBatch = false;
        private int _editingItemIndex = -1;
        private bool _isEditingItem = false;

        public AddPurchaseBatchForm()
        {
            InitializeComponent();
            panelVariantSelection.Visible = false;
            InitializeForm();
        }

        private async void InitializeForm()
        {
            try
            {
                // Get next batch ID
                _currentBatchId = await _batchDL.GetNextBatchId();
                lblBatchId.Text = $"Batch ID: {_currentBatchId}";

                // Bind suppliers
                await BindSuppliers();

                // Set default date
                dtpBatchDate.Value = DateTime.Now;

                // Style grids
                CustomizeGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async void LoadExistingBatch(int batchId, string batchName)
        {
            try
            {
                _isLoadingExistingBatch = true;
                _currentBatchId = batchId;

                var batch = await _batchDL.GetBatchById(batchId);
                if (batch != null)
                {
                    lblBatchId.Text = $"Batch ID: {batch.batch_id}";
                    txtBatchName.Text = batch.BatchName;
                    cmbSupplier.Text = batch.supplier_name;
                    txtTotalAmount.Text = batch.total_price.ToString("N2");
                    txtPaid.Text = batch.paid.ToString("N2");
                    txtRemaining.Text = batch.remaining.ToString("N2");
                    cmbStatus.Text = batch.status;

                    // Load batch items
                    _batchItems = await _batchDL.GetBatchItems(batchId);
                    RefreshBatchItemsGrid();
                    CalculateTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading batch: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingExistingBatch = false;
            }
        }

        private async System.Threading.Tasks.Task BindSuppliers(string keyword = "")
        {
            try
            {
                var suppliers = await _productsDL.GetSupplierNames(keyword);
                cmbSupplier.Items.Clear();
                cmbSupplier.Items.AddRange(suppliers.ToArray());

                cmbSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomizeGrids()
        {
            // Variant selection grid
            var gridVariants = dgvVariantSelection;
            gridVariants.BorderStyle = BorderStyle.None;
            gridVariants.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            gridVariants.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridVariants.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            gridVariants.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            gridVariants.BackgroundColor = Color.White;
            gridVariants.EnableHeadersVisualStyles = false;
            gridVariants.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridVariants.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            gridVariants.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridVariants.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridVariants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridVariants.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            gridVariants.RowTemplate.Height = 30;
            gridVariants.AllowUserToAddRows = false;
            gridVariants.ReadOnly = true;
            gridVariants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Batch items grid
            var gridItems = dgvBatchItems;
            gridItems.BorderStyle = BorderStyle.None;
            gridItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            gridItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridItems.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            gridItems.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            gridItems.BackgroundColor = Color.White;
            gridItems.EnableHeadersVisualStyles = false;
            gridItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            gridItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridItems.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            gridItems.RowTemplate.Height = 30;
            gridItems.AllowUserToAddRows = false;
            gridItems.ReadOnly = true;
        }

        private async void btnSelectVariant_Click(object sender, EventArgs e)
        {
            try
            {
                panelVariantSelection.Visible = true;
                UIHelper.RoundPanelCorners(panelVariantSelection, 20);

                var variants = await _batchDL.GetVariantsForSelection();
                dgvVariantSelection.DataSource = variants;

                // Hide unnecessary columns
                if (dgvVariantSelection.Columns.Contains("variant_id"))
                    dgvVariantSelection.Columns["variant_id"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("purchase_batch_id"))
                    dgvVariantSelection.Columns["purchase_batch_id"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("purchase_batch_item_id"))
                    dgvVariantSelection.Columns["purchase_batch_item_id"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("quantity_received"))
                    dgvVariantSelection.Columns["quantity_received"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("cost_price"))
                    dgvVariantSelection.Columns["cost_price"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("line_total"))
                    dgvVariantSelection.Columns["line_total"].Visible = false;
                if (dgvVariantSelection.Columns.Contains("CreatedAt"))
                    dgvVariantSelection.Columns["CreatedAt"].Visible = false;

                // Rename headers
                if (dgvVariantSelection.Columns.Contains("product_name"))
                    dgvVariantSelection.Columns["product_name"].HeaderText = "Product";
                if (dgvVariantSelection.Columns.Contains("size"))
                    dgvVariantSelection.Columns["size"].HeaderText = "Size";
                if (dgvVariantSelection.Columns.Contains("class_type"))
                    dgvVariantSelection.Columns["class_type"].HeaderText = "Class";
                if (dgvVariantSelection.Columns.Contains("sale_price"))
                    dgvVariantSelection.Columns["sale_price"].HeaderText = "Sale Price";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading variants: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvVariantSelection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvVariantSelection.Rows[e.RowIndex];
                _selectedVariantId = Convert.ToInt32(row.Cells["variant_id"].Value);

                // Populate fields
                txtProduct.Text = row.Cells["product_name"].Value.ToString();
                txtSize.Text = row.Cells["size"].Value.ToString();
                txtClass.Text = row.Cells["class_type"].Value?.ToString() ?? "";
                txtSalePrice.Text = row.Cells["sale_price"].Value.ToString();

                // Clear other fields
                numQuantity.Value = 0;
                txtCostPrice.Text = "0";
                txtLineTotal.Text = "0.00";

                panelVariantSelection.Visible = false;

                // Focus on quantity
                numQuantity.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting variant: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void txtSearchVariant_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearchVariant.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    var allVariants = await _batchDL.GetVariantsForSelection();
                    dgvVariantSelection.DataSource = allVariants;
                }
                else
                {
                    var filteredVariants = await _batchDL.SearchVariantsForSelection(keyword);
                    dgvVariantSelection.DataSource = filteredVariants;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCloseVariantPanel_Click(object sender, EventArgs e)
        {
            panelVariantSelection.Visible = false;
        }

        private void CalculateLineTotal()
        {
            decimal quantity = numQuantity.Value;
            decimal costPrice = decimal.TryParse(txtCostPrice.Text, out var cp) ? cp : 0;
            decimal lineTotal = quantity * costPrice;
            txtLineTotal.Text = lineTotal.ToString("N2");
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            CalculateLineTotal();
        }

        private void txtCostPrice_TextChanged(object sender, EventArgs e)
        {
            CalculateLineTotal();
        }

        private async void btnAddItem_Click(object sender, EventArgs e)
        {
            if (_selectedVariantId == 0)
            {
                MessageBox.Show("Please select a product variant first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal costPrice = decimal.TryParse(txtCostPrice.Text, out var cp) ? cp : 0;
            if (costPrice <= 0)
            {
                MessageBox.Show("Cost price must be greater than zero.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_isEditingItem && _editingItemIndex >= 0)
                {
                    // UPDATE existing item
                    var item = _batchItems[_editingItemIndex];
                    item.quantity_received = numQuantity.Value;
                    item.cost_price = costPrice;
                    item.sale_price = decimal.TryParse(txtSalePrice.Text, out var sp) ? sp : 0;
                    item.line_total = numQuantity.Value * costPrice;

                    _isEditingItem = false;
                    _editingItemIndex = -1;
                    btnAddItem.Text = "Add to Batch";
                    btnAddItem.IconChar = FontAwesome.Sharp.IconChar.Plus;

                    MessageBox.Show("Item updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // ADD new item
                    var item = new PurchaseBatchItem
                    {
                        purchase_batch_item_id = await _batchDL.GetNextItemId(),
                        purchase_batch_id = _currentBatchId,
                        variant_id = _selectedVariantId,
                        product_name = txtProduct.Text,
                        size = txtSize.Text,
                        class_type = string.IsNullOrWhiteSpace(txtClass.Text) ? null : txtClass.Text,
                        quantity_received = numQuantity.Value,
                        cost_price = costPrice,
                        sale_price = decimal.TryParse(txtSalePrice.Text, out var sp) ? sp : 0,
                        line_total = numQuantity.Value * costPrice
                    };

                    _batchItems.Add(item);

                    MessageBox.Show("Item added to batch.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                RefreshBatchItemsGrid();
                CalculateTotals();
                ClearItemFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshBatchItemsGrid()
        {
            dgvBatchItems.DataSource = null;
            dgvBatchItems.DataSource = _batchItems.ToList();

            // Hide unnecessary columns
            if (dgvBatchItems.Columns.Contains("variant_id"))
                dgvBatchItems.Columns["variant_id"].Visible = false;
            if (dgvBatchItems.Columns.Contains("purchase_batch_id"))
                dgvBatchItems.Columns["purchase_batch_id"].Visible = false;
            if (dgvBatchItems.Columns.Contains("purchase_batch_item_id"))
                dgvBatchItems.Columns["purchase_batch_item_id"].Visible = false;
            if (dgvBatchItems.Columns.Contains("CreatedAt"))
                dgvBatchItems.Columns["CreatedAt"].Visible = false;

            // Rename headers
            if (dgvBatchItems.Columns.Contains("product_name"))
                dgvBatchItems.Columns["product_name"].HeaderText = "Product";
            if (dgvBatchItems.Columns.Contains("size"))
                dgvBatchItems.Columns["size"].HeaderText = "Size";
            if (dgvBatchItems.Columns.Contains("class_type"))
                dgvBatchItems.Columns["class_type"].HeaderText = "Class";
            if (dgvBatchItems.Columns.Contains("quantity_received"))
                dgvBatchItems.Columns["quantity_received"].HeaderText = "Quantity";
            if (dgvBatchItems.Columns.Contains("cost_price"))
                dgvBatchItems.Columns["cost_price"].HeaderText = "Cost Price";
            if (dgvBatchItems.Columns.Contains("sale_price"))
                dgvBatchItems.Columns["sale_price"].HeaderText = "Sale Price";
            if (dgvBatchItems.Columns.Contains("line_total"))
                dgvBatchItems.Columns["line_total"].HeaderText = "Line Total";

            UIHelper.AddButtonColumn(dgvBatchItems, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dgvBatchItems, "Remove", "Remove", "Remove");
        }

        private void CalculateTotals()
        {
            decimal calculatedTotal = _batchItems.Sum(x => x.line_total);

            // Only auto-update if field is empty or equals calculated total (user hasn't manually changed it)
            decimal currentTotal = decimal.TryParse(txtTotalAmount.Text, out var ct) ? ct : 0;

            // Auto-update total if it's zero or equals the previous calculated total
            if (currentTotal == 0 || Math.Abs(currentTotal - calculatedTotal) < 0.01m)
            {
                txtTotalAmount.Text = calculatedTotal.ToString("N2");
            }

            decimal total = decimal.TryParse(txtTotalAmount.Text, out var t) ? t : 0;
            decimal paid = decimal.TryParse(txtPaid.Text, out var p) ? p : 0;
            decimal remaining = total - paid;
            txtRemaining.Text = remaining.ToString("N2");

            // Update status
            if (remaining <= 0)
                cmbStatus.Text = "Completed";
            else if (paid > 0)
                cmbStatus.Text = "Partial";
            else
                cmbStatus.Text = "Pending";
        }

        private void txtPaid_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearItemFields();
            MessageBox.Show("Edit cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearItemFields()
        {
            _selectedVariantId = 0;
            _isEditingItem = false;
            _editingItemIndex = -1;
            txtProduct.Clear();
            txtSize.Clear();
            txtClass.Clear();
            txtSalePrice.Clear();
            txtCostPrice.Clear();
            numQuantity.Value = 0;
            txtLineTotal.Text = "0.00";
            btnAddItem.Text = "Add to Batch";
            btnAddItem.IconChar = FontAwesome.Sharp.IconChar.Plus;
            btnAddItem.BackColor = Color.FromArgb(40, 167, 69); // Green color
            btnCancelEdit.Visible = false;
        }

        private async void dgvBatchItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvBatchItems.Columns[e.ColumnIndex].Name;

            if (columnName == "Edit")
            {
                var item = _batchItems[e.RowIndex];

                // Populate fields for editing
                _selectedVariantId = item.variant_id;
                txtProduct.Text = item.product_name;
                txtSize.Text = item.size;
                txtClass.Text = item.class_type ?? "";
                txtSalePrice.Text = item.sale_price.ToString("N2");
                numQuantity.Value = item.quantity_received;
                txtCostPrice.Text = item.cost_price.ToString("N2");
                txtLineTotal.Text = item.line_total.ToString("N2");

                // Set editing mode
                _isEditingItem = true;
                _editingItemIndex = e.RowIndex;
                btnAddItem.Text = "Update Item";
                btnAddItem.IconChar = FontAwesome.Sharp.IconChar.Check;
                btnAddItem.BackColor = Color.FromArgb(255, 193, 7); // Orange color
                btnCancelEdit.Visible = true;

                MessageBox.Show("Edit the item details and click 'Update Item'.", "Edit Mode",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (columnName == "Remove")
            {
                var result = MessageBox.Show("Remove this item from batch?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _batchItems.RemoveAt(e.RowIndex);
                    RefreshBatchItemsGrid();
                    CalculateTotals();
                }
            }
        }

        private async void btnSaveBatch_Click(object sender, EventArgs e)
        {
            string batchName = txtBatchName.Text.Trim();
            string supplierName = cmbSupplier.Text.Trim();

            if (string.IsNullOrEmpty(batchName))
            {
                MessageBox.Show("Please enter batch name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(supplierName))
            {
                MessageBox.Show("Please select a supplier.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_batchItems.Count == 0)
            {
                MessageBox.Show("Please add at least one item to the batch.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int supplierId = await _productsDL.GetSupplierIdByName(supplierName);
                if (supplierId == -1)
                {
                    MessageBox.Show("Invalid supplier selected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal totalPrice = decimal.TryParse(txtTotalAmount.Text, out var tp) ? tp : 0;
                decimal paid = decimal.TryParse(txtPaid.Text, out var p) ? p : 0;

                var batch = new     PurchaseBatch
                {
                    batch_id = _currentBatchId,
                    supplier_id = supplierId,
                    BatchName = batchName,
                    total_price = totalPrice,
                    paid = paid,
                    status = cmbStatus.Text
                };

                bool batchSuccess;
                if (_isLoadingExistingBatch)
                {
                    batchSuccess = await _batchDL.UpdateBatch(batch);
                }
                else
                {
                    batchSuccess = await _batchDL.AddBatch(batch);
                }

                if (batchSuccess)
                {
                    // Save all batch items
                    foreach (var item in _batchItems)
                    {
                        item.purchase_batch_id = _currentBatchId;
                        await _batchDL.AddBatchItem(item);
                    }

                    MessageBox.Show("Batch saved successfully! Stock has been updated.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save batch.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving batch: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cmbSupplier_TextChanged(object sender, EventArgs e)
        {
            _ = BindSuppliers(cmbSupplier.Text);
        }
    }
}