using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Entity.RoleAndPermission
{
    [Table("Roles")]
    public class Role : BaseEntity
    {

        [Key]
        public string RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSysAdmin { get; set; }
        public int ProjectIDSys { get; set; }

        [ForeignKey("ProjectIDSys")]
        public virtual Project_MT Project_MT { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
