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
    public class LocationGroupRepository : Repository<GroupLocation>, ILocationGroupRepository
    {
        private WMSDbContext Db;

        public LocationGroupRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
