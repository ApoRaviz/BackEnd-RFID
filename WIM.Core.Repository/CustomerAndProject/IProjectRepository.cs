using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Repository
{
    public interface IProjectRepository : IRepository<Project_MT>
    {
        object GetProjectByProjectIDSysInclude(int projectIDSys);
        List<LabelControl> GetLabelToDuplicate(int moduleIDSys);
    }
}
