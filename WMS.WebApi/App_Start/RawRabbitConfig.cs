﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.WebApi.ServiceBus.IntegrationEvents;

namespace WMS.WebApi
{
    public static class RawRabbitConfig
    {
        private static IList<string> _listConfig = new List<string>();

        public static IList<string> Initialize()
        {
            AddConfig<ReceiveManualImportIntegrationEvent>();
            //AddConfig<StampImportHistoryEvent>();

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