using System;
using System.Data;
using System.Windows.Forms;
using HardWareStore.Models;
using HardWareStore.DL;

namespace HardWareStore.UI
{
    public partial class ProductsMain : Form
    {
        private readonly ProductsDL _productsDL = new ProductsDL();
        private readonly DatabaseHelper _db = DatabaseHelper.Instance;
        private int _selectedId = -1;

        public ProductsMain()
        {
            InitializeComponent();
            LoadProducts();
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

        private async void LoadProducts()
        {
            try
            {
                var products = await _productsDL.GetActiveProducts();
                dataGridView1.DataSource = products;
                UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                CustomizeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProductsMain_Load(object sender, EventArgs e)
        {
            BindCategories();
            BindSuppliers();
        }

        private async void BindCategories(string keyword = "")
        {
            try
            {
                var categories = await _productsDL.GetCategoryNames(keyword);
                cmbCategory.Items.Clear();
                cmbCategory.Items.AddRange(categories.ToArray());

                cmbCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BindSuppliers(string keyword = "")
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
                MessageBox.Show("Error loading suppliers: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (grid.Columns.Contains("product_id")) grid.Columns["product_id"].Visible = false;
            if (grid.Columns.Contains("category_id")) grid.Columns["category_id"].Visible = false;
            if (grid.Columns.Contains("supplier_id")) grid.Columns["supplier_id"].Visible = false;
            if (grid.Columns.Contains("CreatedAt")) grid.Columns["CreatedAt"].Visible = false;
            if (grid.Columns.Contains("UpdatedAt")) grid.Columns["UpdatedAt"].Visible = false;

            // Rename headers for better display
            if (grid.Columns.Contains("Name")) grid.Columns["Name"].HeaderText = "Product Name";
            if (grid.Columns.Contains("supplier_name")) grid.Columns["supplier_name"].HeaderText = "Supplier";
            if (grid.Columns.Contains("description")) grid.Columns["description"].HeaderText = "Description";
            if (grid.Columns.Contains("has_variants")) grid.Columns["has_variants"].HeaderText = "Has Sizes";
            if (grid.Columns.Contains("is_active")) grid.Columns["is_active"].HeaderText = "Active";
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string categoryName = cmbCategory.Text.Trim();
            string supplierName = cmbSupplier.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Please fill all required fields (Product Name and Category).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int categoryId = await _productsDL.GetCategoryIdByName(categoryName);
                int supplierId = 0;

                if (!string.IsNullOrWhiteSpace(supplierName))
                {
                    supplierId = await _productsDL.GetSupplierIdByName(supplierName);
                }

                if (categoryId == -1)
                {
                    MessageBox.Show("Invalid category selected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var product = new Products
                {
                    Name = name,
                    category_id = categoryId,
                    supplier_id = supplierId,
                    description = description,
                    has_variants = chkHasVariants.Checked,
                    is_active = true
                };

                bool success = await _productsDL.AddProduct(product);

                if (success)
                {
                    MessageBox.Show("Product added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to add product.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dataGridView1.Rows[e.RowIndex];
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Edit")
            {
                _selectedId = Convert.ToInt32(row.Cells["product_id"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtDescription.Text = row.Cells["description"].Value?.ToString() ?? "";

                // Set combobox values
                if (row.Cells["supplier_name"].Value != null)
                    cmbSupplier.Text = row.Cells["supplier_name"].Value.ToString();

                chkHasVariants.Checked = Convert.ToBoolean(row.Cells["has_variants"].Value);
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["is_active"].Value);

                panelEdit.Visible = true;
                UIHelper.RoundPanelCorners(panelEdit, 20);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId == -1)
            {
                MessageBox.Show("No product selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();
            string categoryName = cmbCategory.Text.Trim();
            string supplierName = cmbSupplier.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Please fill all required fields (Product Name and Category).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int categoryId = await _productsDL.GetCategoryIdByName(categoryName);
                int supplierId = 0;

                if (!string.IsNullOrWhiteSpace(supplierName))
                {
                    supplierId = await _productsDL.GetSupplierIdByName(supplierName);
                }

                if (categoryId == -1)
                {
                    MessageBox.Show("Invalid category selected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var product = new Products
                {
                    product_id = _selectedId,
                    Name = name,
                    category_id = categoryId,
                    supplier_id = supplierId,
                    description = description,
                    has_variants = chkHasVariants.Checked,
                    is_active = chkIsActive.Checked
                };

                bool success = await _productsDL.UpdateProduct(product);

                if (success)
                {
                    MessageBox.Show("Product updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    panelEdit.Visible = false;
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to update product.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a product to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["product_id"].Value);
            string name = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete product '{name}'?",
                "Delete Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool success = await _productsDL.DeleteProduct(id);
                    if (success)
                    {
                        MessageBox.Show("Product deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting product: " + ex.Message, "Error",
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
                    LoadProducts();
                }
                else
                {
                    var products = await _productsDL.SearchProducts(keyword);
                    dataGridView1.DataSource = products;
                    UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
                    CustomizeGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching products: " + ex.Message, "Error",
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
            txtName.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbSupplier.SelectedIndex = -1;
            chkHasVariants.Checked = false;
            chkIsActive.Checked = true;
        }

        private void cmbCategory_TextChanged(object sender, EventArgs e)
        {
            BindCategories(cmbCategory.Text);
        }

        private void cmbSupplier_TextChanged(object sender, EventArgs e)
        {
            BindSuppliers(cmbSupplier.Text);
        }

        private void btnManageVariants_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a product to manage variants.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["product_id"].Value);
            string productName = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();

            VariantsMain variantsForm = new VariantsMain(productId, productName);
            variantsForm.ShowDialog();
        }
    }
}