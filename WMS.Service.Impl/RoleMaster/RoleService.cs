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
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class RoleService : IRoleService
    {
        private RoleRepository repo;
        private UserRoleRepository repouser;

        public RoleService()
        {
            repo = new RoleRepository();
            repouser = new UserRoleRepository();
        }        

        public IEnumerable<Role> GetRoles()
        {           
            return repo.Get();
        }

        public IEnumerable<Role> GetRoles(int projectIDSys)
        {
            var roles = repo.Get(projectIDSys);
            return roles.ToList();
        }

        public Role GetRoleByLocIDSys(string id)
        {           
            Role Role = repo.GetByID(id);                                  
            return Role;            
        }

        public string GetRoleByUserAndProject(string UserID, int ProjectIDSys)
        {
            var res = repo.GetByUserAndProject(UserID,ProjectIDSys);
            return res;
        }

        public string CreateRole(Role role)
        {
            using (var scope = new TransactionScope())
            {
                role.RoleID = Guid.NewGuid().ToString();
                try
                {
                    repo.Insert(role);
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
                try
                {
                    repo.Update(role);
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
            //#Oil Comment
            //Wait for reperformance code
            List<UserRoleDto> users = new List<UserRoleDto>();
            List<RolePermissionDto> permissions = new List<RolePermissionDto>();
            if (id != "")
            {
                var user = repouser.GetUserByRoleID(id);
                permissions = repo.GetByRoleIDForDel(id).ToList();
            }

            UserRoles data = new UserRoles();
            RolePermission permission = new RolePermission();
            
            using (var scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        data.UserID = users[i].UserID;
                        data.RoleID = users[i].Name;
                        repouser.Delete(data);
                    }
                    for (int i = 0; i < permissions.Count; i++)
                    {
                        permission.PermissionID = permissions[i].Name;
                        permission.RoleID = permissions[i].RoleID;
                        repo.Delete(permission);
                    }
                var existedRole = repo.GetByID(id);
                repo.Delete(existedRole);
                
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
            List<RolePermissionDto> rolelist = repo.GetRoleByPermissionID(id)
                .Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID                

            }).ToList();
            return rolelist;
        }

        public List<RolePermissionDto> GetRoleNotPermissionID(string id)
        {
            var RoleForPermissionQuery = repo.GetNotPermissionID(id);
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
            return null;
        }

        public List<Role> GetRoleByProjectUser(int id)
        {
            var role = repo.GetByProjectUser(id);
            return role;
        }

    }
}
