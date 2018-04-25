using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.WarehouseManagement
{
    [Table("GroupLocations")]
    public class GroupLocation : BaseEntity
    {
        public GroupLocation()
        {
            this.Location = new HashSet<Location>();
        }

        [Key]
        public int GroupLocIDSys { get; set; }
        public int LocTypeIDSys { get; set; }
        public int? WHIDSys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int16? Left { get; set; }
        public Int16? Top { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public int? RotateType { get; set; }
        public int? ZoneIDSys { get; set; }
        public int? Floor { get; set; }
        public int? ZoneID { get; set; }
        public byte Rows { get; set; }
        public byte Columns { get; set; }

        [ForeignKey("LocTypeIDSys")]
        public virtual LocationType LocationType { get; set; }

        [ForeignKey("GroupLocIDSys")]
        public virtual ICollection<Location> Location { get; set; }
        [NotMapped]
        public List<Location> detail;

    }
}
