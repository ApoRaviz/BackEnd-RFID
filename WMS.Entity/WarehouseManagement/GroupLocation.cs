using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.WarehouseManagement
{

    [Table("GroupLocations")]
    public class GroupLocation : BaseEntity
    {
        [Key]
        public int GroupLocIDSys { get; set; }
        public int LocTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Left { get; set; }
        public int? Top { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Height { get; set; }
        public int? ZoneIDSys { get; set; }
        public int? Floor { get; set; }
        public int? ZoneID { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public virtual IEnumerable<Location> Location { get; set; }
    }
}
