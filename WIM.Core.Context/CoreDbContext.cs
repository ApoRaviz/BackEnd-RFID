using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.Country;
using WIM.Core.Entity.Currency;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.Dimension;
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
        public virtual DbSet<Api_MT> Api_MT { get; set; }
        public virtual DbSet<ApiMenuMapping> ApiMenuMapping { get; set; }
        public virtual DbSet<Customer_MT> Customer_MT { get; set; }
        public virtual DbSet<DimensionLayout_MT> DimensionLayout_MT { get; set; }
        public virtual DbSet<Employee_MT> Employee_MT { get; set; }
        public virtual DbSet<Menu_MT> Menu_MT { get; set; }
        public virtual DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        public virtual DbSet<Person_MT> Person_MT { get; set; }
        public DbSet<Project_MT> Project_MT { get; set; }
        public virtual DbSet<Supplier_MT> Supplier_MT { get; set; }
        public DbSet<Role> Role { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<CurrencyUnit> CurrencyUnit { get; set; }
        public virtual DbSet<Country_MT> Country_MT { get; set; }
        public virtual DbSet<Status_MT> Status_MT { get; set; }
        public virtual DbSet<Module_MT> Module_MT { get; set; }

        public CoreDbContext() : base("name=CORE")
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
            //modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
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

        public virtual string ProcGetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            var columnNamesParameter = /*new ObjectParameter("@columnNames", columnNames);*/
            new SqlParameter("columnNames", columnNames);

            var tableNameParameter = /*new ObjectParameter("@tableName", tableName);*/
            new SqlParameter("tableName", tableName);

            var conditionColumnNamesParameter = /*new ObjectParameter("@conditionColumnNames", conditionColumnNames);*/
            new SqlParameter("conditionColumnNames", conditionColumnNames);

            var keywordParameter = /*new ObjectParameter("@keyword", keyword);*/
            new SqlParameter("keyword", keyword);
            string x;

            var y = Database.SqlQuery<string>("ProcGetDataAutoComplete @columnNames, @tableName, @conditionColumnNames, @keyword", columnNamesParameter, tableNameParameter, conditionColumnNamesParameter, keywordParameter);

            x = y.FirstOrDefault();

            //var y = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<string>
            //    ("exec ProcGetDataAutoComplete @columnNames,@tableName,@conditionColumnNames,@keyword", columnNamesParameter, tableNameParameter, conditionColumnNamesParameter, keywordParameter);
            return x;

        }


    }
}
