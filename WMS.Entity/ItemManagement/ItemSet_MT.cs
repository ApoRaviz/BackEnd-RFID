using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ItemManagement
{
    public class ItemSet_MT
    {
        public ItemSet_MT()
        {
            this.ItemSetDetails = new HashSet<ItemSetDetail>();
        }

        public int ItemSetIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string LineID { get; set; }
        public string ItemSetCode { get; set; }
        public string ItemSetName { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        //public virtual Project_MT Project_MT { get; set; }
        public virtual ICollection<ItemSetDetail> ItemSetDetails { get; set; }
    }
}
