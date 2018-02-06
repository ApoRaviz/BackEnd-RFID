using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Status;

namespace WIM.Core.Entity.Employee
{
    [Table("Resigns")]
    public class Resign : BaseEntity
    {
        [Key]
        public string EmID { get; set; }
        public int Reason { get; set; }
        public Nullable<DateTime> ResignDate { get; set; }
        public string Description { get; set; }
        public string FileUID { get; set; }
        [ForeignKey("EmID")]
        public virtual Employee_MT Employee_MT { get; set; }
        [ForeignKey("Reason")]
        public virtual Status_MT Status_MT { get; set; }

    }
}
