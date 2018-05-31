using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.Receiving
{
    [Table("InvoiceReceives")]
    public class InvoiceReceive : BaseEntity
    {
        [Key]
        public int InvIDSys { get; set; }
        public string InvNO { get; set; }
        public string Remark { get; set; }
        public int Qty { get; set; }
        public bool IsUnknownQty { get; set; }
        public int ReceiveIDSys { get; set; }
    }
}
