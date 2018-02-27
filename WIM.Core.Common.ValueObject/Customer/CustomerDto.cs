using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Common.ValueObject
{
    public class CustomerDto 
    {
        public CustomerDto()
        {
            this.Project_MT = new HashSet<ProjectDto>();
        }

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
        public byte Active { get; set; }

        public virtual ICollection<ProjectDto> Project_MT { get; set; }

    }
}
