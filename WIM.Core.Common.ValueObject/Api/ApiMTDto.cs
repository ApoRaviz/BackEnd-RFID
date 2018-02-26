using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIM.Core.Common.ValueObject
{
    public class ApiMTDto
    {
        public string ApiIDSys { get; set; }
        public string Api { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
    }
}
