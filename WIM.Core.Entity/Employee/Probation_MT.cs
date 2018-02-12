using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Employee
{
    [Table("Probation_MT")]
    public class Probation_MT : BaseEntity
    {
        [Key]
        public int ProbationIDSys { get; set; }
        public int PassStatus { get; set; }
        public bool IsRaise { get; set; }
        public bool IsInsurance { get; set; }
        public int FundStatus { get; set; }
    }
}
