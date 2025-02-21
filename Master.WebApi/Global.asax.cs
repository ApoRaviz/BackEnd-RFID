﻿using Master.WebApi.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WIM.Core.Common.Helpers;

namespace Master.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Initialize();
            ApiHashTableHelper.Initialize();
            // TableHashTableHelper.Initialize();
            RabbitMQMessageListener.Start(RabbitMQConfig.Initialize());
        }

        protected void Application_End()
        {
            RabbitMQMessageListener.Stop();
        }
    }
}
