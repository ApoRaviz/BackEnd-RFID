using Newtonsoft.Json;
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
    public class ReceiveManualImportIntegrationEventHandler : IEventHandler<ReceiveManualImportIntegrationEvent>
    {

        private IImportService _importService;

        public ReceiveManualImportIntegrationEventHandler()
        {
        }

        public string Handle(ReceiveManualImportIntegrationEvent @event)
        {
            _importService = new ImportService();

            ImportDefinitionHeader_MT def = null;
            if (@event.FormatId > 0)
            {
                def = _importService.GetImportDefinitionByImportIDSys(@event.FormatId, "ImportDefinitionDetail_MT");
                def.ImportDefinitionDetail_MT = (from p in def.ImportDefinitionDetail_MT
                                                 select new ImportDefinitionDetail_MT()
                                                 {
                                                     ImportDefHeadIDSys = p.ImportDefHeadIDSys,
                                                     ImportDefDetailIDSys = p.ImportDefDetailIDSys,
                                                     ColumnName = p.ColumnName,
                                                     Digits = p.Digits,
                                                     DataType = p.DataType,
                                                     FixedValue = p.FixedValue,
                                                     Import = p.Import,
                                                     IsActive = p.IsActive,
                                                     CreateAt = p.CreateAt,
                                                     CreateBy = p.CreateBy,
                                                     IsHead = p.IsHead,
                                                     IsRefKey = p.IsRefKey,
                                                     Mandatory = p.Mandatory,
                                                     UpdateAt = p.UpdateAt,
                                                     UpdateBy = p.UpdateBy
                                                 }).ToList();


            }


            ReceiveManualImportReplyIntegrationEvent ret = new ReceiveManualImportReplyIntegrationEvent(@event.FileIds, def);

            return JsonConvert.SerializeObject(ret);
           
        }
        
        
    }
}