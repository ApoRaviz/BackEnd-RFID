using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Data;
using WIM.Core.Entity;

namespace HRMS.Repository.Entity.LeaveRequest
{
    [Table("LeaveDetails")]
    public class LeaveDetail : BaseEntity
    {
        [Key]
        public int LeaveDetailIDSys { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int LeaveIDSys { get; set; }

        public virtual Leave Leave { get; set; }

}
}
