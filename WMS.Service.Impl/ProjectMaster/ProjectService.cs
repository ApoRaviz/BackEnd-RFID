using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WIM.Core.Context;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Security.Context;
using WMS.Repository.Impl.CustomerAndProject;
using WMS.Context;

namespace WMS.Service
{
        public class ProjectService : IProjectService
        {
            private ProjectRepository repo;
            private WMSDbContext proc;

        public ProjectService()
            {
                repo = new ProjectRepository();
                proc = new WMSDbContext();
            }

            public IEnumerable<Project_MT> GetProjects()
            {
                return repo.Get();
            }

            public object GetProjectsByCusID(int CusIDSys)
            {
                return null;
            }


            public Project_MT GetProjectByProjectIDSys(int id)
            {
            return repo.GetByID(id);
            }

            public Project_MT GetProjectByProjectIDSysIncludeCustomer(int id)
            {
                var project = repo.GetProjectByProjIDincluCus(id);
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
                    repo.Insert(project);
                    scope.Complete();
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

            public bool UpdateProject(int id, Project_MT project)
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        repo.Update(project);
                        scope.Complete();
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
                        repo.Delete(id);
                        scope.Complete();
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
            var project = repo.GetProjectHaveMenu(CusID);
                return project.ToList();
            }

            public List<Project_MT> ProjectCustomer(int CusID)
            {
            var project = repo.GetProjectCustomer(CusID);
                return project.ToList();
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

