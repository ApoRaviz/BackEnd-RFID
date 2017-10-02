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
using WMS.Master.Users;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class UserService : IUserService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<User> repo;
        private GenericRepository<Person_MT> repoPerson;
        private GenericRepository<UserRole> repoUserRole;
        private object param = new { };

        public UserService()
        {
            repoPerson = new GenericRepository<Person_MT>(db);
            repo = new GenericRepository<User>(db);
            repoUserRole = new GenericRepository<UserRole>(db);
        }

        public IEnumerable<User> GetUsers()
        {
            return repo.GetAll();
        }

        public User GetUserByUserID(string id)
        {
            User User = db.Users.Find(id);
            return User;
        }
        

        public string GetFirebaseTokenMobileByUserID(string userid, int keyOtp = 0)
        {
            User u;
            try
            {
                 u = (from user in db.Users
                          where user.UserID == userid
                          select user).FirstOrDefault();
                if (keyOtp > 99999)
                {
                    u.KeyOTP = keyOtp;
                    u.KeyOTPDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ValidationException();
            }
            return u.TokenMobile;
        }

        public object GetCustonersByUserID(string userid)
        {
            var query = from ctm in db.Customer_MT
                        join c in db.UserCustomerMappings on ctm.CusIDSys equals c.CusIDSys
                        where c.UserID == userid
                        select new
                        {
                            ctm.CusID,
                            ctm.CusIDSys,
                            ctm.CusName
                        };
            return query.ToList();
        }

        public string CreateUser(User User)
        {
            using (var scope = new TransactionScope())
            {
                    User.UserID = Guid.NewGuid().ToString();
                    User.UserName = User.UserName;
                    User.Email = User.Email;
                    User.Name = User.Name;
                    User.Surname = User.Surname;
                    User.PhoneNumber = User.PhoneNumber;
                    User.PasswordHash = User.PasswordHash;
                    User.EmailConfirmed = false;
                    User.PhoneNumberConfirmed = false;
                    User.TwoFactorEnabled = false;
                    User.CreateDate = DateTime.Now;
                    User.UpdateDate = DateTime.Now;
                    User.AccessFailedCount = 0;
                    User.LockoutEnabled = true;
                    User.LastLogin = DateTime.Now.Date;
                    User.PersonIDSys = User.PersonIDSys;
                    User.LockoutEndDateUtc = DateTime.Now.Date;
                    User.UserUpdate = "1";
                    User.Active = 1;
                repo.Insert(User);
                if(User.UserRoles != null)
                {
                    foreach(var c in User.UserRoles)
                    {
                        c.UserID = User.UserID;
                        repoUserRole.Insert(c);
                    }
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4009));
                    throw ex;
                }
                scope.Complete();
                return User.UserID;
            }
        }

        public bool UpdateUser(User User)
        {
            using (var scope = new TransactionScope())
            {
                if (User.UserRoles != null)
                {
                    foreach (var c in User.UserRoles)
                    {
                        c.UserID = User.UserID;
                        repoUserRole.Insert(c);
                    }
                    User.UserRoles = null;
                }
                User.Email = User.Email;
                User.UserName = User.UserName;
                User.PasswordHash = User.PasswordHash;
                User.Name = User.Name;
                User.Surname = User.Surname;
                User.PhoneNumber = User.PhoneNumber;
                User.UpdateDate = DateTime.Now;
                User.UserUpdate = "1";
                repo.Update(User);
                
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
        }

        public bool DeleteUser(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedUser = repo.GetByID(id);
                existedUser.Active = 0;
                existedUser.UpdateDate = DateTime.Now;
                existedUser.UserUpdate = "1";
                repo.Update(existedUser);
                db.SaveChanges();
                scope.Complete();
                return true;
            }
        }

   

        public bool GetKeyRegisterMobile(string userid, string key)
        {
            if (String.IsNullOrEmpty(userid))
                throw new ValidationException();
            
            using (var scope = new TransactionScope())
            {
                User u = (from user in db.Users
                          where user.UserID == userid
                          select user).FirstOrDefault();
                u.KeyAccess = key;
                u.KeyAccessDate = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (Exception e)
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
                    User u = (from user in db.Users
                              where user.KeyAccess == param.Key
                              && user.KeyAccessDate > datew && user.KeyAccess != null
                              select user).FirstOrDefault();
                    if (u is null)
                    {
                        return false;
                    }
                    u.TokenMobile = param.Token;
                    u.KeyAccess = null;
                    u.KeyAccessDate = null;
                    db.SaveChanges();
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
            var user = (from row in db.Users
                        where !(from o in db.UserRoles
                                where o.RoleID == RoleID
                                select o.UserID).Contains(row.UserID)
                        select row).ToList();
            return user;
        }

        public string CreateUserAndPerson(User User,Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Person.CreateDate = DateTime.Now;
                    Person.UpdateDate = DateTime.Now;
                    Person.UserUpdate = "1";
                    repoPerson.Insert(Person);
                    db.SaveChanges();
                db.Users.Add(new User()
                        {
                            UserID = Guid.NewGuid().ToString(),
                            UserName = User.UserName,
                            Email = User.Email,
                            Name = User.Name,
                            Surname = User.Surname,
                            PhoneNumber = User.PhoneNumber,
                            PasswordHash = User.PasswordHash,
                            EmailConfirmed = false,
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            AccessFailedCount = 0,
                            LockoutEnabled = true,
                            LastLogin = DateTime.Now.Date,
                            PersonIDSys = Person.PersonIDSys,
                            LockoutEndDateUtc = DateTime.Now.Date,
                            UserUpdate = "1",
                            Active = 1
                        });
                   db.SaveChanges();
                    }
                    //repo.Insert(User);
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    catch (DbUpdateException e)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4009));
                        throw ex;
                    }
                    scope.Complete();
                    
                }
                
                return User.UserID;
        }

        public User GetUserByPersonIDSys(int personIDSys)
        {
            var user = from i in db.Users
                       where i.PersonIDSys == personIDSys
                       select i;
            return user.SingleOrDefault();
        }

        public bool UodateTokenMobile(FirebaseTokenModel param)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    User u = (from user in db.Users
                              where user.TokenMobile == param.Token
                              && user.KeyAccessDate == null && user.KeyAccess == null
                              select user).FirstOrDefault();
                    if (u is null)
                    {
                        return false;
                    }
                    u.TokenMobile = param.NewToken;
                    db.SaveChanges();
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
