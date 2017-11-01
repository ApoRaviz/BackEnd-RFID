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
        public string CusName { get; set; }
        public string TaxID { get; set; }
        public string CompName { get; set; }
        public string AddressBill { get; set; }
        public string SubCity { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string TelOffice { get; set; }
        public string TelExt { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public bool IsActive { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public string UserUpdate { get; set; }

        public virtual ICollection<Project_MT> Project_MT { get; set; }

        //public virtual ICollection<UserCustomerMapping> UserCustomerMappings { get; set; }
    }
}
