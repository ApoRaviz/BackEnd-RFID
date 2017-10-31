using HRMS.Context;
using HRMS.Entity.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private HRMSDbContext Db;

        public LeaveRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }       

    }
}
