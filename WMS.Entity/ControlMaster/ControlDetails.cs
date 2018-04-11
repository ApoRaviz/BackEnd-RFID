using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ControlMaster
{
    public class ControlDetails
    {
        public List<ControlValue> ControlValue { get; set; }
        public List<string> ReceiveValue { get; set; }
        public InspectValue InspectValue { get; set; }
    }
}
