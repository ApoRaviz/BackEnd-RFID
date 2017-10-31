using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Entity.LeaveManagement
{
    [Table("LeaveDetails")]
    public class LeaveDetail
    {
        [Key]
        public int LeaveDetailIDSys { get; set; }
        public int LeaveIDSys { get; set; }
        public System.DateTime LeaveDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }
    }
}
