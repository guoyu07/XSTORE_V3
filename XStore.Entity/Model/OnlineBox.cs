using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class OnlineBox
    {
        public string mac { get; set; }
        public bool online { get; set; }
        public DateTime lineTime { get; set; }
    }
}
