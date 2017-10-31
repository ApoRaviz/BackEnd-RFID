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
    [Table("LeaveTypes")]
    public class LeaveType : BaseEntity
    {
        [Key]
        public int LeaveTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
    }
}
