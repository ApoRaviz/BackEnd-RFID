using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Security.Entity.RoleAndPermission
{
    [Table("Permissions")]
    public class Permission
    {
        public Permission()
        {
            this.Roles = new HashSet<ApplicationRole>();
            this.RolePermissions = new HashSet<RolePermission>();
        }

        [Key]
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public Nullable<int> MenuIDSys { get; set; }
        public Nullable<int> ProjectIDSys { get; set; }
        public string Method { get; set; }
        public string ApiIDSys { get; set; }

        public virtual ICollection<ApplicationRole> Roles { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual MenuProjectMapping MenuProjectMapping { get; set; }
        public virtual Api_MT Api_MT { get; set; }
        public ApiMenuMapping Api { get; set; }
    }
    
}
