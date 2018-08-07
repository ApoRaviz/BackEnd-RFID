using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.WebApi.ServiceBus
{
    public interface IEventHandler<in TIntegrationEvent> : IEventHandler
       where TIntegrationEvent : IntegrationEvent
    {
        string Handle(TIntegrationEvent @event);
    }

    public interface IEventHandler
    {
    }
}
