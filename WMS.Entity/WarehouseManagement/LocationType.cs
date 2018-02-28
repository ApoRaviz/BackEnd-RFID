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
    [Table("LocationTypes")]
    public class LocationType : BaseEntity
    {
        [Key]
        public int LocTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 

    }
}
