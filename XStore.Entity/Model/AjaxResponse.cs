using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class AjaxResponse
    {
        public bool success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
}
