using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.ValueObject.LeaveManagement
{
    public class LeaveDto
    {
        public LeaveDto()
        {
            LeaveDetails = new HashSet<LeaveDetailDto>();
        }

        [Key]
        public int LeaveIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public decimal Duration { get; set; }
        public string Comment { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }
        public virtual ICollection<LeaveDetailDto> LeaveDetails { get; set; }

        [NotMapped]
        public string StatusTitle { get; set; }
    }
}
