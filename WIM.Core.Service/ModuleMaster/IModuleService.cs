using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Service
{
    public interface IModuleService : IService
    {
        IEnumerable<Module_MT> GetModules();
        Module_MT GetProjectByModuleIDSys(int id);
        Module_MT CreateModule(Module_MT module);
        bool UpdateModule(Module_MT module);
        bool DeleteModule(int id);

    }
}
