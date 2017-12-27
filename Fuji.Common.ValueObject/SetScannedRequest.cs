using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class SetScannedRequest
    {
        public string HeadID { get; set; }
        public string ItemCode { get; set; }
        public List<string> ItemGroups { get; set; }
        public string UserUpdate { get; set; }
    }
}
