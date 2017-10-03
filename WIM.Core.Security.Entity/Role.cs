using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WIM.Core.Security.Entity
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            this.Permissions = new HashSet<Permission>();
        }

        [Key]
        public string RoleID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}