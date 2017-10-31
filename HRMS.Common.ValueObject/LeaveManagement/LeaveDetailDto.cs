using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.ValueObject.LeaveManagement
{
    public class LeaveDetailDto
    {
        public LeaveDetailDto()
        {

        }

        [Key]
        public int LeaveDetailIDSys { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int LeaveIDSys { get; set; }
        public virtual LeaveDto Leave { get; set; }
    }
}
