using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Entity.LeaveManagement
{
    [Table("LeaveTypes")]
    public class LeaveType
    {
        [Key]
        public int LeaveTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }
    }
}
