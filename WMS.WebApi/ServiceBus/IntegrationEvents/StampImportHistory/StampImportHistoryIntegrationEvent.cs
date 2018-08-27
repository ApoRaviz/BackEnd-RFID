using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Entity.importManagement;

namespace WMS.WebApi.ServiceBus.IntegrationEvents
{
    public class StampImportHistoryIntegrationEvent : IntegrationEvent
    {
        public StampImportHistoryIntegrationEvent()
        {
            Items = new List<StampImportItem>();
        }

        public void AddItems(StampImportItem item)
        {
            Items.Add(item);
        }

        public List<StampImportItem> Items { get; set; }
    }
}