using WIM.Core.Service;
using WMS.Entity.Common;

namespace WMS.Service.Common
{
    public interface IGeneralConfigsService : IService
    {
        GeneralConfig CreateGeneralConfigs(GeneralConfig Country);
        GeneralConfigLocationFormat saveGeneralConfigLocationFormat(GeneralConfigLocationFormat param);
        GeneralConfig GetGeneralConfigs(string Keyword);
        bool UpdateGeneralConfigs(GeneralConfig Country);
    }
}
