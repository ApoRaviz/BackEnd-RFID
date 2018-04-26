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
    [Table("InventoryDetails")]
    public class InventoryDetail : BaseEntity
    {
        [Key]
        public int InvenDetailIDSys { get; set; }
        public int InvenIDSys { get; set; }
        public string SerialNumber { get; set; }
        public int ItemIDSys { get; set; }
        public int? StatusIDSys { get; set; }
    }
}
