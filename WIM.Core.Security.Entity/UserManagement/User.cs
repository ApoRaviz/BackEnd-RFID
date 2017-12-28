using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.Person;
using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Security.Entity.UserManagement
{
    public class User : BaseEntity
    {
        public User()
        {
            this.UserRoles = new HashSet<UserRole>();
        }
        [Key]
        public string UserID { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public System.DateTime LastLogin { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsSysAdmin { get; set; }
        public string KeyAccess { get; set; }
        public Nullable<System.DateTime> KeyAccessDate { get; set; }
        public string TokenMobile { get; set; }

        public Nullable<int> KeyOTP { get; set; }
        public Nullable<System.DateTime> KeyOTPDate { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UserProjectMapping> UserProjectMappings { get; set; }

        public int PersonIDSys { get; set; }
    }
}
