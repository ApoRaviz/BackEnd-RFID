using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class UserRoleRepository : Repository<UserRoles>,IUserRoleRepository
    {
        private CoreDbContext Db { get; set; }

        public UserRoleRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

        public IEnumerable<RoleUserDto> GetRoleByUserID(string userid)
        {
            var RoleUser = (from o in Db.Role
                                          join i in Db.UserRoles on o.RoleID equals i.RoleID
                                          where i.UserID == userid
                                          select o).Include("Project_MT").Select(b => new RoleUserDto()
                                          {
                                              RoleID = b.RoleID,
                                              Name = b.Name,
                                              Description = b.Description,
                                              IsSysAdmin = b.IsSysAdmin,
                                              Project_MT = b.Project_MT
                                          });
            return RoleUser;
        }
        

        public UserRoleDto GetUserRoleByUserID(string id)
        {
            UserRoleDto UserRole = Db.User.Where(a => a.UserID == id).Select(b => new UserRoleDto()
            {
                UserID = b.UserID,
                //Email = b.Email,
                //PhoneNumber = b.PhoneNumber.ToString(),
                PasswordHash = b.PasswordHash
            }).SingleOrDefault();
            return UserRole;
        }
        public RoleUserDto GetRoleUserByRoleID(string id)
        {
            RoleUserDto RoleUser = Db.UserRoles.Include(a => a.Role).Select(b => new RoleUserDto()
            {
                RoleID = b.RoleID,
                Name = b.Role.Name,
                Description = b.Role.Description,
                IsSysAdmin = b.Role.IsSysAdmin
            }).SingleOrDefault();
            return RoleUser;
        }

        
       
    }
}
