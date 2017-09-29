using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface.ItemImport
{
    public class ConfirmPickingRequest
    {
        public string OrderNumber { get; set; }
        public List<string> ItemGroups { get; set; }
    }

    public class RegisterRFIDRequest
    {
        public string BoxNumber { get; set; }
        public List<string> SerialNumbers { get; set; }
        public string RFIDTag { get; set; }
    }
}
