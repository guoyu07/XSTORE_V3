using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("cabinet_check_log")]
    public class CabinetCheckLog
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string mac { get; set; }
        public string openid { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public DateTime createtime { get; set; }
        public string message { get; set; }
        public bool issuccess { get; set; }
    }
}
