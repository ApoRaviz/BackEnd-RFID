using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity;
using WIM.Core.Entity.Country;
using WIM.Core.Entity.Currency;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.Dimension;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.Common;
using WIM.Core.Entity.Logs;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.Module;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.Status;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Entity.View;
using WIM.Core.Entity.Address;

namespace WIM.Core.Context
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<Api_MT> Api_MT { get; set; }
        public virtual DbSet<ApiMenuMapping> ApiMenuMapping { get; set; }
        public virtual DbSet<Customer_MT> Customer_MT { get; set; }
        public virtual DbSet<Employee_MT> Employee_MT { get; set; }
        public virtual DbSet<Menu_MT> Menu_MT { get; set; }
        public virtual DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        public virtual DbSet<Person_MT> Person_MT { get; set; }
        public virtual DbSet<Project_MT> Project_MT { get; set; }
        public virtual DbSet<Supplier_MT> Supplier_MT { get; set; }
        public DbSet<Role> Role { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<CurrencyUnit> CurrencyUnit { get; set; }
        public virtual DbSet<Country_MT> Country_MT { get; set; }
        public virtual DbSet<Status_MT> Status_MT { get; set; }
        public virtual DbSet<SubModules> SubModule { get; set; }
        public virtual DbSet<Module_MT> Module_MT { get; set; }
        public virtual DbSet<LabelControl> LabelControl { get; set; }
        public virtual DbSet<StatusSubModules> StatusSubModule { get; set; }
        public virtual DbSet<HeadReportControl> HeadReportControl { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<GeneralLog> GeneralLogs { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }

        public virtual DbSet<Province_MT> Province_MT { get; set; }
        public virtual DbSet<City_MT> City_MT { get; set; }
        public virtual DbSet<SubCity_MT> SubCity_MT { get; set; }

        /// <summary>
        /// View
        /// </summary>
        /// 
        public virtual DbSet<VPersons> VPersons { get; set; }

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


        public IEnumerable<SubModuleDto> AutoCompleteSM(string txtsearch)
        {
            var submodule = from i in SubModule
                            join m in Module_MT on i.ModuleIDSys equals m.ModuleIDSys
                            where i.SubModuleName.Contains(txtsearch) || m.ModuleName.Contains(txtsearch)
                            select new SubModuleDto()
                            {
                                SubModuleIDSys = i.SubModuleIDSys,
                                ModuleIDSys = i.ModuleIDSys,
                                ModuleName = m.ModuleName,
                                SubModuleName = i.SubModuleName,
                                LabelSubModuleName = m.ModuleName + " : " + i.SubModuleName
                        };
            return submodule.ToList();
        }
        
    }
}
