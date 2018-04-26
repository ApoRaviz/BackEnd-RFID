using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WMS.Common.ValueObject;
using WMS.Entity.WarehouseManagement;

namespace WMS.Repository.Warehouse
{
    public interface ILocationGroupRepository : IRepository<GroupLocation>
    {
        IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term);
        IEnumerable<GroupLocationDto> GetListLocationGroupDto();
        IEnumerable<ZoneLocationDto> GetLocationRecommend(LocationControlDto control);
    }
}
