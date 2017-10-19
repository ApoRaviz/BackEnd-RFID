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
using WIM.Core.Security.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Context;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class PermissionService : IPermissionService
    {
        private PermissionRepository repo;

        public PermissionService()
        {
            repo = new PermissionRepository();
            //repoRolePermission = new GenericRepository<RolePermission>(db);
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return repo.Get();
        }

        public Permission GetPermissionByLocIDSys(string id)
        {
            Permission Permission = repo.GetByID(id);
            return Permission;
        }

        public string CreatePermission(Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                Permission.PermissionID = Guid.NewGuid().ToString();

                try
                {
                    repo.Insert(Permission);
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
                return Permission.PermissionID;
            }
        }

        public bool UpdatePermission(string id, Permission Permission)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(Permission);
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
                return true;
            }
        }

        public bool DeletePermission(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Delete(id);
                    scope.Complete();
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
                RolePermission data = new RolePermission();
                data.PermissionID = PermissionId;
                data.RoleID = RoleId;
                try
                {
                    repo.InsertRolePermission(data);
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

        public string CreateRolePermission(string RoleId, List<PermissionTree> tree)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    //Permission.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                    foreach (var c in tree)
                    {
                        RolePermission data = new RolePermission();
                        data.PermissionID = c.PermissionID;
                        data.RoleID = RoleId;
                        repo.InsertRolePermission(data);
                    }

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
            var RoleForPermissionQuery = repo.GetRolePermissionQuery(PermissionId, RoleId);
            if (RoleForPermissionQuery != null)
            {
                using (var scope = new TransactionScope())
                {
                    RolePermission temp = new RolePermission();
                    temp = RoleForPermissionQuery.SingleOrDefault();
                    if (temp != null)
                    {
                        try
                        {
                            repo.DeleteRolePermission(temp);
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
            return true;
        }

        public List<PermissionTree> GetPermissionTree(int projectid)
        {
            var menu = repo.GetRolePermissionNotApiQuery(projectid);
            var menutemp = repo.GetMenuByPermissionsinProject(projectid);
            List <PermissionTree> menutree = menutemp.Select(b => new PermissionTree()
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
            return menutree;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID)
        {
            var temp = repo.GetPermissionByProjID(ProjectID);
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionByMenuID(int MenuIDSys, int ProjectIDSys)
        {
            var temp = repo.GetPermissionByMenuID(MenuIDSys, ProjectIDSys);
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys)
        {
            var temp = repo.GetPermissionAuto(MenuIDSys, ProjectIDSys);
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public List<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys)
        {
            var temp = repo.GetPermissionByRoleID(RoleID, ProjectIDSys);
            List<Permission> permission = temp.ToList();
            return permission;
        }

        public bool DeleteAllInRole(string permissionID)
        {
            List<RolePermissionDto> temp = new List<RolePermissionDto>();
            if (permissionID != null)
            {
                var temp1 = repo.GetRolePermissionByPerID(permissionID);
                var y = temp1.ToList();
                temp = temp1.Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID,
                    Name = b.PermissionID
                }).ToList();
            }
            RolePermission x = new RolePermission();

            using (var scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < temp.Count; i++)
                {

                    x.RoleID = temp[i].RoleID;
                    x.PermissionID = temp[i].Name;
                    repo.Delete(x);
                }
                
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
            return true;
        }

        public List<Permission> GetPermissionByProjectID(int ProjectID, string UserID)
        {

            var temp = repo.GetPermissionByUserProject(ProjectID, UserID);
            List <Permission> permission = temp.ToList();
            return permission;
        }

    }
}

