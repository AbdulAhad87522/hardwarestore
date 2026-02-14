using HardWareStore.DL;
using HardWareStore.Models;
using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class VariantsMain : Form
    {
        private readonly VariantsDL _variantsDL = new VariantsDL();
        private readonly ProductsDL _productsDL = new ProductsDL();
        private int _selectedId = -1;
        private int _productId;
        private string _productName;

        public VariantsMain(int productId, string productName)
        {
            InitializeComponent();
            _productId = productId;
            _productName = productName;
            lblProductName.Text = $"Managing Variants for: {productName}";
            LoadVariants();
            CustomizeGrid();
            panelEdit.Visible = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Control | Keys.A))
                {
                    btnAdd.PerformClick();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.E))
                {
                    if (dataGridView1.CurrentRow != null)
                    {
                        dataGridView1_CellContentClick(this,
                            new DataGridViewCellEventArgs(dataGridView1.Columns["Edit"].Index,
                            dataGridView1.CurrentRow.Index));
                    }
                    return true;
                }
                else if (keyData == Keys.Delete)
                {
                    btnDelete.PerformClick();
                    return true;
                }
                else if (keyData == Keys.Escape && panelEdit.Visible)
                {
                    panelEdit.Visible = false;
                    ClearFields();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in event listener: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void LoadVariants()
        {
            try
            {
                var variants = await _variantsDL.GetVariantsByProductId(_productId);
                dataGridView1.DataSource = variants;
                UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                CustomizeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading variants: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VariantsMain_Load(object sender, EventArgs e)
        {
            LoadUnitOfMeasures();
        }

        private void LoadUnitOfMeasures()
        {
            var units = _variantsDL.GetUnitOfMeasures();
            cmbUnitOfMeasure.Items.Clear();
            cmbUnitOfMeasure.Items.AddRange(units.ToArray());
            cmbUnitOfMeasure.SelectedIndex = 0;
        }

        private void CustomizeGrid()
        {
            var grid = dataGridView1;
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

            // Hide ID columns
            if (grid.Columns.Contains("variant_id")) grid.Columns["variant_id"].Visible = false;
            if (grid.Columns.Contains("product_id")) grid.Columns["product_id"].Visible = false;
            if (grid.Columns.Contains("product_name")) grid.Columns["product_name"].Visible = false;
            if (grid.Columns.Contains("CeratedAt")) grid.Columns["CeratedAt"].Visible = false;
            if (grid.Columns.Contains("UpdatedAt")) grid.Columns["UpdatedAt"].Visible = false;

            // Rename headers
            if (grid.Columns.Contains("size")) grid.Columns["size"].HeaderText = "Size";
            if (grid.Columns.Contains("class_type")) grid.Columns["class_type"].HeaderText = "Class";
            if (grid.Columns.Contains("unit_of_measure")) grid.Columns["unit_of_measure"].HeaderText = "Unit";
            if (grid.Columns.Contains("price_per_unit")) grid.Columns["price_per_unit"].HeaderText = "Price/Unit";
            if (grid.Columns.Contains("price_per_lenght")) grid.Columns["price_per_lenght"].HeaderText = "Price/Length";
            if (grid.Columns.Contains("lenght_in_feet")) grid.Columns["lenght_in_feet"].HeaderText = "Length (FT)";
            if (grid.Columns.Contains("quantity_in_stock")) grid.Columns["quantity_in_stock"].HeaderText = "Stock";
            if (grid.Columns.Contains("reorder_level")) grid.Columns["reorder_level"].HeaderText = "Reorder Level";
            if (grid.Columns.Contains("minimum_order_quantity")) grid.Columns["minimum_order_quantity"].HeaderText = "Min Order Qty";
            if (grid.Columns.Contains("is_active")) grid.Columns["is_active"].HeaderText = "Active";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Clear fields and show panel for new variant entry
            ClearFields();
            panelEdit.Visible = true;
            UIHelper.RoundPanelCorners(panelEdit, 20);

            // Change panel title for "Add" mode
            label3.Text = "Add New Variant";
            btnUpdate.Text = "Save";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dataGridView1.Rows[e.RowIndex];
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Edit")
            {
                _selectedId = Convert.ToInt32(row.Cells["variant_id"].Value);
                txtSize.Text = row.Cells["size"].Value.ToString();
                txtClassType.Text = row.Cells["class_type"].Value?.ToString() ?? "";
                cmbUnitOfMeasure.Text = row.Cells["unit_of_measure"].Value.ToString();
                numPricePerUnit.Value = Convert.ToDecimal(row.Cells["price_per_unit"].Value);
                numPricePerLength.Value = Convert.ToDecimal(row.Cells["price_per_lenght"].Value);
                numLengthInFeet.Value = Convert.ToDecimal(row.Cells["lenght_in_feet"].Value);
                numStock.Value = Convert.ToDecimal(row.Cells["quantity_in_stock"].Value);
                numReorderLevel.Value = Convert.ToDecimal(row.Cells["reorder_level"].Value);
                numMinOrderQty.Value = Convert.ToDecimal(row.Cells["minimum_order_quantity"].Value);
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["is_active"].Value);

                panelEdit.Visible = true;
                UIHelper.RoundPanelCorners(panelEdit, 20);

                // Change panel title for "Edit" mode
                label3.Text = "Edit Variant / Size";
                btnUpdate.Text = "Update";
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string size = txtSize.Text.Trim();
            string classType = txtClassType.Text.Trim();
            string unit = cmbUnitOfMeasure.Text;
            decimal pricePerUnit = numPricePerUnit.Value;
            decimal pricePerLength = numPricePerLength.Value;
            decimal lengthInFeet = numLengthInFeet.Value;
            decimal stock = numStock.Value;
            decimal reorderLevel = numReorderLevel.Value;
            decimal minOrderQty = numMinOrderQty.Value;

            if (string.IsNullOrWhiteSpace(size) || string.IsNullOrWhiteSpace(unit) || pricePerUnit <= 0)
            {
                MessageBox.Show("Please fill all required fields (Size, Unit, Price per Unit).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success;
                string successMessage;

                if (_selectedId == -1)
                {
                    // ADD mode
                    var variant = new Variants
                    {
                        product_id = _productId,
                        size = size,
                        class_type = string.IsNullOrWhiteSpace(classType) ? null : classType,
                        unit_of_measure = unit,
                        price_per_unit = pricePerUnit,
                        price_per_lenght = pricePerLength,
                        lenght_in_feet = lengthInFeet,
                        quantity_in_stock = stock,
                        reorder_level = reorderLevel,
                        minimum_order_quantity = minOrderQty,
                        is_active = true
                    };

                    success = await _variantsDL.AddVariant(variant);
                    successMessage = "Variant added successfully!";
                }
                else
                {
                    // UPDATE mode
                    var variant = new Variants
                    {
                        variant_id = _selectedId,
                        product_id = _productId,
                        size = size,
                        class_type = string.IsNullOrWhiteSpace(classType) ? null : classType,
                        unit_of_measure = unit,
                        price_per_unit = pricePerUnit,
                        price_per_lenght = pricePerLength,
                        lenght_in_feet = lengthInFeet,
                        quantity_in_stock = stock,
                        reorder_level = reorderLevel,
                        minimum_order_quantity = minOrderQty,
                        is_active = chkIsActive.Checked
                    };

                    success = await _variantsDL.UpdateVariant(variant);
                    successMessage = "Variant updated successfully!";
                }

                if (success)
                {
                    MessageBox.Show(successMessage, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVariants();
                    panelEdit.Visible = false;
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to save variant.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving variant: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a variant to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["variant_id"].Value);
            string size = dataGridView1.CurrentRow.Cells["size"].Value.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete variant '{size}'?",
                "Delete Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool success = await _variantsDL.DeleteVariant(id);
                    if (success)
                    {
                        MessageBox.Show("Variant deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVariants();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete variant.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting variant: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    LoadVariants();
                }
                else
                {
                    var variants = await _variantsDL.SearchVariants(keyword);
                    // Filter by product ID
                    var filtered = variants.FindAll(v => v.product_id == _productId);
                    dataGridView1.DataSource = filtered;
                    UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                    CustomizeGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching variants: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelEdit.Visible = false;
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedId = -1;
            txtSize.Clear();
            txtClassType.Clear();
            cmbUnitOfMeasure.SelectedIndex = 0;
            numPricePerUnit.Value = 0;
            numPricePerLength.Value = 0;
            numLengthInFeet.Value = 0;
            numStock.Value = 0;
            numReorderLevel.Value = 0;
            numMinOrderQty.Value = 1;
            chkIsActive.Checked = true;
        }

        private async void btnLowStock_Click(object sender, EventArgs e)
        {
            try
            {
                var variants = await _variantsDL.GetLowStockVariants();
                // Filter by product ID
                var filtered = variants.FindAll(v => v.product_id == _productId);
                dataGridView1.DataSource = filtered;
                UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                CustomizeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading low stock variants: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnOutOfStock_Click(object sender, EventArgs e)
        {
            try
            {
                var variants = await _variantsDL.GetOutOfStockVariants();
                // Filter by product ID
                var filtered = variants.FindAll(v => v.product_id == _productId);
                dataGridView1.DataSource = filtered;
                UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                CustomizeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading out of stock variants: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            LoadVariants();
        }
    }
}