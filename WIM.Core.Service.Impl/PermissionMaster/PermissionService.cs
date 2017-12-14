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
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Entity.MenuManagement;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Helpers;

namespace WIM.Core.Service.Impl
{
    public class PermissionService : Service, IPermissionService
    {

        public PermissionService()
        {
        }

        public IEnumerable<Permission> GetPermissions()
        {
            IEnumerable<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                permission = repo.Get();
            }
                return permission;
        }

        public Permission GetPermissionByLocIDSys(string id)
        {
            Permission Permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                Permission = repo.GetByID(id);
            }
            return Permission;
        }

        public string CreatePermission(Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                Permission.PermissionID = Guid.NewGuid().ToString();
                Permission Permissionnew = new Permission();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        Permissionnew = repo.Insert(Permission);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                return Permissionnew.PermissionID;
            }
        }

        public bool UpdatePermission(Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        repo.Update(Permission);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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

        public bool DeletePermission(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                catch (DbUpdateException)
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
                RolePermissions data = new RolePermissions();
                data.PermissionID = PermissionId;
                data.RoleID = RoleId;
                RolePermissions datanew = new RolePermissions();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRepository<RolePermissions> repo = new Repository<RolePermissions>(Db);
                        datanew = repo.Insert(data);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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

        public string CreateRolePermission(string RoleId, List<PermissionTree> tree)
        {
            using (var scope = new TransactionScope())
            {
               try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRepository<RolePermissions> repo = new Repository<RolePermissions>(Db);
                        foreach (var c in tree)
                        {
                            RolePermissions data = new RolePermissions();
                            data.PermissionID = c.PermissionID;
                            data.RoleID = RoleId;
                            repo.Insert(data);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<RolePermissions> repo = new Repository<RolePermissions>(Db);
                RolePermissions id = new RolePermissions() { RoleID = RoleId, PermissionID = PermissionId };
                if (id != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        RolePermissions temp = new RolePermissions();
                        temp = id;
                        if (temp != null)
                        {
                            try
                            {
                                repo.Delete(id);
                                Db.SaveChanges();
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
                }
            }
            return true;
        }

        public List<PermissionTree> GetPermissionTree(int projectid)
        {
            List<PermissionTree> menutree;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                IRepository<Menu_MT> repomenu = new Repository<Menu_MT>(Db);
                CoreDbContext Db2 = new CoreDbContext();
                var menu = repo.GetMany(c => c.ProjectIDSys == projectid && !(Db2.ApiMenuMapping.Where(a => a.Type =="A").Select(a => a.ApiIDSys+a.MenuIDSys).Contains(c.ApiIDSys+c.MenuIDSys)));
                var menutemp = repomenu.GetMany(c => (Db2.Permission.Where(a => a.ProjectIDSys == projectid).Select(b => b.MenuIDSys)).Contains(c.MenuIDSys));
                menutree = menutemp.Select(b => new PermissionTree()
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
                    for (int j = 0; j < listpermission.Count; j++)
                    {
                        temp = listpermission[j];
                        if (menutree[i].PermissionID == temp[0].MenuIDSys.ToString())
                        {
                            menutree[i].Group = temp;
                        }
                    }
                }
            }
            return menutree;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID)
        {
            List<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                var temp = repo.GetMany(c => c.ProjectIDSys == ProjectID);
                permission = temp.ToList();
            }
            return permission;
        }

        public List<Permission> GetPermissionByMenuID(int MenuIDSys, int ProjectIDSys)
        {
            List<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                var temp = repo.GetMany((c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys
                && !(Db2.ApiMenuMapping.Where(b => b.Type == "A" && b.MenuIDSys == MenuIDSys).Select(a => a.ApiIDSys).Contains(c.ApiIDSys))));
                permission = temp.ToList();
            }
            return permission;
        }

        public List<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys)
        {
            List<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                var temp = repo.GetManyQueryable((c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys
                && (Db2.ApiMenuMapping.Where(b => b.Type == "A" && b.MenuIDSys == MenuIDSys).Select(a => a.ApiIDSys).Contains(c.ApiIDSys))));
                permission = temp.ToList();
            }
            return permission;
        }

        public List<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys)
        {
            List<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                var temp = repo.GetManyQueryable(c => c.ProjectIDSys == ProjectIDSys && 
                (Db2.RolePermissions.Where(a => a.RoleID == RoleID).Select(b => b.PermissionID).Contains(c.PermissionID)));
                permission = temp.ToList();
            }
            return permission;
        }

        public bool DeleteAllInRole(string permissionID)
        {
            List<RolePermissions> temp = new List<RolePermissions>();
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<RolePermissions> repo = new Repository<RolePermissions>(Db);
                if (permissionID != null)
                {
                    temp = repo.GetMany(c => c.PermissionID == permissionID).ToList();
                }
                RolePermissions x = new RolePermissions();

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < temp.Count; i++)
                        {
                            repo.Delete(temp[i]);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                        throw ex;
                    }
                }
            }
            return true;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID, string UserID)
        {
            List<Permission> permission;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                var temp = repo.GetPermissionByUserProject(ProjectID, UserID);
                permission = temp.ToList();
            }
            return permission;
        }

    }
}

