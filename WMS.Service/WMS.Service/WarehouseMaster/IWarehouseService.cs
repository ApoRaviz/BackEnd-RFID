using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.WarehouseManagement;

namespace WMS.Master
{
    public interface IWarehouseService
    {
        IEnumerable<Warehouse_MT> GetWarehouses();
        Warehouse_MT GetWarehouseByLocIDSys(int id);
        int CreateWarehouse(Warehouse_MT Warehouse , string username);
        bool UpdateWarehouse(Warehouse_MT Warehouse , string username);
        bool DeleteWarehouse(int id);        
    }
}
