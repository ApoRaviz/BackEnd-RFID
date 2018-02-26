using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.MenuManagement
{
    [Table("PermissionGroupApi")]
    public class PermissionGroupApi : BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public string GroupIDSys { get; set; }
        [Key]
        [Column(Order = 1)]
        public string ApiIDSys { get; set; }
        public bool GET { get; set; }
        public bool POST { get; set; }
        public bool PUT { get; set; }
        public bool DEL { get; set; }
        public string Title { get; set; }

        [ForeignKey("GroupIDSys")]
        public virtual PermissionGroup PermissionGroup { get; set; }
        [ForeignKey("ApiIDSys")]
        public virtual Api_MT Api_MT { get; set; }

        [NotMapped]
        public virtual string Api { get; set; }
        [NotMapped]
        public virtual bool IsUpdate { get; set; }
    }
}
