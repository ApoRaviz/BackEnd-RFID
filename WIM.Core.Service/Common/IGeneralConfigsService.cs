using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Common;

namespace WIM.Core.Service.Common
{
    public interface IGeneralConfigsService : IService
    {
        GeneralConfigs CreateGeneralConfigs(GeneralConfigs Country);
        bool UpdateGeneralConfigs(GeneralConfigs Country);
    }
}
