using WIM.Core.Repository;
using WMS.Entity.WarehouseManagement;

namespace WMS.Repository.Warehouse
{
    public interface ILocationRepository: IRepository<Location>
    {
        GroupLocation GetLocationByGroupLocIDSys(int G_LocIDSys);
    }
}
