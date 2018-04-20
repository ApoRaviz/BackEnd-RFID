using HRMS.Context;
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using System.Linq;
using WIM.Core.Context;
using HRMS.Repository.Evaluate;
using HRMS.Entity.Evaluate;

namespace HRMS.Repository.Impl
{
    public class EvaluatedRepository : Repository<Evaluated>, IEvaluatedRepository
    {
        private HRMSDbContext Db { get; set; }

        public EvaluatedRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<object> GetList()
        {

            var evaluatelist = 
                               from eva in Db.Evaluated
                               join vemp in Db.VEmployeeInfo on eva.EmID equals vemp.EmID
                               select new { eva.EvaluatedIDSys, eva.EmID, eva.EvaluateDate, vemp.Name, vemp.Surname, vemp.DepName, vemp.EmTypeName };

            return evaluatelist.ToList();
        }

    }
}
