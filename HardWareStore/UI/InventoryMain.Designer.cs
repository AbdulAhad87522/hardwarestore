namespace HardWareStore.UI
{
    partial class InventoryMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStockStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnShowAll = new FontAwesome.Sharp.IconButton();
            this.btnOutOfStock = new FontAwesome.Sharp.IconButton();
            this.btnLowStock = new FontAwesome.Sharp.IconButton();
            this.btnDelete = new FontAwesome.Sharp.IconButton();
            this.btnAddVariant = new FontAwesome.Sharp.IconButton();
            this.btnAddProduct = new FontAwesome.Sharp.IconButton();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelEditProduct = new System.Windows.Forms.Panel();
            this.chkProductActive = new System.Windows.Forms.CheckBox();
            this.chkHasVariants = new System.Windows.Forms.CheckBox();
            this.btnCancelProduct = new FontAwesome.Sharp.IconButton();
            this.btnSaveProduct = new FontAwesome.Sharp.IconButton();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblProductTitle = new System.Windows.Forms.Label();
            this.panelEditVariant = new System.Windows.Forms.Panel();
            this.chkVariantActive = new System.Windows.Forms.CheckBox();
            this.numMinOrderQty = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numReorderLevel = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numLengthInFeet = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numPricePerLength = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numPricePerUnit = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCancelVariant = new FontAwesome.Sharp.IconButton();
            this.btnSaveVariant = new FontAwesome.Sharp.IconButton();
            this.cmbUnitOfMeasure = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtClassType = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbProductForVariant = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.lblVariantTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelEditProduct.SuspendLayout();
            this.panelEditVariant.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinOrderQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLengthInFeet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPricePerLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPricePerUnit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.panel1.Controls.Add(this.lblStockStatus);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1867, 86);
            this.panel1.TabIndex = 0;
            // 
            // lblStockStatus
            // 
            this.lblStockStatus.AutoSize = true;
            this.lblStockStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStockStatus.ForeColor = System.Drawing.Color.White;
            this.lblStockStatus.Location = new System.Drawing.Point(16, 55);
            this.lblStockStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStockStatus.Name = "lblStockStatus";
            this.lblStockStatus.Size = new System.Drawing.Size(279, 23);
            this.lblStockStatus.TabIndex = 1;
            this.lblStockStatus.Text = "Total Items: 0 | Low Stock: 0 | Out: 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inventory Management";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnShowAll);
            this.panel2.Controls.Add(this.btnOutOfStock);
            this.panel2.Controls.Add(this.btnLowStock);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAddVariant);
            this.panel2.Controls.Add(this.btnAddProduct);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 86);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.panel2.Size = new System.Drawing.Size(1867, 86);
            this.panel2.TabIndex = 1;
            // 
            // btnShowAll
            // 
            this.btnShowAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnShowAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnShowAll.ForeColor = System.Drawing.Color.White;
            this.btnShowAll.IconChar = FontAwesome.Sharp.IconChar.List;
            this.btnShowAll.IconColor = System.Drawing.Color.White;
            this.btnShowAll.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnShowAll.IconSize = 20;
            this.btnShowAll.Location = new System.Drawing.Point(1299, 18);
            this.btnShowAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(120, 49);
            this.btnShowAll.TabIndex = 7;
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShowAll.UseVisualStyleBackColor = false;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // btnOutOfStock
            // 
            this.btnOutOfStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnOutOfStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutOfStock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOutOfStock.ForeColor = System.Drawing.Color.White;
            this.btnOutOfStock.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
            this.btnOutOfStock.IconColor = System.Drawing.Color.White;
            this.btnOutOfStock.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnOutOfStock.IconSize = 20;
            this.btnOutOfStock.Location = new System.Drawing.Point(1139, 18);
            this.btnOutOfStock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOutOfStock.Name = "btnOutOfStock";
            this.btnOutOfStock.Size = new System.Drawing.Size(147, 49);
            this.btnOutOfStock.TabIndex = 6;
            this.btnOutOfStock.Text = "Out Stock";
            this.btnOutOfStock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOutOfStock.UseVisualStyleBackColor = false;
            this.btnOutOfStock.Click += new System.EventHandler(this.btnOutOfStock_Click);
            // 
            // btnLowStock
            // 
            this.btnLowStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnLowStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLowStock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLowStock.ForeColor = System.Drawing.Color.Black;
            this.btnLowStock.IconChar = FontAwesome.Sharp.IconChar.CircleExclamation;
            this.btnLowStock.IconColor = System.Drawing.Color.Black;
            this.btnLowStock.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLowStock.IconSize = 20;
            this.btnLowStock.Location = new System.Drawing.Point(979, 18);
            this.btnLowStock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLowStock.Name = "btnLowStock";
            this.btnLowStock.Size = new System.Drawing.Size(147, 49);
            this.btnLowStock.TabIndex = 5;
            this.btnLowStock.Text = "Low Stock";
            this.btnLowStock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLowStock.UseVisualStyleBackColor = false;
            this.btnLowStock.Click += new System.EventHandler(this.btnLowStock_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            this.btnDelete.IconColor = System.Drawing.Color.White;
            this.btnDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDelete.IconSize = 24;
            this.btnDelete.Location = new System.Drawing.Point(1446, 18);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 49);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddVariant
            // 
            this.btnAddVariant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddVariant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVariant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddVariant.ForeColor = System.Drawing.Color.White;
            this.btnAddVariant.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.btnAddVariant.IconColor = System.Drawing.Color.White;
            this.btnAddVariant.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddVariant.IconSize = 20;
            this.btnAddVariant.Location = new System.Drawing.Point(779, 18);
            this.btnAddVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddVariant.Name = "btnAddVariant";
            this.btnAddVariant.Size = new System.Drawing.Size(173, 49);
            this.btnAddVariant.TabIndex = 3;
            this.btnAddVariant.Text = "Add Variant (V)";
            this.btnAddVariant.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddVariant.UseVisualStyleBackColor = false;
            this.btnAddVariant.Click += new System.EventHandler(this.btnAddVariant_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddProduct.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.btnAddProduct.IconColor = System.Drawing.Color.White;
            this.btnAddProduct.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddProduct.IconSize = 20;
            this.btnAddProduct.Location = new System.Drawing.Point(579, 18);
            this.btnAddProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(173, 49);
            this.btnAddProduct.TabIndex = 2;
            this.btnAddProduct.Text = "Add Product (P)";
            this.btnAddProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(120, 25);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(409, 32);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(27, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Search:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SeaGreen;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 172);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.Size = new System.Drawing.Size(1867, 813);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // panelEditProduct
            // 
            this.panelEditProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelEditProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEditProduct.Controls.Add(this.chkProductActive);
            this.panelEditProduct.Controls.Add(this.chkHasVariants);
            this.panelEditProduct.Controls.Add(this.btnCancelProduct);
            this.panelEditProduct.Controls.Add(this.btnSaveProduct);
            this.panelEditProduct.Controls.Add(this.txtDescription);
            this.panelEditProduct.Controls.Add(this.label7);
            this.panelEditProduct.Controls.Add(this.cmbSupplier);
            this.panelEditProduct.Controls.Add(this.label6);
            this.panelEditProduct.Controls.Add(this.cmbCategory);
            this.panelEditProduct.Controls.Add(this.label5);
            this.panelEditProduct.Controls.Add(this.txtProductName);
            this.panelEditProduct.Controls.Add(this.label4);
            this.panelEditProduct.Controls.Add(this.lblProductTitle);
            this.panelEditProduct.Location = new System.Drawing.Point(267, 246);
            this.panelEditProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEditProduct.Name = "panelEditProduct";
            this.panelEditProduct.Size = new System.Drawing.Size(666, 553);
            this.panelEditProduct.TabIndex = 3;
            this.panelEditProduct.Visible = false;
            // 
            // chkProductActive
            // 
            this.chkProductActive.AutoSize = true;
            this.chkProductActive.Checked = true;
            this.chkProductActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProductActive.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkProductActive.Location = new System.Drawing.Point(360, 400);
            this.chkProductActive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkProductActive.Name = "chkProductActive";
            this.chkProductActive.Size = new System.Drawing.Size(95, 27);
            this.chkProductActive.TabIndex = 12;
            this.chkProductActive.Text = "Is Active";
            this.chkProductActive.UseVisualStyleBackColor = true;
            // 
            // chkHasVariants
            // 
            this.chkHasVariants.AutoSize = true;
            this.chkHasVariants.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkHasVariants.Location = new System.Drawing.Point(40, 400);
            this.chkHasVariants.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkHasVariants.Name = "chkHasVariants";
            this.chkHasVariants.Size = new System.Drawing.Size(126, 27);
            this.chkHasVariants.TabIndex = 11;
            this.chkHasVariants.Text = "Has Variants";
            this.chkHasVariants.UseVisualStyleBackColor = true;
            // 
            // btnCancelProduct
            // 
            this.btnCancelProduct.BackColor = System.Drawing.Color.Gray;
            this.btnCancelProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelProduct.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelProduct.ForeColor = System.Drawing.Color.White;
            this.btnCancelProduct.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCancelProduct.IconColor = System.Drawing.Color.White;
            this.btnCancelProduct.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelProduct.IconSize = 24;
            this.btnCancelProduct.Location = new System.Drawing.Point(360, 455);
            this.btnCancelProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelProduct.Name = "btnCancelProduct";
            this.btnCancelProduct.Size = new System.Drawing.Size(267, 55);
            this.btnCancelProduct.TabIndex = 10;
            this.btnCancelProduct.Text = "Cancel (Esc)";
            this.btnCancelProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelProduct.UseVisualStyleBackColor = false;
            this.btnCancelProduct.Click += new System.EventHandler(this.btnCancelProduct_Click);
            // 
            // btnSaveProduct
            // 
            this.btnSaveProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSaveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProduct.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveProduct.ForeColor = System.Drawing.Color.White;
            this.btnSaveProduct.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnSaveProduct.IconColor = System.Drawing.Color.White;
            this.btnSaveProduct.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSaveProduct.IconSize = 24;
            this.btnSaveProduct.Location = new System.Drawing.Point(40, 455);
            this.btnSaveProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveProduct.Name = "btnSaveProduct";
            this.btnSaveProduct.Size = new System.Drawing.Size(267, 55);
            this.btnSaveProduct.TabIndex = 9;
            this.btnSaveProduct.Text = "Save Product";
            this.btnSaveProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveProduct.UseVisualStyleBackColor = false;
            this.btnSaveProduct.Click += new System.EventHandler(this.btnSaveProduct_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescription.Location = new System.Drawing.Point(40, 320);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(585, 61);
            this.txtDescription.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(40, 289);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Description:";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(40, 240);
            this.cmbSupplier.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(585, 31);
            this.cmbSupplier.TabIndex = 6;
            this.cmbSupplier.TextChanged += new System.EventHandler(this.cmbSupplier_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(40, 209);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Supplier (Optional):";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(40, 160);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(585, 31);
            this.cmbCategory.TabIndex = 4;
            this.cmbCategory.TextChanged += new System.EventHandler(this.cmbCategory_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(40, 129);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Category:*";
            // 
            // txtProductName
            // 
            this.txtProductName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProductName.Location = new System.Drawing.Point(40, 86);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(585, 30);
            this.txtProductName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(40, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Product Name:*";
            // 
            // lblProductTitle
            // 
            this.lblProductTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblProductTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblProductTitle.ForeColor = System.Drawing.Color.White;
            this.lblProductTitle.Location = new System.Drawing.Point(0, 0);
            this.lblProductTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductTitle.Name = "lblProductTitle";
            this.lblProductTitle.Size = new System.Drawing.Size(664, 43);
            this.lblProductTitle.TabIndex = 0;
            this.lblProductTitle.Text = "Edit Product";
            this.lblProductTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEditVariant
            // 
            this.panelEditVariant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelEditVariant.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEditVariant.Controls.Add(this.chkVariantActive);
            this.panelEditVariant.Controls.Add(this.numMinOrderQty);
            this.panelEditVariant.Controls.Add(this.label13);
            this.panelEditVariant.Controls.Add(this.numReorderLevel);
            this.panelEditVariant.Controls.Add(this.label12);
            this.panelEditVariant.Controls.Add(this.numStock);
            this.panelEditVariant.Controls.Add(this.label11);
            this.panelEditVariant.Controls.Add(this.numLengthInFeet);
            this.panelEditVariant.Controls.Add(this.label10);
            this.panelEditVariant.Controls.Add(this.numPricePerLength);
            this.panelEditVariant.Controls.Add(this.label9);
            this.panelEditVariant.Controls.Add(this.numPricePerUnit);
            this.panelEditVariant.Controls.Add(this.label8);
            this.panelEditVariant.Controls.Add(this.btnCancelVariant);
            this.panelEditVariant.Controls.Add(this.btnSaveVariant);
            this.panelEditVariant.Controls.Add(this.cmbUnitOfMeasure);
            this.panelEditVariant.Controls.Add(this.label14);
            this.panelEditVariant.Controls.Add(this.txtClassType);
            this.panelEditVariant.Controls.Add(this.label15);
            this.panelEditVariant.Controls.Add(this.txtSize);
            this.panelEditVariant.Controls.Add(this.label16);
            this.panelEditVariant.Controls.Add(this.cmbProductForVariant);
            this.panelEditVariant.Controls.Add(this.label17);
            this.panelEditVariant.Controls.Add(this.lblVariantTitle);
            this.panelEditVariant.Location = new System.Drawing.Point(922, 234);
            this.panelEditVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEditVariant.Name = "panelEditVariant";
            this.panelEditVariant.Size = new System.Drawing.Size(666, 738);
            this.panelEditVariant.TabIndex = 4;
            this.panelEditVariant.Visible = false;
            // 
            // chkVariantActive
            // 
            this.chkVariantActive.AutoSize = true;
            this.chkVariantActive.Checked = true;
            this.chkVariantActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVariantActive.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkVariantActive.Location = new System.Drawing.Point(360, 615);
            this.chkVariantActive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkVariantActive.Name = "chkVariantActive";
            this.chkVariantActive.Size = new System.Drawing.Size(95, 27);
            this.chkVariantActive.TabIndex = 23;
            this.chkVariantActive.Text = "Is Active";
            this.chkVariantActive.UseVisualStyleBackColor = true;
            // 
            // numMinOrderQty
            // 
            this.numMinOrderQty.DecimalPlaces = 2;
            this.numMinOrderQty.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numMinOrderQty.Location = new System.Drawing.Point(360, 566);
            this.numMinOrderQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numMinOrderQty.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMinOrderQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinOrderQty.Name = "numMinOrderQty";
            this.numMinOrderQty.Size = new System.Drawing.Size(267, 30);
            this.numMinOrderQty.TabIndex = 22;
            this.numMinOrderQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(360, 542);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 20);
            this.label13.TabIndex = 21;
            this.label13.Text = "Min Order Qty:*";
            // 
            // numReorderLevel
            // 
            this.numReorderLevel.DecimalPlaces = 2;
            this.numReorderLevel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numReorderLevel.Location = new System.Drawing.Point(40, 566);
            this.numReorderLevel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numReorderLevel.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numReorderLevel.Name = "numReorderLevel";
            this.numReorderLevel.Size = new System.Drawing.Size(267, 30);
            this.numReorderLevel.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(40, 542);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(109, 20);
            this.label12.TabIndex = 19;
            this.label12.Text = "Reorder Level:";
            // 
            // numStock
            // 
            this.numStock.DecimalPlaces = 2;
            this.numStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numStock.Location = new System.Drawing.Point(360, 492);
            this.numStock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numStock.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numStock.Name = "numStock";
            this.numStock.Size = new System.Drawing.Size(267, 30);
            this.numStock.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(360, 468);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "Stock Quantity:";
            // 
            // numLengthInFeet
            // 
            this.numLengthInFeet.DecimalPlaces = 2;
            this.numLengthInFeet.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numLengthInFeet.Location = new System.Drawing.Point(40, 492);
            this.numLengthInFeet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numLengthInFeet.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numLengthInFeet.Name = "numLengthInFeet";
            this.numLengthInFeet.Size = new System.Drawing.Size(267, 30);
            this.numLengthInFeet.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(40, 468);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 20);
            this.label10.TabIndex = 15;
            this.label10.Text = "Length in Feet:";
            // 
            // numPricePerLength
            // 
            this.numPricePerLength.DecimalPlaces = 2;
            this.numPricePerLength.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numPricePerLength.Location = new System.Drawing.Point(360, 418);
            this.numPricePerLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numPricePerLength.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPricePerLength.Name = "numPricePerLength";
            this.numPricePerLength.Size = new System.Drawing.Size(267, 30);
            this.numPricePerLength.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(360, 394);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 20);
            this.label9.TabIndex = 13;
            this.label9.Text = "Price per Length:";
            // 
            // numPricePerUnit
            // 
            this.numPricePerUnit.DecimalPlaces = 2;
            this.numPricePerUnit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numPricePerUnit.Location = new System.Drawing.Point(40, 418);
            this.numPricePerUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numPricePerUnit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPricePerUnit.Name = "numPricePerUnit";
            this.numPricePerUnit.Size = new System.Drawing.Size(267, 30);
            this.numPricePerUnit.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(40, 394);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "Price per Unit:*";
            // 
            // btnCancelVariant
            // 
            this.btnCancelVariant.BackColor = System.Drawing.Color.Gray;
            this.btnCancelVariant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelVariant.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelVariant.ForeColor = System.Drawing.Color.White;
            this.btnCancelVariant.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCancelVariant.IconColor = System.Drawing.Color.White;
            this.btnCancelVariant.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelVariant.IconSize = 24;
            this.btnCancelVariant.Location = new System.Drawing.Point(360, 665);
            this.btnCancelVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelVariant.Name = "btnCancelVariant";
            this.btnCancelVariant.Size = new System.Drawing.Size(267, 55);
            this.btnCancelVariant.TabIndex = 10;
            this.btnCancelVariant.Text = "Cancel (Esc)";
            this.btnCancelVariant.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelVariant.UseVisualStyleBackColor = false;
            this.btnCancelVariant.Click += new System.EventHandler(this.btnCancelVariant_Click);
            // 
            // btnSaveVariant
            // 
            this.btnSaveVariant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSaveVariant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveVariant.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveVariant.ForeColor = System.Drawing.Color.White;
            this.btnSaveVariant.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnSaveVariant.IconColor = System.Drawing.Color.White;
            this.btnSaveVariant.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSaveVariant.IconSize = 24;
            this.btnSaveVariant.Location = new System.Drawing.Point(40, 665);
            this.btnSaveVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveVariant.Name = "btnSaveVariant";
            this.btnSaveVariant.Size = new System.Drawing.Size(267, 55);
            this.btnSaveVariant.TabIndex = 9;
            this.btnSaveVariant.Text = "Save Variant";
            this.btnSaveVariant.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveVariant.UseVisualStyleBackColor = false;
            this.btnSaveVariant.Click += new System.EventHandler(this.btnSaveVariant_Click);
            // 
            // cmbUnitOfMeasure
            // 
            this.cmbUnitOfMeasure.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbUnitOfMeasure.FormattingEnabled = true;
            this.cmbUnitOfMeasure.Location = new System.Drawing.Point(40, 345);
            this.cmbUnitOfMeasure.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbUnitOfMeasure.Name = "cmbUnitOfMeasure";
            this.cmbUnitOfMeasure.Size = new System.Drawing.Size(585, 31);
            this.cmbUnitOfMeasure.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(40, 320);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(133, 20);
            this.label14.TabIndex = 7;
            this.label14.Text = "Unit of Measure:*";
            // 
            // txtClassType
            // 
            this.txtClassType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtClassType.Location = new System.Drawing.Point(40, 271);
            this.txtClassType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtClassType.Name = "txtClassType";
            this.txtClassType.Size = new System.Drawing.Size(585, 30);
            this.txtClassType.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(40, 246);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(239, 20);
            this.label15.TabIndex = 5;
            this.label15.Text = "Class Type (e.g., Class 0, PN-1.6):";
            // 
            // txtSize
            // 
            this.txtSize.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSize.Location = new System.Drawing.Point(40, 197);
            this.txtSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(585, 30);
            this.txtSize.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(40, 172);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(315, 20);
            this.label16.TabIndex = 3;
            this.label16.Text = "Size (e.g., 2\", 3\", or \"Standard\" for simple):*";
            // 
            // cmbProductForVariant
            // 
            this.cmbProductForVariant.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProductForVariant.FormattingEnabled = true;
            this.cmbProductForVariant.Location = new System.Drawing.Point(40, 123);
            this.cmbProductForVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProductForVariant.Name = "cmbProductForVariant";
            this.cmbProductForVariant.Size = new System.Drawing.Size(585, 31);
            this.cmbProductForVariant.TabIndex = 2;
            this.cmbProductForVariant.TextChanged += new System.EventHandler(this.cmbProductForVariant_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(40, 98);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 20);
            this.label17.TabIndex = 1;
            this.label17.Text = "Product:*";
            // 
            // lblVariantTitle
            // 
            this.lblVariantTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblVariantTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVariantTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblVariantTitle.ForeColor = System.Drawing.Color.White;
            this.lblVariantTitle.Location = new System.Drawing.Point(0, 0);
            this.lblVariantTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariantTitle.Name = "lblVariantTitle";
            this.lblVariantTitle.Size = new System.Drawing.Size(664, 43);
            this.lblVariantTitle.TabIndex = 0;
            this.lblVariantTitle.Text = "Edit Variant";
            this.lblVariantTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InventoryMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1867, 985);
            this.Controls.Add(this.panelEditVariant);
            this.Controls.Add(this.panelEditProduct);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "InventoryMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Management System";
            this.Load += new System.EventHandler(this.InventoryMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelEditProduct.ResumeLayout(false);
            this.panelEditProduct.PerformLayout();
            this.panelEditVariant.ResumeLayout(false);
            this.panelEditVariant.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinOrderQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLengthInFeet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPricePerLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPricePerUnit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStockStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton btnAddProduct;
        private FontAwesome.Sharp.IconButton btnAddVariant;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnLowStock;
        private FontAwesome.Sharp.IconButton btnOutOfStock;
        private FontAwesome.Sharp.IconButton btnShowAll;
        private System.Windows.Forms.DataGridView dataGridView1;

        // Product Panel
        private System.Windows.Forms.Panel panelEditProduct;
        private System.Windows.Forms.Label lblProductTitle;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label7;
        private FontAwesome.Sharp.IconButton btnSaveProduct;
        private FontAwesome.Sharp.IconButton btnCancelProduct;
        private System.Windows.Forms.CheckBox chkHasVariants;
        private System.Windows.Forms.CheckBox chkProductActive;

        // Variant Panel
        private System.Windows.Forms.Panel panelEditVariant;
        private System.Windows.Forms.Label lblVariantTitle;
        private System.Windows.Forms.ComboBox cmbProductForVariant;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtClassType;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbUnitOfMeasure;
        private System.Windows.Forms.Label label14;
        private FontAwesome.Sharp.IconButton btnSaveVariant;
        private FontAwesome.Sharp.IconButton btnCancelVariant;
        private System.Windows.Forms.NumericUpDown numPricePerUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numPricePerLength;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numLengthInFeet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numStock;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numReorderLevel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numMinOrderQty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkVariantActive;
    }
}