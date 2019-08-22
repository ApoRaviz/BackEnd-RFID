using Isuzu.Service;
using Isuzu.Service.Impl.Inbound;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WIM.Core.Service.Impl.StatusManagement;
using WIM.Core.Service.StatusManagement;

namespace Isuzu.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            // Api
            container.RegisterType<IApiMTService, ApiMTService>();

            //inboound 
            container.RegisterType<IInboundService, InboundService>();

            //Core
            //container.RegisterType<IProjectService, ProjectService>();
            //container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<IStatusService, StatusService>();
        }
    }
}