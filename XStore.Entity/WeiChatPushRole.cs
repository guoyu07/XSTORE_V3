using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("weichat_push_role")]
    public class WeiChatPushRole
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string phone { get; set; }
        public DateTime createtime { get; set; }
        public bool successorder { get; set; }
        public bool failorder { get; set; }
        public bool statistics { get; set; }
        public bool systemerror { get; set; }
    }
}
