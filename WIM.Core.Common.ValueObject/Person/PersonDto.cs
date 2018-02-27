using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Common.ValueObject
{
    public class PersonDto  
    {
        public int PersonIDSys { get; set; }
        public string PersonID { get; set; }
        public Nullable<int> PrefixIDSys { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameEn { get; set; }
        public string SurnameEn { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string IdentificationNo { get; set; }
        public string PassportNo { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public UserDto User { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
