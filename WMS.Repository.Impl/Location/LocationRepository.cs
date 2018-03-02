using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;

namespace WMS.Repository.Impl.Location
{


        public LocationRepository()
        {
            Db = new WMSDbContext();
        }

        public LocationRepository(WMSDbContext contex) : base(contex)
        {
            Db = contex;
        }

    }
}
