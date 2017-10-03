using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Entity.ItemManagement
{
    [Table("ImportSerialHead")]
    public class ImportSerialHead
    {
        public ImportSerialHead()
        {
            this.ImportSerialDetail = new HashSet<ImportSerialDetail>();
        }

        [Key]
        public string HeadID { get; set; }
        public string ItemCode { get; set; }
        public string WHID { get; set; }
        public string LotNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public System.DateTime ReceivingDate { get; set; }
        public string DeliveryNote { get; set; }
        public string Remark { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string SerialFormat1 { get; set; }
        public string SerialFormat2 { get; set; }
        public string SerialName1 { get; set; }
        public string SerialName2 { get; set; }
        public byte IsRFID { get; set; }
        public int Qty { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public string Spare6 { get; set; }
        public string Spare7 { get; set; }
        public string Spare8 { get; set; }
        public string Spare9 { get; set; }
        public string Spare10 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual ICollection<ImportSerialDetail> ImportSerialDetail { get; set; }
    }
}
