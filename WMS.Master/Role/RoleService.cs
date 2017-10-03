﻿using AutoMapper;
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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class RoleService : IRoleService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Role> repo;
        private GenericRepository<UserRole> repoUser;
        private GenericRepository<RolePermission> repoRolePermission;

        public RoleService()
        {
            repo = new GenericRepository<Role>(db);
            repoUser = new GenericRepository<UserRole>(db);
            repoRolePermission = new GenericRepository<RolePermission>(db);
        }        

        public IEnumerable<Role> GetRoles()
        {           
            return repo.GetAll();
        }

        public IEnumerable<Role> GetRoles(int projectIDSys)
        {
            var roles = from row in db.Roles
                        where row.ProjectIDSys == projectIDSys
                        select row;
            return roles;
        }

        public Role GetRoleByLocIDSys(string id)
        {           
            Role Role = db.Roles.Find(id);                                  
            return Role;            
        }

        public string GetRoleByUserAndProject(string UserID, int ProjectIDSys)
        {
            var res = (from ur in db.UserRoles
                       join r in db.Roles on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectIDSys
                       select new { r.RoleID }).SingleOrDefault();
            return res.RoleID;
        }

        public string CreateRole(Role role)
        {
            using (var scope = new TransactionScope())
            {
                //Role.Id = db.ProcGetNewID("RL").FirstOrDefault().Substring(0, 13);
                role.RoleID = Guid.NewGuid().ToString();
                try
                {
                    repo.Insert(role);
                }
                catch(DbUnexpectedValidationException e)
                {
                    Console.Write(e);
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return role.RoleID;
            }
        }

        public bool UpdateRole(string id, Role role)
        {           
            using (var scope = new TransactionScope())
            {
                var existedRole = repo.GetByID(id);
                existedRole.Name = role.Name;
                existedRole.Description = role.Description;
                existedRole.IsSysAdmin = role.IsSysAdmin;

                repo.Update(existedRole);
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }
//=======================================
        public bool DeleteRole(string id)
        {
            List<UserRoleDto> users = new List<UserRoleDto>();
            List<RolePermissionDto> permissions = new List<RolePermissionDto>();
            if(id != "") { 
            var user = from row in db.UserRoles
                       where row.RoleID == id
                       select row;
            users = user.Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.RoleID
            }).ToList();
            
            var rolepermission = from row in db.RolePermission
                                 where row.RoleID == id
                                 select row;
            permissions = rolepermission.Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID,
                Name = b.PermissionID
            }).ToList();
            }

            UserRole data = new UserRole();
            RolePermission permission = new RolePermission();
            
            using (var scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        data.UserID = users[i].UserID;
                        data.RoleID = users[i].Name;
                        repoUser.Delete(data);
                        db.SaveChanges();
                    }
                    for (int i = 0; i < permissions.Count; i++)
                    {
                        permission.PermissionID = permissions[i].Name;
                        permission.RoleID = permissions[i].RoleID;
                        repoRolePermission.Delete(permission);
                        db.SaveChanges();
                    }
                    var existedRole = repo.GetByID(id);
                    repo.Delete(existedRole);
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }

                return true;
            }
        }
//=======================================
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

        public List<RolePermissionDto> GetRoleByPermissionID(string id)
        {
            var RoleForPermissionQuery = from row in db.RolePermission
                                         where row.PermissionID == id
                                          select row;
            List<RolePermissionDto> rolelist = db.RolePermission.Where(t => t.PermissionID == id)
                .Select(b => new RolePermissionDto()
            {
                RoleID = b.RoleID                

            }).ToList();
            return rolelist;
        }

        public List<RolePermissionDto> GetRoleNotPermissionID(string id)
        {
            var RoleForPermissionQuery = from row in db.Roles
                                         where !(from o in db.RolePermission
                                                 where o.PermissionID == id
                                                 select o.RoleID).Contains(row.RoleID)
                                         select row;
            List<RolePermissionDto> rolelist = RoleForPermissionQuery.Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID,
                    Name = b.Name,
                    Description = b.Description,
                    IsSysAdmin = b.IsSysAdmin

                }).ToList();
            return rolelist;
        }

        public Role GetRoleByName(string name)
        {
            var role = from row in db.Roles
                       where row.Name == name
                       select row;
            Role get = role.SingleOrDefault();
            return get;
        }

        public List<Role> GetRoleByProjectUser(int id)
        {
            var customer = (from row in db.Project_MT
                           where row.ProjectIDSys == id
                           select row.CusIDSys).SingleOrDefault();
            var role = (from row in db.Roles
                        join row2 in db.RolePermission on row.RoleID equals row2.RoleID
                        join row3 in db.Permissions on row2.PermissionID equals row3.PermissionID
                        join row4 in db.Project_MT on row3.ProjectIDSys equals row4.ProjectIDSys
                        where row4.CusIDSys == customer
                       select row).Include("Project_MT").Distinct().ToList();
            
            return role;
        }

    }
}
