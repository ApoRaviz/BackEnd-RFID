using Fuji.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiPickingGroup
    {
        public FujiPickingGroup(string orderNo,int qty,List<ImportSerialDetail> serialDetail)
        {
            this.OrderNo = orderNo;
            this.Qty = qty;
            this.SerialDetail = serialDetail;
        }
        public string OrderNo { get; set; }
        public int Qty { get; set; }
        public List<ImportSerialDetail> SerialDetail { get; set; }
    }
}
