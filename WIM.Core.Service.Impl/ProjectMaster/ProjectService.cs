using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace WIM.Core.Service.Impl
{
    public class ProjectService : IProjectService
    {
        private IIdentity user { get; set; }
        public ProjectService(IIdentity identity)
        {
            user = identity;
        }

        public IEnumerable<Project_MT> GetProjects()
        {
            IEnumerable<Project_MT> projects;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db,user);
                projects = repo.Get();
            }
            return projects;
        }

        public object GetProjectsByCusID(int CusIDSys)
        {
            IEnumerable<Project_MT> projects;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db,user);
                projects = repo.GetMany((c => c.CusIDSys == CusIDSys)).ToList();
            }
            return projects;
        }


        public Project_MT GetProjectByProjectIDSys(int id)
        {
            Project_MT project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db,user);
                project = repo.GetByID(id);
            }
            return project;
        }

        public Project_MT GetProjectByProjectIDSysIncludeCustomer(int id)
        {
            Project_MT project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db,user);
                string[] include = {"Customer_MT"};
                project = repo.GetWithInclude((c => c.ProjectIDSys == id), include).SingleOrDefault();
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
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProjectRepository repo = new ProjectRepository(Db,user);
                        repo.Insert(project);
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
                return project;
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
                        IProjectRepository repo = new ProjectRepository(Db,user);
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
                        IProjectRepository repo = new ProjectRepository(Db,user);
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
                IProjectRepository repo = new ProjectRepository(Db,user);
                project = repo.GetMany(c => c.CusIDSys == CusID && (Db.MenuProjectMapping).Select(a => a.ProjectIDSys).Contains(c.ProjectIDSys)).ToList();
            }
            return project;
        }

        public List<Project_MT> ProjectCustomer(int CusID)
        {
            List<Project_MT> project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProjectRepository repo = new ProjectRepository(Db,user);
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

