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
    [Table("InventoryTransactions")]
    public class InventoryTransaction : BaseEntity
    {
        [Key]
        public int InvenTranIDSys { get; set; }
        public int ReceiveIDSys { get; set; }
        public Nullable<DateTime> ReceivingDate { get; set; }
        public int ItemIDSys { get; set; }
        public string SerialNo { get; set; }
        public int Qty { get; set; }
        public int UnitIDSys { get; set; }
        public int LocIDSys { get; set; }
        public int ConvertedQty { get; set; }
        public int StatusIDSys { get; set; }
    }
}
