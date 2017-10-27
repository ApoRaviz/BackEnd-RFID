using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Service
{
    public interface IApiMenuMappingService
    {
        IEnumerable<ApiMenuMappingDto> GetApiMenuMapping();
        ApiMenuMappingDto GetApiMenuMapping(string id);
        IEnumerable<ApiMenuMapping> GetListApiMenuMapping(int id);
        string CreateApiMenuMapping(ApiMenuMappingDto ApiMenuMapping , string username);
        string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping , string username);
        bool UpdateApiMenuMapping(ApiMenuMapping ApiMenuMapping , string username);
        bool DeleteApiMenuMapping(string id);        
    }
}
