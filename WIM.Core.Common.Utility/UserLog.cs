using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility
{
    public partial class UserLog
    {
        public int LogID { get; set; }
        public string Machine { get; set; }
        public string RequestIpAddress { get; set; }
        public string RequestUri { get; set; }
        public string RequestContentType { get; set; }
        public string RequestContentBody { get; set; }
        public string RequestMethod { get; set; }
        public Nullable<System.DateTime> RequestTimestamp { get; set; }
        public string ResponseContentType { get; set; }
        public string ResponseContentBody { get; set; }
        public string ResponseStatusCode { get; set; }
        public Nullable<System.DateTime> ResponseTimestamp { get; set; }
    }
}
