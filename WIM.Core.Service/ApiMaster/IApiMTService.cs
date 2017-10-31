﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;


namespace WIM.Core.Service
{
    public interface IApiMTService
    {
        IEnumerable<Api_MT> GetAPIs();
        ApiMTDto GetApiMT(string id);
        string CreateApiMT(List<Api_MT> ApiMT);
        bool UpdateApiMT( Api_MT ApiMT);
        bool DeleteApiMT(string id);        
    }
}
