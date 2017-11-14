using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.RoleAndPermission
{
    [Table("RolePermissions")]
    public class RolePermission : BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public string RoleID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string PermissionID { get; set; }

        //public virtual Permission Permission { get; set; }
        //public virtual Role Role { get; set; }
    }
}
