using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master
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
