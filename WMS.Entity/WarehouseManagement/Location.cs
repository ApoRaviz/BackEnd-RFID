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
    [Table("Locations")]
    public class Location : BaseEntity
    {
        [Key]
        public int LocIDSys { get; set; }
        public string LocNo { get; set; }
        public int GroupLocIDSys { get; set; }
        public int DimensionIDSys { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }

    }
}
