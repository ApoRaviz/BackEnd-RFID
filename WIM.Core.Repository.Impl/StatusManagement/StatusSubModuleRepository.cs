using WIM.Core.Entity.Status;
using WIM.Core.Context;
using WIM.Core.Repository.StatusManagement;

namespace WIM.Core.Repository.Impl.StatusManagement
{
    public class StatusSubModuleRepository : Repository<StatusSubModules>, IStatusSubModuleRepository
    {
        private CoreDbContext Db;

        public StatusSubModuleRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

    }


}
