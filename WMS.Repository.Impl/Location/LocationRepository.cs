using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;
using System.Linq;

namespace WMS.Repository.Impl.Location
{


    public class LocationRepository : Repository<Location_MT>, ILocationRepository
    {
        private WMSDbContext Db;

        public LocationRepository(WMSDbContext contex) : base(contex)
        {
            Db = contex;
        }

        public GroupLocation GetLocationByGroupLocIDSys(int G_LocIDSys)
        {
            GroupLocation res = Db.GroupLocation.Include("LocationType").Include("Location").Where(x => x.GroupLocIDSys == G_LocIDSys).FirstOrDefault();
            return res;
        }

    }
}