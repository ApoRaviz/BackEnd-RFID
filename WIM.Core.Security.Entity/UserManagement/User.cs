using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Security.Entity.UserManagement
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
        }

        [Key]
        public string UserID { get; set; }

        public string Name { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
