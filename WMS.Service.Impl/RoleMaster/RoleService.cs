using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Master;
using WIM.Core.Security.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;

namespace WMS.Service
{
    public class RoleService : IRoleService
    {
        private CoreDbContext CoreDb;
        private SecurityDbContext db;        
        private GenericRepository<Role> repo;
        private GenericRepository<UserRoles> repoUser;
        private GenericRepository<RolePermission> repoRolePermission;

        public RoleService()
        {
            CoreDb = new CoreDbContext();
            db = new SecurityDbContext();
            repo = new GenericRepository<Role>(db);
            repoUser = new GenericRepository<UserRoles>(db);
            repoRolePermission = new GenericRepository<RolePermission>(db);
        }        

        public IEnumerable<Role> GetRoles()
        {           
            return repo.GetAll();
        }

        public IEnumerable<Role> GetRoles(int projectIDSys)
        {
            var roles = from row in CoreDb.Role
                        where row.ProjectIDSys == projectIDSys
                        select row;
            return roles;
        }

        public Role GetRoleByLocIDSys(string id)
        {           
            Role Role = CoreDb.Role.Find(id);                                  
            return Role;            
        }

        public string GetRoleByUserAndProject(string UserID, int ProjectIDSys)
        {
            var res = (from ur in CoreDb.UserRoles
                       join r in CoreDb.Role on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectIDSys
                       select new { r.RoleID }).SingleOrDefault();
            return res.RoleID;
        }

        public string CreateRole(Role role)
        {
            using (var scope = new TransactionScope())
            {
                role.RoleID = Guid.NewGuid().ToString();
                repo.Insert(role);
                try
                {
                    db.SaveChanges();
                    scope.Complete();
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return role.RoleID;
            }
        }

        public bool UpdateRole(string id, Role role)
        {           
            using (var scope = new TransactionScope())
            {
                var existedRole = repo.GetByID(id);
                existedRole.Name = role.Name;
                existedRole.Description = role.Description;
                existedRole.IsSysAdmin = role.IsSysAdmin;
                repo.Update(existedRole);
                try
                {
                    db.SaveChanges();
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
//==========================================================================
        public bool DeleteRole(string id)
        {
            List<UserRoleDto> users = new List<UserRoleDto>();
            List<RolePermissionDto> permissions = new List<RolePermissionDto>();
            if(id != "") { 
            var user = from row in CoreDb.UserRoles
                       where row.RoleID == id
                       select row;
            users = user.Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.RoleID
            }).ToList();
            
            var rolepermission = from row in CoreDb.RolePermission
                                 where row.RoleID == id
                                 select row;
            permissions = rolepermission.Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID,
                Name = b.PermissionID
            }).ToList();
            }

            UserRoles data = new UserRoles();
            RolePermission permission = new RolePermission();
            
            using (var scope = new TransactionScope())
            {
                    for (int i = 0; i < users.Count; i++)
                    {
                        data.UserID = users[i].UserID;
                        data.RoleID = users[i].Name;
                        repoUser.Delete(data);
                    }
                    for (int i = 0; i < permissions.Count; i++)
                    {
                        permission.PermissionID = permissions[i].Name;
                        permission.RoleID = permissions[i].RoleID;
                        repoRolePermission.Delete(permission);
                    }
                var existedRole = repo.GetByID(id);
                repo.Delete(existedRole);
                try
                {
                    db.SaveChanges();
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
//=========================================================================================
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
            var RoleForPermissionQuery = from row in CoreDb.RolePermission
                                         where row.PermissionID == id
                                          select row;
            List<RolePermissionDto> rolelist = CoreDb.RolePermission.Where(t => t.PermissionID == id)
                .Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID                

            }).ToList();
            return rolelist;
        }

        public List<RolePermissionDto> GetRoleNotPermissionID(string id)
        {
            var RoleForPermissionQuery = from row in CoreDb.Role
                                         where !(from o in CoreDb.RolePermission
                                                 where o.PermissionID == id
                                                 select o.RoleID).Contains(row.RoleID)
                                         select row;
            List<RolePermissionDto> rolelist = RoleForPermissionQuery.Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID,
                    Name = b.Name,
                    Description = b.Description,
                    IsSysAdmin = b.IsSysAdmin

                }).ToList();
            return rolelist;
        }

        public Role GetRoleByName(string name)
        {
            var role = from row in CoreDb.Role
                       where row.Name == name
                       select row;
            Role get = role.SingleOrDefault();
            return get;
        }

        public List<Role> GetRoleByProjectUser(int id)
        {
            var customer = (from row in CoreDb.Project_MT
                           where row.ProjectIDSys == id
                           select row.CusIDSys).SingleOrDefault();
            var role = (from row in CoreDb.Role
                        join row2 in CoreDb.RolePermission on row.RoleID equals row2.RoleID
                        join row3 in CoreDb.Permission on row2.PermissionID equals row3.PermissionID
                        join row4 in CoreDb.Project_MT on row3.ProjectIDSys equals row4.ProjectIDSys
                        where row4.CusIDSys == customer
                       select row).Include("Project_MT").Distinct().ToList();
            
            return role;
        }

    }
}
