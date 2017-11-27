using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using TMS.WebApi.Controllers;
using Unity.WebApi;
using System.Security.Principal;

namespace TMS.WebApi
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
            
           
        }
    }
}