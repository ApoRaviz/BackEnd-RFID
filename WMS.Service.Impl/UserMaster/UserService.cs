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
using WMS.Common;
using WIM.Core.Security.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;
using WMS.Repository.Impl;
using WMS.Context;

namespace WMS.Service
{
    public class UserService : IUserService
    {
        private UserRepository repo;
        private UserRoleRepository repoRole;
        private object param = new { };

        public UserService()
        {
            repo = new UserRepository();
            repoRole = new UserRoleRepository();
        }

        public IEnumerable<User> GetUsers()
        {
            return repo.Get();
        }

        public User GetUserByUserID(string id)
        {
            User User = repo.GetByID(id);
            return User;
        }        

        public string GetFirebaseTokenMobileByUserID(string userid, int keyOtp = 0)
        {
            using (WMSDbContext DB = new WMSDbContext()) {
                try
                {
                    UserRepository repo = new UserRepository(DB);
                    u = repo.GetByID(userid);
                    if (keyOtp > 99999)
                    {
                        u.KeyOTP = keyOtp;
                        u.KeyOTPDate = DateTime.Now;
                        repo.Update(u);
                    }

                } catch (ValidationException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new ValidationException();
                }
            }
            return u.TokenMobile;
        }

        public object GetCustonersByUserID(string userid)
        {
            var query = repo.GetCustomerByUser(userid);
            return query;
        }

        public string CreateUser(User User)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    User.UserID = Guid.NewGuid().ToString();
                    User.EmailConfirmed = false;
                    User.PhoneNumberConfirmed = false;
                    User.TwoFactorEnabled = false;
                    User.AccessFailedCount = 0;
                    User.LockoutEnabled = true;
                    User.LastLogin = DateTime.Now.Date;
                    User.LockoutEndDateUtc = DateTime.Now.Date;
                repo.Insert(User);
                if(User.UserRoles != null)
                {
                    foreach(var c in User.UserRoles)
                    {
                        c.UserID = User.UserID;
                        repoRole.Insert(c);
                    }
                }

                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4009));
                    throw ex;
                }
                
                return User.UserID;
            }
        }

        public bool UpdateUser(User User)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (User.UserRoles != null)
                {
                    foreach (var c in User.UserRoles)
                    {
                        c.UserID = User.UserID;
                        repoRole.Insert(c);
                    }
                    User.UserRoles = null;
                }
                User.Email = User.Email;
                User.UserName = User.UserName;
                User.PasswordHash = User.PasswordHash;
                User.Name = User.Name;
                User.Surname = User.Surname;
                //User.PhoneNumber = User.PhoneNumber;
                repo.Update(User);
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
                scope.Complete();
                return true;
            }
        }

        public bool DeleteUser(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Delete(id);
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

   

        public bool GetKeyRegisterMobile(string userid, string key)
        {
            if (String.IsNullOrEmpty(userid))
                throw new ValidationException();
            
            using (var scope = new TransactionScope())
            {
                User u = repo.GetByID(userid);
                u.KeyAccess = key;
                u.KeyAccessDate = DateTime.Now;
                try
                {
                    repo.Update(u);
                    scope.Complete();
                }
                catch (Exception)
                {
                    throw new ValidationException();
                }
                return true;
            }
        }

        public bool RegisterTokenMobile(KeyAccessModel param)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    DateTime datew = DateTime.Now.AddMinutes(-2);
                    User u = repo.GetUserTokenRegis(param, datew);
                    if (u is null)
                    {
                        return false;
                    }
                    u.TokenMobile = param.Token;
                    u.KeyAccess = null;
                    u.KeyAccessDate = null;
                    repo.Update(u);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
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

        public List<User> getUserNotHave(string RoleID)
        {
            var user = repo.GetUserNotHave(RoleID);
            return user.ToList();
        }

        //public string CreateUserAndPerson(User User, Person_MT Person)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            Person.CreateDate = DateTime.Now;
        //            Person.UpdateDate = DateTime.Now;
        //            Person.UserUpdate = "1";
        //            repoPerson.Insert(Person);

        //            CoreDb.User.Add(new User()
        //                {
        //                    UserID = Guid.NewGuid().ToString(),
        //                    UserName = User.UserName,
        //                    Email = User.Email,
        //                    Name = User.Name,
        //                    Surname = User.Surname,
        //                    //PhoneNumber = User.PhoneNumber,
        //                    PasswordHash = User.PasswordHash,
        //                    EmailConfirmed = false,
        //                    PhoneNumberConfirmed = false,
        //                    TwoFactorEnabled = false,
        //                    CreateDate = DateTime.Now,
        //                    UpdateDate = DateTime.Now,
        //                    AccessFailedCount = 0,
        //                    LockoutEnabled = true,
        //                    LastLogin = DateTime.Now.Date,
        //                    PersonIDSys = Person.PersonIDSys,
        //                    LockoutEndDateUtc = DateTime.Now.Date,
        //                    UserUpdate = "1",
        //                    Active = 1
        //                });
        //           SecuDb.SaveChanges();
        //           scope.Complete();
        //        }
        //            //repo.Insert(User);
        //            catch (DbEntityValidationException e)
        //            {
        //                HandleValidationException(e);
        //            }
        //            catch (DbUpdateException)
        //            {
        //                scope.Dispose();
        //                ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4009));
        //                throw ex;
        //            }


        //        }

        //        return User.UserID;
        //}

        public User GetUserByPersonIDSys(int personIDSys)
        {
            var user = repo.GetByPersonIDSys(personIDSys);
            return user;
        }

        public bool UodateTokenMobile(FirebaseTokenModel param)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    User u = repo.GetUserTokenRegis(param);
                    if (u is null)
                    {
                        return false;
                    }
                    u.TokenMobile = param.NewToken;
                    repo.Update(u);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                return true;
            }
        }
    }
}
