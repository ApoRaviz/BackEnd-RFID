using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.WarehouseManagement;

namespace WMS.Master
{
    public interface IWarehouseService
    {
        IEnumerable<Warehouse_MT> GetWarehouses();
        Warehouse_MT GetWarehouseByLocIDSys(int id);
        int CreateWarehouse(Warehouse_MT Warehouse);
        bool UpdateWarehouse(int id, Warehouse_MT Warehouse);
        bool DeleteWarehouse(int id);        
    }
}
