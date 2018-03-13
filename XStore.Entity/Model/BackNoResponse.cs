using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class BackNoResponse
    {
        public string operationStatus { get; set; }
        public List<int> operationMessage { get; set; }
    }
}
