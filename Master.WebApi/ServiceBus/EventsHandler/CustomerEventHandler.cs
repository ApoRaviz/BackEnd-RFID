using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Master.WebApi.ServiceBus.Events;

namespace Master.WebApi.ServiceBus.EventsHandler
{
    public class CustomerEventHandler
    {
        public CustomerEventHandler()
        {

        }

        public void Handle(CustomerEvent @event)
        {
            var j = @event;
        }
    }
}