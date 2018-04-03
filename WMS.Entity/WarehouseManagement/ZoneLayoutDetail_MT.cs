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
    [Table("ZoneLayoutDetail_MT")]
    public class ZoneLayoutDetail_MT : BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int ZoneIDSys { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Floor { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ZoneID { get; set; }
        public Nullable<int> ZoneParentID { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> Left { get; set; }
        public Nullable<int> Top { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Length { get; set; }
        public string Use { get; set; }
        public string Type { get; set; }

        [ForeignKey("ZoneIDSys")]
        public virtual ZoneLayoutHeader_MT ZoneLayoutHeader_MT { get; set; }
    }
}
