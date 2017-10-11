using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ItemManagement
{
    [Table("ItemSet_MT")]
    public class ItemSet_MT
    {
        public ItemSet_MT()
        {
            this.ItemSetDetails = new HashSet<ItemSetDetail>();
        }

        [Key]
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
