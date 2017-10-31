using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Dimension;
using WIM.Core.Entity.SupplierManagement;
using WMS.Entity.ImportManagement;
using WMS.Entity.InspectionManagement;
using WMS.Entity.ItemManagement;
using WMS.Entity.LayoutManagement;
using WMS.Entity.Report;
using WMS.Entity.WarehouseManagement;

namespace WMS.Context
{

    public class WMSDbContext : DbContext
    {
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
        public DbSet<ReportLayoutHeader_MT> ReportLayoutHeader_MT { get; set; }
        public DbSet<Location_MT> Location_MT { get; set; }
        public DbSet<Supplier_MT> Supplier_MT { get; set; }
        public DbSet<ZoneLayoutHeader_MT> ZoneLayoutHeader_MT { get; set; }
        public DbSet<ZoneLayoutDetail_MT> ZoneLayoutDetail_MT { get; set; }
        public DbSet<Warehouse_MT> Warehouse_MT { get; set; }
        

        public WMSDbContext() : base("name=DefaultConnection")
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

        public ObjectResult<Nullable<int>> ProcCreateLabelLayout(string forTable, string formatName, Nullable<decimal> width, string widthUnit, Nullable<decimal> height, string heightUnit, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var forTableParameter = forTable != null ?
                new ObjectParameter("ForTable", forTable) :
                new ObjectParameter("ForTable", typeof(string));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var widthParameter = width.HasValue ?
                new ObjectParameter("Width", width) :
                new ObjectParameter("Width", typeof(decimal));

            var widthUnitParameter = widthUnit != null ?
                new ObjectParameter("WidthUnit", widthUnit) :
                new ObjectParameter("WidthUnit", typeof(string));

            var heightParameter = height.HasValue ?
                new ObjectParameter("Height", height) :
                new ObjectParameter("Height", typeof(decimal));

            var heightUnitParameter = heightUnit != null ?
                new ObjectParameter("HeightUnit", heightUnit) :
                new ObjectParameter("HeightUnit", typeof(string));

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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateLabelLayout", forTableParameter, formatNameParameter, widthParameter, widthUnitParameter, heightParameter, heightUnitParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcUpdateLabelLayout(Nullable<int> labelIDSys, string formatName, Nullable<decimal> width, string widthUnit, Nullable<decimal> height, string heightUnit, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var labelIDSysParameter = labelIDSys.HasValue ?
                new ObjectParameter("LabelIDSys", labelIDSys) :
                new ObjectParameter("LabelIDSys", typeof(int));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var widthParameter = width.HasValue ?
                new ObjectParameter("Width", width) :
                new ObjectParameter("Width", typeof(decimal));

            var widthUnitParameter = widthUnit != null ?
                new ObjectParameter("WidthUnit", widthUnit) :
                new ObjectParameter("WidthUnit", typeof(string));

            var heightParameter = height.HasValue ?
                new ObjectParameter("Height", height) :
                new ObjectParameter("Height", typeof(decimal));

            var heightUnitParameter = heightUnit != null ?
                new ObjectParameter("HeightUnit", heightUnit) :
                new ObjectParameter("HeightUnit", typeof(string));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            var xmlDetailParameter = xmlDetail != null ?
                new ObjectParameter("XmlDetail", xmlDetail) :
                new ObjectParameter("XmlDetail", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateLabelLayout", labelIDSysParameter, formatNameParameter, widthParameter, widthUnitParameter, heightParameter, heightUnitParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public ObjectResult<Nullable<int>> ProcCreateImportDefinition(string forTable, string formatName, string delimiter, Nullable<int> maxHeading, string encoding, Nullable<bool> skipFirstRecode, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var forTableParameter = forTable != null ?
                new ObjectParameter("ForTable", forTable) :
                new ObjectParameter("ForTable", typeof(string));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var delimiterParameter = delimiter != null ?
                new ObjectParameter("Delimiter", delimiter) :
                new ObjectParameter("Delimiter", typeof(string));

            var maxHeadingParameter = maxHeading.HasValue ?
                new ObjectParameter("MaxHeading", maxHeading) :
                new ObjectParameter("MaxHeading", typeof(int));

            var encodingParameter = encoding != null ?
                new ObjectParameter("Encoding", encoding) :
                new ObjectParameter("Encoding", typeof(string));

            var skipFirstRecodeParameter = skipFirstRecode.HasValue ?
                new ObjectParameter("SkipFirstRecode", skipFirstRecode) :
                new ObjectParameter("SkipFirstRecode", typeof(bool));

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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateImportDefinition", forTableParameter, formatNameParameter, delimiterParameter, maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcUpdateImportDefinition(Nullable<int> importIDSys, string formatName, string delimiter, Nullable<int> maxHeading, string encoding, Nullable<bool> skipFirstRecode, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var importIDSysParameter = importIDSys.HasValue ?
                new ObjectParameter("ImportIDSys", importIDSys) :
                new ObjectParameter("ImportIDSys", typeof(int));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var delimiterParameter = delimiter != null ?
                new ObjectParameter("Delimiter", delimiter) :
                new ObjectParameter("Delimiter", typeof(string));

            var maxHeadingParameter = maxHeading.HasValue ?
                new ObjectParameter("MaxHeading", maxHeading) :
                new ObjectParameter("MaxHeading", typeof(int));

            var encodingParameter = encoding != null ?
                new ObjectParameter("Encoding", encoding) :
                new ObjectParameter("Encoding", typeof(string));

            var skipFirstRecodeParameter = skipFirstRecode.HasValue ?
                new ObjectParameter("SkipFirstRecode", skipFirstRecode) :
                new ObjectParameter("SkipFirstRecode", typeof(bool));

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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateImportDefinition", importIDSysParameter, formatNameParameter, delimiterParameter, maxHeadingParameter, encodingParameter, skipFirstRecodeParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public ObjectResult<string> ProcImportDataToTable(Nullable<int> importSysID, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlData)
        {
            var importSysIDParameter = importSysID.HasValue ?
                new ObjectParameter("ImportSysID", importSysID) :
                new ObjectParameter("ImportSysID", typeof(int));

            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));

            var updatedDateParameter = updatedDate.HasValue ?
                new ObjectParameter("UpdatedDate", updatedDate) :
                new ObjectParameter("UpdatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            var xmlDataParameter = xmlData != null ?
                new ObjectParameter("XmlData", xmlData) :
                new ObjectParameter("XmlData", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ProcImportDataToTable", importSysIDParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDataParameter);
        }

        public int ProcInsertImportHistory(Nullable<int> importDefinitionIDSys, string fileName, string result, Nullable<bool> success, Nullable<System.DateTime> createdDate, string userUpdate)
        {
            var importDefinitionIDSysParameter = importDefinitionIDSys.HasValue ?
                new ObjectParameter("ImportDefinitionIDSys", importDefinitionIDSys) :
                new ObjectParameter("ImportDefinitionIDSys", typeof(int));

            var fileNameParameter = fileName != null ?
                new ObjectParameter("FileName", fileName) :
                new ObjectParameter("FileName", typeof(string));

            var resultParameter = result != null ?
                new ObjectParameter("Result", result) :
                new ObjectParameter("Result", typeof(string));

            var successParameter = success.HasValue ?
                new ObjectParameter("Success", success) :
                new ObjectParameter("Success", typeof(bool));

            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));

            var userUpdateParameter = userUpdate != null ?
                new ObjectParameter("UserUpdate", userUpdate) :
                new ObjectParameter("UserUpdate", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcInsertImportHistory", importDefinitionIDSysParameter, fileNameParameter, resultParameter, successParameter, createdDateParameter, userUpdateParameter);
        }

        public int ProcDeleteImportDefinition(Nullable<int> importIDSys)
        {
            var importIDSysParameter = importIDSys.HasValue ?
                new ObjectParameter("ImportIDSys", importIDSys) :
                new ObjectParameter("ImportIDSys", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcDeleteImportDefinition", importIDSysParameter);
        }

        public ObjectResult<Nullable<int>> ProcCreateReportLayout(string forTable, string formatName, string formatType, string fileExtention, string delimiter, string textGualifier, string encoding, Nullable<int> startExportRow, Nullable<bool> includeHeader, Nullable<bool> addHeaderLayout, string headerLayout, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var forTableParameter = forTable != null ?
                new ObjectParameter("ForTable", forTable) :
                new ObjectParameter("ForTable", typeof(string));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var formatTypeParameter = formatType != null ?
                new ObjectParameter("FormatType", formatType) :
                new ObjectParameter("FormatType", typeof(string));

            var fileExtentionParameter = fileExtention != null ?
                new ObjectParameter("FileExtention", fileExtention) :
                new ObjectParameter("FileExtention", typeof(string));

            var delimiterParameter = delimiter != null ?
                new ObjectParameter("Delimiter", delimiter) :
                new ObjectParameter("Delimiter", typeof(string));

            var textGualifierParameter = textGualifier != null ?
                new ObjectParameter("TextGualifier", textGualifier) :
                new ObjectParameter("TextGualifier", typeof(string));

            var encodingParameter = encoding != null ?
                new ObjectParameter("Encoding", encoding) :
                new ObjectParameter("Encoding", typeof(string));

            var startExportRowParameter = startExportRow.HasValue ?
                new ObjectParameter("StartExportRow", startExportRow) :
                new ObjectParameter("StartExportRow", typeof(int));

            var includeHeaderParameter = includeHeader.HasValue ?
                new ObjectParameter("IncludeHeader", includeHeader) :
                new ObjectParameter("IncludeHeader", typeof(bool));

            var addHeaderLayoutParameter = addHeaderLayout.HasValue ?
                new ObjectParameter("AddHeaderLayout", addHeaderLayout) :
                new ObjectParameter("AddHeaderLayout", typeof(bool));

            var headerLayoutParameter = headerLayout != null ?
                new ObjectParameter("HeaderLayout", headerLayout) :
                new ObjectParameter("HeaderLayout", typeof(string));

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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ProcCreateReportLayout", forTableParameter, formatNameParameter, formatTypeParameter, fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter, includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
        }

        public int ProcUpdateReportLayout(Nullable<int> reportIDSys, string formatName, string formatType, string fileExtention, string delimiter, string textGualifier, string encoding, Nullable<int> startExportRow, Nullable<bool> includeHeader, Nullable<bool> addHeaderLayout, string headerLayout, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> updatedDate, string userUpdate, string xmlDetail)
        {
            var reportIDSysParameter = reportIDSys.HasValue ?
                new ObjectParameter("ReportIDSys", reportIDSys) :
                new ObjectParameter("ReportIDSys", typeof(int));

            var formatNameParameter = formatName != null ?
                new ObjectParameter("FormatName", formatName) :
                new ObjectParameter("FormatName", typeof(string));

            var formatTypeParameter = formatType != null ?
                new ObjectParameter("FormatType", formatType) :
                new ObjectParameter("FormatType", typeof(string));

            var fileExtentionParameter = fileExtention != null ?
                new ObjectParameter("FileExtention", fileExtention) :
                new ObjectParameter("FileExtention", typeof(string));

            var delimiterParameter = delimiter != null ?
                new ObjectParameter("Delimiter", delimiter) :
                new ObjectParameter("Delimiter", typeof(string));

            var textGualifierParameter = textGualifier != null ?
                new ObjectParameter("TextGualifier", textGualifier) :
                new ObjectParameter("TextGualifier", typeof(string));

            var encodingParameter = encoding != null ?
                new ObjectParameter("Encoding", encoding) :
                new ObjectParameter("Encoding", typeof(string));

            var startExportRowParameter = startExportRow.HasValue ?
                new ObjectParameter("StartExportRow", startExportRow) :
                new ObjectParameter("StartExportRow", typeof(int));

            var includeHeaderParameter = includeHeader.HasValue ?
                new ObjectParameter("IncludeHeader", includeHeader) :
                new ObjectParameter("IncludeHeader", typeof(bool));

            var addHeaderLayoutParameter = addHeaderLayout.HasValue ?
                new ObjectParameter("AddHeaderLayout", addHeaderLayout) :
                new ObjectParameter("AddHeaderLayout", typeof(bool));

            var headerLayoutParameter = headerLayout != null ?
                new ObjectParameter("HeaderLayout", headerLayout) :
                new ObjectParameter("HeaderLayout", typeof(string));

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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcUpdateReportLayout", reportIDSysParameter, formatNameParameter, formatTypeParameter, fileExtentionParameter, delimiterParameter, textGualifierParameter, encodingParameter, startExportRowParameter, includeHeaderParameter, addHeaderLayoutParameter, headerLayoutParameter, createdDateParameter, updatedDateParameter, userUpdateParameter, xmlDetailParameter);
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
