using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.View
{
    [Table("VPersons")]
    public class VPersons
    {
        [Key]
        public int PersonIDSys { get; set; }
        public string EmID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameEn { get; set; }
        public string SurnameEn { get; set; }
        public string CompNameEn { get; set; }
        public string CusNameEn { get; set; }
        public string DepNameEn { get; set; }
        public string PositionNameEn { get; set; }
        public string Area { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string TelOffice { get; set; }
        public string TelExt { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

    }
}
