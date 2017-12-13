using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Helpers;

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
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProjectRepository repo = new ProjectRepository(Db); 
                        project.ProjectID = Db.ProcGetNewID("PJ");
                        project.ProjectStatus = "Active";
                        
                        projectnew = repo.Insert(project);
                        Db.SaveChanges();
                        scope.Complete();

                        //    Project_MT project1 = db.Project_MT.SingleOrDefault(p => p.ProjectIDSys == 1);
                        //    project1.ProjectConfig = projectConfig;
                        //    db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
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
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }


                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
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

        //public bool CreateUserProject(string UserID, int ProjectIDSys)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        UserProjectMapping project = new UserProjectMapping();
        //        project.UserID = UserID;
        //        project.ProjectIDSys = ProjectIDSys;

        //    SecuDb.UserProjectMapping.Add(project);
        //        try
        //        {
        //        SecuDb.SaveChanges();
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }


        //        return true;
        //    }
        //}

        //public List<UserProjectMapping> GetUserProject(int CusIDSys, string UserID)
        //{

        //    var userproject = (from row in SecuDb.UserProjectMapping
        //                       where row.UserID == UserID && (from o in CoreDb.Project_MT
        //                                                      where o.CusIDSys == CusIDSys
        //                                                      select o.ProjectIDSys).Contains(row.ProjectIDSys)
        //                       select row).ToList();
        //    return userproject;
        //}

        //public bool DeleteUserProject(int projectID, string UserID)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        UserProjectMapping project = new UserProjectMapping();
        //        project.ProjectIDSys = projectID;
        //        project.UserID = UserID;
        //        // #JobComment
        //        //Repo2.Delete(project);
        //        CoreDb.SaveChanges();
        //        scope.Complete();
        //        return true;
        //    }
        //}
    }
}

