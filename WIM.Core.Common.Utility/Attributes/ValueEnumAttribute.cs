using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Attributes
{
    public class ValueEnumAttribute : Attribute
    {
        public ValueEnumAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
