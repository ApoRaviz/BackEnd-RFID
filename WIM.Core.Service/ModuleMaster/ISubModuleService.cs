using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.Status;

namespace WIM.Core.Service
{
    public interface ISubModuleService : IService
    {
        IEnumerable<SubModules> GetSubModules();
        SubModules GetSubModulesByID(int id);
        SubModules CreateModule(SubModules module);
        bool UpdateModule(SubModules module);
        bool DeleteModule(int id);
    }
}
