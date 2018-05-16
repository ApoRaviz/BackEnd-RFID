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
    [Table("Person_MT")]
    public class Person_MT : BaseEntity
    {      
        [Key]
        public int PersonIDSys { get; set; }
        public Nullable<int> CusIDSys { get; set; }
        public Nullable<int> PrefixIDSys { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameEn { get; set; }
        public string SurnameEn { get; set; }
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

        [ForeignKey("CusIDSys")]
        public virtual Customer_MT Customer_MT { get; set; }
        public ICollection<Person_Email> Person_Email { get; set; }

    }
   
}
