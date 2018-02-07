using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.FileManagement;
using WIM.Core.Entity.Status;

namespace WIM.Core.Entity.Employee
{
    [Table("HistoryWarnings")]
    public class HistoryWarning : BaseEntity
    {
        [Key]
        public int WarnIDSys {get;set;}
        public string EmID { get; set; }
        public int StatusIDSys { get; set; }
        public Nullable<DateTime> WarningDate { get; set; }
        public string Description { get; set; }
        public string FileUID { get; set; }

        [ForeignKey("EmID")]
        public virtual Employee_MT Employee_MT { get; set; }
        [ForeignKey("StatusIDSys")]
        public virtual Status_MT Status_MT { get; set; }
        [NotMapped]
        public virtual File_MT File_MT { get; set; }
    }
}
