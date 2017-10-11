using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class ParameterSearch
    {
        public List<string> Columns { get; set; }
        public List<string> Keywords { get; set; }
        public string SpeacialQuery { get; set; }
    }
}
