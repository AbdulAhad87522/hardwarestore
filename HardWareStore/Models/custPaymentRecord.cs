using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    public class custPaymentRecord
    {
        public int PaymentId { get; set; }
        public int customerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Paid { get; set; }
        public decimal RemainingBalance { get; set; }
        public string CustomerName { get; set; }
        public decimal AllocatedAmount { get; internal set; }
        public int SaleId { get; internal set; }

    }
}
