using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Entity.LeaveManagement
{
    [Table("LeaveHistory")]
    public class LeaveHistory
    {
        [Key]
        public int LeaveHistoryIDSys { get; set; }
        public int Status { get; set; }
        public string EmID { get; set; }
        public string Cause { get; set; }
        public decimal Duration { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string Comment { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }

    }
}
