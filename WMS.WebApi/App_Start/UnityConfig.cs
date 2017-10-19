using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WIM.Core.Common;
using WMS.WebApi.Controllers;
using WMS.Master.Label;
using WMS.Master.Report;
using WMS.Master.Inspect;
using WMS.Master.Import;
using WMS.Master.Zone;
using WMS.Master;
using WMS.Service;
namespace WMS.WebApi
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

            //Register Service
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IItemSetService, ItemSetService>();
            container.RegisterType<IUnitService, UnitService>();
            container.RegisterType<ILabelService, LabelService>();
            container.RegisterType<ISupplierService, SupplierService>(); 
            container.RegisterType<IInspectService, InspectService>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IMenuProjectMappingService, MenuProjectMappingService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IApiMTService, ApiMTService>();
            container.RegisterType<IApiMenuMappingService, ApiMenuMappingService>();
            container.RegisterType<IUserRoleService, UserRoleService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<IPersonService, PersonService>();
            container.RegisterType<IImportService, ImportService>();
            container.RegisterType<IDimensionService, DimensionService>();
            container.RegisterType<IWarehouseService, WarehouseService>();
            container.RegisterType<IZoneService, ZoneService>();
            container.RegisterType<ICurrencyService, CurrencyService>();
            container.RegisterType<ICountryService, CountryService>();
        }
    }
}