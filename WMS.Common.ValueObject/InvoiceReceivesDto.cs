

namespace WMS.Common.ValueObject
{
    public class InvoiceReceivesDto
    {

        public int InvIDSys { get; set; }
        public string InvNO { get; set; }
        public string Remark { get; set; }
        public int Qty { get; set; }
        public bool IsUnknownQty { get; set; }
        public int ReceiveIDSys { get; set; }
        public string ReceiveNO { get; set; }

    }
}
