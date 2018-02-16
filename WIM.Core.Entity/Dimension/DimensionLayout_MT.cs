using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Dimension
{
    [Table("DimensionLayout_MT")]
    public class DimensionLayout_MT : BaseEntity
    {
        [Key]
        public int DimensionIDSys { get; set; }
        public string FormatName { get; set; }
        public string Unit { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<System.DateTime> UpdatedDate { get; set; }
        //public string UserUpdate { get; set; }//1232312312312
    }
}
