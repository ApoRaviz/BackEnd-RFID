using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.InventoryManagement
{
    [Table("TempInventoryTransactions")]
    public class TempInventoryTransaction : BaseEntity
    {
        
    [Key]
	public int InvenTranIDSys { get; set; }
	public int? InvenIDSys { get; set; }
	public DateTime ReceivingDate { get; set; }
	public int ItemIDSys { get; set; }
	public decimal Qty { get; set; }
	public int ConvertedQty { get; set; }
	public int UnitIDSys { get; set; }
	public string RefNO { get; set; }
	public int StatusIDSys { get; set; }
	public double Cost { get; set; }
	public double Price { get; set; }
    }
}
