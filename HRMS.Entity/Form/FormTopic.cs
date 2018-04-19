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
    [Table("FormTopics")]

    public class FormTopic: BaseEntity
    {
        [Key]
        public int FormTopicIDSys { get; set; }
        public string FormTopicName { get; set; }
        public string FormTopicNameEn { get; set; }
   


    }
}
