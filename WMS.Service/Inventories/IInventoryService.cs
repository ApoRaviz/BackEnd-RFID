using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.InventoryManagement;

namespace WMS.Service.Inventories
{
    public interface IInventoryService : IService
    {
        void ConfirmReceive(ConfirmReceive confirmReceive);
    }
}
