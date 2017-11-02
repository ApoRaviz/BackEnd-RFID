using HRMS.Context;
using HRMS.Entity.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using System.Security.Principal;
using WIM.Core.Repository.Impl;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
    {
        private HRMSDbContext Db;
        private IIdentity Identity;

        public LeaveTypeRepository(HRMSDbContext context, IIdentity identity) : base(context, identity)
        {
            Db = context;
            Identity = identity;
        }
    }
}
