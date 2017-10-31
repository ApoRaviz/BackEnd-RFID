using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Data;
using WIM.Core.Entity;

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
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }

        public virtual ICollection<LeaveDetail> LeaveDetails { get; set; }

    }

    public class LeaveDto
    {
        [Key]        
        public int LeaveIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public string StatusTitle { get; set; }
        public Decimal Duration { get; set; }
        public string Comment { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }

        public virtual ICollection<LeaveDetailDto> LeaveDetails { get; set; }
    }

    public class LeaveDetailDto
    {
        [Key]
        public int LeaveDetailIDSys { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int LeaveIDSys { get; set; }

        public virtual Leave Leave { get; set; }

    }
}
