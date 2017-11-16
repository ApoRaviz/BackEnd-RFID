using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WIM.Core.Common;
using Master.WebApi.Controllers;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace Master.WebApi
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
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IMenuProjectMappingService, MenuProjectMappingService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IApiMTService, ApiMTService>();
            container.RegisterType<IApiMenuMappingService, ApiMenuMappingService>();
            container.RegisterType<IUserRoleService, UserRoleService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<IPersonService, PersonService>();

        }
    }
}