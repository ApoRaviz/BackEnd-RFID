using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WIM.Core.Entity.Common
{
    [Table("UserLog")]
    public class UserLog : BaseEntity
    {
        [Key]
        public int LogID { get; set; }
        public string Machine { get; set; }
        public string RequestIpAddress { get; set; }
        public string RequestUri { get; set; }
        public string RequestUriFrondEnd { get; set; }
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
