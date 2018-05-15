using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Entity.Person
{
    [Table("Person_Email")]
    public class Person_Email : BaseEntity
    { 
        [Key]
        public int EmailIDSys { get; set; }
        public int PersonIDSys { get; set; }
        public string Email { get; set; }
        public bool IsDefault { get; set; }
    }
   
}
