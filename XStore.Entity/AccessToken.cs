using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("access_token")]
    public class AccessToken
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string access_token { get; set; }
        public DateTime createtime { get; set; }
    }
}
