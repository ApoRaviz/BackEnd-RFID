using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        object GetCustomerByUser(string userid);
    }
}
