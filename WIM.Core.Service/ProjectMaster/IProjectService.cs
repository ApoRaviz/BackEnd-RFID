using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Service
{
    public interface IProjectService
    {
        IEnumerable<Project_MT> GetProjects();
        Project_MT GetProjectByProjectIDSys(int id);
        List<Project_MT> ProjectHaveMenu(int CusID);
        List<Project_MT> ProjectCustomer(int CusID);
        //bool CreateUserProject(string UserID, int ProjectIDSys);
        Project_MT GetProjectByProjectIDSysIncludeCustomer(int id);
        Project_MT CreateProject(Project_MT project , string username);
        bool UpdateProject(Project_MT project , string username);
        bool DeleteProject(int id);
        //List<UserProjectMapping> GetUserProject(int CusIDSys, string UserID);
        //bool DeleteUserProject(int projectID, string UserID);
    }
}
