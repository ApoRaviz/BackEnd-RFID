using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Validation
{
    public class ValidationDbSchema
    {
        public ValidationDbSchema()
        {
            Fn = "";
            Fs = new List<ValidationField>();
        }

        public string Fn { get; set; }
        public List<ValidationField> Fs { get; set; }

        public class ValidationField
        {
            public ValidationField(string key,string value)
            {
                K = key;
                V = value;
            }
            public string K { get; set; }
            public string V { get; set; }
        }
    }

}
