using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Entity.Status
{
    [Table("SubModules")]
    public class SubModules : BaseEntity
    {
        [Key]
        public int SubModuleIDSys { get; set; }
        public Nullable<int> ModuleIDSys { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Acronym { get; set; }
        public string SubModuleName { get; set; }

        public Module_MT Module_MT { get; set; }

    }
}
