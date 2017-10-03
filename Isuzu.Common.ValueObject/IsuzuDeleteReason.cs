using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuDeleteReason
    {
        public string InvNo { get; set; }
        public string ISZJOrder { get; set; }
        public string Reason { get; set; }
        public string Paths { get; set; }
        public string UserName { get; set; }
    }
}
