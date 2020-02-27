using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WMS.WebApi.ServiceBus;

namespace WMS.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Initialize();
            ApiHashTableHelper.Initialize();
            // TableHashTableHelper.Initialize();
            RabbitMQMessageListener.Start(RabbitMQConfig.Initialize()
                ,RawRabbitConfig.Initialize());

        }
    }

    //public class ApiHashTableHelper
    //{
    //    private static CoreDbContext db;
    //    public static Hashtable apiTable;

    //    public static void Initialize()
    //    {
    //        db = new CoreDbContext();
    //        apiTable = new Hashtable();

    //        // #JobComment
    //        var api = (from row in db.Api_MT
    //                   select row).ToList();

    //        foreach (var a in api)
    //        {
    //            apiTable.Add(a.ApiIDSys, a.Api);
    //        }
    //    }
    //}
}
