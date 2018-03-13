using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("login_record")]
    public class LoginRecord
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public DateTime date { get; set; }
        public string ip { get; set; }
        public int state { get; set; }
        public string username { get; set; }
    }
}
