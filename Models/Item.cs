using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Models
{
    public class Item
    {
            public int Id { get; set; }
            public string ItemName { get; set; }
            public string Price { get; set; }
            public int UnitId { get; set; }
            public string CompanyCode { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
    }

    public class Unit
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public string CompanyCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

}
