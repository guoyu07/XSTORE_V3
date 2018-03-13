using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Entity
{
    [Table("hotel")]
    public class Hotel
    {
        [Column(IsPrimaryKey =true)]
        public int id { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public DateTime create_date { get; set; }
        public int? dealer { get; set; }
        public string email { get; set; }
        public string fax { get; set;}
        public string hotel_name { get; set; }
        public string phone { get; set; }
        public string simple_name { get; set; }
        public int? state { get; set; }
        public string tel { get; set; }
        public int? hotel_group_id { get; set; }
        public string code { get; set; }
        public string bank_account { get; set; }
        public string legal_person { get; set;}
        public int? period { get; set; }
    }
}
