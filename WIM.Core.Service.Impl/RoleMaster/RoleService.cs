using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Service.Impl
{
    public class RoleService : Service, IRoleService
    {
        public RoleService()
        {
        }        

        public IEnumerable<Role> GetRoles()
        {
            IEnumerable<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                string[] include = { "Project_MT" };
                role = /*repo.GetWithInclude(x =>x.IsActive == true,include).ToList();*/
                       (from i in Db.Role
                        select i).Include("Project_MT").OrderBy(c => c.Project_MT.ProjectName)
                        .ToList().Select(a => new Role()
                        {
                            RoleID = a.RoleID,
                            Name = a.Name,
                            ProjectIDSys = a.ProjectIDSys,
                            Project_MT = new Project_MT()
                            {
                                ProjectIDSys = a.ProjectIDSys,
                                ProjectName = a.Project_MT.ProjectName
                            }
                        });
                        }
            return role;
        }

        public IEnumerable<Role> GetRoles(int projectIDSys)
        {
            IEnumerable<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                role = (from i in Db.Role
                        where i.ProjectIDSys == projectIDSys
                        select i).Include(x => x.Project_MT)
                        .OrderBy(c => c.Project_MT.ProjectName).ToList().Select(a => new Role()
                        {
                            RoleID = a.RoleID,
                            Name = a.Name,
                            ProjectIDSys = a.ProjectIDSys,
                            Project_MT = new Project_MT()
                            {
                                ProjectIDSys = a.ProjectIDSys,
                                ProjectName = a.Project_MT.ProjectName
                            }
                        });
            }
            return role;
        }

        public Role GetRoleByLocIDSys(string id)
        {
            Role Role ;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                Role = repo.GetByID(id);
            }
                return Role;            
        }

        public string GetRoleByUserAndProject(string UserID, int ProjectIDSys)
        {
            string res;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                res = repo.GetByUserAndProject(UserID, ProjectIDSys);
            }
            return res;
        }

        public string CreateRole(Role role )
        {
            using (var scope = new TransactionScope())
            {   Role rolenew = new Role();
                role.RoleID = Guid.NewGuid().ToString();
                try
                {
                    
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRoleRepository repo = new RoleRepository(Db);
                        rolenew = repo.Insert(role);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }catch(DbUnexpectedValidationException e)
                {
                    Console.Write(e);
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return rolenew.RoleID;
            }
        }

        public bool UpdateRole( Role role )
        {           
            using (var scope = new TransactionScope())
            {     
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRoleRepository repo = new RoleRepository(Db);
                        repo.Update(role);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                
                return true;
            }
        }

        public bool DeleteRole(string id)
        {
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                IRepository<UserRoles> repouser = new Repository<UserRoles>(Db);
                IRepository<RolePermissions> repopermission = new Repository<RolePermissions>(Db);
                List<UserRoles> users = new List<UserRoles>();
            List<RolePermissions> permissions = new List<RolePermissions>();
            if (id != "")
            {
                users = repouser.GetMany(c => c.RoleID == id).ToList();
                permissions = repopermission.GetMany(c => c.RoleID == id).ToList();
            }

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < users.Count; i++)
                        {
                            repouser.Delete(users[i]);
                        }
                        for (int i = 0; i < permissions.Count; i++)
                        {
                            repo.Delete(permissions[i]);
                        }
                        var existedRole = repo.GetByID(id);
                        repo.Delete(existedRole);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4017));
                        throw ex;
                    }
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

        public List<RolePermissionDto> GetRoleByPermissionID(string id)
        {
            List<RolePermissionDto> rolelist;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<RolePermissions> repo = new Repository<RolePermissions>(Db);
                rolelist = repo.GetMany(c => c.PermissionID == id)
                .Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID

                }).ToList();
            }
            return rolelist;
        }

        public List<RolePermissionDto> GetRoleNotPermissionID(string id)
        {
            List<RolePermissionDto> rolelist;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<Role> repo = new Repository<Role>(Db);
                CoreDbContext Db2 = new CoreDbContext();
                var RoleForPermissionQuery = repo.GetMany(c => !(Db2.RolePermissions.Where(a => a.PermissionID == id).Select(b => b.RoleID).Contains(c.RoleID)));
                rolelist = RoleForPermissionQuery.Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID,
                    Name = b.Name,
                    Description = b.Description,
                    IsSysAdmin = b.IsSysAdmin

                }).ToList();
            }
            return rolelist;
        }

        public Role GetRoleByName(string name)
        {
            return null;
        }

        public List<Role> GetRoleByProjectUser(int id , string userid)
        {
            List<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<Role> repo = new Repository<Role>(Db);
                CoreDbContext db = new CoreDbContext();
                string[] include = { "Project_MT" };
                role = repo.GetWithInclude((x => x.ProjectIDSys == id &&
                !(db.UserRoles.Include(p => p.Role).Where(c => c.UserID == userid).Any(p => p.Role.ProjectIDSys == x.ProjectIDSys))), include).Select(a => new Role()
                {
                    RoleID = a.RoleID,
                    Name = a.Name,
                    ProjectIDSys = a.ProjectIDSys,
                    Project_MT = new Project_MT()
                    {
                        ProjectIDSys = a.ProjectIDSys,
                        ProjectName = a.Project_MT.ProjectName
                    }
                }).ToList();
            }
                return role.ToList();
        }

        public List<Role> GetRoleByUserID(string id)
        {
            List<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                role = repo.GetByUser(id);
            }
            return role;
        }
    }
}
