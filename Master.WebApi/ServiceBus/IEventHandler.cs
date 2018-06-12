using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.WebApi.ServiceBus
{
    public interface IEventHandler<in TIntegrationEvent> : IEventHandler
       where TIntegrationEvent : IntegrationEvent
    {
        void Handle(TIntegrationEvent @event);
    }

    public interface IEventHandler
    {
    }
}
