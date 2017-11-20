using HRMS.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WIM.Core.Context;
using HRMS.Repository.StatusManagement;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Common.ValueObject;

namespace HRMS.Repository.Impl.StatusManagement
{
    public class ProjectRepository : Repository<Project_MT>, IProjectRepository
    {
        private CoreDbContext Db;

        public ProjectRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<ProjectDto> GetDto()
        {
            IEnumerable<ProjectDto> project = (from pj in Db.Project_MT
                                where pj.IsActive == true
                                select new ProjectDto
                                {
                                    ProjectIDSys = pj.ProjectIDSys,
                                    ProjectName = pj.ProjectName,

                                }).ToList();

            return project;
        }
    }
}
