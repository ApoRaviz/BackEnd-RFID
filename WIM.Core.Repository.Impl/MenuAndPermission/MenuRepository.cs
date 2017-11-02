using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class MenuRepository : Repository<Menu_MT>,IMenuRepository
    {
        private CoreDbContext Db { get; set; }

        public MenuRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

    }
}
