using HRMS.Entity.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace HRMS.Entity.Evaluate
{

    [Table("Evaluated")]

    public class Evaluated : BaseEntity
    {
        [Key]
        public int EvaluatedIDSys { get; set; }
        public int? FormTopicIDSys { get; set; }
        public string EmID { get; set; }
        public int? ResultProbationIDSys { get; set; } 
        public string EvaluateBy { get; set; }
        public DateTime? EvaluateDate { get; set; }
        public string ApproveBy { get; set; }
        public string HR { get; set; }
        public string Value { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Comment { get; set; }








    }
}
