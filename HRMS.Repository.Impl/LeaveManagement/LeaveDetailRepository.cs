using HRMS.Entity.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveDetailRepository : Repository<LeaveDetail>, ILeaveDetailRepository
    {
        public LeaveDetailRepository(DbContext context) : base(context)
        {

        }
    }
}
