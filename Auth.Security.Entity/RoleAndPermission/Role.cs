using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.Security.Entity.UserManagement;

namespace Auth.Security.Entity.RoleAndPermission
{
    //[Table("Roles")]
    public class Role
    {        
        public Role()
        {
            this.UserRoles = new HashSet<UserRole>();
            this.RolePermissions = new HashSet<RolePermission>();
        }

        [Key]
        public string RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSysAdmin { get; set; }
        public int ProjectIDSys { get; set; }

        //public virtual Project_MT Project_MT { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
