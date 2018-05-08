using Master.WebApi.Controllers;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WIM.Core.Service;
using WIM.Core.Service.Address;
using WIM.Core.Service.Common;
using WIM.Core.Service.EmployeeMaster;
using WIM.Core.Service.FileManagement;
using WIM.Core.Service.Impl;
using WIM.Core.Service.Impl.Common;
using WIM.Core.Service.Impl.EmployeeMaster;
using WIM.Core.Service.Impl.FileManagement;
using WIM.Core.Service.Impl.PermissionGroups;
using WIM.Core.Service.Impl.StatusManagement;
using WIM.Core.Service.PermissionGroups;
using WIM.Core.Service.StatusManagement;

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
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICurrencyService, CurrencyService>();
            container.RegisterType<ICountryService, CountryService>();
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
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<ISubModuleService, SubModuleService>();
            container.RegisterType<IStatusService, StatusService>();
            container.RegisterType<IStatusService, StatusService>();
            container.RegisterType<IModuleService, ModuleService>();
            container.RegisterType<ISubModuleService, SubModuleService>();
            container.RegisterType<IHeadReportControlService, HeadReportControlService>(); 
            container.RegisterType<ISupplierService, SupplierService>();
            container.RegisterType<IGeneralConfigsService, GeneralConfigsService>();
            container.RegisterType<IPermissionGroupService, PermissionGroupService>();
            container.RegisterType<IPermissionGroupApiService, PermissionGroupApiService>();
            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IResignService, ResignService>();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IHistoryWarningService, HistoryWarningService>();
            container.RegisterType<ITableControlService, TableControlService>();

            container.RegisterType<IImportDataService, ImportDataService>();

        }
    }
}