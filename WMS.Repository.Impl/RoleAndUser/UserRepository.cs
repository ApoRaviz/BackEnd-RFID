using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Common;
using WMS.Context;
using WMS.Repository.UserManagement;

namespace WMS.Repository.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private WMSDbContext Db;
        //private ItemSetRepository repo;
        private IIdentity Identity;

        public UserRepository(WMSDbContext context, IIdentity identity) : base(context, identity)
        {
            Db = context;
        }

        public IEnumerable<User> Get()
        {
            var users = from c in Db.User
                      select c;
            return users;
        }

        public User GetByID(object id)
        {
            var user = from c in Db.User
                       where c.UserID == id
                        select c;
            return user.SingleOrDefault();
        }

        public object GetCustomerByUser(string userid)
        {
            var query = (from ctm in Db.Customer_MT
                         join c in Db.Project_MT on ctm.CusIDSys equals c.CusIDSys
                         join d in Db.Role on c.ProjectIDSys equals d.ProjectIDSys
                         join e in Db.UserRoles on d.RoleID equals e.RoleID
                         where e.UserID == userid
                         select new
                         {
                             ctm.CusID,
                             ctm.CusIDSys,
                             ctm.CusName
                         }).Distinct();
            return query.ToList();
        }

        public User GetByPersonIDSys(int personIDSys)
        {
            var user = from i in Db.User
                       where i.PersonIDSys == personIDSys
                       select i;
            return user.SingleOrDefault();
        }

        public IEnumerable<User> GetUserNotHave(string role)
        {
            var user = (from row in Db.User
                        where !(from o in Db.UserRoles
                                where o.RoleID == role
                                select o.UserID).Contains(row.UserID)
                        select row);
            return user;
        } 

        public User GetUserTokenRegis(KeyAccessModel param , DateTime date)
        {
            var user = (from c in Db.User
                        where c.KeyAccess == param.Key
                        && c.KeyAccessDate > date && c.KeyAccess != null
                        select c).FirstOrDefault();
            return user;
        }

        public User GetUserTokenRegis(FirebaseTokenModel param)
        {
            var user = (from c in Db.User
                        where c.TokenMobile == param.Token
                        && c.KeyAccessDate == null && c.KeyAccess == null
                        select c).FirstOrDefault();
            return user;
        }

        public void Insert(User entity)
        {
            //entity.Active = 1;
            Db.User.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedUser = this.GetByID(id);
            //existedUser.Active = 0;
            //existedUser.UpdateDate = DateTime.Now;
            //existedUser.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(User entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(User entityToUpdate)
        {
            var User = this.GetByID(entityToUpdate.UserID);
            User.Email = entityToUpdate.Email;
            User.UserName = entityToUpdate.UserName;
            User.PasswordHash = entityToUpdate.PasswordHash;
            User.Name = entityToUpdate.Name;
            User.Surname = entityToUpdate.Surname;
            //User.PhoneNumber = User.PhoneNumber;
            User.TokenMobile = entityToUpdate.TokenMobile;
            User.KeyAccess = entityToUpdate.KeyAccess;
            User.KeyAccessDate = entityToUpdate.KeyAccessDate;
            Db.SaveChanges();
        }

        public IEnumerable<User> GetMany(Func<User, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetManyQueryable(Func<User, bool> where)
        {
            throw new NotImplementedException();
        }

        public User Get(Func<User, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<User, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetWithInclude(Expression<Func<User, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public User GetSingle(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public User GetFirst(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
