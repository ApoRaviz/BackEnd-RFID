using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class InventoryTransactionDto
    {
       public int InvenTranIDSys { get; set; }
       public int InvenIDSys { get; set; }
       public int ReceiveIDSys { get; set; }
       public int ItemIDSys { get; set; }
       public string ItemName { get; set; }
       public string ItemCode { get; set; }
       public string SerialNumber { get; set; }
       public decimal Qty { get; set; }
       public int UnitIDSys { get; set; }
       public int LocIDSys { get; set; }
       public string LocNo { get; set; }
       public int StatusIDSys { get; set; }
       public double Price { get; set; }
       public double Cost { get; set; }
       public DateTime ReceivingDate { get; set; }
       public Nullable<DateTime> Expire { get; set; }
       public string Serial { get; set; }
       public string Inspect { get; set; }
       public string Dimention { get; set; }
       public string Box { get; set; }
       public string Lot { get; set; }
       public string Pallet { get; set; }
       public double UsedDimension { get; set; }
       public virtual List<InventoryTransactionDto> Child { get; set; }
       public virtual List<InventoryTransactionDetailDto> InventoryTransactionDetail { get; set; }
    }
}
