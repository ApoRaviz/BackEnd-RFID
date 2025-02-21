﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Entity.UserManagement
{
    [Table("UserRoles")]
    public class UserRoles : BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public string UserID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string RoleID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}
