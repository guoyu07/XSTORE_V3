using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("cell")]
    public class Cell
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string mac { get; set; }
        public int pos { get; set; }
        public int part { get; set; }
        public int? product_id { get; set; }
        public int? store_id { get; set; }
    }
}
