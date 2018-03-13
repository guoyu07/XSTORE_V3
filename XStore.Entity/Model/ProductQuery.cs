using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class ProductQuery:Product
    {
        /// <summary>
        /// 是否售罄
        /// </summary>
        public bool sell_out { get; set; } = false;

    }
}
