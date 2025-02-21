﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Entity.ImportManagement;

namespace WMS.WebApi.ServiceBus.IntegrationEvents
{
    public class ReceiveManualImportReplyIntegrationEvent : IntegrationEvent
    {
        public ReceiveManualImportReplyIntegrationEvent(IEnumerable<int> fileIds, ImportDefinitionHeader_MT defHead)
        {
            this.FileIds = fileIds;
            this.DefHead = defHead;
        }
        public IEnumerable<int> FileIds { get; set; }
        public ImportDefinitionHeader_MT DefHead { get; set; }
    }
}