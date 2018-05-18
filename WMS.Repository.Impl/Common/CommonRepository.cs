using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.Common;
using WMS.Repository.Common;

namespace WMS.Repository.Impl.Common
{
    public class CommonRepository : Repository<BaseGeneralConfig>, ICommonRepository
    {
        private WMSDbContext Db { get; set; }
        public CommonRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
