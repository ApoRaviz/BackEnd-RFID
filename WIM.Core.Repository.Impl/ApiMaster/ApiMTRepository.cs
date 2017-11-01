using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class ApiMTRepository : Repository<Api_MT>, IApiMTRepository
    {
        private CoreDbContext Db { get; set; }
        private IIdentity User { get; set; }

        public ApiMTRepository(CoreDbContext context,IIdentity identity) : base(context,identity)
        {
            Db = context;
            User = identity;
        }
    }

}
