using Master.WebApi.ServiceBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Master.WebApi
{
    public static class RabbitMQConfig
    {
        private static IList<string> _listConfig = new List<string>();

        public static IList<string> Initialize()
        {
            AddConfig<TestEvent>();
            AddConfig<CustomerEvent>();
           
            return _listConfig;
        }

        private static void AddConfig<T>()
        {
            var name = typeof(T).Name;
            if(!string.IsNullOrEmpty(name))
            {
                _listConfig.Add(name);
            }
        }
    }
}