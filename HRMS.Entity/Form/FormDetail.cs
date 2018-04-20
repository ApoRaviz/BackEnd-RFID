using HRMS.Entity.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace HRMS.Entity.Form
{
    [Table("FormDetails")]

    public class FormDetail : BaseEntity
    {
        [Key]
        public int FormDetailIDSys { get; set; }
        public int FormQIDSys { get; set; }
        public int? EvaluatedIDSys { get; set; }
        public string EmID{ get; set; }
        public string FormAns { get; set; }







    }
}
