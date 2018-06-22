using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using System.Web.Http.Cors;
using WIM.Core.Entity.Currency;
using WIM.Core.Service;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/currency")]
    public class CurrencyController : ApiController
    {

        private ICurrencyService CurrencyService;

        public CurrencyController(ICurrencyService CurrencyService)
        {
            this.CurrencyService = CurrencyService;
        }

        //get api/Currencys
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<CurrencyUnit>> response = new ResponseData<IEnumerable<CurrencyUnit>>();
            try
            {
                IEnumerable<CurrencyUnit> Currency = CurrencyService.GetCurrency();
                response.SetData(Currency);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Currencys/id

        [HttpGet]
        [Route("{CurrencyIDSys}")]
        public HttpResponseMessage Get(int CurrencyIDSys)
        {
            IResponseData<CurrencyUnit> response = new ResponseData<CurrencyUnit>();
            try
            {
                CurrencyUnit Currency = CurrencyService.GetCurrencyByCurrIDSys(CurrencyIDSys);
                response.SetData(Currency);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]CurrencyUnit Currency)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Currency.Country_MT = null;
                Currency.UpdateBy = User.Identity.Name;
                int id = CurrencyService.CreateCurrency(Currency);
                response.SetData(id);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{CurrencyIDSys}")]
        public HttpResponseMessage Put(int CurrencyIDSys, [FromBody]CurrencyUnit Currency)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                Currency.Country_MT = null;
                bool isUpdated = CurrencyService.UpdateCurrency(Currency);
                response.SetData(isUpdated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("{CurrencyIDSys}")]
        public HttpResponseMessage Delete(int CurrencyIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpdated = CurrencyService.DeleteCurrency(CurrencyIDSys);
                response.SetData(isUpdated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private ICurrencyService GetCurrencyService()
        {
            return CurrencyService;
        }

    }

}
