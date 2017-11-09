using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.WarehouseManagement;

namespace WMS.Service.LocationMaster
{
    public interface ILocationService : IService
    {
        IEnumerable<Location_MT> GetLocations();
        Location_MT GetLocationByLocIDSys(int id);
        int CreateLocation(Location_MT Location);
        bool UpdateLocation(int id, Location_MT Location);
        bool DeleteLocation(int id);        
    }
}
