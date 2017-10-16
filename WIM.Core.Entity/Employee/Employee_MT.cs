using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;

namespace WIM.Core.Entity.Employee
{
    [Table("Employee_MT")]
    public class Employee_MT : Person_MT
    {
        [Key]
        public string EmID { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
        public string Dept { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }

    }
}
