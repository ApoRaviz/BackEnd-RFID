using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Master.WebApi.ServiceBus.Events
{
    public class TestEvent : IntegrationEvent
    {
        public TestEvent(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}