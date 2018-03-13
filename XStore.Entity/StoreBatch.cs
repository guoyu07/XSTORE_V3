using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("store_batch")]
    public class StoreBatch
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public int hotel_id { get; set;}
        public int record_id { get; set; }
        public int remaining { get; set; }
        public int unit { get; set; }
        public int batch_id { get; set; }
    }
}
