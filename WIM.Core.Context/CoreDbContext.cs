using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Country;
using WIM.Core.Entity.Currency;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.Dimension;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Entity.WarehouseManagement;


namespace WIM.Core.Context
{
    public class CoreDbContext : DbContext
    {
        public DbSet<ZoneLayoutHeader_MT> ZoneLayoutHeader_MT { get; set; }
        public DbSet<ZoneLayoutDetail_MT> ZoneLayoutDetail_MT { get; set; }
        public DbSet<RackLayout_MT> RackLayout_MT { get; set; }
        public DbSet<Api_MT> Api_MT { get; set; }
        public DbSet<ApiMenuMapping> ApiMenuMapping { get; set; }
        public DbSet<Customer_MT> Customer_MT { get; set; }
        public DbSet<DimensionLayout_MT> DimensionLayout_MT { get; set; }
        public DbSet<Employee_MT> Employee_MT { get; set; }
        public DbSet<Location_MT> Location_MT { get; set; }
        public DbSet<Menu_MT> Menu_MT { get; set; }
        public DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        public DbSet<Person_MT> Person_MT { get; set; }
        public DbSet<Project_MT> Project_MT { get; set; }
        public DbSet<Supplier_MT> Supplier_MT { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<CurrencyUnit> CurrencyUnit { get; set; }
        public DbSet<Country_MT> Country_MT { get; set; }
        public DbSet<Warehouse_MT> Warehouse_MT { get; set; }

        public CoreDbContext() : base("name=DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static CoreDbContext Create()
        {
            return new CoreDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public virtual string ProcGetNewID(string prefixes)
        {
            var prefixesParameter = new SqlParameter
            {
                ParameterName = "Prefixes",
                Value = prefixes
            };

            return Database.SqlQuery<string>("exec ProcGetNewID @Prefixes", prefixesParameter).SingleOrDefault();
        }

        public ObjectResult<Nullable<int>> ProcCreateZoneLayout(string zoneName, string warehouse, string area, Nullable<int> totalFloor, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var zoneNameParameter = zoneName != null ?
                new ObjectParameter("ZoneName", zoneName) :
                new ObjectParameter("ZoneName", typeof(string));

            var warehouseParameter = warehouse != null ?
                new ObjectParameter("Warehouse", warehouse) :
                new ObjectParameter("Warehouse", typeof(string));

            var areaParameter = area != null ?
                new ObjectParameter("Area", area) :
                new ObjectParameter("Area", typeof(string));

            var totalFloorParameter = totalFloor.HasValue ?
                new ObjectParameter("TotalFloor", totalFloor) :
                new ObjectParameter("TotalFloor", typeof(int));

            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            var xmlDetailParameter = xmlDetail != null ?
                new ObjectParameter("XmlDetail", xmlDetail) :
                new ObjectParameter("XmlDetail", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateZoneLayout", zoneNameParameter, warehouseParameter, areaParameter, totalFloorParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcUpdateZoneLayout(Nullable<int> zoneIDSys, string zoneName, string warehouse, string area, Nullable<int> totalFloor, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ?
                new ObjectParameter("ZoneIDSys", zoneIDSys) :
                new ObjectParameter("ZoneIDSys", typeof(int));

            var zoneNameParameter = zoneName != null ?
                new ObjectParameter("ZoneName", zoneName) :
                new ObjectParameter("ZoneName", typeof(string));

            var warehouseParameter = warehouse != null ?
                new ObjectParameter("Warehouse", warehouse) :
                new ObjectParameter("Warehouse", typeof(string));

            var areaParameter = area != null ?
                new ObjectParameter("Area", area) :
                new ObjectParameter("Area", typeof(string));

            var totalFloorParameter = totalFloor.HasValue ?
                new ObjectParameter("TotalFloor", totalFloor) :
                new ObjectParameter("TotalFloor", typeof(int));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            var xmlDetailParameter = xmlDetail != null ?
                new ObjectParameter("XmlDetail", xmlDetail) :
                new ObjectParameter("XmlDetail", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateZoneLayout", zoneIDSysParameter, zoneNameParameter, warehouseParameter, areaParameter, totalFloorParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcCreateRackLayout(Nullable<int> zoneIDSys, Nullable<int> zoneID, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ?
                new ObjectParameter("ZoneIDSys", zoneIDSys) :
                new ObjectParameter("ZoneIDSys", typeof(int));

            var zoneIDParameter = zoneID.HasValue ?
                new ObjectParameter("ZoneID", zoneID) :
                new ObjectParameter("ZoneID", typeof(int));

            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            var xmlDetailParameter = xmlDetail != null ?
                new ObjectParameter("XmlDetail", xmlDetail) :
                new ObjectParameter("XmlDetail", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcCreateRackLayout", zoneIDSysParameter, zoneIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public ObjectResult<RackLayout> ProcGetRackLayout(Nullable<int> zoneIDSys, Nullable<int> zoneID)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ?
                new ObjectParameter("ZoneIDSys", zoneIDSys) :
                new ObjectParameter("ZoneIDSys", typeof(int));

            var zoneIDParameter = zoneID.HasValue ?
                new ObjectParameter("ZoneID", zoneID) :
                new ObjectParameter("ZoneID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RackLayout>("ProcGetRackLayout", zoneIDSysParameter, zoneIDParameter);
        }

        public ObjectResult<RackLayout> ProcGetRackLayoutByZoneIDSys(Nullable<int> zoneIDSys)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ?
                new ObjectParameter("ZoneIDSys", zoneIDSys) :
                new ObjectParameter("ZoneIDSys", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RackLayout>("ProcGetRackLayoutByZoneIDSys", zoneIDSysParameter);
        }

        public ObjectResult<Nullable<int>> ProcCreateDimensionLayout(string formatName, string unit, Nullable<int> width, Nullable<int> length, Nullable<int> height, Nullable<int> weight, string type, string color, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate)
        {
            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var unitParameter = unit != null ?
                new ObjectParameter("Unit", unit) :
                new ObjectParameter("Unit", typeof(string));

            var widthParameter = width.HasValue ?
                new ObjectParameter("Width", width) :
                new ObjectParameter("Width", typeof(int));

            var lengthParameter = length.HasValue ?
                new ObjectParameter("Length", length) :
                new ObjectParameter("Length", typeof(int));

            var heightParameter = height.HasValue ?
                new ObjectParameter("Height", height) :
                new ObjectParameter("Height", typeof(int));

            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(int));

            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));

            var colorParameter = color != null ?
                new ObjectParameter("Color", color) :
                new ObjectParameter("Color", typeof(string));

            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateDimensionLayout", formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, createdDateParameter, updatedDateParameter, userUpdateParameter);
        }

        public ObjectResult<Nullable<int>> ProcUpdateDimensionLayout(Nullable<int> dimensionIDSys, string formatName, string unit, Nullable<int> width, Nullable<int> length, Nullable<int> height, Nullable<int> weight, string type, string color, Nullable<System.DateTime> updatedDate, string userUpdate)
        {
            var dimensionIDSysParameter = dimensionIDSys.HasValue ?
                new ObjectParameter("DimensionIDSys", dimensionIDSys) :
                new ObjectParameter("DimensionIDSys", typeof(int));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var unitParameter = unit != null ?
                new ObjectParameter("Unit", unit) :
                new ObjectParameter("Unit", typeof(string));

            var widthParameter = width.HasValue ?
                new ObjectParameter("Width", width) :
                new ObjectParameter("Width", typeof(int));

            var lengthParameter = length.HasValue ?
                new ObjectParameter("Length", length) :
                new ObjectParameter("Length", typeof(int));

            var heightParameter = height.HasValue ?
                new ObjectParameter("Height", height) :
                new ObjectParameter("Height", typeof(int));

            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(int));

            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));

            var colorParameter = color != null ?
                new ObjectParameter("Color", color) :
                new ObjectParameter("Color", typeof(string));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcUpdateDimensionLayout", dimensionIDSysParameter, formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, updatedDateParameter, userUpdateParameter);
        }

    }
}
