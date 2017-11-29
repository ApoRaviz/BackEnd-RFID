using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Entity.ProjectManagement
{
    [Table("Module_MT")]
    public class Module_MT : BaseEntity
    {
        public Module_MT()
        {
            this.Project_MT = new HashSet<Project_MT>();
        }

        [Key]
        public int ModuleIDSys { get; set; }
        public string Acronym { get; set; }
        public string ModuleName { get; set; }
        public string FrontEndPath { get; set; }

        public virtual ICollection<Project_MT> Project_MT { get; set; }
    }
}
