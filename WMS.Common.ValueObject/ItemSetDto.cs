using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entity.ItemManagement;
using WMS.Master;


namespace WMS.Common.ValueObject
{
    public class ItemSetDto 
    {
        public int ItemSetIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string LineID { get; set; }
        public string ItemSetCode { get; set; }
        public string ItemSetName { get; set; }
        public int Active { get; set; }
        public string UserUpdate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<ItemSetDetailDto> ItemSetDetails { get; set; }

       
    }
}
