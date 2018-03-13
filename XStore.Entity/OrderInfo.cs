using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("order_info")]
    public class OrderInfo
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public string code { get; set; }
        public int? payType { get; set; }
        public DateTime date { get; set; }
        public bool delivered { get; set; }
        public string month { get; set;}
        public string open_id { get; set; }
        public bool paid { get; set; }
        public string pay_id { get; set; }
        public int pos { get; set; }
        public int price0 { get; set; }
        public int price1 { get; set; }
        public string cabinet_mac { get;set; }
        public int? store_id { get; set; }
        public int product { get; set; }
        public string note { get; set; }
        public bool? has_push { get; set;}
    }
}
