using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class BuyRequest
    {

        public string openId { get; set; }
        public string mac { get; set; }
        public int position { get; set; }
    }
}
