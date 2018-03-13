using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("weichat_push_faillog")]
    public class WeiChatPushFailLog
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string ordercode { get; set; }
        public string openid { get; set; }
        public bool issuccess { get; set; }
        public DateTime createtime { get; set; }
    }
}
