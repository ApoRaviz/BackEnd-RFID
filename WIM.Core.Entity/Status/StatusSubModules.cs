using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Status
{
    [Table("StatusSubModules")]
    public class StatusSubModules : BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public int StatusIDSys { get; set; }
        [Key]
        [Column(Order = 2)]
        public int SubModuleIDSys { get; set; }

        public virtual SubModules SubModule { get; set; }
        public virtual Status_MT Status_MT { get; set; }
    }
}
