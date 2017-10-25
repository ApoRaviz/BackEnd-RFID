using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.WarehouseManagement
{
    [Table("RackLayout")]
    public class RackLayout
    {
        [Key]
        public int ZoneIDSys { get; set; }
        public int ZoneID { get; set; }
        public int RackID { get; set; }
        public Nullable<int> BlockID { get; set; }
        public string Floor { get; set; }
        public Nullable<int> Left { get; set; }
        public Nullable<int> Top { get; set; }
        public string Color { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UserUpdate { get; set; }
    }
}
