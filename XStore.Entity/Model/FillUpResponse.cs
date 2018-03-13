using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity.Model
{
    public class FillUpResponse
    {
        public string operationStatus { get; set; }
        public List<HotelQuery> operationMessage { get; set; }
    }
    public class HotelQuery
    {
        public int hotelId { get; set; }
        public int cabNum { get; set;}
        public int proNum { get; set; }
        public bool hasPush { get; set; } = false;
    }
}
