using System;
using System.Collections;
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

        public ApiMTRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
