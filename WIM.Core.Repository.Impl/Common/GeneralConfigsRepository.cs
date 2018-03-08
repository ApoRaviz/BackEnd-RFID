using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Common;
using WIM.Core.Repository.Common;

namespace WIM.Core.Repository.Impl.Common
{
    public class GeneralConfigsRepository : Repository<GeneralConfigs>,IGeneralConfigsRepository
    {
        private CoreDbContext Db { get; set; }
        public GeneralConfigsRepository(CoreDbContext context):base(context)
        {
            Db = context;
        }
    }
}
