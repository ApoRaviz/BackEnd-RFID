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
        public string InvoiceNo { get; set; }
        public string Mno { get; set; }
        public string Mbl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }

        public bool? IsNoKeyword { get; set; }
    }
    
}
