using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Repository;
using WMS.Common;

namespace WIM.Core.Repository.Impl
{
    public class RoleRepository : IGenericRepository<Role>
    {
        private CoreDbContext Db { get; set; }

        public RoleRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Role> Get()
        {
            var roles = from row in Db.Role
                        select row;
            return roles;
        }

        public IEnumerable<Role> Get(int projectIDSys)
        {
            var roles = from row in Db.Role
                        where row.ProjectIDSys == projectIDSys
                        select row;
            return roles;
        }
        public Role GetByID(object id)
        {
            var role = from c in Db.Role
                       where c.RoleID == id
                       select c;
            return role.SingleOrDefault();
        }

        public string GetByUserAndProject(string UserID, int ProjectIDSys)
        {
            var res = (from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectIDSys
                       select new { r.RoleID }).SingleOrDefault();
            return res.RoleID;
        }

        public IEnumerable<RolePermission> GetRoleByPermissionID(string id)
        {
            var RoleForPermissionQuery = from row in Db.RolePermission
                                         where row.PermissionID == id
                                         select row;
            return RoleForPermissionQuery;
        }

        public IEnumerable<RolePermissionDto> GetByRoleIDForDel(string id)
        {
            var rolepermission = from row in Db.RolePermission
                                 where row.RoleID == id
                                 select row;
            var permissions = rolepermission.Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID,
                Name = b.PermissionID
            });

            return permissions;
        }

        public IEnumerable<Role> GetNotPermissionID(string id)
        {
            var RoleForPermissionQuery = from row in Db.Role
                                         where !(from o in Db.RolePermission
                                                 where o.PermissionID == id
                                                 select o.RoleID).Contains(row.RoleID)
                                         select row;
            return RoleForPermissionQuery;
        }

        public List<Role> GetByProjectUser(int id)
        {
            var customer = (from row in Db.Project_MT
                            where row.ProjectIDSys == id
                            select row.CusIDSys).SingleOrDefault();
            var role = (from row in Db.Role
                        join row2 in Db.RolePermission on row.RoleID equals row2.RoleID
                        join row3 in Db.Permission on row2.PermissionID equals row3.PermissionID
                        join row4 in Db.Project_MT on row3.ProjectIDSys equals row4.ProjectIDSys
                        where row4.CusIDSys == customer
                        select row).Include("Project_MT").Distinct().ToList();

            return role;
        }

        public List<Role> GetByUser(string UserID)
        {
            var res = (from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID 
                       select r).Include(b => b.Project_MT).ToList();
            return res;
        }

        public void Insert(Role entity)
        {
            entity.RoleID = Guid.NewGuid().ToString();
            Db.Role.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Role entityToDelete)
        {
            Db.Role.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Update(Role entityToUpdate)
        {
            var existedRole = (from c in Db.Role
                               where c.RoleID == entityToUpdate.RoleID
                               select c).SingleOrDefault();
            existedRole.Name = entityToUpdate.Name;
            existedRole.Description = entityToUpdate.Description;
            existedRole.IsSysAdmin = entityToUpdate.IsSysAdmin;
            Db.SaveChanges();
        }

        public IEnumerable<Role> GetMany(Func<Role, bool> where)
        {
            return Db.Role.Where(where);
        }

        public IQueryable<Role> GetManyQueryable(Func<Role, bool> where)
        {
            throw new NotImplementedException();
        }

        public Role Get(Func<Role, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Role, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Role> GetWithInclude(Expression<Func<Role, bool>> predicate, params string[] include)
        {
            return Db.Role.Where(predicate).Include(include[0]);
        }

        public IEnumerable<Role> GetWithIncludes(Func<Role, bool> where, params string[] include)
        {
           
            return  Db.Role.Include(include[0]).Where(where);
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Role GetSingle(Func<Role, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Role GetFirst(Func<Role, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
