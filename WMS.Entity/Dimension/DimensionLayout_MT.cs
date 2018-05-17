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
        [Required]
        public string FormatName { get; set; }
        [MaxLength(10)]
        [Required]
        public string Unit { get; set; }
        [Required]
        public double? Width { get; set; }
        [Required]
        public double? Length { get; set; }
        [Required]
        public double? Height { get; set; }
        [Required]
        public double? Weight { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
