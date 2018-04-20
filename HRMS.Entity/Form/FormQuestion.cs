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
    [Table("FormQuestions")] 

    public class FormQuestion : BaseEntity
    {
        [Key]
        public int FormQIDSys { get; set; }
        public int? FormTopicIDSys { get; set; }
        public string FormQ { get; set; }
        public string FormQEn { get; set; }




    }
}
