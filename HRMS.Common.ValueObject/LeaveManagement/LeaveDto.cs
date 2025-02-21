﻿using System;
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
            LeaveTypes = new HashSet<LeaveTypeDto>();
        }

        [Key]
        public int LeaveIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public decimal Duration { get; set; }
        public string Comment { get; set; }
        public string LeaveTypeName { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }
        public IEnumerable<LeaveDetailDto> LeaveDetails { get; set; }
        public virtual IEnumerable<LeaveTypeDto> LeaveTypes { get; set; }

        [NotMapped]
        public string StatusTitle { get; set; }
    }
}
