using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardWareStore.Models
{
    internal class QuotationItemData
    {
        public int QuotationItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int VariantId { get; set; }
        public string Size { get; set; }
        public string ClassType { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal sale_price { get; set; }
        public decimal final { get; set; }
        public string Notes { get; set; }
        public decimal AvailableStock { get; set; }
        public string SupplierName { get; set; }
        public string Category { get; set; }
    }
}
