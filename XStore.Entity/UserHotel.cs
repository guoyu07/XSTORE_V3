using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("user_hotels")]
    public class UserHotel
    {
        [Column(IsPrimaryKey = true)]
        public int hotels_id { get; set; }
        [Column(IsPrimaryKey = true)]
        public string user_username { get; set; }
    }
}
