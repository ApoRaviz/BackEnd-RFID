using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.ValueObject
{
    public class EmployeeDto
    {
        public string EmID { get; set; }
        public int PersonIDSys { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
        public string Dept { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<int> EmTypeIDSys { get; set; }

        public PersonDto Person_MT { get; set; }
    }
}
