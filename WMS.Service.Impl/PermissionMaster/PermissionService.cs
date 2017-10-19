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
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public class PermissionService : IPermissionService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Permission> repo;
        private GenericRepository<RolePermission> repoRolePermission;

        public PermissionService()
        {
            repo = new GenericRepository<Permission>(db);
            repoRolePermission = new GenericRepository<RolePermission>(db);
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return repo.GetAll();
        }

        public Permission GetPermissionByLocIDSys(string id)
        {
            Permission Permission = db.Permissions.Find(id);
            return Permission;
        }

        public string CreatePermission(Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                //Permission.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                Permission.PermissionID = Guid.NewGuid().ToString();
                repo.Insert(Permission);
                try
                {
                    db.SaveChanges();
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
                return Permission.PermissionID;
            }
        }

        public bool UpdatePermission(string id, Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                var existedPermission = repo.GetByID(id);
                existedPermission.PermissionName = Permission.PermissionName;

                repo.Update(existedPermission);
                try
                {
                    db.SaveChanges();
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

        public bool DeletePermission(string id)
        {
            using (var scope = new TransactionScope())
            {
                var existedPermission = repo.GetByID(id);
                repo.Delete(existedPermission);
                try
                {
                db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException )
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                catch (DbUpdateException )
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

        public string CreateRolePermission(string PermissionId, string RoleId)
        {
            using (var scope = new TransactionScope())
            {
                //Permission.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                RolePermission data = new RolePermission();
                data.PermissionID = PermissionId;
                data.RoleID = RoleId;
                repoRolePermission.Insert(data);
                try
                {
                     db.SaveChanges();
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
                return RoleId;
            }
        }

        public string CreateRolePermission(string RoleId, List<PermissionTree> tree)
        {
            using (var scope = new TransactionScope())
            {
                //Permission.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                foreach (var c in tree)
                {
                    RolePermission data = new RolePermission();
                    data.PermissionID = c.PermissionID;
                    data.RoleID = RoleId;
                    repoRolePermission.Insert(data);
                }
                try
                {
                    db.SaveChanges();
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
                
                return RoleId;
            }
        }

        public bool DeleteRolePermission(string PermissionId, string RoleId)
        {   
            var RoleForPermissionQuery = from row in db.RolePermission
                                         where row.PermissionID == PermissionId && row.RoleID == RoleId
                                         select row;
            if(RoleForPermissionQuery != null) { 
            using (var scope = new TransactionScope())
            {
                    RolePermission temp = new RolePermission();
                    temp = RoleForPermissionQuery.SingleOrDefault();
                    if (temp != null)
                    {
                        repoRolePermission.Delete(temp);
                        try
                        {
                        db.SaveChanges();
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
            }return true;
        }

        public List<PermissionTree> GetPermissionTree(int projectid)
        {
            var menu = from row in db.Permissions
                       where row.ProjectIDSys == projectid
                             && !(from o in db.ApiMenuMappings
                                  where o.Type == "A"
                                  select o.ApiIDSys+o.MenuIDSys).Contains(row.ApiIDSys+row.MenuIDSys)
                       select row;
            var menutemp = from row in db.Menu_MT
                           where (from o in db.Permissions
                                  where o.ProjectIDSys == projectid
                                  select o.MenuIDSys).Contains(row.MenuIDSys)
                           select row;
            List<PermissionTree> menutree = menutemp.Select(b => new PermissionTree()
            {
                PermissionName = b.MenuName,
                PermissionID = b.MenuIDSys.ToString()
            }).ToList();
            List<PermissionTree> permissionlist = menu.Select(b => new PermissionTree()
            {
                PermissionID = b.PermissionID,
                PermissionName = b.PermissionName,
                MenuIDSys = b.MenuIDSys,
                Method = b.Method
            }).ToList();
            List<List<PermissionTree>> listpermission = permissionlist.GroupBy(a => a.MenuIDSys).Select(grp => grp.ToList()).ToList();
            List<PermissionTree> temp;
            Console.Write("abc");
            for (int i = 0; i < menutree.Count; i++)
            {
                for(int j = 0; j < listpermission.Count; j++)
                {
                    temp = listpermission[j];
                    if (menutree[i].PermissionID == temp[0].MenuIDSys.ToString())
                    {
                        menutree[i].Group = temp;
                    }
                }
            }
            return menutree;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID)
        {
            var temp = from row in db.Permissions
                       where row.ProjectIDSys == ProjectID
                       select row;
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionByMenuID(int MenuIDSys ,int ProjectIDSys)
        {
            var temp = from row in db.Permissions
                       where row.MenuIDSys == MenuIDSys && row.ProjectIDSys == ProjectIDSys &&
                               !(from i in db.ApiMenuMappings
                                 where i.MenuIDSys == MenuIDSys && i.Type == "A"
                                 select i.ApiIDSys).Contains(row.ApiIDSys)
                       select row;
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys)
        {
            var temp = from row in db.Permissions
                       where row.MenuIDSys == MenuIDSys && row.ProjectIDSys == ProjectIDSys &&
                               (from i in db.ApiMenuMappings
                                 where i.MenuIDSys == MenuIDSys && i.Type == "A"
                                 select i.ApiIDSys).Contains(row.ApiIDSys)
                       select row;
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys)
        {
            var temp = from row in db.Permissions
                       where row.ProjectIDSys == ProjectIDSys && (from o in db.RolePermission
                               where o.RoleID == RoleID
                               select o.PermissionID).Contains(row.PermissionID)
                       select row;
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public bool DeleteAllInRole(string permissionID)
        {
            List<RolePermissionDto> temp = new List<RolePermissionDto>();
            if (permissionID != null)
            {
                var temp1 = (from row in db.RolePermission
                                             where row.PermissionID == permissionID
                                             select row);
                var y = temp1.ToList();
                temp = temp1.Select(b => new RolePermissionDto() {
                    RoleID = b.RoleID,
                    Name = b.PermissionID
                }).ToList();
            }
            RolePermission x = new RolePermission();
                
                using (var scope = new TransactionScope())
                {
                    for (int i = 0; i < temp.Count; i++)
                {
                    
                        x.RoleID = temp[i].RoleID;
                        x.PermissionID = temp[i].Name;
                        repoRolePermission.Delete(x); 
                }
                try
                {
                    db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                catch (DbUpdateException )
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
            }
            return true;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID,string UserID)
        {

            var temp = from ur in db.UserRoles
                       join r in db.Roles on ur.RoleID equals r.RoleID
                       join rp in db.RolePermission on r.RoleID equals rp.RoleID
                       join ps in db.Permissions on rp.PermissionID equals ps.PermissionID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectID
                       select ps;
            List<Permission> permission = temp.ToList();
            return permission;
        }

    }
}

