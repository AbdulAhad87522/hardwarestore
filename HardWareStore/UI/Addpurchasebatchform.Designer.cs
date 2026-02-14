namespace HardWareStore.UI
{
    partial class AddPurchaseBatchForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBatchId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBatchName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpBatchDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectVariant = new FontAwesome.Sharp.IconButton();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtClass = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLineTotal = new System.Windows.Forms.TextBox();
            this.btnCancelEdit = new FontAwesome.Sharp.IconButton();
            this.btnAddItem = new FontAwesome.Sharp.IconButton();
            this.dgvBatchItems = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtPaid = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRemaining = new System.Windows.Forms.TextBox();
            this.btnSaveBatch = new FontAwesome.Sharp.IconButton();
            this.btnCancel = new FontAwesome.Sharp.IconButton();
            this.panelVariantSelection = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSearchVariant = new System.Windows.Forms.TextBox();
            this.dgvVariantSelection = new System.Windows.Forms.DataGridView();
            this.btnCloseVariantPanel = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchItems)).BeginInit();
            this.panel4.SuspendLayout();
            this.panelVariantSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariantSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.panel1.Controls.Add(this.lblBatchId);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1600, 74);
            this.panel1.TabIndex = 0;
            // 
            // lblBatchId
            // 
            this.lblBatchId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBatchId.AutoSize = true;
            this.lblBatchId.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblBatchId.ForeColor = System.Drawing.Color.White;
            this.lblBatchId.Location = new System.Drawing.Point(1267, 25);
            this.lblBatchId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchId.Name = "lblBatchId";
            this.lblBatchId.Size = new System.Drawing.Size(101, 25);
            this.lblBatchId.TabIndex = 1;
            this.lblBatchId.Text = "Batch ID: 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Purchase Batch Details";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtBatchName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cmbSupplier);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dtpBatchDate);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cmbStatus);
            this.panel2.Location = new System.Drawing.Point(27, 98);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1546, 123);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(20, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Batch Name:*";
            // 
            // txtBatchName
            // 
            this.txtBatchName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBatchName.Location = new System.Drawing.Point(20, 49);
            this.txtBatchName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(332, 30);
            this.txtBatchName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(380, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Supplier:*";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(380, 49);
            this.cmbSupplier.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(399, 31);
            this.cmbSupplier.TabIndex = 3;
            this.cmbSupplier.TextChanged += new System.EventHandler(this.cmbSupplier_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(807, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Date:";
            // 
            // dtpBatchDate
            // 
            this.dtpBatchDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpBatchDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBatchDate.Location = new System.Drawing.Point(807, 49);
            this.dtpBatchDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpBatchDate.Name = "dtpBatchDate";
            this.dtpBatchDate.Size = new System.Drawing.Size(265, 30);
            this.dtpBatchDate.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(1100, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Status:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Pending",
            "Partial",
            "Completed"});
            this.cmbStatus.Location = new System.Drawing.Point(1100, 49);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(199, 31);
            this.cmbStatus.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.btnSelectVariant);
            this.panel3.Controls.Add(this.txtProduct);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtSize);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtClass);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.txtSalePrice);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.numQuantity);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.txtCostPrice);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtLineTotal);
            this.panel3.Controls.Add(this.btnCancelEdit);
            this.panel3.Controls.Add(this.btnAddItem);
            this.panel3.Location = new System.Drawing.Point(27, 246);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1546, 147);
            this.panel3.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(20, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "Product:";
            // 
            // btnSelectVariant
            // 
            this.btnSelectVariant.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSelectVariant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectVariant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelectVariant.ForeColor = System.Drawing.Color.White;
            this.btnSelectVariant.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnSelectVariant.IconColor = System.Drawing.Color.White;
            this.btnSelectVariant.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSelectVariant.IconSize = 20;
            this.btnSelectVariant.Location = new System.Drawing.Point(20, 49);
            this.btnSelectVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectVariant.Name = "btnSelectVariant";
            this.btnSelectVariant.Size = new System.Drawing.Size(160, 37);
            this.btnSelectVariant.TabIndex = 1;
            this.btnSelectVariant.Text = "Select Variant";
            this.btnSelectVariant.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSelectVariant.UseVisualStyleBackColor = false;
            this.btnSelectVariant.Click += new System.EventHandler(this.btnSelectVariant_Click);
            // 
            // txtProduct
            // 
            this.txtProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProduct.Location = new System.Drawing.Point(200, 49);
            this.txtProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.ReadOnly = true;
            this.txtProduct.Size = new System.Drawing.Size(265, 30);
            this.txtProduct.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(493, 18);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 23);
            this.label7.TabIndex = 3;
            this.label7.Text = "Size:";
            // 
            // txtSize
            // 
            this.txtSize.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSize.Location = new System.Drawing.Point(493, 49);
            this.txtSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(132, 30);
            this.txtSize.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(647, 18);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "Class:";
            // 
            // txtClass
            // 
            this.txtClass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtClass.Location = new System.Drawing.Point(647, 49);
            this.txtClass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtClass.Name = "txtClass";
            this.txtClass.ReadOnly = true;
            this.txtClass.Size = new System.Drawing.Size(132, 30);
            this.txtClass.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(800, 18);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 23);
            this.label9.TabIndex = 7;
            this.label9.Text = "Sale Price:";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSalePrice.Location = new System.Drawing.Point(800, 49);
            this.txtSalePrice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(190, 30);
            this.txtSalePrice.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(16, 107);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Quantity:*";
            // 
            // numQuantity
            // 
            this.numQuantity.DecimalPlaces = 2;
            this.numQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numQuantity.Location = new System.Drawing.Point(146, 107);
            this.numQuantity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numQuantity.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(160, 30);
            this.numQuantity.TabIndex = 10;
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(329, 111);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 23);
            this.label11.TabIndex = 11;
            this.label11.Text = "Cost Price:*";
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCostPrice.Location = new System.Drawing.Point(478, 104);
            this.txtCostPrice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.Size = new System.Drawing.Size(191, 30);
            this.txtCostPrice.TabIndex = 12;
            this.txtCostPrice.Text = "0";
            this.txtCostPrice.TextChanged += new System.EventHandler(this.txtCostPrice_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(739, 111);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 23);
            this.label12.TabIndex = 13;
            this.label12.Text = "Line Total:";
            // 
            // txtLineTotal
            // 
            this.txtLineTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLineTotal.Location = new System.Drawing.Point(871, 104);
            this.txtLineTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLineTotal.Name = "txtLineTotal";
            this.txtLineTotal.ReadOnly = true;
            this.txtLineTotal.Size = new System.Drawing.Size(192, 30);
            this.txtLineTotal.TabIndex = 14;
            this.txtLineTotal.Text = "0.00";
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelEdit.BackColor = System.Drawing.Color.Gray;
            this.btnCancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelEdit.ForeColor = System.Drawing.Color.White;
            this.btnCancelEdit.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCancelEdit.IconColor = System.Drawing.Color.White;
            this.btnCancelEdit.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelEdit.IconSize = 20;
            this.btnCancelEdit.Location = new System.Drawing.Point(1340, 50);
            this.btnCancelEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(160, 80);
            this.btnCancelEdit.TabIndex = 16;
            this.btnCancelEdit.Text = "Cancel Edit";
            this.btnCancelEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelEdit.UseVisualStyleBackColor = false;
            this.btnCancelEdit.Visible = false;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.btnAddItem.IconColor = System.Drawing.Color.White;
            this.btnAddItem.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddItem.IconSize = 24;
            this.btnAddItem.Location = new System.Drawing.Point(1120, 50);
            this.btnAddItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(200, 80);
            this.btnAddItem.TabIndex = 15;
            this.btnAddItem.Text = "Add to Batch";
            this.btnAddItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // dgvBatchItems
            // 
            this.dgvBatchItems.AllowUserToAddRows = false;
            this.dgvBatchItems.AllowUserToDeleteRows = false;
            this.dgvBatchItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBatchItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvBatchItems.ColumnHeadersHeight = 29;
            this.dgvBatchItems.Location = new System.Drawing.Point(27, 418);
            this.dgvBatchItems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBatchItems.Name = "dgvBatchItems";
            this.dgvBatchItems.ReadOnly = true;
            this.dgvBatchItems.RowHeadersWidth = 51;
            this.dgvBatchItems.RowTemplate.Height = 30;
            this.dgvBatchItems.Size = new System.Drawing.Size(1547, 369);
            this.dgvBatchItems.TabIndex = 3;
            this.dgvBatchItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchItems_CellContentClick);
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.txtTotalAmount);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.txtPaid);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.txtRemaining);
            this.panel4.Controls.Add(this.btnSaveBatch);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Location = new System.Drawing.Point(27, 812);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1546, 123);
            this.panel4.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(20, 25);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 28);
            this.label13.TabIndex = 0;
            this.label13.Text = "Total:";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtTotalAmount.Location = new System.Drawing.Point(20, 55);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(239, 34);
            this.txtTotalAmount.TabIndex = 1;
            this.txtTotalAmount.Text = "0.00";
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalAmount.TextChanged += new System.EventHandler(this.txtTotalAmount_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(287, 25);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 28);
            this.label14.TabIndex = 2;
            this.label14.Text = "Paid:";
            // 
            // txtPaid
            // 
            this.txtPaid.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPaid.Location = new System.Drawing.Point(287, 55);
            this.txtPaid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPaid.Name = "txtPaid";
            this.txtPaid.Size = new System.Drawing.Size(239, 34);
            this.txtPaid.TabIndex = 3;
            this.txtPaid.Text = "0";
            this.txtPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaid.TextChanged += new System.EventHandler(this.txtPaid_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(553, 25);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(118, 28);
            this.label15.TabIndex = 4;
            this.label15.Text = "Remaining:";
            // 
            // txtRemaining
            // 
            this.txtRemaining.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtRemaining.ForeColor = System.Drawing.Color.Red;
            this.txtRemaining.Location = new System.Drawing.Point(553, 55);
            this.txtRemaining.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRemaining.Name = "txtRemaining";
            this.txtRemaining.ReadOnly = true;
            this.txtRemaining.Size = new System.Drawing.Size(239, 34);
            this.txtRemaining.TabIndex = 5;
            this.txtRemaining.Text = "0.00";
            this.txtRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSaveBatch
            // 
            this.btnSaveBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveBatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSaveBatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveBatch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSaveBatch.ForeColor = System.Drawing.Color.White;
            this.btnSaveBatch.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnSaveBatch.IconColor = System.Drawing.Color.White;
            this.btnSaveBatch.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSaveBatch.IconSize = 28;
            this.btnSaveBatch.Location = new System.Drawing.Point(1133, 25);
            this.btnSaveBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveBatch.Name = "btnSaveBatch";
            this.btnSaveBatch.Size = new System.Drawing.Size(187, 74);
            this.btnSaveBatch.TabIndex = 6;
            this.btnSaveBatch.Text = "Save Batch";
            this.btnSaveBatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveBatch.UseVisualStyleBackColor = false;
            this.btnSaveBatch.Click += new System.EventHandler(this.btnSaveBatch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCancel.IconColor = System.Drawing.Color.White;
            this.btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancel.IconSize = 28;
            this.btnCancel.Location = new System.Drawing.Point(1340, 25);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(187, 74);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelVariantSelection
            // 
            this.panelVariantSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelVariantSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVariantSelection.Controls.Add(this.label16);
            this.panelVariantSelection.Controls.Add(this.txtSearchVariant);
            this.panelVariantSelection.Controls.Add(this.dgvVariantSelection);
            this.panelVariantSelection.Controls.Add(this.btnCloseVariantPanel);
            this.panelVariantSelection.Location = new System.Drawing.Point(340, 498);
            this.panelVariantSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelVariantSelection.Name = "panelVariantSelection";
            this.panelVariantSelection.Size = new System.Drawing.Size(799, 615);
            this.panelVariantSelection.TabIndex = 5;
            this.panelVariantSelection.Visible = false;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.label16.Dock = System.Windows.Forms.DockStyle.Top;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(0, 0);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(797, 49);
            this.label16.TabIndex = 0;
            this.label16.Text = "Select Product Variant";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSearchVariant
            // 
            this.txtSearchVariant.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearchVariant.Location = new System.Drawing.Point(20, 68);
            this.txtSearchVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearchVariant.Name = "txtSearchVariant";
            this.txtSearchVariant.Size = new System.Drawing.Size(756, 30);
            this.txtSearchVariant.TabIndex = 1;
            this.txtSearchVariant.TextChanged += new System.EventHandler(this.txtSearchVariant_TextChanged);
            // 
            // dgvVariantSelection
            // 
            this.dgvVariantSelection.AllowUserToAddRows = false;
            this.dgvVariantSelection.AllowUserToDeleteRows = false;
            this.dgvVariantSelection.BackgroundColor = System.Drawing.Color.White;
            this.dgvVariantSelection.ColumnHeadersHeight = 29;
            this.dgvVariantSelection.Location = new System.Drawing.Point(20, 111);
            this.dgvVariantSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvVariantSelection.Name = "dgvVariantSelection";
            this.dgvVariantSelection.ReadOnly = true;
            this.dgvVariantSelection.RowHeadersWidth = 51;
            this.dgvVariantSelection.RowTemplate.Height = 30;
            this.dgvVariantSelection.Size = new System.Drawing.Size(757, 425);
            this.dgvVariantSelection.TabIndex = 2;
            this.dgvVariantSelection.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariantSelection_CellDoubleClick);
            // 
            // btnCloseVariantPanel
            // 
            this.btnCloseVariantPanel.BackColor = System.Drawing.Color.Gray;
            this.btnCloseVariantPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseVariantPanel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCloseVariantPanel.ForeColor = System.Drawing.Color.White;
            this.btnCloseVariantPanel.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCloseVariantPanel.IconColor = System.Drawing.Color.White;
            this.btnCloseVariantPanel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCloseVariantPanel.IconSize = 24;
            this.btnCloseVariantPanel.Location = new System.Drawing.Point(20, 548);
            this.btnCloseVariantPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseVariantPanel.Name = "btnCloseVariantPanel";
            this.btnCloseVariantPanel.Size = new System.Drawing.Size(757, 49);
            this.btnCloseVariantPanel.TabIndex = 3;
            this.btnCloseVariantPanel.Text = "Close";
            this.btnCloseVariantPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseVariantPanel.UseVisualStyleBackColor = false;
            this.btnCloseVariantPanel.Click += new System.EventHandler(this.btnCloseVariantPanel_Click);
            // 
            // AddPurchaseBatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1600, 960);
            this.Controls.Add(this.panelVariantSelection);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.dgvBatchItems);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddPurchaseBatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Purchase Batch";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchItems)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelVariantSelection.ResumeLayout(false);
            this.panelVariantSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariantSelection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBatchId;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBatchName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpBatchDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private FontAwesome.Sharp.IconButton btnSelectVariant;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtClass;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSalePrice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLineTotal;
        private FontAwesome.Sharp.IconButton btnCancelEdit;
        private FontAwesome.Sharp.IconButton btnAddItem;
        private System.Windows.Forms.DataGridView dgvBatchItems;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtPaid;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRemaining;
        private FontAwesome.Sharp.IconButton btnSaveBatch;
        private FontAwesome.Sharp.IconButton btnCancel;
        private System.Windows.Forms.Panel panelVariantSelection;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtSearchVariant;
        private System.Windows.Forms.DataGridView dgvVariantSelection;
        private FontAwesome.Sharp.IconButton btnCloseVariantPanel;
    }
}