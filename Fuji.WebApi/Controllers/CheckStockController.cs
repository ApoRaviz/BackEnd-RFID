using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fuji.Service.ItemImport;
using Fuji.Common.ValueObject;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using Fuji.Entity.StockManagement;
using Fuji.Entity.ItemManagement;
using Fuji.Common.ValueObject.CheckStock;
using System.Net.Http.Headers;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/stock")]
    public class CheckStockController : ApiController
    {

        private ICheckStockService CheckStockService;
        public CheckStockController(ICheckStockService checkStockService)
        {
            this.CheckStockService = checkStockService;
        }



        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateCheckStock()
        {
            ResponseData<CheckStockHead> respones = new ResponseData<CheckStockHead>();
            try
            {
                CheckStockHead item = CheckStockService.CreateCheckStockHead();
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(item);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPost]
        [Route("Progress")]
        public HttpResponseMessage GetStockByProgress()
        {
            ResponseData<CheckStockHead> respones = new ResponseData<CheckStockHead>();
            try
            {
               CheckStockHead items = CheckStockService.GetStockHeadByProgress();
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(items);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateCheckStock([FromBody] CheckStockHead checkStockHead)
        {
            ResponseData<bool> respones = new ResponseData<bool>();
            try
            {

                bool isUpdated = CheckStockService.UpdateCheckStockHead(checkStockHead);
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(isUpdated);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPut]
        [Route("Complete")]
        public HttpResponseMessage UpdateCompleteCheckStock([FromBody] CheckStockHead checkStockHead)
        {
            ResponseData<bool> respones = new ResponseData<bool>();
            try
            {
                CheckStockHead stockHead = CheckStockService.GetStockHeadByID(checkStockHead.CheckStockID);
                if(stockHead != null)
                {
                    stockHead.Status = CheckStockStatus.Completed.GetValueEnum();
                    bool isUpdated = CheckStockService.UpdateCheckStockHead(stockHead);
                    respones.SetStatus(HttpStatusCode.OK);
                    respones.SetData(isUpdated);
                }

            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }


        [HttpGet]
        [Route("Get/{stockID}")]
        public HttpResponseMessage GetStockByID(string stockID)
        {
            ResponseData<CheckStockHead> respones = new ResponseData<CheckStockHead>();
            try
            {
                CheckStockHead item = CheckStockService.GetStockHeadByID(stockID);
                if (item != null)
                {
                    respones.SetStatus(HttpStatusCode.OK);
                    respones.SetData(item);
                }

            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpGet]
        [Route("GetStock/{pageIndex}/{pageSize}")]
        public HttpResponseMessage GetStock(int pageIndex, int pageSize)
        {
            ResponseData<IEnumerable<CheckStockHead>> respones = new ResponseData<IEnumerable<CheckStockHead>>();
            try
            {
                int totalRecord;
                IEnumerable<CheckStockHead> items = CheckStockService.GetStock(pageIndex, pageSize, out totalRecord);
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(items);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [HttpPost]
        [Route("GetStockBy")]
        public HttpResponseMessage GetStockBy([FromBody] ParameterSearch paramSearch)
        {
            ResponseData<IEnumerable<CheckStockHead>> respones = new ResponseData<IEnumerable<CheckStockHead>>();
            try
            {
                IEnumerable<CheckStockHead> items = CheckStockService.SearchStockBy(paramSearch);
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(items);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }


        #region Report
        [HttpGet]
        [Route("Report")]
        public HttpResponseMessage GetReportStockGroup()
        {
            ResponseData<IEnumerable<FujiStockReportHead>> respones = new ResponseData<IEnumerable<FujiStockReportHead>>();
            try
            {
                IEnumerable<FujiStockReportHead> items = CheckStockService.GetStockReportGroup();
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(items);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        [HttpPost]
        [Route("Report")]
        public HttpResponseMessage GetReportStockList([FromBody] FujiStockReportHead head)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result.Content = CheckStockService.GetReportStream(head);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            catch (ValidationException ex)
            {
                result = Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
            return result;
        }



        #endregion


        #region Handy

        [HttpGet]
        [Route("Handy")]
        public HttpResponseMessage GetStatusByHandy()
        {
            ResponseData<int> respones = new ResponseData<int>();
            try
            {
                int status = CheckStockService.HandyGetStatus();
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(status);

            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpPost]
        [Route("Handy")]
        public HttpResponseMessage PostCheckStockByHandy([FromBody] FujiCheckStockHandy checkStock)
        {
            ResponseData<int> respones = new ResponseData<int>();
            try
            {
                int status = CheckStockService.UpdateCheckStockByHandy(checkStock);
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(status);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);
        }

        #endregion



    }
}
