using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class PayAjaxResponse: AjaxResponse
    {
        public bool pay { get; set; }
        public bool deliver { get; set; }
    }
}
