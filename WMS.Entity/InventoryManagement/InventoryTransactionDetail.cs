using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.InventoryManagement
{
    [Table("InventoryTransactionDetails")]
    public class InventoryTransactionDetail : BaseEntity
    {
        [Key]
        public int InvenTranDetailIDSys { get; set; }
        public int InvenTranIDSys { get; set; }
        public string SerialNumber { get; set; }
    }
}
