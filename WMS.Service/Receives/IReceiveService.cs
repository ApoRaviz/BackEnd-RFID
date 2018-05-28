using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.InventoryManagement;
using WMS.Entity.Receiving;

namespace WMS.Service
{
    public interface IReceiveService : IService
    {
        IEnumerable<Receive> GetReceives();
        ReceiveDto GetReceiveByReceiveIDSys(int id);
        int CreateReceive(ReceiveDto receive);
        bool UpdateReceive(ReceiveDto receive);
        bool DeleteReceive(int id);
        //IEnumerable<AutocompleteUnitDto> AutocompleteUnit(string term);
        Receive SaveReceive(Receive receive);
        TempInventoryTransaction SaveTempInventoryTransaction(TempInventoryTransaction transaction);
        IEnumerable<TempInventoryTransaction> SaveTempInventoryTransactions(IEnumerable<TempInventoryTransaction> tempTransactions);
        ReceiveTempInventoryTransaction SaveReceiveAndTempInventoryTransactions(ReceiveTempInventoryTransaction receiveTempTransaction);
        
    }
}
