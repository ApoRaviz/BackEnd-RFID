using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class AutocompleteItemDto
    {
        public int ItemIDSys { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }
}
