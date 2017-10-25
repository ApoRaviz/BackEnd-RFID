﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.MenuManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface IApiMenuMappingService
    {
        IEnumerable<ApiMenuMappingDto> GetApiMenuMapping();
        ApiMenuMappingDto GetApiMenuMapping(string id);
        IEnumerable<ApiMenuMapping> GetListApiMenuMapping(int id);
        string CreateApiMenuMapping(ApiMenuMappingDto ApiMenuMapping);
        string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping);
        bool UpdateApiMenuMapping(string id, ApiMenuMapping ApiMenuMapping);
        bool DeleteApiMenuMapping(string id);        
    }
}
