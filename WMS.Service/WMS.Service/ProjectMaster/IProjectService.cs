using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface IProjectService
    {
        IEnumerable<ProcGetProjects_Result> GetProjects();
        ProcGetProjectByProjectIDSys_Result GetProjectByProjectIDSys(int id);
        List<Project_MT> ProjectHaveMenu(int CusID);
        List<Project_MT> ProjectCustomer(int CusID);
        bool CreateUserProject(string UserID, int ProjectIDSys);
        ProjectDto GetProjectByProjectIDSysIncludeCustomer(int id);
        Project_MT CreateProject(Project_MT project);
        bool UpdateProject(int id, Project_MT project);
        bool DeleteProject(int id);
        List<UserProjectMapping> GetUserProject(int CusIDSys, string UserID);
        bool DeleteUserProject(int projectID, string UserID);
    }
}
