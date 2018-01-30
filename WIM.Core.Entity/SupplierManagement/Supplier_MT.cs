
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Entity.SupplierManagement
{
    [Table("Supplier_MT")]
    public class Supplier_MT : BaseEntity
    {
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

        [ForeignKey("ProjectIDSys")]
        public virtual Project_MT Project_MT { get; set; }

        
    }
}
