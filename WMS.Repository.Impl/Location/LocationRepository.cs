using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;
using System.Linq;
using System.Collections.Generic;
using WMS.Common.ValueObject;

namespace WMS.Repository.Impl
{


    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private WMSDbContext Db;

        public LocationRepository(WMSDbContext contex) : base(contex)
        {
            Db = contex;
        }


        public GroupLocation GetLocationByGroupLocIDSys(int G_LocIDSys)
        {
            GroupLocation res = Db.GroupLocation.Include("LocationType").Include("Location").Include("Location.DimensionLayout_MT").Where(x => x.GroupLocIDSys == G_LocIDSys).FirstOrDefault();
            return res;
        }

    }
}