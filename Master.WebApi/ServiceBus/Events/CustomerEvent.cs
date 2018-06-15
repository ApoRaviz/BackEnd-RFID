using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Master.WebApi.ServiceBus.Events
{
    public class CustomerEvent : IntegrationEvent
    {
        public CustomerEvent(int cusIDSys, string cusID,string cusName )
        {
            CusIDSys = cusIDSys;
            CusID = cusID;
            CusName = cusName;
        }
        public int CusIDSys { get; set; }
        public string CusID { get; set; }
        public string CusName { get; set; }
    }
}