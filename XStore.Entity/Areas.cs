using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XStore.Entity
{
    [Table("areas")]
    public class Areas
    {
        [Column(IsPrimaryKey = true)]
        public int area_id { get; set; }
        public int parent_id { get; set; }
        public string area_name { get; set; }
        public string py_name { get; set; }
        public string py_simple { get; set; }
        public bool area_type { get; set; }
    }
}