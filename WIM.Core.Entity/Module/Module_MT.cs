using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.ProjectManagement;
//using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Entity.Module
{
    [Table("Module_MT")]
    public class Module_MT : BaseEntity
    {
        public Module_MT()
        {
            this.Project_MT = new HashSet<Project_MT>();
            this.Api_MT = new HashSet<Api_MT>();
        }

        [Key]
        public int ModuleIDSys { get; set; }
        public string Acronym { get; set; }
        public string ModuleName { get; set; }
        public string FrontEndPath { get; set; }

        public virtual ICollection<Project_MT> Project_MT { get; set; }
        public virtual ICollection<Api_MT> Api_MT { get; set; }
    }
}
