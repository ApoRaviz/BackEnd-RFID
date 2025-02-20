﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.WarehouseManagement
{
    [Table("RackLayout_MT")]
    public class RackLayout_MT : BaseEntity
    {
        [Key]
        public int ZoneIDSys { get; set; }
        public int ZoneID { get; set; }
        public int RackID { get; set; }
        public Nullable<int> BlockID { get; set; }
        public string Floor { get; set; }
        public Nullable<int> Left { get; set; }
        public Nullable<int> Top { get; set; }
    }
}
