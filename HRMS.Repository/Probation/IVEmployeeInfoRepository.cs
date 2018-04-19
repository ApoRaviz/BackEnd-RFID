using HRMS.Entity.Probation;
using System.Collections.Generic;
using WIM.Core.Repository;

namespace HRMS.Repository
{
    public interface IVEmployeeInfoRepository : IRepository<VEmployeeInfo>
    {
        IEnumerable<VEmployeeInfo> GetList();
        IEnumerable<VEmployeeInfo> GetList2(); 
    }
}

