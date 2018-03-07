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
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
    }
}
