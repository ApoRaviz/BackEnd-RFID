using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class PermissionRepository : Repository<Permission> , IPermissionRepository
    {
        private CoreDbContext Db { get; set; }

        public PermissionRepository(CoreDbContext context):base(context)
        {
            Db = context;
        }

        public bool InsertRolePermission(RolePermission permission)
        {
            Db.RolePermission.Add(permission);
            Db.SaveChanges();
            return true;
        }

        public bool DeleteRolePermission(RolePermission permission)
        {
            Db.RolePermission.Remove(permission);
            return true;
        }

        public IEnumerable<RolePermission> GetRolePermissionByPerID(string permissionID)
        {
            var rolepermission = (from row in Db.RolePermission
                                  where row.PermissionID == permissionID
                                  select row);
            return rolepermission;
        }

        public IEnumerable<RolePermission> GetRolePermissionQuery(string PermissionId, string RoleId)
        {
            var RoleForPermissionQuery = from row in Db.RolePermission
                                         where row.PermissionID == PermissionId && row.RoleID == RoleId
                                         select row;
            return RoleForPermissionQuery;
        }

        public IEnumerable<Permission> GetRolePermissionNotApiQuery(int projectid)
        {
            var permission = from row in Db.Permission
                             where row.ProjectIDSys == projectid
                                   && !(from o in Db.ApiMenuMapping
                                        where o.Type == "A"
                                        select o.ApiIDSys + o.MenuIDSys).Contains(row.ApiIDSys + row.MenuIDSys)
                             select row;
            return permission;
        }

        public IEnumerable<Menu_MT> GetMenuByPermissionsinProject(int projectid)
        {
            var menu = from row in Db.Menu_MT
                       where (from o in Db.Permission
                              where o.ProjectIDSys == projectid
                              select o.MenuIDSys).Contains(row.MenuIDSys)
                       select row;
            return menu;
        }

        public IEnumerable<Permission> GetPermissionByProjID(int ProjectID)
        {
            var temp = from row in Db.Permission
                       where row.ProjectIDSys == ProjectID
                       select row;
            return temp;
        }

        public IEnumerable<Permission> GetPermissionByMenuID(int MenuIDSys, int ProjectIDSys)
        {
            var temp = from row in Db.Permission
                       where row.MenuIDSys == MenuIDSys && row.ProjectIDSys == ProjectIDSys &&
                               !(from i in Db.ApiMenuMapping
                                 where i.MenuIDSys == MenuIDSys && i.Type == "A"
                                 select i.ApiIDSys).Contains(row.ApiIDSys)
                       select row;
            return temp;
        }

        public IEnumerable<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys)
        {
            var temp = from row in Db.Permission
                       where row.MenuIDSys == MenuIDSys && row.ProjectIDSys == ProjectIDSys &&
                               (from i in Db.ApiMenuMapping
                                where i.MenuIDSys == MenuIDSys && i.Type == "A"
                                select i.ApiIDSys).Contains(row.ApiIDSys)
                       select row;
            return temp;
        }

        public IEnumerable<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys)
        {
            var temp = from row in Db.Permission
                       where row.ProjectIDSys == ProjectIDSys && 
                            (from o in Db.RolePermission
                             where o.RoleID == RoleID
                             select o.PermissionID).Contains(row.PermissionID)
                       select row;

            return temp;
        }

        public IEnumerable<Permission> GetPermissionByUserProject(int ProjectID, string UserID)
        {
            var temp = from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       join rp in Db.RolePermission on r.RoleID equals rp.RoleID
                       join ps in Db.Permission on rp.PermissionID equals ps.PermissionID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectID
                       select ps;

            return temp;
        }

        public IEnumerable<Permission> Get()
        {
            var permissions = from c in Db.Permission
                              select c;
            return permissions.ToList();
        }

        public Permission GetByID(object id)
        {
            var permission = from c in Db.Permission
                             where c.PermissionID == id
                             select c;
            return permission.SingleOrDefault();
        }

        public void Insert(Permission entity)
        {
            Db.Permission.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var permission = (from c in Db.Permission
                              where c.PermissionID == id

                              select c).SingleOrDefault();

            Db.Permission.Remove(permission);
            Db.SaveChanges();
        }

        public void Delete(RolePermission id)
        {
            var permission = (from c in Db.RolePermission
                              where c.PermissionID == id.PermissionID
                              && c.RoleID == id.RoleID
                              select c).SingleOrDefault();

            Db.RolePermission.Remove(permission);
            Db.SaveChanges();
        }

        public void Delete(Permission entityToDelete)
        {
            Db.Permission.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Update(Permission entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> GetMany(Func<Permission, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Permission> GetManyQueryable(Func<Permission, bool> where)
        {
            throw new NotImplementedException();
        }

        public Permission Get(Func<Permission, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Permission, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Permission> GetWithInclude(Expression<Func<Permission, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Permission GetSingle(Func<Permission, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Permission GetFirst(Func<Permission, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
