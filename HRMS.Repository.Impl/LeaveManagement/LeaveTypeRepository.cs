using HRMS.Common.ValueObject.LeaveManagement;
using HRMS.Context;
using HRMS.Entity.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using System.Security.Principal;
using WIM.Core.Repository.Impl;
using System.Linq;
using System.Collections.Generic;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
    {
        private HRMSDbContext Db;

        public LeaveTypeRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<LeaveTypeDto> GetDto()
        {
           IEnumerable<LeaveTypeDto> leaveType = (from lt in Db.LeaveTypes
                                      select new LeaveTypeDto {
                                          LeaveTypeIDSys = lt.LeaveTypeIDSys,
                                          LeaveTypeName = lt.Name
                                      }).ToList();
            return leaveType;
        }
    }
}
