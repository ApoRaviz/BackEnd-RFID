using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class ReceiveRequest
    {
        public string HeadID { get; set; }
        public string ItemCode { get; set; }
        public string LocationID { get; set; }
        public List<string> ItemGroups { get; set; }
    }
}
