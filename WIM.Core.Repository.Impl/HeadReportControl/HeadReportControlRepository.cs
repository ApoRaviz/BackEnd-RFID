using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;

namespace WIM.Core.Repository.Impl
{
    public class HeadReportControlRepository : Repository<HeadReportControl>, IHeadReportControlRepository
    {
        private CoreDbContext Db { get; set; }

        public HeadReportControlRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }
    }
}
