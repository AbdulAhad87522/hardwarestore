using HardWareStore.DL;
using HardWareStore.Interfaces;
using HardWareStore.Models;
using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class InventoryMain : Form
    {
        private readonly IInventoryDL _inventoryDL;
        private readonly IProductsDL _productsDL;
        private readonly IVariantsDL _variantsDL;

        private int _selectedProductId = -1;
        private int _selectedVariantId = -1;
        private bool _isEditingProduct = false; // true = editing product, false = editing variant

        public InventoryMain(IInventoryDL inventoryDl, IProductsDL productsDL, IVariantsDL variantsDL)
        {
            InitializeComponent();

            // Pehlay variables assign karein (Zaroori)
            _inventoryDL = inventoryDl;
            _productsDL = productsDL;
            _variantsDL = variantsDL;

            // Ab methods call karein
            LoadInventory();
            CustomizeGrid();

            panelEditProduct.Visible = false;
            panelEditVariant.Visible = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Control | Keys.P))
                {
                    btnAddProduct.PerformClick();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.V))
                {
                    btnAddVariant.PerformClick();
                    return true;
                }
                else if (keyData == Keys.Delete)
                {
                    btnDelete.PerformClick();
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    if (panelEditProduct.Visible)
                    {
                        panelEditProduct.Visible = false;
                        ClearProductFields();
                    }
                    if (panelEditVariant.Visible)
                    {
                        panelEditVariant.Visible = false;
                        ClearVariantFields();
                    }
                    return true;
                }
                else if (keyData == Keys.F5)
                {
                    LoadInventory();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in keyboard shortcut: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void LoadInventory()
        {
            try
            {
                var inventory = await _inventoryDL.GetAllInventory();
                dataGridView1.DataSource = inventory;
                UIHelper.AddButtonColumn(dataGridView1, "EditProduct", "Edit Product", "Edit Product");
                UIHelper.AddButtonColumn(dataGridView1, "EditVariant", "Edit Variant", "Edit Variant");
                CustomizeGrid();
                UpdateStockStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InventoryMain_Load(object sender, EventArgs e)
        {
            BindCategories();
            BindSuppliers();
            BindProducts();
            LoadUnitOfMeasures();
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

        private async void BindProducts(string keyword = "")
        {
            try
            {
                var products = await _productsDL.GetProductNames(keyword);
                cmbProductForVariant.Items.Clear();
                cmbProductForVariant.Items.AddRange(products.ToArray());

                cmbProductForVariant.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbProductForVariant.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            grid.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;

            // Rename headers for better display
            if (grid.Columns.Contains("product_name")) grid.Columns["product_name"].HeaderText = "Product";
            if (grid.Columns.Contains("supplier_name")) grid.Columns["supplier_name"].HeaderText = "Supplier";
            if (grid.Columns.Contains("description")) grid.Columns["description"].HeaderText = "Description";
            if (grid.Columns.Contains("size")) grid.Columns["size"].HeaderText = "Size";
            if (grid.Columns.Contains("class_type")) grid.Columns["class_type"].HeaderText = "Class";
            if (grid.Columns.Contains("unit_of_measure")) grid.Columns["unit_of_measure"].HeaderText = "Unit";
            if (grid.Columns.Contains("price_per_unit")) grid.Columns["price_per_unit"].HeaderText = "Price/Unit";
            if (grid.Columns.Contains("price_per_lenght")) grid.Columns["price_per_lenght"].HeaderText = "Price/Length";
            if (grid.Columns.Contains("lenght_in_feet")) grid.Columns["lenght_in_feet"].HeaderText = "Length (FT)";
            if (grid.Columns.Contains("quantity_in_stock")) grid.Columns["quantity_in_stock"].HeaderText = "Stock";
            if (grid.Columns.Contains("reorder_level")) grid.Columns["reorder_level"].HeaderText = "Reorder";
            if (grid.Columns.Contains("minimum_order_quantity")) grid.Columns["minimum_order_quantity"].HeaderText = "Min Qty";
            if (grid.Columns.Contains("is_active")) grid.Columns["is_active"].HeaderText = "Active";

            // Apply color coding for stock levels
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells["quantity_in_stock"].Value != null && row.Cells["reorder_level"].Value != null)
                {
                    decimal stock = Convert.ToDecimal(row.Cells["quantity_in_stock"].Value);
                    decimal reorder = Convert.ToDecimal(row.Cells["reorder_level"].Value);

                    if (stock == 0)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 200, 200); // Light red
                    }
                    else if (stock <= reorder)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 240, 200); // Light orange
                    }
                }
            }
        }

        private void UpdateStockStatusLabel()
        {
            try
            {
                int totalItems = dataGridView1.Rows.Count;
                int lowStock = 0;
                int outOfStock = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["quantity_in_stock"].Value != null && row.Cells["reorder_level"].Value != null)
                    {
                        decimal stock = Convert.ToDecimal(row.Cells["quantity_in_stock"].Value);
                        decimal reorder = Convert.ToDecimal(row.Cells["reorder_level"].Value);

                        if (stock == 0)
                            outOfStock++;
                        else if (stock <= reorder)
                            lowStock++;
                    }
                }

                lblStockStatus.Text = $"Total Items: {totalItems} | Low Stock: {lowStock} | Out of Stock: {outOfStock}";
            }
            catch { }
        }

        #region Product Operations

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            ClearProductFields();
            panelEditProduct.Visible = true;
            panelEditVariant.Visible = false;
            UIHelper.RoundPanelCorners(panelEditProduct, 20);

            lblProductTitle.Text = "Add New Product";
            btnSaveProduct.Text = "Save Product";
        }

        private async void btnSaveProduct_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
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

                bool success;
                string successMessage;

                if (_selectedProductId == -1)
                {
                    // ADD mode
                    var product = new Products
                    {
                        Name = name,
                        category_id = categoryId,
                        supplier_id = supplierId,
                        description = description,
                        has_variants = chkHasVariants.Checked,
                        is_active = true
                    };

                    success = await _productsDL.AddProduct(product);
                    successMessage = "Product added successfully!";
                }
                else
                {
                    // UPDATE mode
                    var product = new Products
                    {
                        product_id = _selectedProductId,
                        Name = name,
                        category_id = categoryId,
                        supplier_id = supplierId,
                        description = description,
                        has_variants = chkHasVariants.Checked,
                        is_active = chkProductActive.Checked
                    };

                    success = await _productsDL.UpdateProduct(product);
                    successMessage = "Product updated successfully!";
                }

                if (success)
                {
                    MessageBox.Show(successMessage, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInventory();
                    panelEditProduct.Visible = false;
                    ClearProductFields();
                }
                else
                {
                    MessageBox.Show("Failed to save product.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving product: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelProduct_Click(object sender, EventArgs e)
        {
            panelEditProduct.Visible = false;
            ClearProductFields();
        }

        private void ClearProductFields()
        {
            _selectedProductId = -1;
            txtProductName.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbSupplier.SelectedIndex = -1;
            chkHasVariants.Checked = false;
            chkProductActive.Checked = true;
        }

        #endregion

        #region Variant Operations

        private void btnAddVariant_Click(object sender, EventArgs e)
        {
            ClearVariantFields();
            panelEditVariant.Visible = true;
            panelEditProduct.Visible = false;
            UIHelper.RoundPanelCorners(panelEditVariant, 20);

            lblVariantTitle.Text = "Add New Variant";
            btnSaveVariant.Text = "Save Variant";
        }

        private async void btnSaveVariant_Click(object sender, EventArgs e)
        {
            string productName = cmbProductForVariant.Text.Trim();
            string size = txtSize.Text.Trim();
            string classType = txtClassType.Text.Trim();
            string unit = cmbUnitOfMeasure.Text;
            decimal pricePerUnit = numPricePerUnit.Value;
            decimal pricePerLength = numPricePerLength.Value;
            decimal lengthInFeet = numLengthInFeet.Value;
            decimal stock = numStock.Value;
            decimal reorderLevel = numReorderLevel.Value;
            decimal minOrderQty = numMinOrderQty.Value;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(size) ||
                string.IsNullOrWhiteSpace(unit) || pricePerUnit <= 0)
            {
                MessageBox.Show("Please fill all required fields (Product, Size, Unit, Price per Unit).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int productId = await _productsDL.GetProductIdByName(productName);
                if (productId == -1)
                {
                    MessageBox.Show("Invalid product selected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool success;
                string successMessage;

                if (_selectedVariantId == -1)
                {
                    // ADD mode
                    var variant = new Variants
                    {
                        product_id = productId,
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
                        variant_id = _selectedVariantId,
                        product_id = productId,
                        size = size,
                        class_type = string.IsNullOrWhiteSpace(classType) ? null : classType,
                        unit_of_measure = unit,
                        price_per_unit = pricePerUnit,
                        price_per_lenght = pricePerLength,
                        lenght_in_feet = lengthInFeet,
                        quantity_in_stock = stock,
                        reorder_level = reorderLevel,
                        minimum_order_quantity = minOrderQty,
                        is_active = chkVariantActive.Checked
                    };

                    success = await _variantsDL.UpdateVariant(variant);
                    successMessage = "Variant updated successfully!";
                }

                if (success)
                {
                    MessageBox.Show(successMessage, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInventory();
                    panelEditVariant.Visible = false;
                    ClearVariantFields();
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

        private void btnCancelVariant_Click(object sender, EventArgs e)
        {
            panelEditVariant.Visible = false;
            ClearVariantFields();
        }

        private void ClearVariantFields()
        {
            _selectedVariantId = -1;
            cmbProductForVariant.SelectedIndex = -1;
            txtSize.Clear();
            txtClassType.Clear();
            cmbUnitOfMeasure.SelectedIndex = 0;
            numPricePerUnit.Value = 0;
            numPricePerLength.Value = 0;
            numLengthInFeet.Value = 0;
            numStock.Value = 0;
            numReorderLevel.Value = 0;
            numMinOrderQty.Value = 1;
            chkVariantActive.Checked = true;
        }

        #endregion

        #region Grid Operations

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dataGridView1.Rows[e.RowIndex];
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "EditProduct")
            {
                // Edit Product - need to fetch the product details
                string productName = row.Cells["product_name"].Value.ToString();
                var product = await _productsDL.GetProductIdByName(productName);

                if (product != -1)
                {
                    var productDetails = await _productsDL.GetProductById(product);
                    if (productDetails != null)
                    {
                        _selectedProductId = productDetails.product_id;
                        txtProductName.Text = productDetails.Name;
                        txtDescription.Text = productDetails.description ?? "";

                        // Set category
                        var categories = await _productsDL.GetCategoryNames();
                        foreach (var cat in categories)
                        {
                            if (await _productsDL.GetCategoryIdByName(cat) == productDetails.category_id)
                            {
                                cmbCategory.Text = cat;
                                break;
                            }
                        }

                        // Set supplier
                        if (productDetails.supplier_id > 0)
                        {
                            var suppliers = await _productsDL.GetSupplierNames();
                            foreach (var sup in suppliers)
                            {
                                if (await _productsDL.GetSupplierIdByName(sup) == productDetails.supplier_id)
                                {
                                    cmbSupplier.Text = sup;
                                    break;
                                }
                            }
                        }

                        chkHasVariants.Checked = productDetails.has_variants;
                        chkProductActive.Checked = productDetails.is_active;

                        panelEditProduct.Visible = true;
                        panelEditVariant.Visible = false;
                        UIHelper.RoundPanelCorners(panelEditProduct, 20);

                        lblProductTitle.Text = "Edit Product";
                        btnSaveProduct.Text = "Update Product";
                    }
                }
            }
            else if (columnName == "EditVariant")
            {
                // Edit Variant - fetch variant details from the current row
                string productName = row.Cells["product_name"].Value.ToString();
                string size = row.Cells["size"].Value.ToString();
                string classType = row.Cells["class_type"].Value?.ToString();

                int productId = await _productsDL.GetProductIdByName(productName);
                var variant = await _variantsDL.GetVariantByProductAndSize(productId, size, classType);

                if (variant != null)
                {
                    _selectedVariantId = variant.variant_id;
                    cmbProductForVariant.Text = productName;
                    txtSize.Text = variant.size;
                    txtClassType.Text = variant.class_type ?? "";
                    cmbUnitOfMeasure.Text = variant.unit_of_measure;
                    numPricePerUnit.Value = variant.price_per_unit;
                    numPricePerLength.Value = variant.price_per_lenght;
                    numLengthInFeet.Value = variant.lenght_in_feet;
                    numStock.Value = variant.quantity_in_stock;
                    numReorderLevel.Value = variant.reorder_level;
                    numMinOrderQty.Value = variant.minimum_order_quantity;
                    chkVariantActive.Checked = variant.is_active;

                    panelEditVariant.Visible = true;
                    panelEditProduct.Visible = false;
                    UIHelper.RoundPanelCorners(panelEditVariant, 20);

                    lblVariantTitle.Text = "Edit Variant";
                    btnSaveVariant.Text = "Update Variant";
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select an item to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridView1.CurrentRow;
            string productName = row.Cells["product_name"].Value.ToString();
            string size = row.Cells["size"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "What would you like to delete?",
                "Delete Confirmation",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            // Custom dialog would be better, but using Yes=Product, No=Variant, Cancel=Cancel
            var deleteChoice = MessageBox.Show(
                $"Delete:\n\nYes = Delete entire product '{productName}'\nNo = Delete only variant '{size}'\nCancel = Cancel operation",
                "Delete Options",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (deleteChoice == DialogResult.Cancel)
                return;

            try
            {
                bool success = false;

                if (deleteChoice == DialogResult.Yes)
                {
                    // Delete Product
                    int productId = await _productsDL.GetProductIdByName(productName);
                    success = await _productsDL.DeleteProduct(productId);

                    if (success)
                        MessageBox.Show("Product deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (deleteChoice == DialogResult.No)
                {
                    // Delete Variant
                    int productId = await _productsDL.GetProductIdByName(productName);
                    string classType = row.Cells["class_type"].Value?.ToString();
                    var variant = await _variantsDL.GetVariantByProductAndSize(productId, size, classType);

                    if (variant != null)
                    {
                        success = await _variantsDL.DeleteVariant(variant.variant_id);

                        if (success)
                            MessageBox.Show("Variant deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (success)
                    LoadInventory();
                else
                    MessageBox.Show("Failed to delete item.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting item: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Filter Operations

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    LoadInventory();
                }
                else
                {
                    var inventory = await _inventoryDL.SearchInventory(keyword);
                    dataGridView1.DataSource = inventory;
                    UIHelper.AddButtonColumn(dataGridView1, "EditProduct", "Edit Product", "Edit Product");
                    UIHelper.AddButtonColumn(dataGridView1, "EditVariant", "Edit Variant", "Edit Variant");
                    CustomizeGrid();
                    UpdateStockStatusLabel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching inventory: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLowStock_Click(object sender, EventArgs e)
        {
            try
            {
                var inventory = await _inventoryDL.GetLowStockInventory();
                dataGridView1.DataSource = inventory;
                UIHelper.AddButtonColumn(dataGridView1, "EditProduct", "Edit Product", "Edit Product");
                UIHelper.AddButtonColumn(dataGridView1, "EditVariant", "Edit Variant", "Edit Variant");
                CustomizeGrid();
                UpdateStockStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading low stock items: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnOutOfStock_Click(object sender, EventArgs e)
        {
            try
            {
                var inventory = await _inventoryDL.GetOutOfStockInventory();
                dataGridView1.DataSource = inventory;
                UIHelper.AddButtonColumn(dataGridView1, "EditProduct", "Edit Product", "Edit Product");
                UIHelper.AddButtonColumn(dataGridView1, "EditVariant", "Edit Variant", "Edit Variant");
                CustomizeGrid();
                UpdateStockStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading out of stock items: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        #endregion

        #region ComboBox Events

        private void cmbCategory_TextChanged(object sender, EventArgs e)
        {
            BindCategories(cmbCategory.Text);
        }

        private void cmbSupplier_TextChanged(object sender, EventArgs e)
        {
            BindSuppliers(cmbSupplier.Text);
        }

        private void cmbProductForVariant_TextChanged(object sender, EventArgs e)
        {
            BindProducts(cmbProductForVariant.Text);
        }

        #endregion
    }
}