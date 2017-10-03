using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class ParameterSearch
    {
        public List<string> Columns { get; set; }
        public List<string> Keywords { get; set; }
    }
}
