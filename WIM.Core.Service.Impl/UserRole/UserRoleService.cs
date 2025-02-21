﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Globalization;
using System.Timers;
using System.Data.SqlTypes;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Service.Impl
{
    public class UserRoleService : Service, IUserRoleService
    {
        public UserRoleService()
        {
        }        

        public IEnumerable<UserRoles> GetUserRoles()
        {
            IEnumerable<UserRoles> role;
            using(CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                role = repo.Get();
            }

            return role;
        }

        public UserRoles GetUserRoleByLocIDSys(int id)
        {
            UserRoles UserRole;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                UserRole = repo.GetByID(id);
            }
            return UserRole;            
        }                      

        public string CreateUserRole(UserRoles UserRole)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRoleRepository repo = new UserRoleRepository(Db);
                        repo.Insert(UserRole);
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
                return UserRole.UserID;
            }
        }

        public bool UpdateUserRole(UserRoles UserRole)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRoleRepository repo = new UserRoleRepository(Db);
                        repo.Update(UserRole);
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

        public bool DeleteUserRole(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRoleRepository repo = new UserRoleRepository(Db);
                        var existedUserRole = repo.GetByID(id);
                        repo.Delete(id);
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

        public List<RoleUserDto> GetRoleByUserID(string userid)
        {
            // #JobComment
            List<RoleUserDto> RoleUser;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                RoleUser = repo.GetRoleByUserID(userid).ToList();
            }
            return RoleUser;
        }

        public List<UserRoleDto> GetUserByRoleID(string roleid)
        {
            List<UserRoleDto> RoleForPermissionQuery;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                string[] include = { "User" };
                RoleForPermissionQuery = repo.GetWithInclude((row => row.RoleID == roleid),include).Select(b => new UserRoleDto()
                {
                    UserID = b.UserID,
                    //Name = b.User.Name,
                    //Email = b.User.Email,
                    //PhoneNumber = b.User.PhoneNumber,
                    PasswordHash = b.User.PasswordHash,
                    //Surname = b.User.Surname,

                }).ToList();
            }
            return RoleForPermissionQuery.ToList();
        }

        public UserRoleDto GetUserRoleByUserID(string id)
        {
            UserRoleDto UserRole;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                UserRole = repo.GetUserRoleByUserID(id);
            }
            return UserRole;
        }
        public RoleUserDto GetRoleUserByRoleID(string id)
        {
            RoleUserDto RoleUser;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                RoleUser = repo.GetRoleUserByRoleID(id);
            }
            return RoleUser;
        }

        public string CreateUserRoles(string userid , string roleid )
        {
            using (var scope = new TransactionScope())
            {
                UserRoles data = new UserRoles();
                data.RoleID = roleid;
                data.UserID = userid;
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRoleRepository repo = new UserRoleRepository(Db);
                        repo.Insert(data);
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRoleRepository repo = new UserRoleRepository(Db);
                        repo.Insert(data );
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return data.UserID;
            }
        }

        public bool DeleteRolePermission(string UserId, string RoleId)
        {
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRoleRepository repo = new UserRoleRepository(Db);
                var RoleForPermissionQuery = repo.GetSingle(row => row.UserID == UserId && row.RoleID == RoleId);
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
                            Db.SaveChanges();
                            scope.Complete();
                            return true;
                        }
                    }
                }
            }
            return true;
        }

    }
}
