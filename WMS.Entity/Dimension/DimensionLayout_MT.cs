using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.Dimension
{
    [Table("DimensionLayout_MT")]
    public class DimensionLayout_MT : BaseEntity
    {
        [Key]
        public int DimensionIDSys { get; set; }
        public string FormatName { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Length { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
    }
}
