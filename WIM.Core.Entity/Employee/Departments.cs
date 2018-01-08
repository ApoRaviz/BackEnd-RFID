using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Employee
{
    [Table("Departments")]
    public class Departments : BaseEntity
    {
        [Key]
        public int DepIDSys { get; set; }
        public string DepID { get; set; }
        public string Acronym { get; set; }
        public string DepName { get; set; }
        public Nullable<int> ParentIDSys { get; set; }
    }
}
