using HardWareStore.DL;
using HardWareStore.Interfaces;
using HardWareStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HardWareStore.UI
{
    public partial class CustomerReturns : Form
    {
        private readonly IReturnsDL _returnsDL;
        private readonly IBillsDL _billsDL;

        // Items the user has added to the return list
        private readonly List<ReturnLineItem> _returnItems = new List<ReturnLineItem>();

        // Currently loaded bill
        private int _loadedBillId = -1;

        public CustomerReturns(IReturnsDL returnsDL, IBillsDL billsDL)
        {
            InitializeComponent();
            _returnsDL = returnsDL;
            _billsDL = billsDL;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Form Load
        // ─────────────────────────────────────────────────────────────────────

        private void CustomerReturns_Load(object sender, EventArgs e)
        {
            CustomizeBillItemsGrid();
            CustomizeReturnItemsGrid();
            ResetForm();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Bill Search
        // ─────────────────────────────────────────────────────────────────────

        private void btnSearchBill_Click(object sender, EventArgs e)
        {
            string billNumber = txtBillNumber.Text.Trim();
            if (string.IsNullOrWhiteSpace(billNumber))
            {
                MessageBox.Show("Please enter a bill number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSearchBill.Enabled = false;
                lblBillInfo.Text = "Searching...";

                var bill = _billsDL.GetBillByNumber(billNumber);

                if (bill == null)
                {
                    MessageBox.Show($"No bill found with number '{billNumber}'.",
                        "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblBillInfo.Text = "Bill not found.";
                    ClearBillSection();
                    return;
                }

                _loadedBillId = bill.bill_id;

                lblBillInfo.Text =
                    $"Bill: {bill.bill_number}   |   " +
                    $"Customer: {bill.customer_name}   |   " +
                    $"Date: {bill.bill_date:dd MMM yyyy}   |   " +
                    $"Total: Rs {bill.total_amount:N2}";

                // Load bill line items
                var items = _billsDL.GetBillItems(bill.bill_id);
                dgvBillItems.DataSource = items;
                CustomizeBillItemsGrid();

                panelBillItems.Visible = true;
                _returnItems.Clear();
                RefreshReturnGrid();
                RecalcRefund();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bill: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearchBill.Enabled = true;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Add item to return list (via "↩ Return" button in bill items grid)
        // ─────────────────────────────────────────────────────────────────────

        private void dgvBillItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvBillItems.Columns[e.ColumnIndex].Name != "colAddReturn") return;

            var row = dgvBillItems.Rows[e.RowIndex];

            int variantId = Convert.ToInt32(row.Cells["variant_id"].Value);
            string productName = row.Cells["product_name"].Value?.ToString() ?? "";
            string size = row.Cells["size"].Value?.ToString() ?? "";
            string unit = row.Cells["unit_of_measure"].Value?.ToString() ?? "";
            decimal maxQty = Convert.ToDecimal(row.Cells["quantity"].Value);
            decimal unitPrice = Convert.ToDecimal(row.Cells["unit_price"].Value);

            // Ask for quantity via small dialog
            using (var qtyForm = new ReturnQtyDialog(productName, size, unit, maxQty, unitPrice))
            {
                if (qtyForm.ShowDialog() != DialogResult.OK) return;

                decimal returnQty = qtyForm.SelectedQty;

                // If same variant already in list — merge quantities
                var existing = _returnItems.Find(x => x.VariantId == variantId);
                if (existing != null)
                {
                    decimal combined = existing.Quantity + returnQty;
                    if (combined > maxQty)
                    {
                        MessageBox.Show(
                            $"Total return quantity ({combined}) exceeds original sold quantity ({maxQty}).",
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    existing.Quantity = combined;
                    existing.LineTotal = combined * unitPrice;
                }
                else
                {
                    _returnItems.Add(new ReturnLineItem
                    {
                        VariantId = variantId,
                        ProductName = productName,
                        Size = size,
                        Unit = unit,
                        Quantity = returnQty,
                        UnitPrice = unitPrice,
                        LineTotal = returnQty * unitPrice,
                        MaxQuantity = maxQty
                    });
                }
            }

            RefreshReturnGrid();
            RecalcRefund();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Remove item from return list
        // ─────────────────────────────────────────────────────────────────────

        private void dgvReturnItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvReturnItems.Columns[e.ColumnIndex].Name != "colRemove") return;

            _returnItems.RemoveAt(e.RowIndex);
            RefreshReturnGrid();
            RecalcRefund();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Refund amount manually changed by staff
        // ─────────────────────────────────────────────────────────────────────

        private void numRefundAmount_ValueChanged(object sender, EventArgs e)
        {
            decimal calculated = GetCalculatedRefund();
            decimal adjusted = numRefundAmount.Value;
            decimal diff = adjusted - calculated;

            if (diff > 0)
                lblAdjustmentNote.Text = $"▲ Rs {diff:N2} added by staff";
            else if (diff < 0)
                lblAdjustmentNote.Text = $"▼ Rs {Math.Abs(diff):N2} deducted by staff";
            else
                lblAdjustmentNote.Text = "No adjustment";
        }

        // ─────────────────────────────────────────────────────────────────────
        // Process Return
        // ─────────────────────────────────────────────────────────────────────

        private void btnProcessReturn_Click(object sender, EventArgs e)
        {
            if (_loadedBillId == -1)
            {
                MessageBox.Show("Please search and load a bill first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_returnItems.Count == 0)
            {
                MessageBox.Show("No items added to the return list.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtReturnReason.Text))
            {
                MessageBox.Show("Please provide a reason for the return.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool restoreStock = chkRestoreInventory.Checked;
            decimal refundAmount = numRefundAmount.Value;
            string reason = txtReturnReason.Text.Trim();
            string notes = txtReturnNotes.Text.Trim();

            string stockMsg = restoreStock
                ? "✔ Stock WILL be restored to inventory."
                : "✘ Stock will NOT be restored.";

            DialogResult confirm = MessageBox.Show(
                $"Confirm Return?\n\n" +
                $"Bill: {txtBillNumber.Text.Trim()}\n" +
                $"Items: {_returnItems.Count}\n" +
                $"Refund Amount: Rs {refundAmount:N2}\n" +
                $"{stockMsg}\n\nProceed?",
                "Confirm Return",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                btnProcessReturn.Enabled = false;

                var returnRequest = new ReturnRequest
                {
                    BillId = _loadedBillId,
                    RefundAmount = refundAmount,
                    Reason = reason,
                    Notes = string.IsNullOrWhiteSpace(notes) ? null : notes,
                    RestoreStock = restoreStock,
                    Items = _returnItems
                };

                bool success = _returnsDL.ProcessReturn(returnRequest);

                if (success)
                {
                    MessageBox.Show(
                        "Return processed successfully!\n" +
                        (restoreStock ? "Inventory has been updated." : "Inventory was not changed."),
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Failed to process return. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing return: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnProcessReturn.Enabled = true;
            }
        }

        private void btnCancelReturn_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────────

        private decimal GetCalculatedRefund()
        {
            decimal total = 0;
            foreach (var item in _returnItems)
                total += item.LineTotal;
            return total;
        }

        private void RecalcRefund()
        {
            decimal calculated = GetCalculatedRefund();
            numRefundAmount.Value = calculated;
            lblCalculatedRefund.Text = $"Calculated Refund: Rs {calculated:N2}";
            lblAdjustmentNote.Text = "No adjustment";
        }

        private void RefreshReturnGrid()
        {
            dgvReturnItems.DataSource = null;

            var dt = new DataTable();
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("Unit Price", typeof(decimal));
            dt.Columns.Add("Line Total", typeof(decimal));

            foreach (var item in _returnItems)
                dt.Rows.Add(item.ProductName, item.Size, item.Unit,
                            item.Quantity, item.UnitPrice, item.LineTotal);

            dgvReturnItems.DataSource = dt;
            CustomizeReturnItemsGrid();
            AddRemoveButtonToReturnGrid();

            lblReturnItemCount.Text = $"Return Items: {_returnItems.Count}";
        }

        private void AddRemoveButtonToReturnGrid()
        {
            if (dgvReturnItems.Columns.Contains("colRemove")) return;
            var btn = new DataGridViewButtonColumn
            {
                Name = "colRemove",
                HeaderText = "",
                Text = "Remove",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
            dgvReturnItems.Columns.Add(btn);
        }

        private void ClearBillSection()
        {
            _loadedBillId = -1;
            dgvBillItems.DataSource = null;
            panelBillItems.Visible = false;
            _returnItems.Clear();
            RefreshReturnGrid();
            RecalcRefund();
        }

        private void ResetForm()
        {
            txtBillNumber.Clear();
            lblBillInfo.Text = "No bill loaded";
            ClearBillSection();
            txtReturnReason.Clear();
            txtReturnNotes.Clear();
            chkRestoreInventory.Checked = true;
            numRefundAmount.Value = 0;
            lblAdjustmentNote.Text = "No adjustment";
            lblCalculatedRefund.Text = "Calculated Refund: Rs 0.00";
            lblReturnItemCount.Text = "Return Items: 0";
        }

        // ─────────────────────────────────────────────────────────────────────
        // Enter key on Bill Number textbox
        // ─────────────────────────────────────────────────────────────────────

        private void txtBillNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearchBill.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Restore inventory checkbox toggle — update warning label
        // ─────────────────────────────────────────────────────────────────────

        private void chkRestoreInventory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRestoreInventory.Checked)
            {
                lblInventoryNote.ForeColor = Color.Gray;
                lblInventoryNote.Text = "Uncheck if returned items are damaged and cannot be resold";
            }
            else
            {
                lblInventoryNote.ForeColor = Color.OrangeRed;
                lblInventoryNote.Text = "⚠ Stock will NOT be added back to inventory";
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Grid Styling
        // ─────────────────────────────────────────────────────────────────────

        private void CustomizeBillItemsGrid()
        {
            StyleGrid(dgvBillItems);

            // Hide internal ID columns from user
            if (dgvBillItems.Columns.Contains("bill_item_id"))
                dgvBillItems.Columns["bill_item_id"].Visible = false;
            if (dgvBillItems.Columns.Contains("bill_id"))
                dgvBillItems.Columns["bill_id"].Visible = false;
            if (dgvBillItems.Columns.Contains("product_id"))
                dgvBillItems.Columns["product_id"].Visible = false;
            if (dgvBillItems.Columns.Contains("notes"))
                dgvBillItems.Columns["notes"].Visible = false;

            // Keep variant_id visible=false but accessible for click handler
            if (dgvBillItems.Columns.Contains("variant_id"))
                dgvBillItems.Columns["variant_id"].Visible = false;

            // Friendly header names
            if (dgvBillItems.Columns.Contains("product_name"))
                dgvBillItems.Columns["product_name"].HeaderText = "Product";
            if (dgvBillItems.Columns.Contains("size"))
                dgvBillItems.Columns["size"].HeaderText = "Size";
            if (dgvBillItems.Columns.Contains("unit_of_measure"))
                dgvBillItems.Columns["unit_of_measure"].HeaderText = "Unit";
            if (dgvBillItems.Columns.Contains("quantity"))
                dgvBillItems.Columns["quantity"].HeaderText = "Qty Sold";
            if (dgvBillItems.Columns.Contains("unit_price"))
                dgvBillItems.Columns["unit_price"].HeaderText = "Unit Price";
            if (dgvBillItems.Columns.Contains("line_total"))
                dgvBillItems.Columns["line_total"].HeaderText = "Line Total";

            // Add Return button column only once
            if (!dgvBillItems.Columns.Contains("colAddReturn"))
            {
                var btn = new DataGridViewButtonColumn
                {
                    Name = "colAddReturn",
                    HeaderText = "Action",
                    Text = "↩ Return",
                    UseColumnTextForButtonValue = true,
                    Width = 90,
                    FlatStyle = FlatStyle.Flat
                };
                dgvBillItems.Columns.Add(btn);
            }
            dgvBillItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CustomizeReturnItemsGrid()
        {
            StyleGrid(dgvReturnItems);
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            grid.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            grid.BackgroundColor = Color.White;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grid.RowTemplate.Height = 32;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = false; // buttons must remain clickable
        }
    }
}