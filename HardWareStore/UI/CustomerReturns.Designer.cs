namespace HardWareStore.UI
{
    partial class CustomerReturns
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblBillNumberLabel = new System.Windows.Forms.Label();
            this.txtBillNumber = new System.Windows.Forms.TextBox();
            this.btnSearchBill = new FontAwesome.Sharp.IconButton();
            this.lblBillInfo = new System.Windows.Forms.Label();
            this.panelBillItems = new System.Windows.Forms.Panel();
            this.lblBillItemsTitle = new System.Windows.Forms.Label();
            this.dgvBillItems = new System.Windows.Forms.DataGridView();
            this.panelReturnItems = new System.Windows.Forms.Panel();
            this.lblReturnItemsTitle = new System.Windows.Forms.Label();
            this.lblReturnItemCount = new System.Windows.Forms.Label();
            this.dgvReturnItems = new System.Windows.Forms.DataGridView();
            this.panelReturnDetails = new System.Windows.Forms.Panel();
            this.lblReasonLabel = new System.Windows.Forms.Label();
            this.txtReturnReason = new System.Windows.Forms.TextBox();
            this.lblNotesLabel = new System.Windows.Forms.Label();
            this.txtReturnNotes = new System.Windows.Forms.TextBox();
            this.chkRestoreInventory = new System.Windows.Forms.CheckBox();
            this.lblInventoryNote = new System.Windows.Forms.Label();
            this.panelRefund = new System.Windows.Forms.Panel();
            this.lblRefundDescription = new System.Windows.Forms.Label();
            this.lblCalculatedRefund = new System.Windows.Forms.Label();
            this.lblRefundLabel = new System.Windows.Forms.Label();
            this.numRefundAmount = new System.Windows.Forms.NumericUpDown();
            this.lblAdjustmentNote = new System.Windows.Forms.Label();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnProcessReturn = new FontAwesome.Sharp.IconButton();
            this.btnCancelReturn = new FontAwesome.Sharp.IconButton();
            this.panelHeader.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelBillItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillItems)).BeginInit();
            this.panelReturnItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnItems)).BeginInit();
            this.panelReturnDetails.SuspendLayout();
            this.panelRefund.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefundAmount)).BeginInit();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1400, 86);
            this.panelHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(240)))));
            this.lblSubtitle.Location = new System.Drawing.Point(18, 57);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(522, 23);
            this.lblSubtitle.TabIndex = 0;
            this.lblSubtitle.Text = "Process product returns, manage refunds and inventory restoration";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(269, 41);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Customer Returns";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.lblBillNumberLabel);
            this.panelSearch.Controls.Add(this.txtBillNumber);
            this.panelSearch.Controls.Add(this.btnSearchBill);
            this.panelSearch.Controls.Add(this.lblBillInfo);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 86);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.panelSearch.Size = new System.Drawing.Size(1400, 80);
            this.panelSearch.TabIndex = 1;
            // 
            // lblBillNumberLabel
            // 
            this.lblBillNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBillNumberLabel.AutoSize = true;
            this.lblBillNumberLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBillNumberLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblBillNumberLabel.Location = new System.Drawing.Point(16, 16);
            this.lblBillNumberLabel.Name = "lblBillNumberLabel";
            this.lblBillNumberLabel.Size = new System.Drawing.Size(98, 20);
            this.lblBillNumberLabel.TabIndex = 0;
            this.lblBillNumberLabel.Text = "Bill Number:";
            // 
            // txtBillNumber
            // 
            this.txtBillNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBillNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBillNumber.Location = new System.Drawing.Point(110, 12);
            this.txtBillNumber.Name = "txtBillNumber";
            this.txtBillNumber.Size = new System.Drawing.Size(300, 30);
            this.txtBillNumber.TabIndex = 1;
            this.txtBillNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBillNumber_KeyDown);
            // 
            // btnSearchBill
            // 
            this.btnSearchBill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchBill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.btnSearchBill.FlatAppearance.BorderSize = 0;
            this.btnSearchBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchBill.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearchBill.ForeColor = System.Drawing.Color.White;
            this.btnSearchBill.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnSearchBill.IconColor = System.Drawing.Color.White;
            this.btnSearchBill.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSearchBill.IconSize = 16;
            this.btnSearchBill.Location = new System.Drawing.Point(424, 9);
            this.btnSearchBill.Name = "btnSearchBill";
            this.btnSearchBill.Size = new System.Drawing.Size(130, 34);
            this.btnSearchBill.TabIndex = 2;
            this.btnSearchBill.Text = "  Search Bill";
            this.btnSearchBill.UseVisualStyleBackColor = false;
            this.btnSearchBill.Click += new System.EventHandler(this.btnSearchBill_Click);
            // 
            // lblBillInfo
            // 
            this.lblBillInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBillInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBillInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.lblBillInfo.Location = new System.Drawing.Point(708, 16);
            this.lblBillInfo.Name = "lblBillInfo";
            this.lblBillInfo.Size = new System.Drawing.Size(452, 24);
            this.lblBillInfo.TabIndex = 3;
            this.lblBillInfo.Text = "No bill loaded";
            // 
            // panelBillItems
            // 
            this.panelBillItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBillItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.panelBillItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBillItems.Controls.Add(this.lblBillItemsTitle);
            this.panelBillItems.Controls.Add(this.dgvBillItems);
            this.panelBillItems.Location = new System.Drawing.Point(16, 175);
            this.panelBillItems.Name = "panelBillItems";
            this.panelBillItems.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelBillItems.Size = new System.Drawing.Size(680, 300);
            this.panelBillItems.TabIndex = 2;
            this.panelBillItems.Visible = false;
            // 
            // lblBillItemsTitle
            // 
            this.lblBillItemsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblBillItemsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBillItemsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBillItemsTitle.ForeColor = System.Drawing.Color.White;
            this.lblBillItemsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblBillItemsTitle.Name = "lblBillItemsTitle";
            this.lblBillItemsTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblBillItemsTitle.Size = new System.Drawing.Size(678, 36);
            this.lblBillItemsTitle.TabIndex = 0;
            this.lblBillItemsTitle.Text = "📋  Bill Items — Click \'↩ Return\' to add items to return list";
            this.lblBillItemsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvBillItems
            // 
            this.dgvBillItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvBillItems.ColumnHeadersHeight = 29;
            this.dgvBillItems.Location = new System.Drawing.Point(0, 0);
            this.dgvBillItems.Name = "dgvBillItems";
            this.dgvBillItems.RowHeadersWidth = 51;
            this.dgvBillItems.Size = new System.Drawing.Size(678, 290);
            this.dgvBillItems.TabIndex = 1;
            this.dgvBillItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBillItems_CellContentClick);
            // 
            // panelReturnItems
            // 
            this.panelReturnItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelReturnItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(255)))), ((int)(((byte)(250)))));
            this.panelReturnItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReturnItems.Controls.Add(this.lblReturnItemsTitle);
            this.panelReturnItems.Controls.Add(this.lblReturnItemCount);
            this.panelReturnItems.Controls.Add(this.dgvReturnItems);
            this.panelReturnItems.Location = new System.Drawing.Point(712, 175);
            this.panelReturnItems.Name = "panelReturnItems";
            this.panelReturnItems.Size = new System.Drawing.Size(660, 300);
            this.panelReturnItems.TabIndex = 3;
            // 
            // lblReturnItemsTitle
            // 
            this.lblReturnItemsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(34)))));
            this.lblReturnItemsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReturnItemsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblReturnItemsTitle.ForeColor = System.Drawing.Color.White;
            this.lblReturnItemsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblReturnItemsTitle.Name = "lblReturnItemsTitle";
            this.lblReturnItemsTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblReturnItemsTitle.Size = new System.Drawing.Size(658, 36);
            this.lblReturnItemsTitle.TabIndex = 0;
            this.lblReturnItemsTitle.Text = "↩  Return Items — Items to be returned";
            this.lblReturnItemsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReturnItemCount
            // 
            this.lblReturnItemCount.AutoSize = true;
            this.lblReturnItemCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReturnItemCount.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblReturnItemCount.Location = new System.Drawing.Point(10, 44);
            this.lblReturnItemCount.Name = "lblReturnItemCount";
            this.lblReturnItemCount.Size = new System.Drawing.Size(118, 20);
            this.lblReturnItemCount.TabIndex = 1;
            this.lblReturnItemCount.Text = "Return Items: 0";
            // 
            // dgvReturnItems
            // 
            this.dgvReturnItems.ColumnHeadersHeight = 29;
            this.dgvReturnItems.Location = new System.Drawing.Point(0, 65);
            this.dgvReturnItems.Name = "dgvReturnItems";
            this.dgvReturnItems.RowHeadersWidth = 51;
            this.dgvReturnItems.Size = new System.Drawing.Size(658, 228);
            this.dgvReturnItems.TabIndex = 1;
            this.dgvReturnItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturnItems_CellContentClick);
            // 
            // panelReturnDetails
            // 
            this.panelReturnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelReturnDetails.BackColor = System.Drawing.Color.White;
            this.panelReturnDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReturnDetails.Controls.Add(this.lblReasonLabel);
            this.panelReturnDetails.Controls.Add(this.txtReturnReason);
            this.panelReturnDetails.Controls.Add(this.lblNotesLabel);
            this.panelReturnDetails.Controls.Add(this.txtReturnNotes);
            this.panelReturnDetails.Controls.Add(this.chkRestoreInventory);
            this.panelReturnDetails.Controls.Add(this.lblInventoryNote);
            this.panelReturnDetails.Location = new System.Drawing.Point(16, 490);
            this.panelReturnDetails.Name = "panelReturnDetails";
            this.panelReturnDetails.Padding = new System.Windows.Forms.Padding(16);
            this.panelReturnDetails.Size = new System.Drawing.Size(680, 200);
            this.panelReturnDetails.TabIndex = 4;
            // 
            // lblReasonLabel
            // 
            this.lblReasonLabel.AutoSize = true;
            this.lblReasonLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReasonLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblReasonLabel.Location = new System.Drawing.Point(16, 16);
            this.lblReasonLabel.Name = "lblReasonLabel";
            this.lblReasonLabel.Size = new System.Drawing.Size(127, 20);
            this.lblReasonLabel.TabIndex = 0;
            this.lblReasonLabel.Text = "Return Reason: *";
            // 
            // txtReturnReason
            // 
            this.txtReturnReason.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtReturnReason.Location = new System.Drawing.Point(130, 12);
            this.txtReturnReason.Name = "txtReturnReason";
            this.txtReturnReason.Size = new System.Drawing.Size(520, 27);
            this.txtReturnReason.TabIndex = 1;
            // 
            // lblNotesLabel
            // 
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblNotesLabel.Location = new System.Drawing.Point(16, 56);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new System.Drawing.Size(129, 20);
            this.lblNotesLabel.TabIndex = 2;
            this.lblNotesLabel.Text = "Notes (optional):";
            // 
            // txtReturnNotes
            // 
            this.txtReturnNotes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtReturnNotes.Location = new System.Drawing.Point(130, 52);
            this.txtReturnNotes.Multiline = true;
            this.txtReturnNotes.Name = "txtReturnNotes";
            this.txtReturnNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReturnNotes.Size = new System.Drawing.Size(520, 55);
            this.txtReturnNotes.TabIndex = 2;
            // 
            // chkRestoreInventory
            // 
            this.chkRestoreInventory.AutoSize = true;
            this.chkRestoreInventory.Checked = true;
            this.chkRestoreInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRestoreInventory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkRestoreInventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.chkRestoreInventory.Location = new System.Drawing.Point(16, 128);
            this.chkRestoreInventory.Name = "chkRestoreInventory";
            this.chkRestoreInventory.Size = new System.Drawing.Size(242, 24);
            this.chkRestoreInventory.TabIndex = 3;
            this.chkRestoreInventory.Text = "Restore Quantity to Inventory";
            this.chkRestoreInventory.CheckedChanged += new System.EventHandler(this.chkRestoreInventory_CheckedChanged);
            // 
            // lblInventoryNote
            // 
            this.lblInventoryNote.AutoSize = true;
            this.lblInventoryNote.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInventoryNote.ForeColor = System.Drawing.Color.Gray;
            this.lblInventoryNote.Location = new System.Drawing.Point(220, 131);
            this.lblInventoryNote.Name = "lblInventoryNote";
            this.lblInventoryNote.Size = new System.Drawing.Size(414, 20);
            this.lblInventoryNote.TabIndex = 4;
            this.lblInventoryNote.Text = "Uncheck if returned items are damaged and cannot be resold";
            // 
            // panelRefund
            // 
            this.panelRefund.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRefund.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.panelRefund.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRefund.Controls.Add(this.lblRefundDescription);
            this.panelRefund.Controls.Add(this.lblCalculatedRefund);
            this.panelRefund.Controls.Add(this.lblRefundLabel);
            this.panelRefund.Controls.Add(this.numRefundAmount);
            this.panelRefund.Controls.Add(this.lblAdjustmentNote);
            this.panelRefund.Location = new System.Drawing.Point(712, 490);
            this.panelRefund.Name = "panelRefund";
            this.panelRefund.Padding = new System.Windows.Forms.Padding(16);
            this.panelRefund.Size = new System.Drawing.Size(660, 200);
            this.panelRefund.TabIndex = 5;
            // 
            // lblRefundDescription
            // 
            this.lblRefundDescription.AutoSize = true;
            this.lblRefundDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRefundDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblRefundDescription.Location = new System.Drawing.Point(16, 16);
            this.lblRefundDescription.Name = "lblRefundDescription";
            this.lblRefundDescription.Size = new System.Drawing.Size(172, 23);
            this.lblRefundDescription.TabIndex = 0;
            this.lblRefundDescription.Text = "💰  Refund Amount";
            // 
            // lblCalculatedRefund
            // 
            this.lblCalculatedRefund.AutoSize = true;
            this.lblCalculatedRefund.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCalculatedRefund.ForeColor = System.Drawing.Color.DimGray;
            this.lblCalculatedRefund.Location = new System.Drawing.Point(16, 46);
            this.lblCalculatedRefund.Name = "lblCalculatedRefund";
            this.lblCalculatedRefund.Size = new System.Drawing.Size(183, 20);
            this.lblCalculatedRefund.TabIndex = 1;
            this.lblCalculatedRefund.Text = "Calculated Refund: Rs 0.00";
            // 
            // lblRefundLabel
            // 
            this.lblRefundLabel.AutoSize = true;
            this.lblRefundLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRefundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.lblRefundLabel.Location = new System.Drawing.Point(16, 88);
            this.lblRefundLabel.Name = "lblRefundLabel";
            this.lblRefundLabel.Size = new System.Drawing.Size(164, 20);
            this.lblRefundLabel.TabIndex = 2;
            this.lblRefundLabel.Text = "Adjusted Refund (Rs):";
            // 
            // numRefundAmount
            // 
            this.numRefundAmount.DecimalPlaces = 2;
            this.numRefundAmount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.numRefundAmount.Location = new System.Drawing.Point(175, 83);
            this.numRefundAmount.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numRefundAmount.Name = "numRefundAmount";
            this.numRefundAmount.Size = new System.Drawing.Size(200, 32);
            this.numRefundAmount.TabIndex = 1;
            this.numRefundAmount.ThousandsSeparator = true;
            this.numRefundAmount.ValueChanged += new System.EventHandler(this.numRefundAmount_ValueChanged);
            // 
            // lblAdjustmentNote
            // 
            this.lblAdjustmentNote.AutoSize = true;
            this.lblAdjustmentNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblAdjustmentNote.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblAdjustmentNote.Location = new System.Drawing.Point(16, 125);
            this.lblAdjustmentNote.Name = "lblAdjustmentNote";
            this.lblAdjustmentNote.Size = new System.Drawing.Size(104, 20);
            this.lblAdjustmentNote.TabIndex = 3;
            this.lblAdjustmentNote.Text = "No adjustment";
            // 
            // panelActions
            // 
            this.panelActions.BackColor = System.Drawing.Color.White;
            this.panelActions.Controls.Add(this.btnProcessReturn);
            this.panelActions.Controls.Add(this.btnCancelReturn);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(0, 850);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.panelActions.Size = new System.Drawing.Size(1400, 60);
            this.panelActions.TabIndex = 6;
            // 
            // btnProcessReturn
            // 
            this.btnProcessReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProcessReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(34)))));
            this.btnProcessReturn.FlatAppearance.BorderSize = 0;
            this.btnProcessReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessReturn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProcessReturn.ForeColor = System.Drawing.Color.White;
            this.btnProcessReturn.IconChar = FontAwesome.Sharp.IconChar.CircleCheck;
            this.btnProcessReturn.IconColor = System.Drawing.Color.White;
            this.btnProcessReturn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnProcessReturn.IconSize = 18;
            this.btnProcessReturn.Location = new System.Drawing.Point(16, 8);
            this.btnProcessReturn.Name = "btnProcessReturn";
            this.btnProcessReturn.Size = new System.Drawing.Size(200, 44);
            this.btnProcessReturn.TabIndex = 0;
            this.btnProcessReturn.Text = "  Process Return";
            this.btnProcessReturn.UseVisualStyleBackColor = false;
            this.btnProcessReturn.Click += new System.EventHandler(this.btnProcessReturn_Click);
            // 
            // btnCancelReturn
            // 
            this.btnCancelReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnCancelReturn.FlatAppearance.BorderSize = 0;
            this.btnCancelReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelReturn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelReturn.ForeColor = System.Drawing.Color.White;
            this.btnCancelReturn.IconChar = FontAwesome.Sharp.IconChar.CircleXmark;
            this.btnCancelReturn.IconColor = System.Drawing.Color.White;
            this.btnCancelReturn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelReturn.IconSize = 18;
            this.btnCancelReturn.Location = new System.Drawing.Point(228, 8);
            this.btnCancelReturn.Name = "btnCancelReturn";
            this.btnCancelReturn.Size = new System.Drawing.Size(160, 44);
            this.btnCancelReturn.TabIndex = 1;
            this.btnCancelReturn.Text = "  Reset Form";
            this.btnCancelReturn.UseVisualStyleBackColor = false;
            this.btnCancelReturn.Click += new System.EventHandler(this.btnCancelReturn_Click);
            // 
            // CustomerReturns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1400, 910);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelRefund);
            this.Controls.Add(this.panelReturnDetails);
            this.Controls.Add(this.panelReturnItems);
            this.Controls.Add(this.panelBillItems);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelHeader);
            this.Name = "CustomerReturns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Returns";
            this.Load += new System.EventHandler(this.CustomerReturns_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelBillItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillItems)).EndInit();
            this.panelReturnItems.ResumeLayout(false);
            this.panelReturnItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnItems)).EndInit();
            this.panelReturnDetails.ResumeLayout(false);
            this.panelReturnDetails.PerformLayout();
            this.panelRefund.ResumeLayout(false);
            this.panelRefund.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefundAmount)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // ── Header ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        // ── Search ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblBillNumberLabel;
        private System.Windows.Forms.TextBox txtBillNumber;
        private FontAwesome.Sharp.IconButton btnSearchBill;
        private System.Windows.Forms.Label lblBillInfo;

        // ── Bill Items ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelBillItems;
        private System.Windows.Forms.Label lblBillItemsTitle;
        private System.Windows.Forms.DataGridView dgvBillItems;

        // ── Return Items ──────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelReturnItems;
        private System.Windows.Forms.Label lblReturnItemsTitle;
        private System.Windows.Forms.Label lblReturnItemCount;
        private System.Windows.Forms.DataGridView dgvReturnItems;

        // ── Return Details ────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelReturnDetails;
        private System.Windows.Forms.Label lblReasonLabel;
        private System.Windows.Forms.TextBox txtReturnReason;
        private System.Windows.Forms.Label lblNotesLabel;
        private System.Windows.Forms.TextBox txtReturnNotes;
        private System.Windows.Forms.CheckBox chkRestoreInventory;
        private System.Windows.Forms.Label lblInventoryNote;

        // ── Refund ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelRefund;
        private System.Windows.Forms.Label lblRefundDescription;
        private System.Windows.Forms.Label lblCalculatedRefund;
        private System.Windows.Forms.Label lblRefundLabel;
        private System.Windows.Forms.NumericUpDown numRefundAmount;
        private System.Windows.Forms.Label lblAdjustmentNote;

        // ── Actions ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelActions;
        private FontAwesome.Sharp.IconButton btnProcessReturn;
        private FontAwesome.Sharp.IconButton btnCancelReturn;
    }
}