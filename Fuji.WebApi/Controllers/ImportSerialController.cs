using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using Fuji.Service.ItemImport;
using Fuji.Common.ValueObject;
using Fuji.Entity.ItemManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

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
              
                IEnumerable<FujiPickingGroup> items = ItemImportService.GetPickingGroup();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HttpGet]
        [Route("importSerial/pickingGroupTop/{top}")]
        public HttpResponseMessage GetPickingGroupsTopTen(int top)
        {
            ResponseData<IEnumerable<FujiPickingGroup>> response = new ResponseData<IEnumerable<FujiPickingGroup>>();
            try
            {
                IEnumerable<FujiPickingGroup> items = ItemImportService.GetPickingGroup(top);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (AppValidationException ex)
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
                //string userName = User.Identity.GetUserName() ?? "SYSTEM";
                bool result = ItemImportService.ClearPickingGroup(id);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(result);
            }
            catch (AppValidationException ex)
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
                FujiPickingGroup result = ItemImportService.GetPickingByOrderNo(id);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(new List<FujiPickingGroup>() { result });
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
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
                DataTable dt = FujiReportHelper.getFujiPickingGroupDataTable(pickingGroup);
                var ms = FujiReportHelper.parseExcelToDownload(dt, filePath);
                result.Content = new ByteArrayContent(ms.ToArray()); 
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            }
            return result;
        }

        #endregion


        #region Handy

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/setscanned")]
        public HttpResponseMessage SetScanned([FromBody]SetScannedRequest receive)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                bool flag = ItemImportService.SetScanned(receive);
                response.SetData(flag ? 1 : 0);
            }
            catch (AppValidationException ex)
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
                bool flag = ItemImportService.Receive(receive);
                response.SetData(flag ? 1 : 0);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/handy/itemGroups/{*orderNo}")]
        public HttpResponseMessage GetByOrder(string orderNo)
        {
            IResponseData<List<string>> response = new ResponseData<List<string>>();
            try
            {
                List<string> itemGroups = ItemImportService.GetItemGroupByOrderNo_Handy(orderNo);
                response.SetData(itemGroups);
            }
            catch (AppValidationException ex)
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
                bool flag = ItemImportService.ConfirmPicking(confirmRequest);
                response.SetData(flag ? 1 : 0);
            }
            catch (AppValidationException ex)
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


        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/registerRFID")]
        public HttpResponseMessage RegisterRFID_HANDY([FromBody]RegisterRFIDRequest registerRequest)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                bool flag = ItemImportService.RegisterRFID_HANDY(registerRequest);
                response.SetData(flag ? 1 : 0);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        #endregion


        // GET: api/Items
        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead>> response = new ResponseData<IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead>>();
            try
            {
                //string userName = User.Identity.GetUserName() ?? "SYSTEM";
                IEnumerable<Fuji.Entity.ItemManagement.ImportSerialHead> items = ItemImportService.GetItems();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (AppValidationException ex)
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
            ResponseData<FujiDataImportSerialHead> response = new ResponseData<FujiDataImportSerialHead>();
            try
            {
                IEnumerable<ImportSerialHead> items = ItemImportService.GetItems(pageIndex, pageSize, out int totalRecord);
                if (totalRecord > 0)
                {
                    FujiDataImportSerialHead ret = new FujiDataImportSerialHead(totalRecord, items);
                    response.SetStatus(HttpStatusCode.OK);
                    response.SetData(ret);
                }
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/header/{id}")]
        public HttpResponseMessage GetHeaderPDF(string id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                ImportSerialHead item = ItemImportService.GetItemByDocID(id, true);
                if (item != null)
                {
                    result.Content = ItemImportService.GetReportStream(item);
                }
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            catch (AppValidationException ex)
            {
                result = Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }

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

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id,true);
            if (serialHead != null)
            {
                ItemImportService.UpdateStausExport(serialHead);

                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Import_For_L-CAT", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                DataTable dt = FujiReportHelper.getImportSerailDataTable(serialHead);
                var ms = FujiReportHelper.parseExcelToDownload(dt, filePath);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("importSerial/export-waranty/{id}")]
        public HttpResponseMessage ExportToWarantyExcel(string id)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id,true);
            if (serialHead != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_To_Waranty", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                DataTable dt = FujiReportHelper.getImportSerailGroupDataTable(serialHead);
                var ms = FujiReportHelper.parseExcelToDownload(dt, filePath);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("importSerial/export-receive/{id}")]
        public HttpResponseMessage ExportToReceiveExcel(string id)
        {

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            ImportSerialHead serialHead = ItemImportService.GetItemByDocID(id,true);
            if (serialHead != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_All_Receive", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                DataTable dt = FujiReportHelper.getImportSerailByStatusDataTable(serialHead);
                var ms = FujiReportHelper.parseExcelToDownload(dt, filePath);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            }
            return result;
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
                ImportSerialHead newItem = ItemImportService.CreateItem(item);
                response.SetData(newItem);
            }
            catch (AppValidationException ex)
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
                bool isUpated = ItemImportService.UpdateItem(id, item);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
                bool result = ItemImportService.UpdateStausExport(item);
                if (result)
                    response.SetStatus(HttpStatusCode.OK);
                else
                    response.SetStatus(HttpStatusCode.InternalServerError);

                response.SetData(result);

            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/bycolumn")]
        public HttpResponseMessage GetDataByColumn([FromBody]ParameterSearch param)
        {
            ResponseData<FujiDataImportSerialHead> response = new ResponseData<FujiDataImportSerialHead>();
            try
            {
                int totalRecord;
                IEnumerable<ImportSerialHead> result = ItemImportService.GetDataByColumn(param, out totalRecord);
                if (result != null)
                {
                    FujiDataImportSerialHead ret = new FujiDataImportSerialHead(totalRecord,result);
                    response.SetData(ret);
                }
                
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //[Authorize]
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
            catch (AppValidationException e)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [Authorize]
        [HttpPost]
        [Route("ImportPickingWin")]
        public HttpResponseMessage PostFormWin([FromBody]FujiPickingFromWinRequest receive)
        {
            IResponseData<IEnumerable<ImportSerialDetail>> response = new ResponseData<IEnumerable<ImportSerialDetail>>();

            try
            {
                IEnumerable<ImportSerialDetail> listDetail = ItemImportService.UpdateStatus(receive.ListPicking);
                response.SetData(listDetail);
                response.SetStatus(HttpStatusCode.OK);

            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.InternalServerError);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

       

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/byCriteria")]
        public HttpResponseMessage FindImportSerialDetailByCriteria([FromBody]ParameterSearch parameterSearch)
        {
            ResponseData<FujiDataImportSerialDetail> respones = new ResponseData<FujiDataImportSerialDetail>();
            try
            {
                int totalRecord;
                IEnumerable<ImportSerialDetail> items = ItemImportService.FindImportSerialDetailByCriteria(parameterSearch,out totalRecord);
                if (items != null)
                {
                    FujiDataImportSerialDetail ret = new FujiDataImportSerialDetail(totalRecord,items);
                    respones.SetStatus(HttpStatusCode.OK);
                    respones.SetData(ret);
                }
            }
            catch (AppValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/GetBoxNumberAndAmount")]
        public HttpResponseMessage GetBoxNumberAndAmount([FromBody]ParameterSearch parameterSearch)
        {
            ResponseData<IEnumerable<FujiBoxNumberAndAmountModel>> respones = new ResponseData<IEnumerable<FujiBoxNumberAndAmountModel>>();
            IEnumerable<FujiBoxNumberAndAmountModel> boxes = new List<FujiBoxNumberAndAmountModel>();
            boxes = ItemImportService.GetBoxNumberAndAmountList(parameterSearch);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(boxes);
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("importSerial/GetItemsInBoxNumber/{boxNumber}")]
        public HttpResponseMessage GetItemsInBoxNumber(string boxNumber)
        {
            ResponseData<IEnumerable<FujiSerialAndRFIDModel>> respones = new ResponseData<IEnumerable<FujiSerialAndRFIDModel>>();
            IEnumerable<FujiSerialAndRFIDModel> items = new List<FujiSerialAndRFIDModel>();
            items = ItemImportService.GetItemsInBoxNumber(boxNumber);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(items);
            return Request.ReturnHttpResponseMessage(respones);
        }

        [HttpGet]
        [Route("importSerial/GetLastestItemsBoxNumber")]
        public HttpResponseMessage GetLastestItemsBoxNumber()
        {
            ResponseData<FujiCheckRegister> respones = new ResponseData<FujiCheckRegister>();
            FujiCheckRegister items = new FujiCheckRegister();
            items = ItemImportService.GetItemScanLastest();
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(items);
            return Request.ReturnHttpResponseMessage(respones);
        }

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/Test")]
        public HttpResponseMessage Test()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            ResponseData<int> respones = new ResponseData<int>();
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(1);
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/SetSerial2Box/{boxNumberFrom}/{boxNumberTo}")]
        public HttpResponseMessage SetSerial2Box(string boxNumberFrom, string boxNumberTo, [FromBody]ItemGroupRequest itemGroup)
        {
            string userName = Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(User.Identity) ?? "SYSTEM";

            ResponseData<int> respones = new ResponseData<int>();       
            int flag = ItemImportService.SetSerial2Box(boxNumberFrom, boxNumberTo, itemGroup, userName);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(flag);
            return Request.ReturnHttpResponseMessage(respones);
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("importSerial/handy/SetBox2Location/{locationTo}")]
        public HttpResponseMessage SetBox2Location(string locationTo, [FromBody]ItemGroupRequest itemGroup)
        {
            string userName = Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(User.Identity) ?? "SYSTEM";

            ResponseData<int> respones = new ResponseData<int>();
            int flag = ItemImportService.SetBox2Location(locationTo, itemGroup, userName);
       
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(flag);
            return Request.ReturnHttpResponseMessage(respones);
        }

        [HttpPost]
        [Route("importSerial/GetRFIDInfo")]
        public HttpResponseMessage GetRFIDInformation([FromBody]ParameterSearch query)
        {
            ResponseData<string> respones = new ResponseData<string>();
            string datas = ItemImportService.GetRFIDInfo(query);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(datas);
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPost]
        [Route("importSerial/GetHeadTop")]
        public HttpResponseMessage GetHeadTop([FromBody]ParameterSearch query)
        {
            ResponseData<IEnumerable<ImportSerialHead>> respones = new ResponseData<IEnumerable<ImportSerialHead>>();
            IEnumerable<ImportSerialHead> items = ItemImportService.GetHeadDataTopten(query);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(items);
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPost]
        [Route("importSerial/GetRFIDInfoByDate")]
        public HttpResponseMessage GetRFIDInfoByDate([FromBody]ParameterSearch query)
        {
            ResponseData<IEnumerable<FujiTagReport>> respones = new ResponseData<IEnumerable<FujiTagReport>>();
            IEnumerable<FujiTagReport> items = ItemImportService.GetReportByYearRang(query);
            respones.SetStatus(HttpStatusCode.OK);
            respones.SetData(items);
            return Request.ReturnHttpResponseMessage(respones);

        }

    }
}
