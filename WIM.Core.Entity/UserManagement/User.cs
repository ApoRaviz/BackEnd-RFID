using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;

namespace WIM.Core.Entity.UserManagement
{
//: Person_MT
    [Table("Users")]
    public class User : BaseEntity
    {
        public User()
        {
            this.UserRoles = new HashSet<UserRoles>();
        }
        [Key]
        public string UserID { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public System.DateTime LastLogin { get; set; }
        public string SecurityStamp { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsSysAdmin { get; set; }
        public string KeyAccess { get; set; }
        public int PersonIDSys { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> KeyAccessDate { get; set; }

        public string TokenMobile { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<int> KeyOTP { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> KeyOTPDate { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        [ForeignKey("PersonIDSys")]
        public virtual Person_MT Person_MT { get; set; }
        
    }
}
