using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.Status;
using WIM.Core.Repository;


namespace HRMS.Repository.StatusManagement
{
    public interface IProjectRepository : IRepository<Project_MT>
    {
        IEnumerable<ProjectDto> GetDto();
    }
}
