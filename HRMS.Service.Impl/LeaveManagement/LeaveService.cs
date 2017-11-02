using HRMS.Service.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Entity.LeaveManagement;
using HRMS.Common.ValueObject.LeaveManagement;
using System.Security.Principal;

namespace HRMS.Service.Impl.LeaveManagement
{
    public class LeaveService : Service, ILeaveService
    { 

        public bool ApproveLeave(int id)
        {            
            throw new NotImplementedException();
        }

        public Leave CreateLeave(Leave leave)
        {
            throw new NotImplementedException();
        }

        public Leave GetLeaveByID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Leave> GetLeaves()
        {
            throw new NotImplementedException();
        }        

        public bool RejectLeave(int id)
        {
            throw new NotImplementedException();
        }

        public Leave UpdateLeave(LeaveDto leave)
        {
            throw new NotImplementedException();
        }
    }
}
