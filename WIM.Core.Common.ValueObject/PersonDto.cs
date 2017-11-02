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
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public User User { get; set; }
        public Employee_MT Employee { get; set; }
    }
}
