using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.Receiving
{
    [Table("Receives")]
    public class Receive : BaseEntity
    {
        [Key]
        public int ReceiveIDSys { get; set; }
        public string ReceiveNO { get; set; }
        public string InvoiceNO { get; set; }
        public string PONO { get; set; }
        public int? SupplierIDSys { get; set; }
        public string Remark { get; set; }
        public int? ReceivingType { get; set; }
        public int? StatusIDSys { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
