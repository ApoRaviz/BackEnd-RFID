using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Entity.LeaveManagement
{
    [Table("LeaveQuotas")]
    public class LeaveQuota
    {
        [Key]
        public int IDSys { get; set; }
        public int TypeIDSys { get; set; }
        public decimal WorkExperience { get; set; }
        public decimal QuotaDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }
    }
}
