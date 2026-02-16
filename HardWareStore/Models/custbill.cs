using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    public class custbill
    {
        public string full_name { get; set; }
        public int customer_id { get; set; }
        public decimal total_amount { get; set; }
        public decimal paid { get; set; }
        public decimal remaining { get; set; }
    }
}
