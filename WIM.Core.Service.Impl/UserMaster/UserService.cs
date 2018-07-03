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
using System.Globalization;
using System.Timers;
using System.Data.SqlTypes;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.Person;
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
    public class UserService : Service, IUserService
    {
        private object param = new { };
        public UserService()
        {
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRepository repo = new UserRepository(Db);
                users = repo.Get();
            }
            return users;
        }

        public User GetUserByUserID(string id)
        {
            User User;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRepository repo = new UserRepository(Db);
                User = repo.GetByID(id);
            }
            return User;
        }

        public string GetFirebaseTokenMobileByUserID(string userid, int keyOtp = 0)
        {
            try
            {
                using (CoreDbContext Db = new CoreDbContext())
                {
                    var u = (from us in Db.User where us.UserID == userid select us).SingleOrDefault();
                    if (keyOtp > 99999)
                    {
                        u.KeyOTP = keyOtp;
                        u.KeyOTPDate = DateTime.Now;
                    }
                    Db.SaveChanges();
                    return u.TokenMobile;
                }

            }
            catch (AppValidationException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new AppValidationException();
            }
        }

        public object GetCustonersByUserID(string userid)
        {
            object query;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRepository repo = new UserRepository(Db);
                query = repo.GetCustomerByUser(userid);
            }
            return query;
        }

        public string CreateUser(User User)
        {
            using (var scope = new TransactionScope())
            {
                User usernew = new User();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        IRepository<UserRoles> repoRole = new Repository<UserRoles>(Db);
                        var userole = User.UserRoles;
                        User.UserRoles = null;
                        User.UserID = Guid.NewGuid().ToString();
                        User.EmailConfirmed = false;
                        User.PhoneNumberConfirmed = false;
                        User.TwoFactorEnabled = false;
                        User.AccessFailedCount = 0;
                        User.LockoutEnabled = true;
                        User.LastLogin = DateTime.Now.Date;
                        User.LockoutEndDateUtc = DateTime.Now.Date;
                        User.TokenMobile = "csgB-N8-waE:APA91bGFH7LKHsHjaW9Xec7XzvpR5DdDo6l3BA9G1TufgF_ECePlKE0Yg7Z4zfYmRuiUXtR8faSLa-hG2Zvn-2CaIseVxqIaQ_dfQa0cPvn3HzEHTuyHFrln0GY02pRVDEFngOGnfHSN";
                        usernew = repo.Insert(User);

                        if (userole != null)
                        {
                            foreach (var c in userole)
                            {
                                c.UserID = usernew.UserID;
                                 repoRole.Insert(c);
                            }
                        }
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
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        IRepository<UserRoles> repoRole = new Repository<UserRoles>(Db);
                        if (User.UserRoles != null)
                        {
                            foreach (var c in User.UserRoles)
                            {
                                c.UserID = User.UserID;
                                repoRole.Insert(c );
                            }
                            User.UserRoles = null;
                            Db.SaveChanges();
                        }
                        //User.Email = User.Email;
                        //User.UserName = User.UserName;
                        //User.PasswordHash = User.PasswordHash;
                        //User.LastLogin = DateTime.Now;
                        //User.PhoneNumber = User.PhoneNumber;
                        //repo.Update(User );
                        //Db.SaveChanges();
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
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                ;
                return true;
            }
        }

        public bool DeleteUser(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        repo.Delete(id);
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
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);

                }

                return true;
            }
        }



        public bool GetKeyRegisterMobile(string userid, string key)
        {
            if (String.IsNullOrEmpty(userid))
                throw new AppValidationException();

            using (var scope = new TransactionScope())
            {try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        User u = repo.GetByID(userid);
                        u.KeyAccess = key;
                        u.KeyAccessDate = DateTime.Now;

                        repo.Update(u );
                        scope.Complete();
                    }
                }
                catch (Exception)
                {
                    throw new AppValidationException();
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        DateTime datew = DateTime.Now.AddMinutes(-2);
                        User u = repo.GetSingle(c => c.KeyAccess == param.Key
                        && c.KeyAccessDate > datew && c.KeyAccess != null);
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
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                return true;
            }
        }

        public List<User> getUserNotHave(string RoleID)
        {
            List<User> user;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRepository repo = new UserRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                user = repo.GetMany(c => !(Db2.UserRoles.Where(a => a.RoleID == RoleID).Select(a => a.UserID).Contains(c.UserID))).ToList();
            }
            return user.ToList();
        }


        public User GetUserByPersonIDSys(int personIDSys)
        {
            User user;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IUserRepository repo = new UserRepository(Db);
                user = repo.Get(c => c.PersonIDSys == personIDSys);
            }
            return user;
        }

        public bool UodateTokenMobile(FirebaseTokenModel param)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IUserRepository repo = new UserRepository(Db);
                        User u = repo.GetSingle(c => c.TokenMobile == param.Token
                        && c.KeyAccessDate == null && c.KeyAccess == null);
                        if (u is null)
                        {
                            return false;
                        }
                        u.TokenMobile = param.NewToken;
                        repo.Update(u);
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                return true;
            }
        }
    }
}
