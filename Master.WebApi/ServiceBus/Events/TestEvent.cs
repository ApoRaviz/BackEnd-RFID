using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Master.WebApi.ServiceBus.Events
{
    public class TestEvent : IntegrationEvent
    {
        public TestEvent(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}