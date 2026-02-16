using System;

namespace HardWareStore.Models
{
    public class SupplierPaymentRecord
    {
        public int payment_id { get; set; }
        public int supplier_id { get; set; }
        public int batch_id { get; set; }
        public decimal payment_amount { get; set; }
        public DateTime payment_date { get; set; }
        public string remarks { get; set; }
        public DateTime created_at { get; set; }

        // For display purposes
        public string supplier_name { get; set; }
        public string batch_name { get; set; }
    }
}