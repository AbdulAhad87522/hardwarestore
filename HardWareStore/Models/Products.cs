using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    public  class Products
    {
        public int product_id {  get; set; }
        public string Name { get; set; }
        public int category_id {  get; set; }
        public string description {  get; set; }
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public bool has_variants { get; set; }
        public bool is_active {  get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt { get; set; }



    }
}
