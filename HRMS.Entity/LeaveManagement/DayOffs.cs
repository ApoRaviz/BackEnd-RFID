using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Entity.LeaveManagement
{
    [Table("DayOffs")]
    public class DayOff
    {
        [Key]
        public int DayOffIDSys { get; set; }
        public System.DateTime DateDayOff { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string Title { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }
    }
}
