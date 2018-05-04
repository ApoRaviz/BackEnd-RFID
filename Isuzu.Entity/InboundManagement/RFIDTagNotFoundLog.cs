using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace Isuzu.Entity.InboundManagement
{
    [Table("RFIDTagNotFoundLogs")]
    public class RFIDTagNotFoundLog : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public string IDRef { get; set; }

        public string FunctionName { get; set; }
    }
}
