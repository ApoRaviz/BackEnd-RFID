using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace HRMS.Entity.LeaveManagement
{
    [Table("Leaves")]
    public class Leave : BaseEntity
    {
        public Leave()
        {
            LeaveDetails = new HashSet<LeaveDetail>();
        }

        [Key]
        public int LeaveIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public decimal Duration { get; set; }
        public string Comment { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }
        public virtual ICollection<LeaveDetail> LeaveDetails { get; set; }

        [NotMapped]
        public string StatusTitle { get; set; }
    }
}
