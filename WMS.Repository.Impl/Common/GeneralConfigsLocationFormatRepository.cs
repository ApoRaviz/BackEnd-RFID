using WMS.Context;
using WMS.Entity.Common;
using WMS.Repository.Common;
using WIM.Core.Repository.Impl;

namespace WMS.Repository.Impl.Common
{
    public class GeneralConfigsLocationFormatRepository : Repository<BaseGeneralConfig>, IGeneralConfigsLocationFormatRepository
    {
        private WMSDbContext Db { get; set; }
        public GeneralConfigsLocationFormatRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
