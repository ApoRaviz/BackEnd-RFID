using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.FileManagement;
using WIM.Core.Entity.SupplierManagement;
using WMS.Common.ValueObject;
using WMS.Entity.Common;
using WMS.Entity.ControlMaster;
using WMS.Entity.Dimension;
using WMS.Entity.ImportManagement;
using WMS.Entity.InspectionManagement;
using WMS.Entity.InventoryManagement;
using WMS.Entity.ItemManagement;
using WMS.Entity.LayoutManagement;
using WMS.Entity.Receiving;
using WMS.Entity.Report;
using WMS.Entity.SpareField;
using WMS.Entity.WarehouseManagement;

namespace WMS.Context
{

    public class WMSDbContext : DbContext
    {
        //$DbSet
        public DbSet<SpareFieldDetail> SpareFieldDetails { get; set; }



        public DbSet<BaseGeneralConfig> BaseGeneralConfig { get; set; }
        public DbSet<InspectType> InspectType { get; set; }
        public DbSet<Inspect_MT> Inspect_MT { get; set; }
        public DbSet<Category_MT> Category_MT { get; set; }
        public DbSet<Item_MT> Item_MT { get; set; }
        public DbSet<ItemSet_MT> ItemSet_MT { get; set; }
        public DbSet<ItemSetDetail> ItemSetDetail { get; set; }
        public DbSet<Unit_MT> Unit_MT { get; set; }
        public DbSet<DimensionLayout_MT> DimensionLayout_MT { get; set; }
        public DbSet<LabelLayoutHeader_MT> LabelLayoutHeader_MT { get; set; }
        public DbSet<LabelLayoutDetail_MT> LabelLayoutDetail_MT { get; set; }
        public DbSet<ImportDefinitionHeader_MT> ImportDefinitionHeader_MT { get; set; }
        public DbSet<ImportDefinitionDetail_MT> ImportDefinitionDetail_MT { get; set; }
        public DbSet<ItemInspectMapping> ItemInspectMapping { get; set; }
        public DbSet<ItemUnitMapping> ItemUnitMapping { get; set; }
        public DbSet<ReportLayoutHeader_MT> ReportLayoutHeader_MT { get; set; }
        public DbSet<ReportLayout_MT> ReportLayout_MT { get; set; }
        public DbSet<Location_MT> Location_MT { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Receive> Receive { get; set; }
        public DbSet<Supplier_MT> Supplier_MT { get; set; }
        public DbSet<ZoneLayoutHeader_MT> ZoneLayoutHeader_MT { get; set; }
        public DbSet<ZoneLayoutDetail_MT> ZoneLayoutDetail_MT { get; set; }
        public DbSet<Warehouse_MT> Warehouse_MT { get; set; }
        public DbSet<ZoneType> ZoneType { get; set; }
        public DbSet<SpareField> SpareField { get; set; }
        public DbSet<Control_MT> Control_MT { get; set; }
        public DbSet<GroupLocation> GroupLocation { get; set; }
        public DbSet<LocationType> LocationType { get; set; }
        public DbSet<InventoryTransaction> InventoryTransaction { get; set; }
        public DbSet<InventoryTransactionDetail> InventoryTransactionDetail { get; set; }
        public DbSet<InventoryDetail> InventoryDetail { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<File_MT> File_MT { get; set; }

        public WMSDbContext() : base("name=YUT_WMS")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static WMSDbContext Create()
        {
            return new WMSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public string ProcGetNewID(string prefixes)
        {
            var prefixesParameter = new SqlParameter
            {
                ParameterName = "Prefixes",
                Value = prefixes
            };

            return Database.SqlQuery<string>("exec ProcGetNewID @Prefixes", prefixesParameter).SingleOrDefault();
        }

        public ICollection<SpareFieldsDto> ProcGetSpareFieldsByTableAndRefID(int ProjectIDSys, string TableName, int RefID = 0)
        {
            var x1 = new SqlParameter { ParameterName = "ProjectIDSys", Value = ProjectIDSys };
            var x2 = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var x3 = new SqlParameter { ParameterName = "RefID", Value = RefID };
            return Database.SqlQuery<SpareFieldsDto>("exec ProcGetSpareFieldsByTableAndRefID @ProjectIDSys , @TableName , @RefID", x1, x2, x3).ToList();

        }

        public IEnumerable<CheckDependentPKDto> ProcCheckDependentPK(string TableName, string ColumnName, string Value = "")
        {
            var x1 = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var x2 = new SqlParameter { ParameterName = "ColumnName", Value = ColumnName };
            var x3 = new SqlParameter { ParameterName = "Value", Value = Value };
            return Database.SqlQuery<CheckDependentPKDto>("exec ProcCheckDependentPK @tableName , @columnName , @value", x1, x2, x3).ToList();
        }


        public Nullable<int> ProcCreateLabelLayout(string forTable, string formatName, Nullable<decimal> width, string widthUnit, Nullable<decimal> height, string heightUnit, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
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

            var widthParameter = width.HasValue ? new SqlParameter
            {
                ParameterName = "Width",
                Value = width
            } : new SqlParameter("Width", 0);

            var widthUnitParameter = widthUnit != null ? new SqlParameter
            {
                ParameterName = "WidthUnit",
                Value = widthUnit
            } : new SqlParameter("WidthUnit", DBNull.Value);

            var heightParameter = height.HasValue ? new SqlParameter
            {
                ParameterName = "Height",
                Value = height
            } : new SqlParameter("Height", 0);

            var heightUnitParameter = heightUnit != null ? new SqlParameter
            {
                ParameterName = "HeightUnit",
                Value = heightUnit
            } : new SqlParameter("HeightUnit", DBNull.Value);

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
            } : new SqlParameter("UserUpdate", DateTime.Now);

            var xmlDetailParameter = xmlDetail != null ? new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            } : new SqlParameter("XmlDetail", DateTime.Now);

            return Database.SqlQuery<Nullable<int>>("exec ProcCreateLabelLayout @ForTable , @FormatName , @Width , @WidthUnit ," +
    "@Height , @HeightUnit , @CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", forTableParameter, formatNameParameter, widthParameter,
    widthUnitParameter, heightParameter, heightUnitParameter, createdDateParameter, updatedDateParameter,
    userUpdateParameter, xmlDetailParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateLabelLayout", forTableParameter, formatNameParameter, widthParameter, widthUnitParameter, heightParameter, heightUnitParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public object ProcUpdateLabelLayout(Nullable<int> labelIDSys, string formatName, Nullable<decimal> width, string widthUnit, Nullable<decimal> height, string heightUnit, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var labelIDSysParameter = labelIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "LabelIDSys",
                Value = labelIDSys
            } : new SqlParameter("LabelIDSys", 0);

            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var widthParameter = width.HasValue ? new SqlParameter
            {
                ParameterName = "Width",
                Value = width
            } : new SqlParameter("Width", 0);

            var widthUnitParameter = widthUnit != null ? new SqlParameter
            {
                ParameterName = "WidthUnit",
                Value = widthUnit
            } : new SqlParameter("WidthUnit", DBNull.Value);

            var heightParameter = height.HasValue ? new SqlParameter
            {
                ParameterName = "Height",
                Value = height
            } : new SqlParameter("Height", 0);

            var heightUnitParameter = heightUnit != null ? new SqlParameter
            {
                ParameterName = "HeightUnit",
                Value = heightUnit
            } : new SqlParameter("HeightUnit", DBNull.Value);


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

            return Database.SqlQuery<object>("exec ProcUpdateLabelLayout @LabelIDSys , @FormatName , @Width , @WidthUnit ," +
    "@Height , @HeightUnit , @UpdatedDate , @UserUpdate , @XmlDetail ", labelIDSysParameter, formatNameParameter, widthParameter,
    widthUnitParameter, heightParameter, heightUnitParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateLabelLayout", labelIDSysParameter, formatNameParameter, widthParameter, widthUnitParameter, heightParameter, heightUnitParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
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

        public IEnumerable<int> ProcInsertImportHistory(Nullable<int> importDefinitionIDSys, string fileName, string result, Nullable<bool> success, Nullable<System.DateTime> createdDate, string userUpdate)
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


            return Database.SqlQuery<int>("exec ProcInsertImportHistory @ImportDefinitionIDSys , @FileName , @Result " +
                ", @Success , @CreatedDate , @UserUpdate ", importDefinitionIDSysParameter, fileNameParameter,
                resultParameter, successParameter, createdDateParameter, userUpdateParameter);
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

        public Nullable<int> ProcCreateReportLayout(string forTable, string formatName, string formatType, string fileExtention, string delimiter, string textGualifier, string encoding, Nullable<int> startExportRow, Nullable<bool> includeHeader, Nullable<bool> addHeaderLayout, string headerLayout, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
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

            var formatTypeParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatType",
                Value = formatType
            } : new SqlParameter("FormatType", DBNull.Value);

            var fileExtentionParameter = fileExtention != null ? new SqlParameter
            {
                ParameterName = "FileExtention",
                Value = fileExtention
            } : new SqlParameter("FileExtention", DBNull.Value);

            var delimiterParameter = delimiter != null ? new SqlParameter
            {
                ParameterName = "Delimiter",
                Value = delimiter
            } : new SqlParameter("Delimiter", DBNull.Value);

            var textGualifierParameter = textGualifier != null ? new SqlParameter
            {
                ParameterName = "TextGualifier",
                Value = textGualifier
            } : new SqlParameter("TextGualifier", DBNull.Value);

            var encodingParameter = encoding != null ? new SqlParameter
            {
                ParameterName = "Encoding",
                Value = encoding
            } : new SqlParameter("Encoding", DBNull.Value);

            var startExportRowParameter = startExportRow.HasValue ? new SqlParameter
            {
                ParameterName = "StartExportRow",
                Value = startExportRow
            } : new SqlParameter("StartExportRow", 0);

            var includeHeaderParameter = includeHeader.HasValue ? new SqlParameter
            {
                ParameterName = "IncludeHeader",
                Value = includeHeader
            } : new SqlParameter("IncludeHeader", false);

            var addHeaderLayoutParameter = addHeaderLayout.HasValue ? new SqlParameter
            {
                ParameterName = "AddHeaderLayout",
                Value = addHeaderLayout
            } : new SqlParameter("AddHeaderLayout", false);

            var headerLayoutParameter = headerLayout != null ? new SqlParameter
            {
                ParameterName = "HeaderLayout",
                Value = headerLayout
            } : new SqlParameter("HeaderLayout", DBNull.Value);

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

            return Database.SqlQuery<Nullable<int>>("exec ProcCreateReportLayout @ForTable , @FormatName , @FormatType , @FileExtention ," +
                "@Delimiter , @TextGualifier , @Encoding , @StartExportRow , @IncludeHeader , @AddHeaderLayout , @HeaderLayout ," +
                "@CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", forTableParameter, formatNameParameter, formatTypeParameter,
                fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter,
                includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter,
                userUpdateParameter, xmlDetailParameter).FirstOrDefault();
            //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateReportLayout", forTableParameter, formatNameParameter, formatTypeParameter, fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter, includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public object ProcUpdateReportLayout(Nullable<int> reportIDSys, string formatName, string formatType, string fileExtention, string delimiter, string textGualifier, string encoding, Nullable<int> startExportRow, Nullable<bool> includeHeader, Nullable<bool> addHeaderLayout, string headerLayout, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {

            var reportIDSysParameter = reportIDSys != null ? new SqlParameter
            {
                ParameterName = "ReportIDSys",
                Value = reportIDSys
            } : new SqlParameter("ReportIDSys", 0);

            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var formatTypeParameter = formatType != null ? new SqlParameter
            {
                ParameterName = "FormatType",
                Value = formatType
            } : new SqlParameter("FormatType", DBNull.Value);

            var fileExtentionParameter = fileExtention != null ? new SqlParameter
            {
                ParameterName = "FileExtention",
                Value = fileExtention
            } : new SqlParameter("FileExtention", DBNull.Value);

            var delimiterParameter = delimiter != null ? new SqlParameter
            {
                ParameterName = "Delimiter",
                Value = delimiter
            } : new SqlParameter("Delimiter", DBNull.Value);

            var textGualifierParameter = textGualifier != null ? new SqlParameter
            {
                ParameterName = "TextGualifier",
                Value = textGualifier
            } : new SqlParameter("TextGualifier", DBNull.Value);

            var encodingParameter = encoding != null ? new SqlParameter
            {
                ParameterName = "Encoding",
                Value = encoding
            } : new SqlParameter("Encoding", DBNull.Value);

            var startExportRowParameter = startExportRow != null ? new SqlParameter
            {
                ParameterName = "StartExportRow",
                Value = startExportRow
            } : new SqlParameter("StartExportRow", DBNull.Value);

            var includeHeaderParameter = includeHeader != null ? new SqlParameter
            {
                ParameterName = "IncludeHeader",
                Value = includeHeader
            } : new SqlParameter("IncludeHeader", DBNull.Value);

            var addHeaderLayoutParameter = addHeaderLayout != null ? new SqlParameter
            {
                ParameterName = "AddHeaderLayout",
                Value = addHeaderLayout
            } : new SqlParameter("AddHeaderLayout", DBNull.Value);

            var headerLayoutParameter = headerLayout != null ? new SqlParameter
            {
                ParameterName = "HeaderLayout",
                Value = headerLayout
            } : new SqlParameter("HeaderLayout", DBNull.Value);

            var createdDateParameter = createdDate != null ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DBNull.Value);

            var updatedDateParameter = updatedDate != null ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DBNull.Value);

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

            return Database.SqlQuery<object>("exec ProcUpdateReportLayout @ReportIDSys , @FormatName , @FormatType , @FileExtention ," +
    "@Delimiter , @TextGualifier , @Encoding , @StartExportRow , @IncludeHeader , @AddHeaderLayout , @HeaderLayout ," +
    "@CreatedDate , @UpdatedDate , @UserUpdate , @XmlDetail ", reportIDSysParameter, formatNameParameter, formatTypeParameter,
    fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter,
    includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter,
    userUpdateParameter, xmlDetailParameter).FirstOrDefault();
            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateReportLayout", reportIDSysParameter, formatNameParameter, formatTypeParameter, fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter, includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public List<RackLayout> ProcGetRackLayout(Nullable<int> zoneIDSys, Nullable<int> zoneID)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneIDSys",
                Value = zoneIDSys
            } : new SqlParameter("ZoneIDSys", 0);

            var zoneIDParameter = zoneID.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneID",
                Value = zoneID
            } : new SqlParameter("ZoneID", 0);

            return Database.SqlQuery<RackLayout>("exec ProcGetRackLayout @ZoneIDSys , @ZoneID", zoneIDSysParameter, zoneIDParameter).ToList();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RackLayout>("ProcGetRackLayout", zoneIDSysParameter, zoneIDParameter);
        }

        public List<RackLayout> ProcGetRackLayoutByZoneIDSys(Nullable<int> zoneIDSys)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneIDSys",
                Value = zoneIDSys
            } : new SqlParameter("ZoneIDSys", 0);

            return Database.SqlQuery<RackLayout>("exec ProcGetRackLayoutByZoneIDSys @ZoneIDSys ", zoneIDSysParameter).ToList();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RackLayout>("ProcGetRackLayoutByZoneIDSys", zoneIDSysParameter);
        }

        public Nullable<int> ProcCreateZoneLayout(string zoneName, string warehouse, string area, Nullable<int> totalFloor, string userUpdate, string xmlDetail)
        {
            var zoneNameParameter = zoneName != null ? new SqlParameter
            {
                ParameterName = "ZoneName",
                Value = zoneName
            } : new SqlParameter("ZoneName", DBNull.Value);

            var warehouseParameter = warehouse != null ? new SqlParameter
            {
                ParameterName = "Warehouse",
                Value = warehouse
            } : new SqlParameter("Warehouse", DBNull.Value);

            var areaParameter = area != null ? new SqlParameter
            {
                ParameterName = "Area",
                Value = area
            } : new SqlParameter("Area", DBNull.Value);

            var totalFloorParameter = totalFloor.HasValue ? new SqlParameter
            {
                ParameterName = "TotalFloor",
                Value = totalFloor
            } : new SqlParameter("TotalFloor", 0);

            var createByParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "CreateBy",
                Value = userUpdate
            } : new SqlParameter("CreateBy", DBNull.Value);

            var updateByParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UpdateBy",
                Value = userUpdate
            } : new SqlParameter("UpdateBy", DBNull.Value);

            var xmlDetailParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            };

            var createAtParameter = new SqlParameter("CreateAt", DateTime.Now);
            var updateAtParameter = new SqlParameter("UpdateAt", DateTime.Now);
            var isActiveParameter = new SqlParameter("IsActive", true);

            return Database.SqlQuery<Nullable<int>>("exec ProcCreateZoneLayout @ZoneName , @Warehouse ,@Area ,@TotalFloor ,@CreateAt, @CreateBy , @UpdateAt ,@UpdateBy ,@IsActive,@XmlDetail "
                , zoneNameParameter
                , warehouseParameter
                , areaParameter
                , totalFloorParameter
                , createAtParameter
                , createByParameter
                , updateAtParameter
                , updateByParameter
                , isActiveParameter
                , xmlDetailParameter).FirstOrDefault();
            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateZoneLayout", zoneNameParameter, warehouseParameter, areaParameter, totalFloorParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public object ProcUpdateZoneLayout(Nullable<int> zoneIDSys, string zoneName, string warehouse, string area, Nullable<int> totalFloor, string userUpdate, string xmlDetail)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneIDSys",
                Value = zoneIDSys
            } : new SqlParameter("ZoneIDSys", 0);

            var zoneNameParameter = zoneName != null ? new SqlParameter
            {
                ParameterName = "ZoneName",
                Value = zoneName
            } : new SqlParameter("ZoneName", DBNull.Value);

            var warehouseParameter = warehouse != null ? new SqlParameter
            {
                ParameterName = "Warehouse",
                Value = warehouse
            } : new SqlParameter("Warehouse", DBNull.Value);

            var areaParameter = area != null ? new SqlParameter
            {
                ParameterName = "Area",
                Value = area
            } : new SqlParameter("Area", DBNull.Value);

            var totalFloorParameter = totalFloor.HasValue ? new SqlParameter
            {
                ParameterName = "TotalFloor",
                Value = totalFloor
            } : new SqlParameter("TotalFloor", 0);

            var updateByParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UpdateBy",
                Value = userUpdate
            } : new SqlParameter("UpdateBy", DBNull.Value);

            var xmlDetailParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            };

            var updateAtParameter = new SqlParameter("UpdateAt", DateTime.Now);
            var isActiveParamerter = new SqlParameter("IsActive", true);

            return Database.SqlQuery<object>("exec ProcUpdateZoneLayout @ZoneIDSys, @ZoneName , @Warehouse ,@Area ,@TotalFloor , @UpdateAt ,@UpdateBy,@IsActive , @XmlDetail "
                , zoneIDSysParameter
                , zoneNameParameter
                , warehouseParameter
                , areaParameter
                , totalFloorParameter
                , updateAtParameter
                , updateByParameter
                , isActiveParamerter
                , xmlDetailParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateZoneLayout", zoneIDSysParameter, zoneNameParameter, warehouseParameter, areaParameter, totalFloorParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcCreateRackLayout(Nullable<int> zoneIDSys, Nullable<int> zoneID, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var zoneIDSysParameter = zoneIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneIDSys",
                Value = zoneIDSys
            } : new SqlParameter("ZoneIDSys", 0);


            var zoneIDParameter = zoneID.HasValue ? new SqlParameter
            {
                ParameterName = "ZoneID",
                Value = zoneID
            } : new SqlParameter("ZoneID", 0);

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
                Value = updatedDate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            var xmlDetailParameter = new SqlParameter
            {
                ParameterName = "XmlDetail",
                Value = xmlDetail
            };
            return Database.SqlQuery<int>("exec ProcCreateRackLayout @ZoneIDSys ,@ZoneID ," +
               "@CreatedDate , @UpdatedDate ,@UserUpdate , @XmlDetail ", zoneIDSysParameter, zoneIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter).FirstOrDefault();
            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcCreateRackLayout", zoneIDSysParameter, zoneIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public Nullable<int> ProcCreateDimensionLayout(string formatName, string unit, Nullable<double> width, Nullable<double> length, Nullable<double> height, Nullable<double> weight, string type, string color, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate)
        {
            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var unitParameter = unit != null ? new SqlParameter
            {
                ParameterName = "Unit",
                Value = unit
            } : new SqlParameter("Unit", DBNull.Value);

            var widthParameter = width.HasValue ? new SqlParameter
            {
                ParameterName = "Width",
                Value = width
            } : new SqlParameter("Width", 0);

            var lengthParameter = length.HasValue ? new SqlParameter
            {
                ParameterName = "Length",
                Value = length
            } : new SqlParameter("Length", 0);

            var heightParameter = height.HasValue ? new SqlParameter
            {
                ParameterName = "Height",
                Value = height
            } : new SqlParameter("Height", 0);

            var weightParameter = weight.HasValue ? new SqlParameter
            {
                ParameterName = "Weight",
                Value = weight
            } : new SqlParameter("Weight", 0);

            var typeParameter = type != null ? new SqlParameter
            {
                ParameterName = "Type",
                Value = type
            } : new SqlParameter("Type", DBNull.Value);

            var colorParameter = color != null ? new SqlParameter
            {
                ParameterName = "Color",
                Value = color
            } : new SqlParameter("Color", DBNull.Value);

            var createdDateParameter = createdDate != null ? new SqlParameter
            {
                ParameterName = "CreatedDate",
                Value = createdDate
            } : new SqlParameter("CreatedDate", DateTime.Now);


            var updatedDateParameter = updatedDate != null ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            return Database.SqlQuery<Nullable<int>>("exec ProcCreateDimensionLayout @FormatName ,@Unit ,@Width ,@Length" +
                " , @Height ,@Weight ,@Type ,@Color , @CreatedDate ,@UpdatedDate , @UserUpdate "
                , formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, createdDateParameter, updatedDateParameter, userUpdateParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateDimensionLayout", 
            //    formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, createdDateParameter, updatedDateParameter, userUpdateParameter);
        }

        public Nullable<int> ProcUpdateDimensionLayout(Nullable<int> dimensionIDSys, string formatName, string unit, Nullable<double> width, Nullable<double> length, Nullable<double> height, Nullable<double> weight, string type, string color, Nullable<System.DateTime> updatedDate, string userUpdate)
        {
            var dimensionIDSysParameter = dimensionIDSys.HasValue ? new SqlParameter
            {
                ParameterName = "DimensionIDSys",
                Value = dimensionIDSys
            } : new SqlParameter("DimensionIDSys", 0);

            var formatNameParameter = formatName != null ? new SqlParameter
            {
                ParameterName = "FormatName",
                Value = formatName
            } : new SqlParameter("FormatName", DBNull.Value);

            var unitParameter = unit != null ? new SqlParameter
            {
                ParameterName = "Unit",
                Value = unit
            } : new SqlParameter("Unit", DBNull.Value);

            var widthParameter = width.HasValue ? new SqlParameter
            {
                ParameterName = "Width",
                Value = width
            } : new SqlParameter("Width", 0);

            var lengthParameter = length.HasValue ? new SqlParameter
            {
                ParameterName = "Length",
                Value = length
            } : new SqlParameter("Length", 0);

            var heightParameter = height.HasValue ? new SqlParameter
            {
                ParameterName = "Height",
                Value = height
            } : new SqlParameter("Height", 0);

            var weightParameter = weight.HasValue ? new SqlParameter
            {
                ParameterName = "Weight",
                Value = weight
            } : new SqlParameter("Weight", 0);

            var typeParameter = type != null ? new SqlParameter
            {
                ParameterName = "Type",
                Value = type
            } : new SqlParameter("Type", DBNull.Value);

            var colorParameter = color != null ? new SqlParameter
            {
                ParameterName = "Color",
                Value = color
            } : new SqlParameter("Color", DBNull.Value);

            var updatedDateParameter = updatedDate != null ? new SqlParameter
            {
                ParameterName = "UpdatedDate",
                Value = updatedDate
            } : new SqlParameter("UpdatedDate", DateTime.Now);

            var userUpdateParameter = userUpdate != null ? new SqlParameter
            {
                ParameterName = "UserUpdate",
                Value = userUpdate
            } : new SqlParameter("UserUpdate", DBNull.Value);

            return Database.SqlQuery<Nullable<int>>("exec ProcUpdateDimensionLayout @DimensionIDSys, @FormatName ,@Unit ,@Width ,@Length" +
               " , @Height ,@Weight ,@Type , @Color ,@UpdatedDate , @UserUpdate "
               , dimensionIDSysParameter, formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, updatedDateParameter, userUpdateParameter).FirstOrDefault();

            //return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcUpdateDimensionLayout", dimensionIDSysParameter, formatNameParameter, unitParameter, widthParameter, lengthParameter, heightParameter, weightParameter, typeParameter, colorParameter, updatedDateParameter, userUpdateParameter);
        }

        public string GetTableDescriptionWms(string tableName)
        {
            WMSDbContext wms = new WMSDbContext();
            return wms.Database.SqlQuery<string>("ProcGetTableDescription @tableName"
                , new SqlParameter("@tableName", tableName)).FirstOrDefault();
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
            string data;

            var rs = Database.SqlQuery<string>("ProcGetDataAutoComplete @columnNames, @tableName, @conditionColumnNames, @keyword", columnNamesParameter, tableNameParameter, conditionColumnNamesParameter, keywordParameter);

            data = rs.FirstOrDefault();

            //var y = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<string>
            //    ("exec ProcGetDataAutoComplete @columnNames,@tableName,@conditionColumnNames,@keyword", columnNamesParameter, tableNameParameter, conditionColumnNamesParameter, keywordParameter);
            return data;

        }

        public IEnumerable<TableColumnsDescription> GetTableColumnsDescription(string tableName)
        {
            IEnumerable<TableColumnsDescription> tableColumnsDescription;
            using (WMSDbContext Db = new WMSDbContext())
            {
                var tableNameParameter = /*new ObjectParameter("@tableName", tableName);*/
          new SqlParameter("tableName", tableName);

                tableColumnsDescription = Db.Database.SqlQuery<TableColumnsDescription>("ProcGetTableColumnsDescription @tableName", tableNameParameter).ToList();

            }
            return tableColumnsDescription;
        }
    }
}
