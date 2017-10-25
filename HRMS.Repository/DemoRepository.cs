using HRMS.Repository.Context;
using HRMS.Repository.Entity.LeaveRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Repository.Impl
{
    public class DemoRepository
    {
        private HRMSDbContext HRMSDb;

        public DemoRepository()
        {
            HRMSDb = new HRMSDbContext();
        }

        public IEnumerable<Leave> GetList()
        {
            return null;
        }

    }
}
