using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common
{
    public class HandheldErrorLog
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
    }
}
