using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.WarehouseManagement;

namespace WMS.Service
{
    public interface ILocationService
    {
        IEnumerable<Location_MT> GetLocations();
        Location_MT GetLocationByLocIDSys(int id);
        int CreateLocation(Location_MT Location);
        bool UpdateLocation(int id, Location_MT Location);
        bool DeleteLocation(int id);        
    }
}
