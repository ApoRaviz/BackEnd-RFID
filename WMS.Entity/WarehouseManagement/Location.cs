﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;
using WMS.Entity.Dimension;

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
        public double? AvailableArea { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }
        public virtual DimensionLayout_MT DimensionLayout_MT { get;set;}
    }
}
