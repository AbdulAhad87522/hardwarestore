using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    internal class QuotationData
    {
        public int QuotationId { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime QuotationDate { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime ValidUntil { get; set; }
        public string Notes { get; set; }
        public BindingList<QuotationItemData> Items { get; set; }
    }
}
