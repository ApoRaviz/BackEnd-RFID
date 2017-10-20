using HRMS.Repository.Entity;
using HRMS.Repository.Entity.LeaveRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Service
{
    public interface IDemoService
    {
        IEnumerable<Leave> GetLeaves();
    }
}
