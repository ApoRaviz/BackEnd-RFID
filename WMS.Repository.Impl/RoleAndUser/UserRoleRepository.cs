using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class UserRoleRepository : IGenericRepository<UserRoles>
    {
        private CoreDbContext Db { get; set; }

        public UserRoleRepository()
        {
            Db = new CoreDbContext();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserRoles entityToDelete)
        {
            Db.UserRoles.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Delete(Func<UserRoles, bool> where)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRoles> Get()
        {
            var userrole = from c in Db.UserRoles
                           select c;
            return userrole.ToList();
        }

        public UserRoles Get(Func<UserRoles, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRoles> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserRoles GetByID(object id)
        {
            var userrole = from c in Db.UserRoles
                           where c.UserID == id
                           select c;
            return userrole.SingleOrDefault();
        }

        public IEnumerable<RoleUserDto> GetRoleByUserID(string userid)
        {
            var RoleUser = (from o in Db.Role
                                          join i in Db.UserRoles on o.RoleID equals i.RoleID
                                          where i.UserID == userid
                                          select o).Include("Project_MT").Select(b => new RoleUserDto()
                                          {
                                              RoleID = b.RoleID,
                                              Name = b.Name,
                                              Description = b.Description,
                                              IsSysAdmin = b.IsSysAdmin,
                                              Project_MT = null//b.Project_MT
                                          });
            return RoleUser;
        }

        public IEnumerable<UserRoleDto> GetUserByRoleID(string roleid)
        {
            var RoleForPermissionQuery = from row in Db.UserRoles
                                         where row.RoleID == roleid
                                         select row;
            var userlist = RoleForPermissionQuery.Include(a => a.User).Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.User.Name,
                Email = b.User.Email,
                //PhoneNumber = b.User.PhoneNumber.ToString(),
                PasswordHash = b.User.PasswordHash,
                Surname = b.User.Surname,

            });
            return userlist;
        }

        public UserRoleDto GetUserRoleByUserID(string id)
        {
            UserRoleDto UserRole = Db.User.Where(a => a.UserID == id).Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.Name,
                Email = b.Email,
                //PhoneNumber = b.PhoneNumber.ToString(),
                PasswordHash = b.PasswordHash,
                Surname = b.Surname
            }).SingleOrDefault();
            return UserRole;
        }
        public RoleUserDto GetRoleUserByRoleID(string id)
        {
            RoleUserDto RoleUser = Db.UserRoles.Include(a => a.Role).Select(b => new RoleUserDto()
            {
                RoleID = b.RoleID,
                Name = b.Role.Name,
                Description = b.Role.Description,
                IsSysAdmin = b.Role.IsSysAdmin
            }).SingleOrDefault();
            return RoleUser;
        }

        public UserRoles GetUserRole(string UserId, string RoleId)
        {
            var RoleForPermissionQuery = from row in Db.UserRoles
                                         where row.UserID == UserId && row.RoleID == RoleId
                                         select row;
            return RoleForPermissionQuery.SingleOrDefault();        }

        public UserRoles GetFirst(Func<UserRoles, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRoles> GetMany(Func<UserRoles, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserRoles> GetManyQueryable(Func<UserRoles, bool> where)
        {
            throw new NotImplementedException();
        }

        public UserRoles GetSingle(Func<UserRoles, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserRoles> GetWithInclude(Expression<Func<UserRoles, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public void Insert(UserRoles entity)
        {
            Db.UserRoles.Add(entity);
            Db.SaveChanges();
        }

        public void Update(UserRoles entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
