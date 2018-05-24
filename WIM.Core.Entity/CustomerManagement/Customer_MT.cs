using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Entity.CustomerManagement
{
    [Table("Customer_MT")]
    public class Customer_MT : BaseEntity
    {        
        public Customer_MT()
        {
            this.Project_MT = new HashSet<Project_MT>();
            // #JobComment
            //this.UserCustomerMappings = new HashSet<UserCustomerMapping>();
        }

        [Key]
        public int CusIDSys { get; set; }
        public string CusID { get; set; }
        public Nullable<int> CusParentIDSys { get; set; }
        public string CusName { get; set; }
        public string CusNameEn { get; set; }
        public string TaxID { get; set; }
        public string CompName { get; set; }
        public string CompNameEn { get; set; }
        public string Address { get; set; }
        public string AddressEn { get; set; }
        public string AddressBill { get; set; }
        public string AddressBillEn { get; set; }
        public string SubCity { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string CountryCode { get; set; }
        public string ContName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string TelOffice { get; set; }
        public string TelExt { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }

        public virtual ICollection<Project_MT> Project_MT { get; set; }
        [ForeignKey("CusParentIDSys")]
        public virtual Customer_MT CusParent { get; set; }

        //public virtual ICollection<UserCustomerMapping> UserCustomerMappings { get; set; }
    }
}
