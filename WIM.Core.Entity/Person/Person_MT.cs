using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Entity.Person
{
    [Table("Person_MT")]
    public class Person_MT : BaseEntity
    {      
        [Key] 
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

    }
   
}
