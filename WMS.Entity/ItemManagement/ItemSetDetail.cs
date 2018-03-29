using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.ItemManagement 
{
    [Table("ItemSetDetail")]
    public class ItemSetDetail : BaseEntity
    {
 
        [Key]
        public int ItemSetDetailIDSys { get; set; }
        public int ItemSetIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int Qty { get; set; }
       
        public virtual Item_MT Item_MT { get; set; }
    }
}
