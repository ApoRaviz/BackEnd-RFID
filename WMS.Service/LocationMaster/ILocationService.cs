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
        IEnumerable<GroupLocation> GetList();
        GroupLocation GetLocationByLocIDSys(int id);
        GroupLocation CreateLocation(GroupLocation Location);
        bool UpdateLocation(int id, GroupLocation Location);
        bool DeleteLocation(int id);        
    }
}
