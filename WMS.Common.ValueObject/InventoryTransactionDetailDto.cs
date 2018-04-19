using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class InventoryTransactionDetailDto
    {
        public int InvenTranDetailIDSys { get; set; }
        public int InvenTranIDSys { get; set; }
        public string SerialNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int UnitIDSys { get; set; }
        public int LocIDSys { get; set; }
        public string LocNo { get; set; }
        public string Box { get; set; }
        public string Lot { get; set; }
        public string Pallet { get; set; }
    }
}
