using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Employee
{
    [Table("Positions")]
    public class Positions : BaseEntity
    {
        [Key]
        public int PositionIDSys { get; set; }
        public string Acronym { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEn { get; set; }
        public string PositionDescription { get; set; }
        public Nullable<int> ParentIDSys { get; set; }
    }
}
