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
    [Table("DayOffs")]
    public class DayOff : BaseEntity
    {

        public DayOff()
        {

        }

        [Key]
        public int DayOffIDSys { get; set; }
        public System.DateTime DateDayOff { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
    }
}
