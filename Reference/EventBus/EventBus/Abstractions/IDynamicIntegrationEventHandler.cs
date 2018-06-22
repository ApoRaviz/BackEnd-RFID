using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YUTSYS_SERVICES.BuildingBlocks.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
