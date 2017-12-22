using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.LabelManagement
{
    public class Label
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Label(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
