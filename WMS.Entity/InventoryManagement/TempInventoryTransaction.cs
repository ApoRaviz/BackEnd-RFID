using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Entity;

namespace WMS.Entity.InventoryManagement
{
    [Table("TempInventoryTransactions")]
    public class TempInventoryTransaction : BaseEntity
    {
        [Key]
        public int InvenTranIDSys { get; set; }
        public int ItemIDSys { get; set; }
        [CreateOnly]
        [SameCreateAt]
        public DateTime ReceivingDate { get; set; }
        public decimal Qty { get; set; }
        public int ConvertedQty { get; set; }
        public int UnitIDSys { get; set; }
        public string RefNO { get; set; }
        public int StatusIDSys { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        
        //Additional Field, Don't Exists in InventoryTransactions Table
        public string ControlLevel1 { get; set; }
        public string ControlLevel2 { get; set; }
        public string ControlLevel3 { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }        
        public int LocIDSys { get; set; }
        public string LocNo { get; set; }
        public DateTime Expire { get; set; }
        public string Inspect { get; set; }
        public string Dimention { get; set; }
        public double UsedDimension { get; set; }
    }
}
