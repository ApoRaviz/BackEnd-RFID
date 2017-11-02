using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.WarehouseManagement
{
    [Table("Warehouse_MT")]
    public class Warehouse_MT : BaseEntity
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

    }
}
