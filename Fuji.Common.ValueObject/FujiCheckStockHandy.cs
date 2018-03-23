using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiCheckStockHandy
    {
        public string Location { get; set; }
        public List<string> RFIDTags { get; set; }

    }
}
