using WMS.Context;
using WMS.Entity.Common;
using WMS.Repository.Common;
using WIM.Core.Repository.Impl;

namespace WMS.Repository.Impl.Common
{
    public class GeneralConfigsRepository : Repository<GeneralConfig>,IGeneralConfigsRepository
    {
        private WMSDbContext Db { get; set; }
        public GeneralConfigsRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
