using HRMS.Context;
using HRMS.Entity.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private HRMSDbContext Db;
        private IIdentity Identity;

        public LeaveRepository(HRMSDbContext context, IIdentity identity) : base(context, identity)
        {
            Db = context;
            Identity = identity;
        }       
    }
}
