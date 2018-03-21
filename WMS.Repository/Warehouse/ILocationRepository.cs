using WIM.Core.Repository;
using WMS.Entity.WarehouseManagement;

namespace WMS.Repository.Warehouse
{
    public interface ILocationRepository: IRepository<Location_MT>
    {
        GroupLocation GetLocationByGroupLocIDSys(int G_LocIDSys);
    }
}
