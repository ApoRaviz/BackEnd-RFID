using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.ValueObject.LeaveManagement
{
    public class LeaveTypeDto
    {
        public LeaveTypeDto()
        {

        }

        [Key]
        public int LeaveTypeIDSys { get; set; }
        public string LeaveTypeName { get; set; }


    }
}
