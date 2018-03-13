using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("user_role")]
    public class UserRole
    {
        [Column(IsPrimaryKey = true)]
        public int role_id { get; set; }
        public string username { get; set; }
    }
}
