using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;

namespace WIM.Core.Entity
{
    [Table("Employee_MT")]
    public class Employee_MT : BaseEntity
    {
        [Key]
        public string EmID { get; set; }
        public string Area { get; set; }
        public int DepIDSys { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        public Nullable<System.DateTime> HiredDate { get; set; }
        public Nullable<int> PositionIDSys { get; set; }
        public int PersonIDSys { get; set; }
        public virtual Person_MT Person_MT { get; set; }
    }
}
