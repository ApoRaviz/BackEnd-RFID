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
using System.Globalization;
using System.Timers;
using System.Data.SqlTypes;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class UserRoleService : IUserRoleService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<UserRole> repo;
        

        public UserRoleService()
        {
            repo = new GenericRepository<UserRole>(db);
        }        

        public IEnumerable<UserRole> GetUserRoles()
        {           
            return repo.GetAll();
        }

        public UserRole GetUserRoleByLocIDSys(int id)
        {           
            UserRole UserRole = db.UserRoles.Find(id);                                  
            return UserRole;            
        }                      

        public string CreateUserRole(UserRole UserRole)
        {
            using (var scope = new TransactionScope())
            {
               
                //repo.Insert(UserRole);
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
                return UserRole.UserID;
            }
        }

        public bool UpdateUserRole(int id, UserRole UserRole)
        {           
            using (var scope = new TransactionScope())
            {
                var existedUserRole = repo.GetByID(id);
              
                repo.Update(existedUserRole);
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

        public bool DeleteUserRole(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedUserRole = repo.GetByID(id);
                repo.Update(existedUserRole);
                try
                {
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

        public List<RoleUserDto> GetRoleByUserID(string userid)
        {
            List<RoleUserDto> RoleUser = (from o in db.Roles
                                          join i in db.UserRoles on o.RoleID equals i.RoleID
                                          where i.UserID == userid
                                          select o).Include("Project_MT").Select(b => new RoleUserDto()
                                          {
                                              RoleID = b.RoleID,
                                              Name = b.Name,
                                              Description = b.Description,
                                              IsSysAdmin = b.IsSysAdmin,
                                              Project_MT = b.Project_MT
                                          }).ToList();
            return RoleUser;
        }
        public List<UserRoleDto> GetUserByRoleID(string roleid)
        {
            var RoleForPermissionQuery = from row in db.UserRoles
                                         where row.RoleID == roleid
                                         select row;
            List<UserRoleDto> userlist = RoleForPermissionQuery.Include(a => a.User).Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.User.Name,
                Email = b.User.Email,
                PhoneNumber = b.User.PhoneNumber,
                PasswordHash = b.User.PasswordHash,
                Surname = b.User.Surname,

            }).ToList();
            return userlist;
        }

        public UserRoleDto GetUserRoleByUserID(string id)
        {
            UserRoleDto UserRole = db.Users.Where(a => a.UserID == id).Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                Name = b.Name,
                Email = b.Email,
                PhoneNumber = b.PhoneNumber,
                PasswordHash = b.PasswordHash,
                Surname = b.Surname
            }).SingleOrDefault();
            return UserRole;
        }
        public RoleUserDto GetRoleUserByRoleID(string id)
        {
            RoleUserDto RoleUser = db.UserRoles.Include(a => a.Role).Select(b => new RoleUserDto()
            {
                RoleID = b.RoleID,
                Name = b.Role.Name,
                Description = b.Role.Description,
                IsSysAdmin = b.Role.IsSysAdmin
            }).SingleOrDefault();
            return RoleUser;
        }

        public string CreateUserRoles(string userid , string roleid)
        {
            using (var scope = new TransactionScope())
            {
                UserRole data = new UserRole();
                data.RoleID = roleid;
                data.UserID = userid;
                repo.Insert(data);
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
                return data.UserID;
            }
        }

        public string CreateRoleUsers(string userid, string roleid)
        {
            using (var scope = new TransactionScope())
            {
                UserRole data = new UserRole();
                data.RoleID = roleid;
                data.UserID = userid;
                repo.Insert(data);
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
                return data.UserID;
            }
        }

        public bool DeleteRolePermission(string UserId, string RoleId)
        {
            var RoleForPermissionQuery = from row in db.UserRoles
                                         where row.UserID == UserId && row.RoleID == RoleId
                                         select row;
            if (RoleForPermissionQuery != null)
            {
                using (var scope = new TransactionScope())
                {
                    /*RolePermission temp = RoleForPermissionQuery.Select(b => new RolePermission()
                    {
                        RoleID = b.RoleID,
                        PermissionID = b.PermissionID
                    }).SingleOrDefault();*/
                    UserRole temp = new UserRole();
                    temp = RoleForPermissionQuery.SingleOrDefault();
                    if (temp != null)
                    {
                        repo.Delete(temp);
                        db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                }
            }
            return true;
        }

    }
}
