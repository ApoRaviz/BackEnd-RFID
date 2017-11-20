using HRMS.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using HRMS.Common.ValueObject.LeaveManagement;
using WIM.Core.Entity.Status;
using WIM.Core.Context;
using HRMS.Repository.StatusManagement;
using WIM.Core.Common.ValueObject;

namespace HRMS.Repository.Impl.StatusManagement
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
