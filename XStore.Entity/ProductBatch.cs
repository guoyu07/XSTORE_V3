using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("product_batch")]
    public class ProductBatch
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public string batch_no { get; set; }
        public DateTime expire_date { get; set; }
        public int price { get; set; }
        public DateTime product_date { get; set; }
        public int product_id { get; set; }
    }
}
