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
       public int ReceiveIDSys { get; set; }
       public int ItemIDSys { get; set; }
       public string ItemName { get; set; }
       public string ItemCode { get; set; }
       public string SerialNo { get; set; }
       public int Qty { get; set; }
       public int UnitIDSys { get; set; }
       public int LocIDSys { get; set; }
       public string LocNo { get; set; }
       public int StatusIDSys { get; set; }
       public Nullable<DateTime> ReceivingDate { get; set; }
       public Nullable<DateTime> Expire { get; set; }
       public string Serial { get; set; }
       public string Inspect { get; set; }
       public string Dimention { get; set; }
       public string Box { get; set; }
       public string Lot { get; set; }
       public string Pallet { get; set; }
    }
}
