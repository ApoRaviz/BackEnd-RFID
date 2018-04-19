using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIM.Core.Common.ValueObject
{
    public class ApiDesc
    {
        public string ID { get; set; }
        public string ControllerName { get; set; }
        public string RelativePath { get; set; }
        public string ApiPath { get; set; }
        public string Method { get; set; }

    }
}
