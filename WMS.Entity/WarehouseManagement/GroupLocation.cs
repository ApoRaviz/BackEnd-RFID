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
        public string Name { get; set; }
        public string Description { get; set; }
        public Int16? Left { get; set; }
        public Int16? Top { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public int? ZoneIDSys { get; set; }
        public int? Floor { get; set; }
        public int? ZoneID { get; set; }
        public byte Rows { get; set; }
        public byte Columns { get; set; }

        [NotMapped]
        public string StyleObject
        {
            get
            {
                string s = "{\"left\":\"" + (this.Left == null? 0 : this.Left) + "px"
                    + "\",\"top\":\"" + (this.Top == null ? 0 : this.Top) + "px"
                    + "\",\"width\":\"" + (this.Width == null ? 0 : this.Width) + "px"
                    + "\",\"length\":\"" + (this.Length == null ? 0 : this.Length) + "px"
                    + "\",\"height\":\"" + (this.Height == null ? 0 : this.Height) + "px"
                    + "\",\"rotate\":\"10deg\"}";
                return s;
            }

            set
            {

            }
        }

        [ForeignKey("LocTypeIDSys")]
        public virtual LocationType LocationType { get; set; }

        [ForeignKey("GroupLocIDSys")]
        public virtual ICollection<Location> Location { get; set; }
        public List<Location> detail;

    }
}
