using Master.WebApi.ServiceBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace Master.WebApi.ServiceBus.EventsHandler
{
    public class TestEventHandler : IEventHandler<TestEvent>
    {

        private  ICustomerService _customerService;

        public TestEventHandler()
        {
        }

        public void Handle(TestEvent @event)
        {
            _customerService = new CustomerService();
            var customer = AutoMapper.Mapper.Map<TestEvent, Customer_MT>(@event);
           
            //var cus = _customerService.UpdateCustomer(customer);
        }
    }
}