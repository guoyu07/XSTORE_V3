using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("product")]
    public class Product
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        public string barcode { get; set; }
        public int category { get; set; }
        public string code { get; set; }
        /// <summary>
        /// 是否是复合商品
        /// </summary>
        public int composite { get; set; }
        public int hotel_id { get; set; }
        public string html { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 地板价（单位：分）
        /// </summary>
        public int price0 { get; set; }
        /// <summary>
        /// 销售价（单位：分）
        /// </summary>
        public int price1 { get; set; }
        public int state { get; set; }
        public int to_expire { get; set; }
        public string unit { get; set; }
    }
}
