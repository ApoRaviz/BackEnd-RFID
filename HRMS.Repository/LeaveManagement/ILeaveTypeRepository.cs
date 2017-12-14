using HRMS.Common.ValueObject.LeaveManagement;
using HRMS.Entity.LeaveManagement;
using System.Collections.Generic;
using WIM.Core.Repository;

namespace HRMS.Repository.LeaveManagement
{
    public interface ILeaveTypeRepository : IRepository<LeaveType>
    {
        IEnumerable<LeaveTypeDto> GetDto();
    }

}
