using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Repository.MenuAndPermission;
using WIM.Core.Repository.Impl.ApiMaster;
using WIM.Core.Repository;

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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }
                return true;
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                                AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
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
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionRepository repo = new PermissionRepository(Db);
                IMenuRepository repomenu = new MenuRepository(Db);
                IPermissionGroupRepository repogroup = new PermissionGroupRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                string[] include = { "PermissionGroup" };
                var x = repogroup.GetPermissionByGroupAndMenu(projectid).ToList();
                return x;
            }

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
                //var temp = repo.GetMany((c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys
                //&& !(Db2.ApiMenuMapping.Where(b => b.Type == "A" && b.MenuIDSys == MenuIDSys).Select(a => a.ApiIDSys).Contains(c.ApiIDSys))));
                //permission = temp.ToList();
                var temp = repo.GetMany(c => c.MenuIDSys == MenuIDSys && c.ProjectIDSys == ProjectIDSys);
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
                        AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
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

        public bool CreatePermissionByGroup(string GroupIDSys, MenuProjectMapping menu)
        {

            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionRepository repo = new PermissionRepository(Db);
                        IPermissionGroupApiRepository repo2 = new PermissionGroupApiRepository(Db);
                        var apies = repo2.GetMany(a => a.GroupIDSys == GroupIDSys).ToList();
                        foreach (var api in apies)
                        {
                            Permission data = new Permission();
                            data.PermissionID = Guid.NewGuid().ToString();
                            data.PermissionName = api.Title;
                            if (api.GET) { data.Method = "GET"; }
                            else if (api.POST) { data.Method = "POST"; }
                            else if (api.PUT) { data.Method = "PUT"; }
                            else if (api.DEL) { data.Method = "DEL"; }
                            data.ApiIDSys = api.ApiIDSys;
                            data.ProjectIDSys = menu.ProjectIDSys;
                            data.MenuIDSys = menu.MenuIDSys;
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
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }

                return true;
            }
        }

        public bool DeletePermissionByGroup(string GroupIDSys, MenuProjectMapping menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        CoreDbContext Db2 = new CoreDbContext();
                        IPermissionRepository repo = new PermissionRepository(Db);
                        IPermissionGroupApiRepository repo2 = new PermissionGroupApiRepository(Db);
                        var mainpermission = repo.GetPermissionByGroupMenu(GroupIDSys, menu);
                        foreach (var permission in mainpermission)
                        {
                            var rolePer = Db.RolePermissions.Where(a => a.PermissionID == permission.PermissionID).ToList();
                            if (rolePer != null)
                            {
                                Db.RolePermissions.RemoveRange(rolePer);
                                Db.SaveChanges();
                            }
                        }

                        foreach (var permission in mainpermission)
                        {
                            repo.Delete(permission);
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
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}

