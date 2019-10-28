using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuSearchModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Status { get; set; }
    }
}
