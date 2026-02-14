using System;

namespace HardWareStore.Models
{
    public class PurchaseBatch
    {
        public int batch_id { get; set; }
        public int supplier_id { get; set; }
        public string BatchName { get; set; }
        public decimal total_price { get; set; }
        public decimal paid { get; set; }
        public string status { get; set; } // "Pending", "Completed", "Partial"
        public DateTime CreatedAt { get; set; }

        // For display purposes
        public string supplier_name { get; set; }
        public decimal remaining { get; set; }
    }
}