using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Master.Customer;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
        public class ProjectService : IProjectService
        {
            private MasterContext Db = MasterContext.Create();
            private GenericRepository<Project_MT> Repo;
            private GenericRepository<UserProjectMapping> Repo2;

            public ProjectService()
            {
                Repo = new GenericRepository<Project_MT>(Db);
                Repo2 = new GenericRepository<UserProjectMapping>(Db);
            }

            public IEnumerable<ProcGetProjects_Result> GetProjects()
            {
                return Db.ProcGetProjects().ToList();
            }

            public object GetProjectsByCusID(int CusIDSys)
            {
                return null;
            }


            public ProcGetProjectByProjectIDSys_Result GetProjectByProjectIDSys(int id)
            {
                return Db.ProcGetProjectByProjectIDSys(id).FirstOrDefault();
            }

            public ProjectDto GetProjectByProjectIDSysIncludeCustomer(int id)
            {
                var project = GetProjectByProjectIDSys(id);
                if (project != null)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<ProcGetProjectByProjectIDSys_Result, ProjectDto>());
                    ProjectDto projectDto = Mapper.Map<ProcGetProjectByProjectIDSys_Result, ProjectDto>(project);

                    var customer = Db.ProcGetCustomerByCusIDSys(projectDto.CusIDSys).FirstOrDefault();
                    Mapper.Initialize(cfg => cfg.CreateMap<ProcGetCustomerByCusIDSys_Result, CustomerDto>());
                    projectDto.Customer_MT = Mapper.Map<ProcGetCustomerByCusIDSys_Result, CustomerDto>(customer);
                    return projectDto;
                }
                return null;
            }




            public Project_MT CreateProject(Project_MT project)
            {
                using (var scope = new TransactionScope())
                {
                    project.ProjectID = Db.ProcGetNewID("PJ").FirstOrDefault();
                    project.ProjectStatus = "Active";
                    project.CreatedDate = DateTime.Now;
                    project.UpdateDate = DateTime.Now;
                    project.UserUpdate = "1";

                    Repo.Insert(project);
                    try
                    {
                        Db.SaveChanges();
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
                    var existedProject = Repo.GetByID(id);
                    existedProject.ProjectName = project.ProjectName;
                    existedProject.UpdateDate = DateTime.Now;
                    existedProject.UserUpdate = "1";
                    Repo.Update(existedProject);
                    try
                    {
                        Db.SaveChanges();
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
                    var existedProject = Repo.GetByID(id);
                    existedProject.ProjectStatus = "Inactive";
                    existedProject.UpdateDate = DateTime.Now;
                    existedProject.UserUpdate = "1";
                    Repo.Update(existedProject);
                    try
                    {
                        Db.SaveChanges();
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
                var project = from row in Db.Project_MT
                              where (from o in Db.MenuProjectMappings
                                     select o.ProjectIDSys).Contains(row.ProjectIDSys)
                                     && row.CusIDSys == CusID
                              select row;
                return project.ToList();
            }

            public List<Project_MT> ProjectCustomer(int CusID)
            {
                var project = from row in Db.Project_MT
                              where row.CusIDSys == CusID
                              select row;
                return project.ToList();
            }

            public bool CreateUserProject(string UserID, int ProjectIDSys)
            {
                using (var scope = new TransactionScope())
                {
                    UserProjectMapping project = new UserProjectMapping();
                    project.UserID = UserID;
                    project.ProjectIDSys = ProjectIDSys;
                    Repo2.Insert(project);
                    try
                    {
                        Db.SaveChanges();
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

            public List<UserProjectMapping> GetUserProject(int CusIDSys, string UserID)
            {

                var userproject = (from row in Db.UserProjectMappings
                                   where row.UserID == UserID && (from o in Db.Project_MT
                                                                  where o.CusIDSys == CusIDSys
                                                                  select o.ProjectIDSys).Contains(row.ProjectIDSys)
                                   select row).ToList();
                return userproject;
            }

            public bool DeleteUserProject(int projectID, string UserID)
            {
                using (var scope = new TransactionScope())
                {
                    UserProjectMapping project = new UserProjectMapping();
                    project.ProjectIDSys = projectID;
                    project.UserID = UserID;
                    Repo2.Delete(project);
                    Db.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
    }
}

