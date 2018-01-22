using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Module;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Entity.MenuManagement
{
    [Table("Api_MT")]
    public class Api_MT : BaseEntity
    {
        public Api_MT()
        {
            this.ApiMenuMappings = new HashSet<ApiMenuMapping>();
            // #JobComment
            this.Permissions = new HashSet<Permission>();
            IsActive = true;
        }

        [Key]
        public string ApiIDSys { get; set; }
        public string Api { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
        public int ModuleIDSys { get; set; }

        public virtual ICollection<ApiMenuMapping> ApiMenuMappings { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        [ForeignKey("ModuleIDSys")]
        public virtual Module_MT Module_MT { get; set; }
    }
}
