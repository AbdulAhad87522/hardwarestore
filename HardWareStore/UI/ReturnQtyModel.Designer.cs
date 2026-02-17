namespace HardWareStore.UI
{
    partial class ReturnQtyDialog
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
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.separator = new System.Windows.Forms.Panel();
            this.lblQtyLabel = new System.Windows.Forms.Label();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.lblMax = new System.Windows.Forms.Label();
            this.btnOk = new FontAwesome.Sharp.IconButton();
            this.btnCancel = new FontAwesome.Sharp.IconButton();

            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            this.SuspendLayout();

            // ── panelHeader ──────────────────────────────────────────────────
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            this.panelHeader.Controls.Add(this.lblProductName);
            this.panelHeader.Controls.Add(this.lblDetail);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(380, 62);
            this.panelHeader.TabIndex = 0;

            // ── lblProductName ───────────────────────────────────────────────
            this.lblProductName.AutoSize = true;
            this.lblProductName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductName.ForeColor = System.Drawing.Color.White;
            this.lblProductName.Location = new System.Drawing.Point(12, 10);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Text = "Product Name";

            // ── lblDetail ────────────────────────────────────────────────────
            this.lblDetail.AutoSize = true;
            this.lblDetail.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDetail.ForeColor = System.Drawing.Color.FromArgb(180, 200, 240);
            this.lblDetail.Location = new System.Drawing.Point(12, 36);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Text = "Size: —   Unit: —   Unit Price: Rs 0.00";

            // ── separator ────────────────────────────────────────────────────
            this.separator.BackColor = System.Drawing.Color.FromArgb(220, 220, 235);
            this.separator.Location = new System.Drawing.Point(0, 62);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(380, 1);
            this.separator.TabIndex = 1;

            // ── lblQtyLabel ──────────────────────────────────────────────────
            this.lblQtyLabel.AutoSize = true;
            this.lblQtyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQtyLabel.ForeColor = System.Drawing.Color.FromArgb(20, 25, 72);
            this.lblQtyLabel.Location = new System.Drawing.Point(16, 80);
            this.lblQtyLabel.Name = "lblQtyLabel";
            this.lblQtyLabel.Text = "Quantity to Return:";

            // ── numQty ───────────────────────────────────────────────────────
            this.numQty.DecimalPlaces = 2;
            this.numQty.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.numQty.Location = new System.Drawing.Point(175, 76);
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(170, 30);
            this.numQty.TabIndex = 0;
            this.numQty.ThousandsSeparator = true;

            // ── lblMax ───────────────────────────────────────────────────────
            this.lblMax.AutoSize = true;
            this.lblMax.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblMax.ForeColor = System.Drawing.Color.DimGray;
            this.lblMax.Location = new System.Drawing.Point(175, 112);
            this.lblMax.Name = "lblMax";
            this.lblMax.Text = "Max: 0";

            // ── btnOk ────────────────────────────────────────────────────────
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.IconChar = FontAwesome.Sharp.IconChar.Check;
            this.btnOk.IconColor = System.Drawing.Color.White;
            this.btnOk.IconSize = 16;
            this.btnOk.Location = new System.Drawing.Point(16, 148);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 36);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "  Confirm";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

            // ── btnCancel ────────────────────────────────────────────────────
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(180, 60, 60);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.btnCancel.IconColor = System.Drawing.Color.White;
            this.btnCancel.IconSize = 16;
            this.btnCancel.Location = new System.Drawing.Point(148, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "  Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── ReturnQtyDialog Form ─────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(380, 204);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.lblQtyLabel);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReturnQtyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Return Quantity";
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.Panel separator;
        private System.Windows.Forms.Label lblQtyLabel;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Label lblMax;
        private FontAwesome.Sharp.IconButton btnOk;
        private FontAwesome.Sharp.IconButton btnCancel;
    }
}