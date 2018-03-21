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

        //public Person_MT GetByUserID(string id)
        //{
        //    var person = (from i in Db.Person_MT
        //                  where (from o in Db.User
        //                         where o.UserID == id
        //                         select o.PersonIDSys)
        //                       .Contains(i.PersonIDSys)
        //                  select i).SingleOrDefault();
        //    return person;
        //}

        public IEnumerable<VEmployeeInfo> GetList()
        {
            var probation = from i in Db.VEmployeeInfo
                            where i.EmTypeIDSys == 3
                            select i;
            return probation.ToList();
        }

    }
}
