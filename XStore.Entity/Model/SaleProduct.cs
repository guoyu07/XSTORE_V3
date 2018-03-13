using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class SaleProduct:Product
    {
        public int salesCount { get; set; } = 0;
        public float totalAmount { get; set; } = 0;
        public DateTime saleTime { get; set; }
        public DateTime minTime { get; set; }
        public DateTime maxTime { get; set; }
    }
}
