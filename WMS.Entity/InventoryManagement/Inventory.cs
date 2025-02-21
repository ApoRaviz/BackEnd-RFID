﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WMS.Entity.WarehouseManagement;

namespace WMS.Entity.InventoryManagement
{
    [Table("Inventories")]
    public class Inventory : BaseEntity
    {
        [Key]
        public int InvenIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int AvailableQty { get; set; }
        public int InboundQty { get; set; }
        public int OutboundQty { get; set; }
        public int LocIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public string Inspect { get; set; }
        public Nullable<DateTime> Expire { get; set; }
        public string Dimension { get; set; }
        public string ControlLevel1 { get; set; }
        public string ControlLevel2 { get; set; }
        public string ControlLevel3 { get; set; }
        public int? ItemModeIDSys { get; set; }
        public int? ItemSetIDSys { get; set; }
        public string Remark { get; set; }

        public virtual Location Location { get; set; }
    }
}
