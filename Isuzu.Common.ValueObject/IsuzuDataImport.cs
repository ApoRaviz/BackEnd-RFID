using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuDataImport
    {
        public IsuzuDataImport()
        {

        }
        public bool isDuplicated { get; set; }
        public List<InboundItems> listItem { get; set; }
    }
}
