using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Employee;
using WIM.Core.Repository.Personalize;

namespace WIM.Core.Repository.Impl.Personalize
{
    public class HistoryWarningRepository : Repository<HistoryWarning> , IHistoryWarningRepository
    {
        private CoreDbContext Db { get; set; }

        public HistoryWarningRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }
    }
}
