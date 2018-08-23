using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Entity.ImportManagement;

namespace WMS.WebApi.ServiceBus.Events
{
    public class ReceiveManualImportReplyEvent : IntegrationEvent
    {
        public ReceiveManualImportReplyEvent(IEnumerable<int> fileIds, ImportDefinitionHeader_MT cusID)
        {
            this.FileIds = fileIds;
            this.CusID = cusID;
        }
        public IEnumerable<int> FileIds { get; set; }
        public ImportDefinitionHeader_MT CusID { get; set; }
    }
}