using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.MenuManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface IApiMTService
    {
        IEnumerable<Api_MT> GetAPIs();
        ApiMTDto GetApiMT(string id);
        string CreateApiMT(List<Api_MT> ApiMT);
        bool UpdateApiMT(string id, Api_MT ApiMT);
        bool DeleteApiMT(string id);        
    }
}
