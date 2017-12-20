using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class HashidsAttrbute : Attribute
    {        
        public HashidsAttrbute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class HashidsHexsAttrbute : Attribute
    {
        public HashidsHexsAttrbute()
        {
            
        }
    }
}
