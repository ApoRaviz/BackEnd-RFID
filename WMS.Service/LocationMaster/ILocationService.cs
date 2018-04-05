using System.Collections.Generic;
using WIM.Core.Service;
using WMS.Entity.WarehouseManagement;

namespace WMS.Service.LocationMaster
{
    public interface ILocationService : IService
    {
        IEnumerable<Location> GetList();
        Location GetLocationByLocIDSys(int id);
        GroupLocation GetLocationByGroupLocIDSys(int id);
        Location CreateLocation(Location Location);
        bool UpdateLocation(int id, Location Location);
        bool DeleteLocation(int id);        
    }
}
