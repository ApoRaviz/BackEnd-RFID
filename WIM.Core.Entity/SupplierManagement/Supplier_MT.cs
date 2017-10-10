using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Entity.SupplierManagement
{
    [Table("Supplier_MT")]
    public class Supplier_MT
    {
        public Supplier_MT()
        {
            
        }

        [Key]
        public int SupIDSys { get; set; }
        public string SupID { get; set; }
        public int ProjectIDSys { get; set; }
        public string CompName { get; set; }
        public string Address { get; set; }
        public string SubCity { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string CountryCode { get; set; }
        public string ContName { get; set; }
        public string Email { get; set; }
        public string TelOffice { get; set; }
        public string TelExt { get; set; }
        public string Mobile { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual Project_MT Project_MT { get; set; }

        
    }
}
