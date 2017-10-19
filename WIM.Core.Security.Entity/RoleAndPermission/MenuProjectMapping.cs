using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WIM.Core.Security.Entity.RoleAndPermission
{
    [Table("MenuProjectMapping")]
    public class MenuProjectMapping
    {        
        public MenuProjectMapping()
        {
            // #JobComment
            this.Permissions = new HashSet<Permission>();
        }

        [Key]
        [Column(Order = 1)]
        public int MenuIDSys { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProjectIDSys { get; set; }
        public byte Sort { get; set; }
        public string MenuName { get; set; }
        public byte[] MenuPic { get; set; }
        public int MenuIDSysParent { get; set; }

        public virtual Menu_MT Menu_MT { get; set; }
        //public virtual Project_MT Project_MT { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
