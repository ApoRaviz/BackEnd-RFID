using System.Linq;
using WIM.Core.Entity.Status;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository.StatusManagement;

namespace WIM.Core.Repository.Impl.StatusManagement
{
    public class SubModuleRepository : Repository<SubModules>, ISubModuleRepository
    {
        private CoreDbContext Db;

        public SubModuleRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public SubModuleDto GetDto()
        {
            SubModuleDto subModule = (from sm in Db.SubModule
                              join md in Db.Module_MT on sm.ModuleIDSys equals md.ModuleIDSys
                              select new SubModuleDto
                              {
                                  SubModuleIDSys = sm.SubModuleIDSys,
                                  ModuleIDSys = sm.ModuleIDSys,
                                  SubModuleName = sm.SubModuleName,
                                  ModuleName = md.ModuleName
                              }).SingleOrDefault();

            return subModule;
        }

    }


}
