using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    public  class Variants
    {
        public int variant_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string size {  get; set; }
        public string unit_of_measure {  get; set; }
        public string class_type { get; set; }
        public decimal price_per_unit { get; set; }
        public decimal price_per_lenght { get; set; }
        public decimal lenght_in_feet { get; set; }
        public decimal quantity_in_stock { get; set; }
        public decimal reorder_level { get; set; }
        public decimal minimum_order_quantity { get; set; }

        public bool is_active { get; set; }
        public DateTime CeratedAt {  get; set; }
        public DateTime UpdatedAt {  get; set; }


    }
}
