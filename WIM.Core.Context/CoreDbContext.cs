using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity;
using WIM.Core.Entity.Country;
using WIM.Core.Entity.Currency;
using WIM.Core.Entity.CustomerManagement;
//using WIM.Core.Entity.Dimension;
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
using WIM.Core.Entity.FileManagement;
using WIM.Core.Entity.Address;
using WIM.Core.Entity.TableControl;
using System;
using WIM.Core.Entity.importManagement;

namespace WIM.Core.Context
{
    public class CoreDbContext : DbContext
    {
        //public System.IO.TextWriter Log { get; set; }
        //$DbSet
        public virtual DbSet<TableControl> TableControls { get; set; }

        public virtual DbSet<Api_MT> Api_MT { get; set; }
        public virtual DbSet<ApiMenuMapping> ApiMenuMapping { get; set; }
        public virtual DbSet<Customer_MT> Customer_MT { get; set; }
        public virtual DbSet<Resign> Resign { get; set; }
        public virtual DbSet<HistoryWarning> HistoryWarning { get; set; }
        public virtual DbSet<Employee_MT> Employee_MT { get; set; }
        public virtual DbSet<Menu_MT> Menu_MT { get; set; }
        public virtual DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        public virtual DbSet<Person_MT> Person_MT { get; set; }
        public virtual DbSet<Project_MT> Project_MT { get; set; }
        public virtual DbSet<Supplier_MT> Supplier_MT { get; set; }
        public virtual DbSet<Role> Role { get; set; }
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
        public virtual DbSet<File_MT> File_MT { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }
        public virtual DbSet<PermissionGroupApi> PermissionGroupApi { get; set; }
        public virtual DbSet<GeneralConfigs> GeneralConfigs { get; set; }
        public virtual DbSet<Probation_MT> Probation_MT { get; set; }
        public virtual DbSet<ImportDefinitionHeader_MT> ImportDefinitionHeader_MT { get; set; }
        public virtual DbSet<ImportDefinitionDetail_MT> ImportDefinitionDetail_MT { get; set; }

        public virtual DbSet<Province_MT> Province_MT { get; set; }
        public virtual DbSet<City_MT> City_MT { get; set; }
        public virtual DbSet<SubCity_MT> SubCity_MT { get; set; }
        public virtual DbSet<WarehouseTest> WarehouseTests { get; set; }
        

        /// <summary>
        /// View
        /// </summary>
        /// 
        public virtual DbSet<VPersons> VPersons { get; set; }

        public CoreDbContext() : base("name=YUT_CORE")
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


        public object ProcUpdateImportDefinition(Nullable<int> importIDSys, string formatName, string delimiter, Nullable<int> maxHeading, string encoding, Nullable<bool> skipFirstRecode, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var importIDSysParameter = importIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ImportIDSys",
                Value = importIDSys
            } : new SqlParameter("ImportIDSys", 0);

            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var delimiterParameter = delimiter != null ? new SqlParameter
            {
                ParameterName = "Delimiter",
                Value = delimiter
            } : new SqlParameter("Delimiter", DBNull.Value);

            var maxHeadingParameter = maxHeading.HasValue ? new SqlParameter
            {
                ParameterName = "MaxHeading",
                Value = maxHeading
            } : new SqlParameter("MaxHeading", 0);

            var encodingParameter = encoding != null ? new SqlParameter
            {
                ParameterName = "Encoding",
                Value = encoding
            } : new SqlParameter("Encoding", DBNull.Value);

            var skipFirstRecodeParameter = skipFirstRecode.HasValue ? new SqlParameter
            {
                ParameterName = "SkipFirstRecode",
                Value = skipFirstRecode
            } : new SqlParameter("SkipFirstRecode", false);

            var createdDateParameter = createdDate.HasValue ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DateTime.Now);

            var updatedDateParameter = updatedDate.HasValue ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            var xmlDetailParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            };

            return Database.SqlQuery<object>("exec ProcUpdateImportDefinition @ImportIDSys , @FormatName , @Delimiter , @MaxHeading ," +
            "@Encoding , @SkipFirstRecode , @CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", importIDSysParameter, formatNameParameter, delimiterParameter,
            maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateImportDefinition", importIDSysParameter, formatNameParameter, delimiterParameter, maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public Nullable<int> ProcCreateImportDefinition(string forTable, string formatName, string delimiter, Nullable<int> maxHeading, string encoding, Nullable<bool> skipFirstRecode, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var forTableParameter = forTable != null ? new SqlParameter
            {
                ParameterName = "ForTable",
                Value = forTable
            } : new SqlParameter("ForTable", DBNull.Value);

            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var delimiterParameter = delimiter != null ? new SqlParameter
            {
                ParameterName = "Delimiter",
                Value = delimiter
            } : new SqlParameter("Delimiter", DBNull.Value);

            var maxHeadingParameter = maxHeading.HasValue ? new SqlParameter
            {
                ParameterName = "MaxHeading",
                Value = maxHeading
            } : new SqlParameter("MaxHeading", 0);

            var encodingParameter = encoding != null ? new SqlParameter
            {
                ParameterName = "Encoding",
                Value = encoding
            } : new SqlParameter("Encoding", DBNull.Value);

            var skipFirstRecodeParameter = skipFirstRecode.HasValue ? new SqlParameter
            {
                ParameterName = "SkipFirstRecode",
                Value = skipFirstRecode
            } : new SqlParameter("SkipFirstRecode", false);

            var createdDateParameter = createdDate.HasValue ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DateTime.Now);

            var updatedDateParameter = updatedDate.HasValue ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            var xmlDetailParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            };

            return Database.SqlQuery<Nullable<int>>("exec ProcCreateImportDefinition @ForTable , @FormatName , @Delimiter , @MaxHeading ," +
    "@Encoding , @SkipFirstRecode , @CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", forTableParameter, formatNameParameter, delimiterParameter,
    maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateImportDefinition", forTableParameter, formatNameParameter, delimiterParameter, maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public string ProcImportDataToTable(Nullable<int> importSysID, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlData)
        {
            var importSysIDParameter = importSysID.HasValue ? new SqlParameter
            {
                ParameterName = "ImportSysID",
                Value = importSysID
            } : new SqlParameter("ImportSysID", 0);

            var createdDateParameter = createdDate.HasValue ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DateTime.Now);

            var updatedDateParameter = updatedDate.HasValue ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            var xmlDataParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlData
            };

            return Database.SqlQuery<string>("exec ProcImportDataToTable @ImportSysID , @CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", importSysIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDataParameter).FirstOrDefault();
            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ProcImportDataToTable", importSysIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDataParameter);
        }

        public object ProcInsertImportHistory(Nullable<int> importDefinitionIDSys, string fileName, string result, Nullable<bool> success, Nullable<System.DateTime> createdDate, string userUpdate)
        {
            var importDefinitionIDSysParameter = importDefinitionIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ImportDefinitionIDSys",
                Value = importDefinitionIDSys
            } : new SqlParameter("ImportDefinitionIDSys", 0);

            var fileNameParameter = fileName != null ? new SqlParameter
            {
                ParameterName = "FileName",
                Value = fileName
            } : new SqlParameter("FileName", DBNull.Value);

            var resultParameter = result != null ? new SqlParameter
            {
                ParameterName = "Result",
                Value = result
            } : new SqlParameter("Result", DBNull.Value);

            var successParameter = success.HasValue ? new SqlParameter
            {
                ParameterName = "Success",
                Value = success
            } : new SqlParameter("Success", true);

            var createdDateParameter = createdDate.HasValue ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);


            return Database.SqlQuery<object>("exec ProcInsertImportHistory @ImportDefinitionIDSys , @FileName , @Result " +
                ", @Success , @CreatedDate , @UserUpdate ", importDefinitionIDSysParameter, fileNameParameter,
                resultParameter, successParameter, createdDateParameter, userUpdateParameter).FirstOrDefault();
            ;
            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcInsertImportHistory", importDefinitionIDSysParameter, fileNameParameter, resultParameter, successParameter, createdDateParameter, userUpdateParameter);
        }

        public int ProcDeleteImportDefinition(Nullable<int> importIDSys)
        {
            var importIDSysParameter = importIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ImportIDSys",
                Value = importIDSys
            } : new SqlParameter("ImportIDSys", 0);

            return Database.SqlQuery<int>("exec ProcDeleteImportDefinition @ImportIDSys", importIDSysParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcDeleteImportDefinition", importIDSysParameter);
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
