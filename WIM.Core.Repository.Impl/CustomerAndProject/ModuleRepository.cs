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
using WIM.Core.Entity.Module;

namespace WIM.Core.Repository.Impl
{
    public class ModuleRepository : Repository<Module_MT>, IModuleRepository
    {
        private CoreDbContext Db;

        public ModuleRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

    }
}
