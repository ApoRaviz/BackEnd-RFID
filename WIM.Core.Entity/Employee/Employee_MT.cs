using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;

namespace WIM.Core.Entity.Employee : BaseEntity
{
    [Table("Employee_MT")]
    public class Employee_MT 
    {
        [Key]
        public string EmID { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
        public string Dept { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        //public byte Active { get; set; }
        public int PersonIDSys { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public string UserUpdate { get; set; }
        public Nullable<System.DateTime> HiredDate { get; set; }
        public Nullable<int> PositionIDSys { get; set; }

        public virtual Person_MT Person_MT { get; set; }
    }
}
