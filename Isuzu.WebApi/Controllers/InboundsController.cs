using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Text;
using System.Security.Claims;
using Isuzu.Service.Impl;
using Isuzu.Repository;
using Isuzu.Common.ValueObject;


namespace Isuzu.Service.Impl
{
    [RoutePrefix("api/v1/isuzu/inbounds")]
    public class InboundsController : ApiController
    {
        private IInboundService InboundService;

        public InboundsController(IInboundService inboundService)
        {
            this.InboundService = inboundService;
        }

        #region ======================== HANDY =============================
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/iszjOrder/{iszjOrder}")]
        public HttpResponseMessage GetInboundItemByISZJOrder_HANDY([FromUri]string iszjOrder)
        {
            ResponseData<InboundItemHandyDto> responseHandy = new ResponseData<InboundItemHandyDto>();
            try
            {
                InboundItemHandyDto item = InboundService.GetInboundItemByISZJOrder_HANDY(iszjOrder);
                responseHandy.SetData(item);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/items")]
        public HttpResponseMessage RegisterInboundItem_HANDY([FromBody]InboundItemHandyDto inboundItem)
        {
            if (inboundItem.IsRepeat == 0 && InboundService.CheckScanRepeatRegisterInboundItem_HANDY(inboundItem))
            {
                ResponseData<int> responseCheckRepeat = new ResponseData<int>();
                responseCheckRepeat.SetData(2);
                return Request.ReturnHttpResponseMessage(responseCheckRepeat);
            }

            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.RegisterInboundItem_HANDY(inboundItem, "SYSTEM");
                responseHandy.SetData(1);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/amountInInvoice/{rfid}")]
        public HttpResponseMessage GetAmountInboundItemInInvoiceByRFID_HANDY([FromUri]string rfid)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                int amount = InboundService.GetAmountInboundItemInInvoiceByRFID_HANDY(rfid);
                responseHandy.SetData(amount);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/rfid/{rfid}")]
        public HttpResponseMessage GetInboundItemByRFID_HANDY([FromUri]string rfid)
        {
            ResponseData<InboundItemHandyDto> responseHandy = new ResponseData<InboundItemHandyDto>();
            try
            {
                InboundItemHandyDto item = InboundService.GetInboundItemByRFID_HANDY(rfid);
                responseHandy.SetData(item);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/invoice/{invNo}")]
        public HttpResponseMessage GetInboundItemsByInvoice_HANDY([FromUri]string invNo)
        {
            ResponseData<IEnumerable<InboundItemHandyDto>> responseHandy = new ResponseData<IEnumerable<InboundItemHandyDto>>();
            try
            {
                IEnumerable<InboundItemHandyDto> items = InboundService.GetInboundItemsByInvoice_HANDY(invNo);
                responseHandy.SetData(items);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/holding")]
        public HttpResponseMessage PerformHolding_HANDY([FromBody]InboundItemHoldingHandyRequest inboundItemHolding)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.PerformHolding_HANDY(inboundItemHolding, "SYSTEM");
                responseHandy.SetData(1);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/shipping")]
        public HttpResponseMessage PerformShipping_HANDY([FromBody]InboundItemShippingHandyRequest inboundItemShipping)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.PerformShipping_HANDY(inboundItemShipping, "SYSTEM");
                responseHandy.SetData(1);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/packingCarton")]
        public HttpResponseMessage PerformPackingCarton_HANDY([FromBody]InboundItemCartonPackingHandyRequest itemReq)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.PerformPackingCarton_HANDY(itemReq, "SYSTEM");
                responseHandy.SetData(1);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/packingCase")]
        public HttpResponseMessage PerformPackingCase_HANDY([FromBody]InboundItemCasePackingHandyRequest itemReq)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.PerformPackingCase_HANDY(itemReq, "SYSTEM");
                responseHandy.SetData(1);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/rfid/carton/{rfid}")]
        public HttpResponseMessage GetInboundItemCartonByRFID_HANDY([FromUri]string rfid)
        {
            ResponseData<InboundItemCartonHandyDto> responseHandy = new ResponseData<InboundItemCartonHandyDto>();
            try
            {
                InboundItemCartonHandyDto item = InboundService.GetInboundItemCartonByRFID_HANDY(rfid);
                responseHandy.SetData(item);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("handy/items/rfids")]
        public HttpResponseMessage GetInboundItemsByRFIDs_HANDY([FromBody]RFIDList rfids)
        {
            ResponseData<IEnumerable<InboundItems>> responseHandy = new ResponseData<IEnumerable<InboundItems>>();
            try
            {
                IEnumerable<InboundItems> items = InboundService.GetInboundItemsByRFIDs_HANDY(rfids);
                responseHandy.SetData(items);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        #endregion

        #region =============================== DEFAULT =================================

        #endregion

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("items")]
        public HttpResponseMessage Get()
        {
            ResponseData<List<InboundItemsHead>> respones = new ResponseData<List<InboundItemsHead>>();
            try
            {
                List<InboundItemsHead> items = InboundService.GetInboundGroup();
                respones.SetData(items);
                respones.SetStatus(HttpStatusCode.OK);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("items/{pageIndex}/{pageSize}")]
        public HttpResponseMessage GetByPaging(int pageIndex,int pageSize)
        {
            ResponseData<DataInboundGroupItems> respones = new ResponseData<DataInboundGroupItems>();
            try
            {
                 
                int totalRecord = 0;
                List<InboundItemsHead> items = InboundService.GetInboundGroupPaging(pageIndex,pageSize,out totalRecord);
                if(items != null && totalRecord > 0)
                {
                    DataInboundGroupItems dataItem = new DataInboundGroupItems(totalRecord,items);
                    respones.SetData(dataItem);
                    respones.SetStatus(HttpStatusCode.OK);
                }
               
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("itemsInvNo/{invNo}")]
        public HttpResponseMessage GetByInvNo(string invNo)
        {
            ResponseData<InboundItemsHead> respones = new ResponseData<InboundItemsHead>();
            try
            {
                InboundItemsHead item = InboundService.GetInboundGroupByInvoiceNumber(invNo);
                if (item != null)
                {
                    respones.SetData(item);
                    respones.SetStatus(HttpStatusCode.OK);
                }

            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("items/{iszjOrder}")]
        public HttpResponseMessage GetInboundItemByISZJOrder([FromUri]string iszjOrder)
        {
            ResponseData<InboundItemHandyDto> responseHandy = new ResponseData<InboundItemHandyDto>();
            try
            {
                InboundItemHandyDto item = InboundService.GetInboundItemByISZJOrder_HANDY(iszjOrder);
                responseHandy.SetData(item);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }


        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("search/bycolumn/{column}/{keyword}")]
        public HttpResponseMessage GetDataByColumn(string column, string keyword)
        {

            ResponseData<IEnumerable<InboundItemsHead>> response = new ResponseData<IEnumerable<InboundItemsHead>>();
            if (string.IsNullOrEmpty(keyword))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                IEnumerable<InboundItemsHead> result = InboundService.GetDataGroupByColumn(column, keyword);
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
        [Route("itemImport/{pageIndex}/{pageSize}")]
        public HttpResponseMessage GetItemByPaging(int pageIndex, int pageSize)
        {
            ResponseData<DataInboundItems> respones = new ResponseData<DataInboundItems>();
            try
            {
                int totalRecord = 0;
                List<InboundItems> items = InboundService.GetInboundItemPaging(pageIndex, pageSize, out totalRecord);
                if (totalRecord > 0)
                {
                    DataInboundItems ret = new DataInboundItems(totalRecord, items);
                    respones.SetStatus(HttpStatusCode.OK);
                    respones.SetData(ret);
                }
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("itemImport/bycolumns")]
        public HttpResponseMessage SearchItemByColumn([FromBody]ParameterSearch parameterSearch)
        {
            ResponseData<IEnumerable<InboundItems>> respones = new ResponseData<IEnumerable<InboundItems>>();
            try
            {
                IEnumerable<InboundItems> items = InboundService.GetDataByColumn(parameterSearch);
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

        //[HttpPost]
        //[Route("import")]
        //public HttpResponseMessage PostFormIText([FromBody]List<InboundItem> receive)
        //{
        //    IResponseData<List<InboundItem>> response = new ResponseData<List<InboundItem>>();

        //    try
        //    {
        //        List<InboundItem> listDetail = InboundService.ImportInboundItemList(receive);
        //        if(listDetail.Count > 0)
        //        {
        //            response.SetData(listDetail);
        //        }
        //        else
        //            response.SetData(null);

        //        response.SetStatus(HttpStatusCode.OK);

        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.OK);
        //    }

        //    return Request.ReturnHttpResponseMessage(response);
        //}

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("import")]
        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            DateTime d = DateTime.Now;
            string root = @"D:\Uploads\Isuzu\" + d.Year.ToString() + "\\" + d.Month.ToString("00");//HttpContext.Current.Server.MapPath("~/Handy/Upload");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var userID = "SYSTEM";
            ResponseData<List<InboundItems>> response = new ResponseData<List<InboundItems>>();
            List<InboundItems> inboundList = new List<InboundItems>();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);


                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    int num = new Random().Next(100);
                    string dir = System.IO.Path.GetDirectoryName(file.LocalFileName);
                    string newFileName = dir + "\\" + "IMPORT_" + num + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                    System.IO.File.Copy(file.LocalFileName, newFileName + ".xlsx");
                    System.IO.File.Delete(file.LocalFileName);

                    string path = newFileName + ".xlsx";

                    #region ExcelPackage
                    using (var pck = new ExcelPackage())
                    {
                        using (var stream = File.OpenRead(path))
                        {
                            pck.Load(stream);
                        }

                        var ws = pck.Workbook.Worksheets.First();
                        if (ws != null)
                        {

                            for (int i = 2; i <= ws.Dimension.End.Row; i++)
                            {
                                if (!string.IsNullOrEmpty(ws.Cells[i, 1].Text))
                                {
                                    InboundItems inbound = new InboundItems();
                                    inbound.InvNo = GetAutoGen(ws.Cells[i, 1].Text);
                                    inbound.SeqNo = Convert.ToInt32(ws.Cells[i, 2].Text);
                                    inbound.ITAOrder = ws.Cells[i, 3].Text;
                                    inbound.ISZJOrder = ws.Cells[i, 4].Text;
                                    inbound.PartNo = ws.Cells[i, 5].Text;
                                    inbound.ParrtName = ws.Cells[i, 6].Text;
                                    inbound.Qty = Convert.ToInt32(ws.Cells[i, 7].Text);
                                    inbound.Vendor = ws.Cells[i, 8].Text;
                                    inbound.Shelf = ws.Cells[i, 9].Text;
                                    inbound.Destination = ws.Cells[i, 10].Text;
                                    if (!string.IsNullOrEmpty(inbound.ISZJOrder))
                                        inboundList.Add(inbound);
                                }
                            }
                        }
                    }
                    #endregion

                  

                    var duplicateKeys = inboundList.GroupBy(gb => gb.ISZJOrder)
                         .Where(w => w.Count() > 1)
                         .Select(s => s.FirstOrDefault());


                    
                    if (duplicateKeys.Count() > 0)
                    {
                        inboundList = duplicateKeys.ToList();
                        response.SetData(inboundList);
                        response.SetStatus(HttpStatusCode.Conflict);
                    }
                    else
                    {
                        //response.SetData(InboundService.GetInboundItemByInvoiceNumber("V170646"));
                        //response.SetStatus(HttpStatusCode.OK);
                        List<InboundItems> listDuplicateInbound = InboundService.ImportInboundItemList(inboundList, userID);
                        if (listDuplicateInbound.Count > 0)
                        {
                            response.SetData(listDuplicateInbound);
                            response.SetStatus(HttpStatusCode.Conflict);
                        }
                        else
                        {
                            response.SetData(inboundList);
                            response.SetStatus(HttpStatusCode.OK);
                        }
                    }

                }

            }
            catch (ValidationException e)
            {
                response.SetErrors(e.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }
        private string GetAutoGen(string input)
        {
            //if (input.Length == 1)
            //{
            //    switch (input.Trim().ToUpper())
            //    {
            //        case "C":
            //            input = input + DateTime.Now.ToString("yyMMdd");
            //            break;
            //        case "R":
            //            input = input + DateTime.Now.ToString("yyMMdd");
            //            break;
            //    }
            //}
            return input;
        }

        [HttpGet]
        [Route("generateRFID/{qty}")]
        public HttpResponseMessage GenerateRFID(string qty)
        {
            ResponseData<bool> response = new ResponseData<bool>();
           
            try
            {
               
                response.SetData(true);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("export/{id}")]
        public HttpResponseMessage ExportToExcel(string id)
        {

            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            List<InboundItems> inboundItems = InboundService.GetInboundItemByInvoiceNumber(id,true);
            if (inboundItems != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_Isuzu_", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                Encoding encoding = Encoding.Default;
                DataTable dt = getExportInboundDataTable(inboundItems);
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

        private DataTable getExportInboundDataTable(List<InboundItems> items)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (items != null)
            {
                dataTable.Columns.Add(new DataColumn("No."));
                dataTable.Columns.Add(new DataColumn("ITA Order"));
                dataTable.Columns.Add(new DataColumn("ISZJ Order"));
                dataTable.Columns.Add(new DataColumn("Part No."));
                dataTable.Columns.Add(new DataColumn("Part Name"));
                dataTable.Columns.Add(new DataColumn("Q'ty"));
                dataTable.Columns.Add(new DataColumn("Vendor"));
                dataTable.Columns.Add(new DataColumn("Shelf"));
                dataTable.Columns.Add(new DataColumn("Destination"));
                dataTable.Columns.Add(new DataColumn("Carton No."));
                dataTable.Columns.Add(new DataColumn("Case No."));
                
                for (int i = 0; i < items.Count();i++) 
                {
                    //if(items[i].Status == IsuzuStatus.SHIPPED.ToString())
                    //{ 
                        object[] obj = new object[11];
                        obj[0] = items[i].SeqNo;
                        obj[1] = items[i].ITAOrder;
                        obj[2] = items[i].ISZJOrder;
                        obj[3] = items[i].PartNo;
                        obj[4] = items[i].ParrtName;
                        obj[5] = items[i].Qty.ToString();
                        obj[6] = items[i].Vendor;
                        obj[7] = items[i].Shelf;
                        obj[8] = items[i].Destination;
                        obj[9] = items[i].CartonNo;
                        obj[10] = items[i].CaseNo;
                        dataTable.Rows.Add(obj);
                    //}
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


        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("updateexport")]
        public HttpResponseMessage UpdateInboundGroup([FromBody]InboundItemsHead item)
        {

            ResponseData<bool> response = new ResponseData<bool>();

            try
            {
               
                if (item != null)
                {
                    var identity = User.Identity as ClaimsIdentity;
                    item.UpdateBy = identity != null ? (!string.IsNullOrEmpty(identity.GetUserName()) ? identity.GetUserName() : "SYSTEMa") : "SYSTEMb";

                    bool result = InboundService.UpdateStausExport(item);
                    response.SetData(result);
                    response.SetStatus(HttpStatusCode.OK);
                }
                
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpPost]
        [Route("deleteResonPath")]
        public async Task<HttpResponseMessage> PostDeleteReason()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            DateTime d = DateTime.Now;
            string root = @"D:\Uploads\Isuzu\delete\" + d.Year.ToString() + "\\" + d.Month.ToString("00");//HttpContext.Current.Server.MapPath("~/Handy/Upload");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            ResponseData<List<string>> response = new ResponseData<List<string>>();
            List<string> AllPath = new List<string>() { };
            List<InboundItems> inboundList = new List<InboundItems>();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                
                foreach (MultipartFileData file in provider.FileData)
                {
                    int num = new Random().Next(100);
                    string dir = System.IO.Path.GetDirectoryName(file.LocalFileName);
                    string newFileName = dir + "\\"+ "DELETED_"+ num + "_"  + DateTime.Now.ToString("ddMMyyyyHHmmss");
                    System.IO.File.Copy(file.LocalFileName, newFileName + ".pdf");
                    System.IO.File.Delete(file.LocalFileName);

                    string path = newFileName + ".pdf";
                    AllPath.Add(path);
                   
                }
                response.SetData(AllPath);
                response.SetStatus(HttpStatusCode.OK);

            }
            catch (ValidationException e)
            {
                response.SetErrors(e.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("deleteReson")]
        public HttpResponseMessage DeleteReason([FromBody]IsuzuDeleteReason item)
        {

            ResponseData<bool> response = new ResponseData<bool>();

            try
            {

                if (item != null)
                {
                    var identity = User.Identity as ClaimsIdentity;
                    item.UserName = identity != null ? (!string.IsNullOrEmpty(identity.GetUserName()) ? identity.GetUserName() : "SYSTEMa") : "SYSTEMb";

                    bool result = InboundService.UpdateDeleteReason(item);
                    result = InboundService.UpdateQtyInboundHead(item.InvNo);
                    response.SetData(result);
                    response.SetStatus(HttpStatusCode.OK);
                }

            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }

    public class DataBarcode
    {

        public byte[] Barcode { get; private set; }
        public string BarcodeInfo { get; private set; }
        public DataBarcode(byte[] barcode, string barcodeInfo)
        {
            this.Barcode = barcode;
            this.BarcodeInfo = barcodeInfo;
        }
    }
    public class DataInboundItems
    {
        public DataInboundItems(int totalRecord, IEnumerable<InboundItems> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<InboundItems> Items { get; set; }
    }
    public class DataInboundGroupItems
    {
        public DataInboundGroupItems(int totalRecord, IEnumerable<InboundItemsHead> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<InboundItemsHead> Items { get; set; }
    }
}
