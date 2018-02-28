using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.WarehouseManagement
{
    [Table("ZoneTypes")]
    public class ZoneType : BaseEntity
    {
        [Key]
        public int ZoneTypeIDSys { get; set; }
        public int Priority { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set;}
        public string BgColor { get; set; }
        public string Color { get; set; }

    }
}
