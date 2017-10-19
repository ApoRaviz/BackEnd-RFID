using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuDataInboundItems
    {
        public IsuzuDataInboundItems(int totalRecord, IEnumerable<InboundItems> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<InboundItems> Items { get; set; }
    }
}
