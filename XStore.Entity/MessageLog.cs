using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("message_log")]
    public class MessageLog
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string code { get; set; }
        public string phone { get; set; }
        public string openId { get; set;}
        public DateTime createTime { get; set; }
    }
}
