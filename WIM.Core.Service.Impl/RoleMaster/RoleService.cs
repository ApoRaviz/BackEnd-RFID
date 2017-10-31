﻿using AutoMapper;
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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Service.Impl
{
    public class RoleService : IRoleService
    {
        public RoleService()
        {
        }        

        public IEnumerable<Role> GetRoles()
        {
            IEnumerable<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                role = repo.Get();
            }
            return role;
        }

        public IEnumerable<Role> GetRoles(int projectIDSys)
        {
            IEnumerable<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                role = repo.GetMany(c => c.ProjectIDSys == projectIDSys);
            }
            return role.ToList();
        }

        public Role GetRoleByLocIDSys(string id)
        {
            Role Role ;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                Role = repo.GetByID(id);
            }
                return Role;            
        }

        public string GetRoleByUserAndProject(string UserID, int ProjectIDSys)
        {
            string res;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                res = repo.GetByUserAndProject(UserID, ProjectIDSys);
            }
            return res;
        }

        public string CreateRole(Role role , string username)
        {
            using (var scope = new TransactionScope())
            {
                role.RoleID = Guid.NewGuid().ToString();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRoleRepository repo = new RoleRepository(Db);
                        repo.Insert(role , username);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }catch(DbUnexpectedValidationException e)
                {
                    Console.Write(e);
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
                return role.RoleID;
            }
        }

        public bool UpdateRole( Role role , string username)
        {           
            using (var scope = new TransactionScope())
            {     
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IRoleRepository repo = new RoleRepository(Db);
                        repo.Update(role , username);
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

        public bool DeleteRole(string id)
        {
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                IRepository<UserRoles> repouser = new Repository<UserRoles>(Db);
                IRepository<RolePermission> repopermission = new Repository<RolePermission>(Db);
                List<UserRoles> users = new List<UserRoles>();
            List<RolePermission> permissions = new List<RolePermission>();
            if (id != "")
            {
                users = repouser.GetMany(c => c.RoleID == id).ToList();
                permissions = repopermission.GetMany(c => c.RoleID == id).ToList();
            }

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < users.Count; i++)
                        {
                            repouser.Delete(users[i]);
                        }
                        for (int i = 0; i < permissions.Count; i++)
                        {
                            repo.Delete(permissions[i]);
                        }
                        var existedRole = repo.GetByID(id);
                        repo.Delete(existedRole);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                        throw ex;
                    }
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

        public List<RolePermissionDto> GetRoleByPermissionID(string id)
        {
            List<RolePermissionDto> rolelist;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<RolePermission> repo = new Repository<RolePermission>(Db);
                rolelist = repo.GetMany(c => c.PermissionID == id)
                .Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID

                }).ToList();
            }
            return rolelist;
        }

        public List<RolePermissionDto> GetRoleNotPermissionID(string id)
        {
            List<RolePermissionDto> rolelist;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<Role> repo = new Repository<Role>(Db);
                var RoleForPermissionQuery = repo.GetMany(c => !(Db.RolePermission.Where(a => a.PermissionID == id).Select(b => b.RoleID).Contains(c.RoleID)));
                rolelist = RoleForPermissionQuery.Select(b => new RolePermissionDto()
                {
                    RoleID = b.RoleID,
                    Name = b.Name,
                    Description = b.Description,
                    IsSysAdmin = b.IsSysAdmin

                }).ToList();
            }
            return rolelist;
        }

        public Role GetRoleByName(string name)
        {
            return null;
        }

        public List<Role> GetRoleByProjectUser(int id , string userid)
        {
            List<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRepository<Role> repo = new Repository<Role>(Db);
                string[] include = { "Project_MT" };
                role = repo.GetWithInclude((x => x.ProjectIDSys == id &&
                !(Db.UserRoles.Include(p => p.Role).Where(c => c.UserID == userid).Any(p => p.Role.ProjectIDSys == x.ProjectIDSys))), include).ToList();
            }
                return role.ToList();
        }

        public List<Role> GetRoleByUserID(string id)
        {
            List<Role> role;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IRoleRepository repo = new RoleRepository(Db);
                role = repo.GetByUser(id);
            }
            return role;
        }
    }
}
