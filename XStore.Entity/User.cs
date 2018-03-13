using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XStore.Entity
{
    [Table("user")]
    public class User
    {
        [Column(IsPrimaryKey = true)]
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