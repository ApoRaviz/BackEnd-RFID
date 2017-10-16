using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuDataInboundGroupItems
    {
        public IsuzuDataInboundGroupItems(int totalRecord, IEnumerable<InboundItemsHead> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<InboundItemsHead> Items { get; set; }
    }
}
