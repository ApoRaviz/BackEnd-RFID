using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface.ItemImport
{
    public class ReceiveRequest
    {
        public string HeadID { get; set; }
        public string ItemCode { get; set; }
        public string LocationID { get; set; }
        public List<string> ItemGroups { get; set; }
        public string UserUpdate { get; set; }
    }

    public class CheckReceiveRequest
    {
        public List<string> ItemGroups { get; set; }
    }

    public class SetScannedRequest
    {
        public string HeadID { get; set; }
        public string ItemCode { get; set; }       
        public List<string> ItemGroups { get; set; }
        public string UserUpdate { get; set; }
    }
}
