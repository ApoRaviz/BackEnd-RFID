using Fuji.Service.Impl.ItemImport;
using Fuji.Service.Impl.PrintLabel;
using Fuji.Service.Impl.ProgramVersion;
using Fuji.Service.ItemImport;
using Fuji.Service.PrintLabel;
using Fuji.Service.ProgramVersion;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;

using Fuji.WebApi.Controllers;


namespace Fuji.WebApi
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
            //container.RegisterType<AccountController>(new InjectionConstructor());

            //Fuji
            container.RegisterType<IItemImportService, ItemImportService>();
            container.RegisterType<IProgramVersionService, ProgramVersionService>();
            container.RegisterType<IPrintLabelService, PrintLabelService>();

        }
    }
}