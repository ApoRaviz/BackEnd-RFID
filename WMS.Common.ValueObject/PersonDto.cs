using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.UserManagement;
using WMS.Repository;

namespace WMS.Common
{
    public class PersonDto : BaseEntityDto
    {
        public int PersonIDSys { get; set; }
        public string PersonID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public System.DateTime? BirthDate { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UserUpdate { get; set; }

        public User User { get; set; }
        public Employee_MT Employee { get; set; }
    }
}
