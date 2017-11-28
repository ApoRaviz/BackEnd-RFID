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
    public class ZoneLayoutHeaderRepository : Repository<ZoneLayoutHeader_MT>, IZoneLayoutHeaderRepository
    {
        private WMSDbContext Db;

        public ZoneLayoutHeaderRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
