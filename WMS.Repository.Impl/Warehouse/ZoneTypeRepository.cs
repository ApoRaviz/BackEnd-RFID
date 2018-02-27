using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;

namespace WMS.Repository.Impl.Warehouse
{
    public class ZoneTypeRepository : Repository<ZoneType>, IZoneTypeRepository
    {
        private WMSDbContext Db;

        public ZoneTypeRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
