using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("cabinet_layout")]
    public class CabinetLayout
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public int hotel_id { get; set; }
        public string products { get; set; }
    }
}
