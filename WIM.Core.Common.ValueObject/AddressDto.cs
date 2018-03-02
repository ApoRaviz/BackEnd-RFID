using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WIM.Core.Common.ValueObject
{
    public class AddressDto 
    {
        public string district { get; set; }
        public string amphoe { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
    }
}
