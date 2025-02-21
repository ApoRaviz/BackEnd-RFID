﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.WarehouseManagement
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
        public int Left { get; set; }
        public int Top { get; set; }
        public string Color { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }

    }
}
