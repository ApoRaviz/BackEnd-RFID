using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.Receiving;

namespace WMS.Entity.InventoryManagement
{
    public class ReceiveTempInventoryTransaction
    {
        public Receive Receive { get; set; }
        public IEnumerable<TempInventoryTransaction> TempTransactions { get; set; }
    }
}
