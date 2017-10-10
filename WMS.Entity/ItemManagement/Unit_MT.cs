using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WMS.Entity.ItemManagement
{
    public class Unit_MT
    {
        public Unit_MT()
        {
            this.ItemUnitMapping = new HashSet<ItemUnitMapping>();
        }

        public int UnitIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual Project_MT Project_MT { get; set; }
        public virtual ICollection<ItemUnitMapping> ItemUnitMapping { get; set; }
    }
}
