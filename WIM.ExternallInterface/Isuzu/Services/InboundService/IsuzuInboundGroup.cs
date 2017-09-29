using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface.Isuzu.Services.InboundService
{
    public class IsuzuInboundGroup
    {
        public IsuzuInboundGroup(string invNo,int qty,bool isExport)
        {
            this.InvNo = invNo;
            this.Qty = qty;
            this.IsExport = isExport;
        }
        public string InvNo { get; set; }
        public int Qty { get; set; }
        public string UpdateBy { get; set; }
        public bool IsExport { get; set; }


    }
}
