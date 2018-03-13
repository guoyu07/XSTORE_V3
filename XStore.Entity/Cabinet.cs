using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("cabinet")]
    public class Cabinet
    {
        [Column(IsPrimaryKey = true)]
        public string mac { get; set; }
        public DateTime? date { get; set; }
        public int? dealer { get; set; }
        public string flow_id { get; set; }
        public int? hotel { get; set; }
        public bool? online { get; set; }
        public string products { get; set; }
        public string room { get; set; }
        public int state { get; set; }
        public string type { get; set; }
        //public string version { get; set; }
        public string code { get; set; }
        public string iccid { get; set; }
        public string iot { get; set; }
        public DateTime? last_offline_date { get; set; }
        public string version { get; set; }
    }
}
