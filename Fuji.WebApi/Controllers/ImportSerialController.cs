using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using BarcodeLib;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using OfficeOpenXml;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Data;
using Fuji.Service.ItemImport;
using Fuji.Common.ValueObject;
using Fuji.Entity.ItemManagement;
using Microsoft.AspNet.Identity;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/external")]
    public class ImportSerialController : ApiController
    {

        private IItemImportService ItemImportService;
        public ImportSerialController(IItemImportService itemImportService)
        {
            this.ItemImportService = itemImportService;
        }

        // GET: api/Items
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead>> response = new ResponseData<IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead>>();
            try
            {
                string userName = User.Identity.GetUserName() ?? "SYSTEM";
                IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead> items = ItemImportService.GetItems();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/paging/{pageIndex}/{pageSize}")]
        public HttpResponseMessage GetPaging(int pageIndex, int pageSize)
        {
            ResponseData<DataImportSerailHead> response = new ResponseData<DataImportSerailHead>();
            try
            {
                int totalRecord = 0;
                string userName = User.Identity.GetUserName() ?? "SYSTEM";
                IEnumerable<ImportSerialHead> items = ItemImportService.GetItems(pageIndex, pageSize, out totalRecord);
                if (totalRecord > 0)
                {
                    DataImportSerailHead ret = new DataImportSerailHead(totalRecord, items);
                    response.SetStatus(HttpStatusCode.OK);
                    response.SetData(ret);
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        #region Picking
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/pickingGroup")]
        public HttpResponseMessage GetPickingGroups()
        {
            ResponseData<IEnumerable<FujiPickingGroup>> response = new ResponseData<IEnumerable<FujiPickingGroup>>();
            try
            {
                string userName = User.Identity.GetUserName() ?? "SYSTEM";
                IEnumerable<FujiPickingGroup> items = ItemImportService.GetPickingGroup();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/clearPicking/{id}")]
        public HttpResponseMessage ClearPickingGroups(string id)
        {
            ResponseData<Boolean> response = new ResponseData<Boolean>();
            try
            {
                string userName = User.Identity.GetUserName() ?? "SYSTEM";
                bool result = ItemImportService.ClearPickingGroup(id);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(result);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/pickingGroup/{id}")]
        public HttpResponseMessage GetPickingByOrderNo(string id)
        {
            ResponseData<List<FujiPickingGroup>> response = new ResponseData<List<FujiPickingGroup>>();
            try
            {
                string userName = User.Identity.GetUserName() ?? "SYSTEM";
                FujiPickingGroup result = ItemImportService.GetPickingByOrderNo(id);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(new List<FujiPickingGroup>() { result });
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("importSerial/exportPickingGroup/{id}")]
        public HttpResponseMessage ExportPickingGroup(string id)
        {
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            FujiPickingGroup pickingGroup = ItemImportService.GetPickingByOrderNo(id, true);
            if (pickingGroup != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_Picking_Group", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                Encoding encoding = Encoding.Default;
                DataTable dt = getFujiPickingGroupDataTable(pickingGroup);
                MemoryStream ms = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                    int idxRow = 1;
                    int idxCol = 1;
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Columns[col].ColumnName;
                    }

                    idxRow = idxRow + 1;
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            var cell = sheet.Cells[idxRow + row, idxCol + col];
                            if (dt.Rows[row][col] is DateTime)
                            {
                                DateTime ddt = (DateTime)dt.Rows[row][col];
                                cell.Value = ddt.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US"));
                            }
                            else
                                cell.Value = dt.Rows[row][col].ToString();

                        }
                    }

                    sheet.Cells.AutoFitColumns();

                    package.SaveAs(ms);

                }
                DownloadFile(ms, fileName);

            }
            result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

        private DataTable getFujiPickingGroupDataTable(FujiPickingGroup item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("HeadID"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Serial Number"));
                dataTable.Columns.Add(new DataColumn("Box Number"));
                dataTable.Columns.Add(new DataColumn("Item Group"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Item Type"));


                var q = from p in item.SerialDetail
                        orderby p.ItemGroup, p.ItemType
                        select p;

                foreach (var itemDetail in q)
                {
                    object[] obj = new object[7];
                    obj[0] = itemDetail.HeadID;
                    obj[1] = itemDetail.ItemCode;
                    obj[2] = itemDetail.SerialNumber;
                    obj[3] = itemDetail.BoxNumber;
                    obj[4] = itemDetail.ItemGroup;
                    obj[5] = itemDetail.Status;
                    obj[6] = itemDetail.ItemType;

                    dataTable.Rows.Add(obj);
                }
            }


            return dataTable;
        }


        #endregion

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [HttpGet]
        [Route("importSerial/{id}")]
        public HttpResponseMessage Get(string id)
        {
            IResponseData<ImportSerialHead> response = new ResponseData<ImportSerialHead>();
            try
            {
                ImportSerialHead item = ItemImportService.GetItemByDocID(id);
                response.SetData(item);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/ImportSerial/5
        // GET: api/Items/1
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/handy/{id}")]
        public HttpResponseMessage GetHandy(string id)
        {
            IResponseData<ItemImportDto> response = new ResponseData<ItemImportDto>();
            try
            {
                ItemImportDto item = ItemImportService.GetItemByDocID_Handy(id);
                response.SetData(item);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/header/{id}")]
        public HttpResponseMessage GetHeaderPDF(string id)
        {
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            try
            {
                response.SetData(1);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension;
            List<DataBarcode> barcodeList = new List<DataBarcode>();
            List<DataBarcodeDetail> barcodeDetailList = new List<DataBarcodeDetail>();
            BarcodeLib.Barcode bc = new BarcodeLib.Barcode();

            ImportSerialHead item = ItemImportService.GetItemByDocID(id);
            if (item != null)
            {
                string barcodeInfo = item.HeadID;
                byte[] barcodeImage = bc.EncodeToByte(TYPE.CODE128A, barcodeInfo, Color.Black, Color.White, 400, 200);
                DataBarcode barcode = new DataBarcode(
                    barcodeImage,
                    barcodeInfo,
                    item.WHID,
                    item.ItemCode,
                    item.InvoiceNumber,
                    item.LotNumber,
                    item.ReceivingDate.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US")),
                    item.Qty.ToString(),
                    item.Location);
                barcodeList.Add(barcode);
                if (item.ImportSerialDetail.Count() > 0)
                {
                    foreach (var itemDetail in item.ImportSerialDetail)
                    {
                        DataBarcodeDetail detail = new DataBarcodeDetail(itemDetail.ItemCode,
                            itemDetail.SerialNumber,
                            itemDetail.BoxNumber,
                            itemDetail.ItemGroup);
                        barcodeDetailList.Add(detail);
                    }

                }



            }

            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/GenerateHeaderReport.rdlc";

                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = barcodeList;

                ReportDataSource rds2 = new ReportDataSource();
                rds2.Name = "DataSet2";
                rds2.Value = barcodeDetailList;


                reportViewer.LocalReport.DataSources.Add(rds1);
                reportViewer.LocalReport.DataSources.Add(rds2);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }
            result.StatusCode = HttpStatusCode.OK;
            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
        }

        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/export/{id}")]
        public HttpResponseMessage ExportToExcel(string id)
        {

            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id);
            if (serialHead != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Import_For_L-CAT", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                Encoding encoding = Encoding.Default;
                DataTable dt = getImportSerailDataTable(serialHead);
                MemoryStream ms = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                    int idxRow = 1;
                    int idxCol = 1;
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Columns[col].ColumnName;
                    }

                    idxRow = idxRow + 1;
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            var cell = sheet.Cells[idxRow + row, idxCol + col];
                            if (dt.Rows[row][col] is DateTime)
                            {
                                DateTime ddt = (DateTime)dt.Rows[row][col];
                                cell.Value = ddt.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US"));
                            }
                            else
                                cell.Value = dt.Rows[row][col].ToString();

                        }
                    }

                    sheet.Cells.AutoFitColumns();

                    package.SaveAs(ms);

                }
                DownloadFile(ms, fileName);

            }
            result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

        [HttpGet]
        [Route("importSerial/export-waranty/{id}")]
        public HttpResponseMessage ExportToWarantyExcel(string id)
        {

            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id);
            if (serialHead != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_To_Waranty", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                Encoding encoding = Encoding.Default;
                DataTable dt = getImportSerailGroupDataTable(serialHead);
                MemoryStream ms = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                    int idxRow = 1;
                    int idxCol = 1;
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Columns[col].ColumnName;
                    }

                    idxRow = idxRow + 1;
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            var cell = sheet.Cells[idxRow + row, idxCol + col];
                            if (dt.Rows[row][col] is DateTime)
                            {
                                DateTime ddt = (DateTime)dt.Rows[row][col];
                                cell.Value = ddt.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US"));
                            }
                            else
                                cell.Value = dt.Rows[row][col].ToString();

                        }
                    }

                    sheet.Cells.AutoFitColumns();

                    package.SaveAs(ms);

                }
                DownloadFile(ms, fileName);

            }
            result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

        [HttpGet]
        [Route("importSerial/export-receive/{id}")]
        public HttpResponseMessage ExportToReceiveExcel(string id)
        {

            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id);
            if (serialHead != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_All_Receive", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                Encoding encoding = Encoding.Default;
                DataTable dt = getImportSerailByStatusDataTable(serialHead);
                MemoryStream ms = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                    int idxRow = 1;
                    int idxCol = 1;
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Columns[col].ColumnName;
                    }

                    idxRow = idxRow + 1;
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            var cell = sheet.Cells[idxRow + row, idxCol + col];
                            if (dt.Rows[row][col] is DateTime)
                            {
                                DateTime ddt = (DateTime)dt.Rows[row][col];
                                cell.Value = ddt.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US"));
                            }
                            else
                                cell.Value = dt.Rows[row][col].ToString();

                        }
                    }

                    sheet.Cells.AutoFitColumns();

                    package.SaveAs(ms);

                }
                DownloadFile(ms, fileName);

            }
            result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }


        private DataTable getImportSerailByStatusDataTable(ImportSerialHead item, string status = "RECEIVED")
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Warehouse Code"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Allocated Serial No. (Ref1)"));
                dataTable.Columns.Add(new DataColumn("Location No."));
                dataTable.Columns.Add(new DataColumn("Allocated Lot No. (Ref2)"));
                dataTable.Columns.Add(new DataColumn("Allocated INV.No (Ref3)"));
                dataTable.Columns.Add(new DataColumn("Shipping Date"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Ref4"));
                dataTable.Columns.Add(new DataColumn("Ref5"));
                dataTable.Columns.Add(new DataColumn("Spare 1 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 2 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 3 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 4 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 5 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 6 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 7 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 8 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 9 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 10 (Details)"));
                dataTable.Columns.Add(new DataColumn("Order No."));


                var itemGroups = from p in item.ImportSerialDetail
                                 where p.Status.Equals(status)
                                 group p
                                 by p.ItemGroup into g
                                 select new { GroupID = g.Key, GroupList = g.ToList() };

                foreach (var itemGroup in itemGroups)
                {
                    string sItemCode = "", sSerialNumber = "", sBoxNumber = "", sStatus = "", sOrderNo = "", ref5 = "", sSpare1 = "", sSpare3 = "", sSpare4 = "";

                    var groupOrdered = itemGroup.GroupList.OrderBy(d => d.ItemType).ToList();
                    for (int i = 0; i < groupOrdered.Count; i++)
                    {
                        if (i == 0)//Main serial
                        {
                            sItemCode = groupOrdered[i].ItemCode;
                            sSerialNumber = groupOrdered[i].SerialNumber;
                            sBoxNumber = groupOrdered[i].BoxNumber;
                            sStatus = groupOrdered[i].Status;
                            sOrderNo = groupOrdered[i].OrderNo;
                        }
                        else if (i == 1)//Secound serial
                            ref5 = groupOrdered[i].SerialNumber;
                        else if (i == 2)
                            sSpare1 = groupOrdered[i].SerialNumber;
                        else if (i == 3)
                            sSpare3 = groupOrdered[i].SerialNumber;
                        else if (i == 4)
                            sSpare4 = groupOrdered[i].SerialNumber;
                    }
                    object[] obj = new object[21];
                    obj[0] = item.WHID;
                    obj[1] = sItemCode;
                    obj[2] = sSerialNumber;
                    obj[3] = item.Location;
                    obj[4] = sBoxNumber;
                    obj[5] = item.InvoiceNumber;
                    obj[6] = item.ReceivingDate.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    obj[7] = sStatus;
                    obj[8] = item.Remark;
                    obj[9] = ref5;
                    obj[10] = sSpare1;
                    obj[11] = itemGroup.GroupID;
                    obj[12] = sSpare3;
                    obj[13] = sSpare4;
                    obj[14] = "";
                    obj[15] = "";
                    obj[16] = "";
                    obj[17] = "";
                    obj[18] = "";
                    obj[19] = "";
                    obj[20] = sOrderNo;
                    dataTable.Rows.Add(obj);
                }

            }



            return dataTable;
        }
        private DataTable getImportSerailDataTable(ImportSerialHead item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Warehouse Code"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Allocated Serial No. (Ref1)"));
                dataTable.Columns.Add(new DataColumn("Location No."));
                dataTable.Columns.Add(new DataColumn("Allocated Lot No. (Ref2)"));
                dataTable.Columns.Add(new DataColumn("Allocated INV.No (Ref3)"));
                dataTable.Columns.Add(new DataColumn("Shipping Date"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Ref4"));
                dataTable.Columns.Add(new DataColumn("Ref5"));
                dataTable.Columns.Add(new DataColumn("Spare 1 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 2 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 3 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 4 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 5 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 6 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 7 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 8 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 9 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 10 (Details)"));
                dataTable.Columns.Add(new DataColumn("Order No."));


                var itemGroups = from p in item.ImportSerialDetail
                                 group p
                                 by p.ItemGroup into g
                                 select new { GroupID = g.Key, GroupList = g.ToList() };

                foreach (var itemGroup in itemGroups)
                {
                    string sItemCode = "", sSerialNumber = "", sBoxNumber = "", sStatus = "", sOrderNo = "", ref5 = "", sSpare1 = "", sSpare3 = "", sSpare4 = "";

                    var groupOrdered = itemGroup.GroupList.OrderBy(d => d.ItemType).ToList();
                    for (int i = 0; i < groupOrdered.Count; i++)
                    {
                        if (i == 0)
                        {
                            sItemCode = groupOrdered[i].ItemCode;
                            sSerialNumber = groupOrdered[i].SerialNumber;
                            sBoxNumber = groupOrdered[i].BoxNumber;
                            sStatus = groupOrdered[i].Status;
                            sOrderNo = groupOrdered[i].OrderNo;
                        }
                        else if (i == 1)
                            ref5 = groupOrdered[i].SerialNumber;
                        else if (i == 2)
                            sSpare1 = groupOrdered[i].SerialNumber;
                        else if (i == 3)
                            sSpare3 = groupOrdered[i].SerialNumber;
                        else if (i == 4)
                            sSpare4 = groupOrdered[i].SerialNumber;
                    }
                    object[] obj = new object[21];
                    obj[0] = item.WHID;
                    obj[1] = sItemCode;
                    obj[2] = sSerialNumber;
                    obj[3] = item.Location;
                    obj[4] = sBoxNumber;
                    obj[5] = item.InvoiceNumber;
                    obj[6] = item.ReceivingDate.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    obj[7] = sStatus;
                    obj[8] = item.Remark;
                    obj[9] = ref5;
                    obj[10] = sSpare1;
                    obj[11] = itemGroup.GroupID;
                    obj[12] = sSpare3;
                    obj[13] = sSpare4;
                    obj[14] = "";
                    obj[15] = "";
                    obj[16] = "";
                    obj[17] = "";
                    obj[18] = "";
                    obj[19] = "";
                    obj[20] = sOrderNo;
                    dataTable.Rows.Add(obj);
                }
            }



            return dataTable;
        }
        private DataTable getImportSerailGroupDataTable(ImportSerialHead item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Item Group"));
                dataTable.Columns.Add(new DataColumn("RECDATE"));
                dataTable.Columns.Add(new DataColumn("Model"));
                dataTable.Columns.Add(new DataColumn("SERIAL"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Box No"));


                var q = from p in item.ImportSerialDetail
                        orderby p.ItemGroup, p.ItemType
                        select p;
                foreach (var itemDetail in q)
                {
                    object[] obj = new object[7];
                    obj[0] = itemDetail.ItemCode;
                    obj[1] = itemDetail.ItemGroup;
                    obj[2] = item.ReceivingDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                    obj[3] = "";
                    obj[4] = itemDetail.SerialNumber;
                    obj[5] = itemDetail.Status;
                    obj[6] = itemDetail.BoxNumber;

                    dataTable.Rows.Add(obj);
                }
            }


            return dataTable;
        }

        private static bool DownloadFile(MemoryStream ms, string fileName)
        {
            //if (!File.Exists(path))
            //    return false;

            System.Web.HttpResponse respone = System.Web.HttpContext.Current.Response;
            respone.ClearContent();
            respone.Clear();
            respone.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            respone.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            ms.WriteTo(respone.OutputStream);
            //respone.WriteFile(path);
            respone.Flush();
            respone.Close();

            return true;
        }


        // POST: api/ImportSerial
        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial")]
        public HttpResponseMessage Post([FromBody]ImportSerialHead item)
        {
            IResponseData<ImportSerialHead> response = new ResponseData<ImportSerialHead>();
            try
            {
                item.UserUpdate = User.Identity.GetUserName();
                ImportSerialHead newItem = ItemImportService.CreateItem(item);
                response.SetData(newItem);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Items/5
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPut]
        [Route("importSerial/{id}")]
        public HttpResponseMessage Put(string id, [FromBody]ImportSerialHead item)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                item.UserUpdate = User.Identity.GetUserName() ?? "SYSTEM";
                bool isUpated = ItemImportService.UpdateItem(id, item);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Items/5
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpDelete]
        [Route("importSerial/{id}")]
        public HttpResponseMessage Delete(string id)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                ItemImportService.DeleteItem(id);
                response.SetData(true);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/statusexport")]
        public HttpResponseMessage PostUpdateStatusExport([FromBody]ImportSerialHead item)
        {
            IResponseData<Boolean> response = new ResponseData<Boolean>();
            try
            {
                item.UserUpdate = User.Identity.GetUserName() ?? "SYSTEM";
                bool result = ItemImportService.UpdateStausExport(item);
                if (result)
                    response.SetStatus(HttpStatusCode.OK);
                else
                    response.SetStatus(HttpStatusCode.InternalServerError);

                response.SetData(result);

            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/autocomplete")]
        public HttpResponseMessage GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            IResponseData<string> response = new ResponseData<string>();
            if (string.IsNullOrEmpty(keyword))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                string result = ItemImportService.GetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword);
                response.SetData(result);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/bycolumn/{column}/{keyword}")]
        public HttpResponseMessage GetDataByColumn(string column, string keyword)
        {

            ResponseData<IEnumerable<ImportSerialHead>> response = new ResponseData<IEnumerable<ImportSerialHead>>();
            if (string.IsNullOrEmpty(keyword))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                IEnumerable<ImportSerialHead> result = ItemImportService.GetDataByColumn(column, keyword);
                response.SetData(result);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("ImportPickingListTest")]
        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = HttpContext.Current.Server.MapPath("~/Handy/Upload");
            DateTime d = DateTime.Now;
            string root = @"D:\Uploads\Fuji\" + d.Year.ToString() + "\\" + d.Month.ToString("00");//HttpContext.Current.Server.MapPath("~/Handy/Upload");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            ResponseData<List<ImportSerialDetail>> response = new ResponseData<List<ImportSerialDetail>>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    var fileName = Request.Headers.GetValues("fileName").FirstOrDefault().Replace("\"", string.Empty);
                    string filePath = System.IO.Path.GetDirectoryName(file.LocalFileName) + "\\" + fileName;
                    System.IO.File.Copy(file.LocalFileName, filePath);
                    System.IO.File.Delete(file.LocalFileName);
                }

            }
            catch (ValidationException e)
            {
                response.SetErrors(e.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("ImportPickingConnect")]
        public HttpResponseMessage GetTestConnection()
        {
            IResponseData<string> response = new ResponseData<string>();

            try
            {
                response.SetData("Connected");
                response.SetStatus(HttpStatusCode.OK);

            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("ImportPickingWin")]
        public HttpResponseMessage PostFormWin([FromBody]PickingFromWinRequest receive)
        {
            IResponseData<IEnumerable<ImportSerialDetail>> response = new ResponseData<IEnumerable<ImportSerialDetail>>();

            try
            {
                IEnumerable<ImportSerialDetail> listDetail = ItemImportService.UpdateStatus(receive.ListPicking, receive.UserID);
                response.SetData(listDetail);
                response.SetStatus(HttpStatusCode.OK);

            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.InternalServerError);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/setscanned")]
        public HttpResponseMessage SetScanned([FromBody]SetScannedRequest receive)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                receive.UserUpdate = User.Identity.GetUserName() ?? "SYSTEM";

                bool flag = ItemImportService.SetScanned(receive);
                response.SetData(flag ? 1 : 0);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/receive")]
        public HttpResponseMessage Receive([FromBody]ReceiveRequest receive)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                receive.UserUpdate = User.Identity.GetUserName() ?? "SYSTEM";

                bool flag = ItemImportService.Receive(receive);
                response.SetData(flag ? 1 : 0);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/handy/itemGroups/{orderNo}")]
        public HttpResponseMessage GetByOrder(string orderNo)
        {
            IResponseData<List<string>> response = new ResponseData<List<string>>();
            try
            {
                List<string> itemGroups = ItemImportService.GetItemGroupByOrderNo_Handy(orderNo);
                response.SetData(itemGroups);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/ConfirmPickingList")]
        public HttpResponseMessage ConfirmPickingList([FromBody]ConfirmPickingRequest confirmRequest)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                string username = User.Identity.GetUserName() ?? "SYSTEM";
                bool flag = ItemImportService.ConfirmPicking(confirmRequest, username);
                response.SetData(flag ? 1 : 0);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        /*[Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/registerRFID")]
        public HttpResponseMessage RegisterRFID([FromBody]RegisterRFIDRequest registerRequest)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                string username = User.Identity.GetUserName() ?? "SYSTEM";
                bool flag = ItemImportService.ConfirmPicking(registerRequest, username);
                response.SetData(flag ? 1 : 0);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }*/

        public class DataBarcode
        {
            public byte[] Barcode { get; private set; }
            public string BarcodeInfo { get; private set; }
            public string WarehouseCode { get; private set; }
            public string ItemCode { get; private set; }
            public string InvoiceNumber { get; private set; }
            public string LotNumber { get; private set; }
            public string ReceivingDate { get; private set; }
            public string Qty { get; private set; }
            public string Location { get; private set; }

            public DataBarcode(byte[] barcode, string barcodeInfo, string warehouseCode, string itemCode, string invoiceNumber, string lotNumber, string receivingDate, string qty, string location)
            {
                this.Barcode = barcode;
                this.BarcodeInfo = barcodeInfo;
                this.WarehouseCode = warehouseCode;
                this.ItemCode = itemCode;
                this.InvoiceNumber = invoiceNumber;
                this.LotNumber = lotNumber;
                this.ReceivingDate = receivingDate;
                this.Qty = qty;
                this.Location = location;
            }
        }
        public class DataBarcodeDetail
        {
            public string ItemCode { get; private set; }
            public string SerialNumber { get; private set; }
            public string BoxNumber { get; private set; }
            public string ItemGroup { get; private set; }
            public DataBarcodeDetail(string itemCode, string serialNumber, string boxNumber, string itemGroup)
            {
                this.ItemCode = itemCode;
                this.SerialNumber = serialNumber;
                this.BoxNumber = boxNumber;
                this.ItemGroup = itemGroup;
            }

        }
        public class DataImportSerailHead
        {

            public DataImportSerailHead(int totalRecord, IEnumerable<ImportSerialHead> items)
            {
                this.TotalRecord = totalRecord;
                this.Items = items;
            }
            public int TotalRecord { get; set; }
            public IEnumerable<ImportSerialHead> Items { get; set; }
        }
        public class PickingFromWinRequest
        {
            public string UserID { get; set; }
            public List<PickingRequest> ListPicking { get; set; }
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/registerRFID")]
        public HttpResponseMessage RegisterRFID_HANDY([FromBody]RegisterRFIDRequest registerRequest)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                string username = User.Identity.GetUserName() ?? "SYSTEM";
                bool flag = ItemImportService.RegisterRFID_HANDY(registerRequest, username);
                response.SetData(flag ? 1 : 0);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/byCriteria")]
        public HttpResponseMessage FindImportSerialDetailByCriteria([FromBody]ParameterSearch parameterSearch)
        {
            ResponseData<IEnumerable<ImportSerialDetail>> respones = new ResponseData<IEnumerable<ImportSerialDetail>>();
            try
            {
                IEnumerable<ImportSerialDetail> items = ItemImportService.FindImportSerialDetailByCriteria(parameterSearch);
                if (items != null)
                {
                    respones.SetStatus(HttpStatusCode.OK);
                    respones.SetData(items);
                }
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

    }
}
