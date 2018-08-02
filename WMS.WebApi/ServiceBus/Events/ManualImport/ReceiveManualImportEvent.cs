using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Entity.importManagement;

namespace WMS.WebApi.ServiceBus.Events
{
    public class ReceiveManualImportEvent : IntegrationEvent
    {
        public ReceiveManualImportEvent(IEnumerable<int> fileIds)
        {
            this.FileIds = fileIds;
        }
        public IEnumerable<int> FileIds { get; set; }
        public int FormatId { get; set; }
    }
}