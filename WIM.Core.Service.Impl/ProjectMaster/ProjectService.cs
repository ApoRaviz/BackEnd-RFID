using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;

namespace WIM.Core.Service.Impl
{
    public class ProjectService : Service, IProjectService
    {
        public ProjectService()
        {
        }

        public IEnumerable<Project_MT> GetProjects()
        {
            IEnumerable<Project_MT> projects;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                projects = repo.Get();
            }
            return projects;
        }

        public IEnumerable<Project_MT> GetProjects(int projectID)
        {
            IEnumerable<Project_MT> projects;
            using (CoreDbContext Db = new CoreDbContext())
            {
                CoreDbContext db = new CoreDbContext();
                IProjectRepository repo = new ProjectRepository(Db);
                projects = repo.GetMany(x => (db.Project_MT.Where(c => c.ProjectIDSys == projectID).Select(a=>a.CusIDSys).Contains(x.CusIDSys)));
            }
            return projects;
        }

        public object GetProjectsByCusID(int CusIDSys)
        {
            IEnumerable<Project_MT> projects;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                projects = repo.GetMany((c => c.CusIDSys == CusIDSys)).ToList();
            }
            return projects;
        }


        public Project_MT GetProjectByProjectIDSys(int id)
        {
            Project_MT project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                project = repo.GetByID(id);
            }
            return project;
        }

        public Project_MT GetProjectByProjectIDSysIncludeCustomer(int id)
        {
            Project_MT project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                string[] include = {"Customer_MT"};
                project = repo.GetWithInclude((c => c.ProjectIDSys == id), include).SingleOrDefault();
            }
            if (project != null)
            {
                return project;
            }
            return null;
        }

        public object GetProjectByProjectIDSysIncludeModule(int id)
        {
            object project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                project = repo.GetProjectByProjectIDSysInclude(id);
            }
            if (project != null)
            {
                return project;
            }
            return null;

        }


        public Project_MT CreateProject(Project_MT project)
        {
            using (var scope = new TransactionScope())
            {
                Project_MT projectnew = new Project_MT();
                List<LabelControl> labelcontrol = new List<LabelControl>();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProjectRepository repo = new ProjectRepository(Db);
                        ILabelControlRepository repoLB = new LabelControlRepository(Db);
                        project.ProjectStatus = "Active";
                        projectnew = repo.Insert(project);
                        Db.SaveChanges();

                        labelcontrol = repo.GetLabelToDuplicate(Convert.ToInt32(project.ModuleIDSys));
                        LabelControl label = new LabelControl();
                        foreach(LabelControl lb in labelcontrol)
                        {
                            label.Lang = lb.Lang;
                            label.ProjectIDSys = projectnew.ProjectIDSys;
                            label.LabelConfig = lb.LabelConfig;
                            repoLB.Insert(label);
                        };
                        Db.SaveChanges();
                        scope.Complete();

                        //    Project_MT project1 = db.Project_MT.SingleOrDefault(p => p.ProjectIDSys == 1);
                        //    project1.ProjectConfig = projectConfig;
                        //    db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw e;
                }

                return projectnew;
            }
        }

        public bool UpdateProject(Project_MT project)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProjectRepository repo = new ProjectRepository(Db);
                        repo.Update(project);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return true;
            }
        }

        public bool DeleteProject(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProjectRepository repo = new ProjectRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }


                return true;
            }
        }
        public List<Project_MT> ProjectHaveMenu(int CusID)
        {
            List<Project_MT> project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                project = repo.GetMany(c => c.CusIDSys == CusID && (Db2.MenuProjectMapping).Select(a => a.ProjectIDSys).Contains(c.ProjectIDSys)).ToList();
            }
            return project;
        }

        public List<Project_MT> ProjectCustomer(int CusID)
        {
            List<Project_MT> project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db);
                project = repo.GetMany(c => c.CusIDSys == CusID).ToList();
            }
            return project;
        }



       
    }
}

