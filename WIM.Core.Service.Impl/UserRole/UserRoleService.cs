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
using System.Globalization;
using System.Timers;
using System.Data.SqlTypes;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;

namespace WIM.Core.Service.Impl
{
    public class UserRoleService : IUserRoleService
    {
        private UserRoleRepository repo;

        public UserRoleService()
        {
            repo = new UserRoleRepository();
        }        

        public IEnumerable<UserRoles> GetUserRoles()
        {           
            return repo.Get();
        }

        public UserRoles GetUserRoleByLocIDSys(int id)
        {           
            UserRoles UserRole = repo.GetByID(id);                                  
            return UserRole;            
        }                      

        public string CreateUserRole(UserRoles UserRole)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Insert(UserRole);
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
                return UserRole.UserID;
            }
        }

        public bool UpdateUserRole(int id, UserRoles UserRole)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(UserRole);
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

        public bool DeleteUserRole(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var existedUserRole = repo.GetByID(id);
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

        public List<RoleUserDto> GetRoleByUserID(string userid)
        {
            // #JobComment
            List<RoleUserDto> RoleUser = repo.GetRoleByUserID(userid).ToList();
            return RoleUser;
        }

        public List<UserRoleDto> GetUserByRoleID(string roleid)
        {
            var RoleForPermissionQuery = repo.GetUserByRoleID(roleid);
            return RoleForPermissionQuery.ToList();
        }

        public UserRoleDto GetUserRoleByUserID(string id)
        {
            UserRoleDto UserRole = repo.GetUserRoleByUserID(id);
            return UserRole;
        }
        public RoleUserDto GetRoleUserByRoleID(string id)
        {
            RoleUserDto RoleUser = repo.GetRoleUserByRoleID(id);
            return RoleUser;
        }

        public string CreateUserRoles(string userid , string roleid)
        {
            using (var scope = new TransactionScope())
            {
                UserRoles data = new UserRoles();
                data.RoleID = roleid;
                data.UserID = userid;
                try
                {
                    repo.Insert(data);
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
                return data.UserID;
            }
        }

        public string CreateRoleUsers(string userid, string roleid)
        {
            using (var scope = new TransactionScope())
            {
                UserRoles data = new UserRoles();
                data.RoleID = roleid;
                data.UserID = userid;
                try
                {
                    repo.Insert(data);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return data.UserID;
            }
        }

        public bool DeleteRolePermission(string UserId, string RoleId)
        {
            var RoleForPermissionQuery = repo.GetUserRole(UserId, RoleId);
            if (RoleForPermissionQuery != null)
            {
                using (var scope = new TransactionScope())
                {
                    /*RolePermission temp = RoleForPermissionQuery.Select(b => new RolePermission()
                    {
                        RoleID = b.RoleID,
                        PermissionID = b.PermissionID
                    }).SingleOrDefault();*/
                    UserRoles temp = new UserRoles();
                    temp = RoleForPermissionQuery;
                    if (temp != null)
                    {
                        repo.Delete(temp);
                        scope.Complete();
                        return true;
                    }
                }
            }
            return true;
        }

    }
}
