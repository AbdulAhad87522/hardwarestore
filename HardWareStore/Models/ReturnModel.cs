using System.Collections.Generic;

namespace HardWareStore.Models
{
    /// <summary>
    /// One line item being returned (built from a bill_items row)
    /// </summary>
    public class ReturnLineItem
    {
        public int VariantId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal MaxQuantity { get; set; }  // Original qty on that bill line
    }

    /// <summary>
    /// Everything needed to create a return record
    /// </summary>
    public class ReturnRequest
    {
        public int BillId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public bool RestoreStock { get; set; }
        public List<ReturnLineItem> Items { get; set; } = new List<ReturnLineItem>();
    }

    /// <summary>
    /// Flat row returned from GetBillByNumber / GetBillItems queries
    /// </summary>
    public class BillSummary
    {
        public int bill_id { get; set; }
        public string bill_number { get; set; }
        public string customer_name { get; set; }
        public System.DateTime bill_date { get; set; }
        public decimal total_amount { get; set; }
    }

    public class BillItemRow
    {
        public int bill_item_id { get; set; }
        public int bill_id { get; set; }
        public int product_id { get; set; }
        public int variant_id { get; set; }
        public string product_name { get; set; }
        public string size { get; set; }
        public string unit_of_measure { get; set; }
        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal line_total { get; set; }
        public string notes { get; set; }
    }
}