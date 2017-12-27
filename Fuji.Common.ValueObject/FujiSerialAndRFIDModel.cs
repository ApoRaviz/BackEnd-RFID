using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiSerialAndRFIDModel
    {
        public string SerialNumber { get; set; }
        public string RFIDTag { get; set; }
        public bool IsValid { get; set; }
    }
}
