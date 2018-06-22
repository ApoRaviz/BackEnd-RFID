using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.SupplierManagement;
using WMS.Entity.InventoryManagement;

namespace WMS.Entity.Receiving
{
    [Table("Receives")]
    public class Receive : BaseEntity
    {
        private int _receiveIDSys;
        [Key]
        public int ReceiveIDSys
        {
            get
            {
                return _receiveIDSys;
            }
            set
            {
                _receiveIDSys = value;
                //NotifyPropertyChanged();
            }
        }
  
        public string ReceiveNO { get; set; }
        public int ProjectIDSys { get; set; }
        public string InvoiceNO { get; set; }
        public string PONO { get; set; }
        public int? SupplierIDSys { get; set; }
        public string Remark { get; set; }
        public int? ReceivingType { get; set; }
        public int? StatusIDSys { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string FileRefID { get; set; }

        [NotMapped]
        public virtual IEnumerable<InventoryTransaction> InventoryTransactions { get; set; }
        [NotMapped]
        public virtual Supplier_MT Supplier_MT { get; set; }
    }
}
