using System;

namespace HardWareStore.Models
{
    public class PurchaseBatchItem
    {
        public int purchase_batch_item_id { get; set; }
        public int purchase_batch_id { get; set; }
        public int variant_id { get; set; }
        public decimal quantity_received { get; set; }
        public decimal cost_price { get; set; }
        public decimal line_total { get; set; }
        public DateTime CreatedAt { get; set; }

        // For display purposes
        public string product_name { get; set; }
        public string size { get; set; }
        public string class_type { get; set; }
        public decimal sale_price { get; set; } // price_per_unit from variants
    }
}