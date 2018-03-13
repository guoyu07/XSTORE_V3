using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("user_history")]
    public class UserHistory
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public string username { get; set; }
        public string company { get; set; }
        public DateTime create_date { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string qq { get; set; }
        public string realname { get; set; }
        public int state { get; set; }
        public string weichat { get; set; }
    }
}
