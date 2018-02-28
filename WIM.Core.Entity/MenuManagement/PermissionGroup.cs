using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.MenuManagement
{
    [Table("PermissionGroup")]
    public class PermissionGroup : BaseEntity
    {
        [Key]
        public string GroupIDSys { get; set; }
        public int MenuIDSys { get; set; }
        public string GroupName { get; set; }

        [ForeignKey("MenuIDSys")]
        public virtual Menu_MT Menu_MT { get; set; }
        public virtual ICollection<PermissionGroupApi> PermissionGroupApi { get; set; }

        [NotMapped]
        public virtual bool IsUpdate { get; set; }
    }
}
