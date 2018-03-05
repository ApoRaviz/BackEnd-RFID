using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;

namespace WMS.Repository.Impl.Location
{


    public class LocationRepository : Repository<Entity.WarehouseManagement.Location>, ILocationRepository
    {
        private WMSDbContext Db;

        public LocationRepository(WMSDbContext contex) : base(contex)
        {
            Db = contex;
        }

    }
}
