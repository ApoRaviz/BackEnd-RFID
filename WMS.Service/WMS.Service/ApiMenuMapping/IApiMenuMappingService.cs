using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface IApiMenuMappingService
    {
        IEnumerable<ApiMenuMappingDto> GetCategories();
        ApiMenuMappingDto GetApiMenuMapping(string id);
        List<ApiMenuMapping> GetListApiMenuMapping(int id);
        string CreateApiMenuMapping(ApiMenuMappingDto ApiMenuMapping);
        string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping);
        bool UpdateApiMenuMapping(string id, ApiMenuMapping ApiMenuMapping);
        bool DeleteApiMenuMapping(string id);        
    }
}
