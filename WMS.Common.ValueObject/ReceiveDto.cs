using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class ReceiveDto
    {
        public int ReceiveIDSys { get; set; }
        public string ReceiveNO { get; set; }
        public string InvoiceNO { get; set; }
        public string FileRefID { get; set; }
        public string PONO { get; set; }
        public int? SupplierIDSys { get; set; }
        public string Remark { get; set; }
        public int? ReceivingType { get; set; }
        public int? StatusIDSys { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string CompName { get; set; }
        public string FileName { get; set; }
        public virtual List<InventoryTransactionDto> InventoryTransactions { get; set; }
    }
}
