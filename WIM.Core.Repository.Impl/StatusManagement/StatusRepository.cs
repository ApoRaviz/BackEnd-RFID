using System;
using System.Collections.Generic;
using System.Linq;
using WIM.Core.Entity.Status;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using System.Data.Entity;
using WIM.Core.Repository.StatusManagement;

namespace WIM.Core.Repository.Impl.StatusManagement
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

        public IEnumerable<StatusSubModuleDto> GetDto()
        {
            var statussub = (from i in Db.SubModule
                             join m in Db.Module_MT on i.ModuleIDSys equals m.ModuleIDSys
                             join n in Db.StatusSubModule on i.SubModuleIDSys equals n.SubModuleIDSys
                             join o in Db.Status_MT on n.StatusIDSys equals o.StatusIDSys
                             select new StatusSubModuleDto()
                             {
                                 StatusIDSys = n.StatusIDSys,
                                 Title = o.Title,
                                 ModuleName = m.ModuleName,
                                 SubModuleName = i.SubModuleName,
                             }).ToList();

            return statussub;
        }


            
   

    }


}
