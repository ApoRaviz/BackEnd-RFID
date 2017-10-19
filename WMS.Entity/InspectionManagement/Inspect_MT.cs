using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.ItemManagement;

namespace WMS.Entity.InspectionManagement
{
    [Table("Inspect_MT")]
    public class Inspect_MT
    {
        public Inspect_MT()
        {
            this.ItemInspectMapping = new HashSet<ItemInspectMapping>();
        }

        [Key]
        public int InspectIDSys { get; set; }
        public string InspectID { get; set; }
        public string InspectName { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual ICollection<ItemInspectMapping> ItemInspectMapping { get; set; }
    }
}
