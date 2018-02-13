using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Http
{
    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string action)
        {
            this.Action = action;
        }

        public string Action { get; set; }
    }
}
