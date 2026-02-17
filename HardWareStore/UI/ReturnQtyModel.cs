using System;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class ReturnQtyDialog : Form
    {
        /// <summary>The quantity confirmed by staff.</summary>
        public decimal SelectedQty { get; private set; }

        private readonly decimal _maxQty;

        public ReturnQtyDialog(string productName, string size, string unit,
                               decimal maxQty, decimal unitPrice)
        {
            InitializeComponent();

            _maxQty = maxQty;

            // Populate labels
            lblProductName.Text = productName;
            lblDetail.Text = $"Size: {size}    Unit: {unit}    Unit Price: Rs {unitPrice:N2}";

            // Configure numeric input
            numQty.Minimum = 0.01M;
            numQty.Maximum = maxQty;
            numQty.Value = maxQty;   // default = full quantity
            numQty.Increment = 1;

            lblMax.Text = $"Max: {maxQty} {unit}";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (numQty.Value <= 0 || numQty.Value > _maxQty)
            {
                MessageBox.Show(
                    $"Quantity must be between 0.01 and {_maxQty}.",
                    "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedQty = numQty.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}