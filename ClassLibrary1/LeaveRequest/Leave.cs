using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Data;

namespace HRMS.Repository.Entity.LeaveRequest
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
        [NotMapped]
        public string StatusTitle { get; set; }
        public Decimal Duration { get; set; }
        public string Comment { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string EmID { get; set; }

        public virtual ICollection<LeaveDetail> LeaveDetails { get; set; }

    }

    public class LeaveDto
    {
        public string Comment { get; set; }
        public string StatusTitle { get; set; }

        public virtual ICollection<LeaveDetail> LeaveDetails { get; set; }
    }
}
