using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Isuzu.Common.ValueObject
{
    public class InboundItemsHeadModel
    {
        public string InvNo { get; set; }
        public int? Qty { get; set; }
        public string Status { get; set; }
        public string StatusDate{ get; set; }
        public string StatusDetail { get; set; }
        public bool? IsExport { get; set; }
        public string Remark { get; set; }
    }

    public class InboundItemsStatusModel
    {
        //public string ID { get; set; }
        public string InvNo { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }
    }
}