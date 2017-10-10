using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.InspectionManagement;

namespace WMS.Entity.ItemManagement
{
    public class ItemInspectMapping
    {
        public int ItemInspectIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int InspectIDSys { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual Inspect_MT Inspect_MT { get; set; }
        public virtual Item_MT Item_MT { get; set; }
    }
}
