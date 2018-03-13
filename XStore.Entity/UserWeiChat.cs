using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("user_weichat")]
    public class UserWeiChat
    {
        [Column(IsPrimaryKey =true)]
        public string openid { get; set; }
        public string nickname { get; set; }
        public string headpic { get; set; }
        public string unionid { get; set; }
        public DateTime createtime { get; set; }
    }
}
