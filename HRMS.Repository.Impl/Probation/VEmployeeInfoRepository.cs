using HRMS.Context;
using HRMS.Entity.Probation;
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using System.Linq;
using WIM.Core.Context;

namespace HRMS.Repository.Impl
{
    public class VEmployeeInfoRepository : Repository<VEmployeeInfo>, IVEmployeeInfoRepository
    {
        private HRMSDbContext Db { get; set; }
        public VEmployeeInfoRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<VEmployeeInfo> GetList()
        {
            var probation = from i in Db.VEmployeeInfo
                            where i.EmTypeIDSys == 3
                            select i;
            return probation.ToList();
        }

        public IEnumerable<VEmployeeInfo> GetList2() 
        {
            var probation = from i in Db.VEmployeeInfo
                            where (i.PositionTypeIDSys == 1) || (i.PositionTypeIDSys == 2)
                            select i;
            return probation.ToList();
        }

    }
}
