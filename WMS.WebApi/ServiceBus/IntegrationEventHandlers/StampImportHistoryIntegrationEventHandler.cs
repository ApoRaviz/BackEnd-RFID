using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WMS.Entity.ImportManagement;
using WMS.Service.Impl.Import;
using WMS.Service.Import;
using WMS.WebApi.ServiceBus.IntegrationEvents;

namespace WMS.WebApi.ServiceBus.EventsHandler
{
    public class StampImportHistoryIntegrationEventHandler : IEventHandler<StampImportHistoryIntegrationEvent>
    {

        private IImportService _importService;

        public StampImportHistoryIntegrationEventHandler()
        {
        }

        public string Handle(StampImportHistoryIntegrationEvent @event)
        {
            _importService = new ImportService();
            
            if (@event.Items.Count > 0)
            {
                foreach(var item in @event.Items)
                {
                    _importService.InsertImportHistory(
                    item.ImportDefHeadIDSys,
                    item.FileName,
                    item.Result,
                    item.Success,
                    item.User);
                }
                

            }

            return "";
        }
    }
}