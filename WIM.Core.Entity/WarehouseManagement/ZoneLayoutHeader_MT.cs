﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.WarehouseManagement
{
    [Table("ZoneLayoutDetail_MT")]
    public class ZoneLayoutHeader_MT
    {
        public ZoneLayoutHeader_MT()
        {
            this.ZoneLayoutDetail_MT = new HashSet<ZoneLayoutDetail_MT>();
        }

        [Key]
        public int ZoneIDSys { get; set; }
        public string ZoneName { get; set; }
        public string Warehouse { get; set; }
        public string Area { get; set; }
        public Nullable<int> TotalFloor { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual ICollection<ZoneLayoutDetail_MT> ZoneLayoutDetail_MT { get; set; }
        public List<ZoneLayoutDetail_MT> detail;
    }
}
