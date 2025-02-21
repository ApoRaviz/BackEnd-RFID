using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using HRMS.WebApi.Controllers;
using Unity.WebApi;
using HRMS.Service.LeaveManagement;
using HRMS.Service.Impl.LeaveManagement;
using WIM.Core.Common;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using HRMS.Service.Probation;
using HRMS.Service.Impl;
using HRMS.Service.Form;
using HRMS.Service.Impl.Form;

namespace HRMS.WebApi
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

            //Register OWin Authen
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<ILeaveService, LeaveService>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<IVEmployeeInfoService, VEmployeeInfoService>();
            container.RegisterType<IFormService, FormService>();

        }
    }
}