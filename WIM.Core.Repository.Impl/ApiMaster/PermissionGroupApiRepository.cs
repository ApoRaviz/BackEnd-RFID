using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository.MenuAndPermission;

namespace WIM.Core.Repository.Impl.ApiMaster
{
    public class PermissionGroupApiRepository : Repository<PermissionGroupApi>, IPermissionGroupApiRepository
    {
        private CoreDbContext Db { get; set; }

        public PermissionGroupApiRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
