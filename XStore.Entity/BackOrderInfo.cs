using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("back_order_info")]
    public class BackOrderInfo
    {
        public int id { get; set; }
        public bool composite { get; set; }
        public DateTime date { get; set; }
        public bool delivered { get; set; }
        public string pos { get; set; }
        public int store { get; set; }
        public string cabinet_mac { get; set; }
        public string operator_username { get;set;}
        public int? product { get; set; }
    }

}
