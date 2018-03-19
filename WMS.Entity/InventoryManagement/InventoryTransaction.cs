using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WMS.Entity.ItemManagement;
using WMS.Entity.Receiving;

namespace WMS.Entity.InventoryManagement
{
    [Table("InventoryTransactions")]
    public class InventoryTransaction : BaseEntity
    {
        [Key]
        public int InvenTranIDSys { get; set; }
        public int InvenIDSys { get; set; }
        public int ReceiveIDSys { get; set; }
        public DateTime ReceivingDate { get; set; }
        public int ItemIDSys { get; set; }
        public string SerialNo { get; set; }
        public decimal Qty { get; set; }
        public int UnitIDSys { get; set; }
        //public int LocIDSys { get; set; }
        public int ConvertedQty { get; set; }
        public int StatusIDSys { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Receive Receive { get; set; }
        public virtual Item_MT Item_MT { get; set; }
    }
}
