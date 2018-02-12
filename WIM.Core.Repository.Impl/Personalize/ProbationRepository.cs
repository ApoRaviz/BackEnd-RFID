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
    public class ProbationRepository : Repository<Probation_MT>, IProbationRepository
    {
        private CoreDbContext Db { get; set; }

        public ProbationRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }
    }
}
