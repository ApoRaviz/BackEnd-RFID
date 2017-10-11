using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.WarehouseManagement
{
    [Table("Warehouse_MT")]
    public class Warehouse_MT
    {
        [Key]
        public int WHIDSys { get; set; }
        public string WHID { get; set; }
        public string WHName { get; set; }
        public int Size { get; set; }
        public string Address { get; set; }
        public string SubCity { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string CountryCode { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
    }
}
