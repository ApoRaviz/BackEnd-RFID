using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class PickingRequest
    {
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public string OrderNumber { get; set; }
    }
}
