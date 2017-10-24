using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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
using WIM.Core.Entity.Status;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Entity.UserManagement;


namespace WIM.Core.Context
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Api_MT> Api_MT { get; set; }
        public DbSet<ApiMenuMapping> ApiMenuMapping { get; set; }
        public DbSet<Customer_MT> Customer_MT { get; set; }
        public DbSet<DimensionLayout_MT> DimensionLayout_MT { get; set; }
        public DbSet<Employee_MT> Employee_MT { get; set; }
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

        public DbSet<Status_MT> Status_MT { get; set; }

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

        public virtual ObjectResult<string> ProcGetNewID(string prefixes)
        {
            var prefixesParameter = prefixes != null ?
                new ObjectParameter("Prefixes", prefixes) :
                new ObjectParameter("Prefixes", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ProcGetNewID", prefixesParameter);
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
