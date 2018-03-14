using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WIM.Core.Common;
using WMS.WebApi.Controllers;
using WMS.Service;
using WMS.Service.Inspect;
using WMS.Service.Impl.Inspect;
using WMS.Master;
using WMS.Service.WarehouseMaster;
using WMS.Service.Impl.WarehouseMaster;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WMS.Service.LocationMaster;
using WMS.Service.Impl.Label;
using WMS.Service.Label;
using WMS.Service.Report;
using WMS.Service.Impl.Report;
using WMS.Service.Import;
using WMS.Service.Impl.Import;
using WMS.Service.Impl;

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
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IItemSetService, ItemSetService>();
            container.RegisterType<IUnitService, UnitService>();
            // #JobComment
            container.RegisterType<ILabelService, LabelService>();
            container.RegisterType<ISupplierService, SupplierService>(); 
            container.RegisterType<IInspectService, InspectService>();
            //container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IImportService, ImportService>();
            container.RegisterType<ICategoryService, CategoryService>();
            //container.RegisterType<IDimensionService, DimensionService>();
            container.RegisterType<IWarehouseService, WarehouseService>();
            container.RegisterType<IZoneService, ZoneService>();
            container.RegisterType<ILocationGroupService, LocationGroupService>();
            container.RegisterType<IReceiveService, ReceiveService>();

        }
    }
}