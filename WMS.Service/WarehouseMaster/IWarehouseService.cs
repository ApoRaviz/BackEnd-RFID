﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.WarehouseManagement;

namespace WMS.Master
{
    public interface IWarehouseService : IService
    {
        IEnumerable<Warehouse_MT> GetWarehouses();
        Warehouse_MT GetWarehouseByLocIDSys(int id);
        int CreateWarehouse(Warehouse_MT Warehouse);
        bool UpdateWarehouse(Warehouse_MT Warehouse);
        bool DeleteWarehouse(int id);        
    }
}
