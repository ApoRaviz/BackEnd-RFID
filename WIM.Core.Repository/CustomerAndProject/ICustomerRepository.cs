using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.CustomerManagement;

namespace WIM.Core.Repository
{
    public interface ICustomerRepository : IRepository<Customer_MT>
    {
        object GetByUserID(string userid);
        object GetProjectByUserIDCusID(string userid, int cusIDSys);
    }
}
