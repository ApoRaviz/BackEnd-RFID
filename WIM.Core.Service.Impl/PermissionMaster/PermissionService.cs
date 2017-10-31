using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
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

namespace WIM.Core.Service.Impl
{
    public class PermissionService : IPermissionService
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

        public string CreatePermission(Permission Permission , string username)
        {
            using (var scope = new TransactionScope())
            {
                Permission.PermissionID = Guid.NewGuid().ToString();

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        repo.Insert(Permission , username);
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
                return Permission.PermissionID;
            }
        }

        public bool UpdatePermission(Permission Permission , string username)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        repo.Update(Permission,username);
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

        public string CreateRolePermission(string PermissionId, string RoleId , string username)
        {
            using (var scope = new TransactionScope())
            {
                //Permission.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                RolePermission data = new RolePermission();
                data.PermissionID = PermissionId;
                data.RoleID = RoleId;
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRepository<RolePermission> repo = new Repository<RolePermission>(Db);
                        repo.Insert(data , username);
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

        public string CreateRolePermission(string RoleId, List<PermissionTree> tree, string username)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRepository<RolePermission> repo = new Repository<RolePermission>(Db);
                        foreach (var c in tree)
                        {
                            RolePermission data = new RolePermission();
                            data.PermissionID = c.PermissionID;
                            data.RoleID = RoleId;
                            repo.Insert(data,username);
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
                IRepository<RolePermission> repo = new Repository<RolePermission>(Db);
                RolePermission id = new RolePermission() {RoleID = RoleId , PermissionID = PermissionId };
                var RoleForPermissionQuery = repo.GetByID(id);
                if (RoleForPermissionQuery != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        RolePermission temp = new RolePermission();
                        temp = RoleForPermissionQuery;
                        if (temp != null)
                        {
                            try
                            {
                                repo.Delete(temp);
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
                var menu = repo.GetMany(c => c.ProjectIDSys == projectid && !(Db.ApiMenuMapping.Where(a => a.Type =="A").Select(a => a.ApiIDSys+a.MenuIDSys).Contains(c.ApiIDSys+c.MenuIDSys)));
                var menutemp = repomenu.GetMany(c => (Db.Permission.Where(a => a.ProjectIDSys == projectid).Select(b => b.MenuIDSys)).Contains(c.MenuIDSys));
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
                var temp = repo.GetMany((c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys
                && !(Db.ApiMenuMapping.Where(b => b.Type == "A" && b.MenuIDSys == MenuIDSys).Select(a => a.ApiIDSys).Contains(c.ApiIDSys))));
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
                var temp = repo.GetManyQueryable((c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys
                && (Db.ApiMenuMapping.Where(b => b.Type == "A" && b.MenuIDSys == MenuIDSys).Select(a => a.ApiIDSys).Contains(c.ApiIDSys))));
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
                var temp = repo.GetManyQueryable(c => c.ProjectIDSys == ProjectIDSys && 
                (Db.RolePermission.Where(a => a.RoleID == RoleID).Select(b => b.PermissionID).Contains(c.PermissionID)));
                permission = temp.ToList();
            }
            return permission;
        }

        public bool DeleteAllInRole(string permissionID)
        {
            List<RolePermission> temp = new List<RolePermission>();
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<RolePermission> repo = new Repository<RolePermission>(Db);
                if (permissionID != null)
                {
                    temp = repo.GetMany(c => c.PermissionID == permissionID).ToList();
                }
                RolePermission x = new RolePermission();

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < temp.Count; i++)
                        {
                            repo.Delete(x);
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

