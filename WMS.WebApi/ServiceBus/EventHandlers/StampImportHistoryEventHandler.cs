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
    public class StampImportHistoryEventHandler : IEventHandler<StampImportHistoryEvent>
    {

        private IImportService _importService;

        public StampImportHistoryEventHandler()
        {
        }

        public void Handle(StampImportHistoryEvent @event)
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
        }
    }
}