using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WMS.Entity.ItemManagement;

namespace WMS.Entity.InspectionManagement
{
    [Table("Inspect_MT")]
    public class Inspect_MT: BaseEntity
    {
        public Inspect_MT()
        {
            this.ItemInspectMapping = new HashSet<ItemInspectMapping>();
        }

        [Key]
        public int InspectIDSys { get; set; }
        public string InspectID { get; set; }
        public string InspectName { get; set; }


        public virtual ICollection<ItemInspectMapping> ItemInspectMapping { get; set; }
    }
}
