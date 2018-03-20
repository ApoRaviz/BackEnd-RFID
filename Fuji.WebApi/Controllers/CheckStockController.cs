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



        [HttpGet]
        [Route("Import")]
        public HttpResponseMessage ImportCheckStock()
        {
            ResponseData<bool> respones = new ResponseData<bool>();
            try
            {

                bool isImported = CheckStockService.ImportCheckStock();
                respones.SetStatus(HttpStatusCode.OK);
                respones.SetData(isImported);
            }
            catch (ValidationException ex)
            {
                respones.SetErrors(ex.Errors);
            }
            return Request.ReturnHttpResponseMessage(respones);

        }

        [HttpGet]
        [Route("{stockID}")]
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






    }
}
