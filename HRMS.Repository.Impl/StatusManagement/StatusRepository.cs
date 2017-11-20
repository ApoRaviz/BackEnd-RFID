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
using System.Data.Entity;

namespace HRMS.Repository.Impl.StatusManagement
{
    public class StatusRepository : Repository<Status_MT>, IStatusRepository
    {
        private CoreDbContext Db;

        public StatusRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public StatusDto GetDto(int id)
        {
            StatusDto status = (from st in Db.Status_MT
                          where st.StatusIDSys == id
                          select new StatusDto()
                          {
                              StatusIDSys = st.StatusIDSys,
                              Title = st.Title
                          }).SingleOrDefault();

            var submodule = (from i in Db.SubModule
                            join m in Db.Module_MT on i.ModuleIDSys equals m.ModuleIDSys
                            join n in Db.StatusSubModule on i.SubModuleIDSys equals n.SubModuleIDSys
                            where n.StatusIDSys == id
                            select new SubModuleDto()
                            {
                                SubModuleIDSys = i.SubModuleIDSys,
                                ModuleIDSys = i.ModuleIDSys,
                                ModuleName = m.ModuleName,
                                SubModuleName = i.SubModuleName,
                                LabelSubModuleName = m.ModuleName + " : " + i.SubModuleName
                            }).ToList();

            status.StatusSubModule = submodule;

            return status;
        }

    }


}
