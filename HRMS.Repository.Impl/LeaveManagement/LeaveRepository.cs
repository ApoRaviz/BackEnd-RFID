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
using HRMS.Common.ValueObject.LeaveManagement;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private HRMSDbContext Db;

        public LeaveRepository(HRMSDbContext contex) : base(contex)
        {
            Db = contex;
        }

        public LeaveDto GetDto(int id)
        {
            LeaveDto leave = (from l in Db.Leaves
                     where l.LeaveIDSys == id
                     join st in Db.Status_MT on l.StatusIDSys equals st.StatusIDSys
                     join lt in Db.LeaveTypes on l.LeaveTypeIDSys equals lt.LeaveTypeIDSys
                     select new LeaveDto
                     {
                         LeaveTypeIDSys = l.LeaveTypeIDSys,
                         StatusIDSys = l.StatusIDSys,
                         StatusTitle = st.Title,
                         Comment = l.Comment,
                         LeaveTypeName = lt.Name,
                     }).SingleOrDefault();

            return leave;
        }

        public IEnumerable<LeaveDetailDto> GetDetailDto(int id)
        {
            IEnumerable<LeaveDetailDto> leave = (from ld in Db.LeaveDetails
                                                 where ld.LeaveIDSys == id
                                                 select new LeaveDetailDto
                                                 {
                                                     LeaveIDSys = ld.LeaveIDSys,
                                                     LeaveDetailIDSys = ld.LeaveDetailIDSys,
                                                     StartDate = ld.StartDate,
                                                     EndDate = ld.EndDate
                                                 }).ToList();
            return leave;
        }
    }
}
