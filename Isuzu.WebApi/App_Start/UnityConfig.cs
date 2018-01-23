using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WIM.Core.Common;
using Isuzu.WebApi.Controllers;
using Isuzu.Service;
using Isuzu.Service.Impl.Inbound;
using System.Security.Principal;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

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

            //Register OWin Authen
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<IIdentity>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication.User.Identity));

            //Register Service
            container.RegisterType<ICommonService, CommonService>();
            //container.RegisterType<ICustomerService, CustomerService>();
            //container.RegisterType<IProjectService, ProjectService>();
            //container.RegisterType<ICategoryService, CategoryService>();
            //container.RegisterType<IItemService, ItemService>();
            //container.RegisterType<IItemSetService, ItemSetService>();
            //container.RegisterType<IUnitService, UnitService>();
            //container.RegisterType<ILabelService, LabelService>();
            //container.RegisterType<ISupplierService, SupplierService>(); 
            //container.RegisterType<IInspectService, InspectService>();
            //container.RegisterType<ILocationService, LocationService>();
            //container.RegisterType<IMenuService, MenuService>();
            //container.RegisterType<IMenuProjectMappingService, MenuProjectMappingService>();
            //container.RegisterType<IReportService, ReportService>();
            //container.RegisterType<IRoleService, RoleService>();
            //container.RegisterType<IPermissionService, PermissionService>();
            //container.RegisterType<IUserService, UserService>();
            //container.RegisterType<IApiMTService, ApiMTService>();
            //container.RegisterType<IApiMenuMappingService, ApiMenuMappingService>();
            //container.RegisterType<IUserRoleService, UserRoleService>();
            //container.RegisterType<IImportService, ImportService>();
            //container.RegisterType<IDimensionService, DimensionService>();

            //Fuji
            //container.RegisterType<IItemImportService, ItemImportService>();
            //container.RegisterType<IProgramVersionService, ProgramVersionService>();
            //container.RegisterType<IPrintLabelService, PrintLabelService>();
            //container.RegisterType<IHelperService, HelperService>();

            //Isuzu
            container.RegisterType<IInboundService, InboundService>();

            //CORE
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IMenuProjectMappingService, MenuProjectMappingService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IUserService, UserService>();

        }
    }
}