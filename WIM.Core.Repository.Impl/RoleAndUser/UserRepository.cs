using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class UserRepository : Repository<User> , IUserRepository
    {
        private CoreDbContext Db { get; set; }
        private IIdentity User { get; set; }

        public UserRepository(CoreDbContext context,IIdentity identity):base(context,identity)
        {
            Db = context;
            User = identity;
        }

        public object GetCustomerByUser(string userid)
        {
            var query = (from ctm in Db.Customer_MT
                         join c in Db.Project_MT on ctm.CusIDSys equals c.CusIDSys
                         join d in Db.Role on c.ProjectIDSys equals d.ProjectIDSys
                         join e in Db.UserRoles on d.RoleID equals e.RoleID
                         where e.UserID == userid
                         select new
                         {
                             ctm.CusID,
                             ctm.CusIDSys,
                             ctm.CusName
                         }).Distinct();
            return query.ToList();
        }

    }
}
