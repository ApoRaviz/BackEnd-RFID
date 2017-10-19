using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;


namespace WMS.Common
{
    public class ItemSetDto 
    {
        public int ItemSetIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string LineID { get; set; }
        public string ItemSetCode { get; set; }
        public string ItemSetName { get; set; }
        public byte Active { get; set; }

       public List<ItemSetDetailDto> ItemSetDetail { get; set; }

       
    }
}
