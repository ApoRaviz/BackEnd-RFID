using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.InventoryManagement
{
    [Table("Inventories")]
    public class Inventory : BaseEntity
    {
        [Key]
        public int InvenIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int QtyAvailable { get; set; }
        public int LocIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public string Serial { get; set; }
        public string Inspect { get; set; }
        public DateTime Expire { get; set; }
        public string Dimension { get; set; }
        public string Box { get; set; }
        public string Lot { get; set; }
        public string Pallet { get; set; }
        public int? ItemSetIDSys { get; set; }

    }
}
