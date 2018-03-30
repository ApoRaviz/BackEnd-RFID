using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class AdjustWeight
    {
        public String ISZJOrder { get; set; }
        public decimal Weight { get; set; }
        public int IsRepeat { get; set; }
    }
}
