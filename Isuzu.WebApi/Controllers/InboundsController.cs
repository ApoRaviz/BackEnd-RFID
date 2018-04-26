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

using System.Text;
using System.Security.Claims;
using Isuzu.Service.Impl;
using Isuzu.Common.ValueObject;
using Isuzu.Entity;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using System.Net.Http.Headers;
using WIM.Core.Service.Impl.FileManagement;
using WIM.Core.Service.FileManagement;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace Isuzu.Service.Impl
{
    [RoutePrefix("api/v1/isuzu/inbounds")]
    public class InboundsController : ApiController
    {
        private IInboundService InboundService;
        private string Username { get; set; }

        public InboundsController(IInboundService inboundService)
        {
            this.InboundService = inboundService;
            Username = User.Identity.GetUserName() ?? "SYSTEM";
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
            /*if (inboundItem.IsRepeat == 0 && InboundService.CheckScanRepeatRegisterInboundItem_HANDY(inboundItem))
            {
                ResponseData<int> responseCheckRepeat = new ResponseData<int>();
                responseCheckRepeat.SetData(2);
                return Request.ReturnHttpResponseMessage(responseCheckRepeat);
            }*/

            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.RegisterInboundItem_HANDY(inboundItem);
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
        [HttpGet]
        [Route("handy/items/invoice/registered/{invNo}")]
        public HttpResponseMessage GetInboundItemsRegisteredByInvoice_HANDY([FromUri]string invNo)
        {
            ResponseData<IEnumerable<InboundItemHandyDto>> responseHandy = new ResponseData<IEnumerable<InboundItemHandyDto>>();
            try
            {
                IEnumerable<InboundItemHandyDto> items = InboundService.GetInboundItemsRegisteredByInvoice_HANDY(invNo);
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
        public HttpResponseMessage PerformHolding_HANDY([FromBody]ReceiveParamsList inboundItemHolding)
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                InboundService.PerformHolding_HANDY(inboundItemHolding.ReceiveParams);
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
                InboundService.PerformShipping_HANDY(inboundItemShipping);
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
                InboundService.PerformPackingCarton_HANDY(itemReq);
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
                InboundService.PerformPackingCase_HANDY(itemReq);
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

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("handy/items/amountRegistered")]
        public HttpResponseMessage GetAmountRegistered()
        {
            ResponseData<int> responseHandy = new ResponseData<int>();
            try
            {
                int amount = InboundService.GetAmountRegistered_HANDY();
                responseHandy.SetData(amount);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);
        }

        #endregion

        #region =============================== DEFAULT =================================



        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("items")]
        public HttpResponseMessage Get()
        {
            IResponseData<IEnumerable<InboundItemsHead>> respones = new ResponseData<IEnumerable<InboundItemsHead>>();
            try
            {
                var x = InboundService.GetInboundItemByQty(10, true);
                IEnumerable<InboundItemsHead> items = InboundService.GetInboundGroup();
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
        public HttpResponseMessage GetByPaging([FromUri]int pageIndex, [FromUri]int pageSize)
        {
            IResponseData<IsuzuDataInboundGroupItems> respones = new ResponseData<IsuzuDataInboundGroupItems>();
            try
            {
                 
                int totalRecord = 0;
                IEnumerable<InboundItemsHead> items = InboundService.GetInboundGroupPaging(pageIndex,pageSize,out totalRecord);
                if(items != null && totalRecord > 0)
                {
                    IsuzuDataInboundGroupItems dataItem = new IsuzuDataInboundGroupItems(totalRecord,items);
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
        public HttpResponseMessage GetByInvNo([FromUri]string invNo)
        {
            IResponseData<InboundItemsHead> respones = new ResponseData<InboundItemsHead>();
            try
            {
                InboundItemsHead item = InboundService.GetInboundGroupByInvoiceNumber(invNo,true);
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

        /*[Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("itemsInvNoAsync/{invNo}")]
        public async Task<HttpResponseMessage> GetByInvNoAnyc([FromUri]string invNo)
        {
            IResponseData<InboundItemsHead> respones = new ResponseData<InboundItemsHead>();
            try
            {
                InboundItemsHead item = await InboundService.GetInboundGroupByInvoiceNumberAsync(invNo, true);
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
        }*/

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("items/{iszjOrder}")]
        public HttpResponseMessage GetInboundItemByISZJOrder([FromUri]string iszjOrder)
        {
            IResponseData<InboundItemHandyDto> responseHandy = new ResponseData<InboundItemHandyDto>();
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
        public HttpResponseMessage GetDataByColumn([FromUri]string column, [FromUri]string keyword)
        {

            IResponseData<IEnumerable<InboundItemsHead>> response = new ResponseData<IEnumerable<InboundItemsHead>>();
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
        public HttpResponseMessage GetItemByPaging([FromUri]int pageIndex, [FromUri]int pageSize)
        {
            IResponseData<IsuzuDataInboundItems> respones = new ResponseData<IsuzuDataInboundItems>();
            try
            {
                int totalRecord = 0;
                IEnumerable<InboundItems> items = InboundService.GetInboundItemPaging(pageIndex, pageSize, out totalRecord);
                if (totalRecord > 0)
                {
                    IsuzuDataInboundItems ret = new IsuzuDataInboundItems(totalRecord, items);
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
        [HttpGet]
        [Route("itemImportDeleted/{pageIndex}/{pageSize}")]
        public HttpResponseMessage GetItemDeletedByPaging([FromUri]int pageIndex, [FromUri]int pageSize)
        {
            ResponseData<IsuzuDataInboundItems> respones = new ResponseData<IsuzuDataInboundItems>();
            try
            {
                int totalRecord = 0;
                IEnumerable<InboundItems> items = InboundService.GetInboundItemDeletedPaging(pageIndex, pageSize, out totalRecord);
                if (totalRecord > 0)
                {
                    IsuzuDataInboundItems ret = new IsuzuDataInboundItems(totalRecord, items);
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
            IResponseData<IEnumerable<InboundItems>> respones = new ResponseData<IEnumerable<InboundItems>>();
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
        [Authorize]
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
            ResponseData<List<InboundItems>> response = new ResponseData<List<InboundItems>>();
            List<InboundItems> inboundList = new List<InboundItems>();
            bool isDubplicat = false;
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);


                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    IsuzuDataImport returnImported = InboundService.OpenReadExcel(file.LocalFileName);
                    if (returnImported != null)
                    {
                        if (returnImported.isDuplicated)
                        {
                            isDubplicat = true;
                            inboundList = returnImported.listItem;
                            break;
                        }
                        else
                            inboundList.AddRange(returnImported.listItem);
                    }
                }

                if (isDubplicat)
                {
                    response.SetData(inboundList);
                    response.SetStatus(HttpStatusCode.Conflict);
                }
                else
                {
                    response.SetData(inboundList);
                    response.SetStatus(HttpStatusCode.OK);
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
        [Route("generateRFID/{qty}")]
        public HttpResponseMessage GenerateRFID([FromUri]string qty)
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
        public HttpResponseMessage ExportToExcel([FromUri]string id)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            IEnumerable<InboundItems> inboundItems = InboundService.GetInboundItemByInvoiceNumber(id,true);
            if (inboundItems != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Temps/tmpexcel_" + Guid.NewGuid() + ".xlsx");

                string fileName = "{0}_{1}.{2}";
                fileName = String.Format(fileName, "Export_Isuzu_", DateTime.Now.ToString("yyyy-MM-dd_HHmmss", new System.Globalization.CultureInfo("en-US")), "xlsx");
                DataTable dt = IsuzuReportHelper.getExportInboundDataTable(inboundItems.ToList());
                var ms = IsuzuReportHelper.parseExcelToDownload(dt, filePath, fileName, HttpContext.Current.Response);
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
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("updateexport")]
        public HttpResponseMessage UpdateInboundGroup([FromBody]InboundItemsHead item)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
               
                if (item != null)
                {
                    //var identity = User.Identity as ClaimsIdentity;
                    //item.UpdateBy = identity != null ? (!string.IsNullOrEmpty(identity.GetUserName()) ? identity.GetUserName() : "SYSTEMa") : "SYSTEMb";

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

        [Authorize]
        [HttpPost]
        [Route("deleteReasonPath")]
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
            IResponseData<IEnumerable<string>> response = new ResponseData<IEnumerable<string>>();
            List<string> AllPath = new List<string>() { };
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

                    AllPath.Add(InboundService.CreateDeletedFileID(path));
                   
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
        [Route("deleteReason")]
        public HttpResponseMessage DeleteReason([FromBody]IsuzuDeleteReason item)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {

                if (item != null)
                {
                   
                    bool result = InboundService.UpdateDeleteReason(item);
                    //result = InboundService.UpdateQtyInboundHead(item.InvNo, item.UserName);
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

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("getFileDeleteReason/{FileIDSys}")]
        public HttpResponseMessage Get(string FileIDSys)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                InboundService.GetDeletedFileID(FileIDSys);
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
        [Route("DeleteReasonByInvoice/{invNo}")]
        public HttpResponseMessage DeleteReasonByInvoice(string invNo, [FromBody]IsuzuDeleteReason item)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {

                if (item != null)
                {

                    bool result = InboundService.UpdateDeleteReasonByInvoice(invNo, item);
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
        [Route("GetRFIDInfo")]
        public HttpResponseMessage GetRFIDInformation([FromBody]ParameterSearch query)
        {
            IResponseData<string> response = new ResponseData<string>();

            try
            {
                string datas = InboundService.GetRFIDInfo(query);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(datas);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("GetRFIDInfoByDate")]
        public HttpResponseMessage GetRFIDInfoByDate([FromBody]ParameterSearch query)
        {
            ResponseData<IEnumerable<IsuzuTagReport>> respones = new ResponseData<IEnumerable<IsuzuTagReport>>();
            try
            {
                IEnumerable<IsuzuTagReport> items = InboundService.GetReportByYearRang(query);
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(items);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
                respones.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPost]
        [Route("adjustWeight")]
        public HttpResponseMessage AdjustWeight([FromBody]InboundItemHandyDto adjustWeight)
        {
            if (adjustWeight.IsRepeat == 0)
            {
                ResponseData<InboundItemHandyDto> responseCheckRepeat = new ResponseData<InboundItemHandyDto>();
                InboundItemHandyDto adjustWeightReturn = InboundService.GetBeforeAdjustWeight(adjustWeight);
                responseCheckRepeat.SetData(adjustWeightReturn);
                return Request.ReturnHttpResponseMessage(responseCheckRepeat);
            }

            ResponseData<InboundItemHandyDto> responseHandy = new ResponseData<InboundItemHandyDto>();
            try
            {
                InboundService.AdjustWeight(adjustWeight);
                responseHandy.SetData(adjustWeight);
            }
            catch (ValidationException ex)
            {
                responseHandy.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(responseHandy);

        }

        #endregion
    }
}
