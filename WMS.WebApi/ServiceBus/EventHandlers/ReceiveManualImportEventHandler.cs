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
using WMS.WebApi.ServiceBus.Events;

namespace WMS.WebApi.ServiceBus.EventsHandler
{
    public class ReceiveManualImportEventHandler : IEventHandler<ReceiveManualImportEvent>
    {

        private IImportService _importService;

        public ReceiveManualImportEventHandler()
        {
        }

        public void Handle(ReceiveManualImportEvent @event)
        {
            _importService = new ImportService();

            ImportDefinitionHeader_MT def = null;
            if (@event.FormatId > 0)
                def = _importService.GetImportDefinitionByImportIDSys(@event.FormatId, "ImportDefinitionDetail_MT");

            if (def != null)
            {
                ReceiveManualImportReplyEvent ret = new ReceiveManualImportReplyEvent(@event.FileIds, def);
                RabbitMQMessageListener.Publish(ret);
            }
        }
    }
}