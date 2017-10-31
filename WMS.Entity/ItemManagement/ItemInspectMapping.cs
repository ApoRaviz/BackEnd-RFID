using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WMS.Entity.InspectionManagement;

namespace WMS.Entity.ItemManagement
{
    [Table("ItemInspectMapping")]
    public class ItemInspectMapping : BaseEntity
    {
        [Key]
        public int ItemInspectIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int InspectIDSys { get; set; }


        public virtual Inspect_MT Inspect_MT { get; set; }
        public virtual Item_MT Item_MT { get; set; }
    }
}
