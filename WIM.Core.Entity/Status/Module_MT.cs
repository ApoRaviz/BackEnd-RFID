using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Status
{
    [Table("Module_MT")]
    public class Module_MT : BaseEntity
    {
        [Key]
        public int ModuleIDSys { get; set; }
        public string Acronym { get; set; }
        public string ModuleName { get; set; }
    }
}
