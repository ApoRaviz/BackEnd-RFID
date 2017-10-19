using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using WIM.Core.Common.Extensions;
using WMS.Master;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Web.Http.Cors;
using WMS.WebApi.Report;
using WMS.Common;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/country")]
    public class CountryController : ApiController
    {

        private ICountryService CountryService;

        public CountryController(ICountryService CountryService)
        {
            this.CountryService = CountryService;
        }

        //get api/Countrys
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Country_MT>> response = new ResponseData<IEnumerable<Country_MT>>();
            try
            {
                IEnumerable<Country_MT> Country = CountryService.GetCountry();
                response.SetData(Country);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Countrys/id

        [HttpGet]
        [Route("{CountryIDSys}")]
        public HttpResponseMessage Get(int CountryIDSys)
        {
            IResponseData<Country_MT> response = new ResponseData<Country_MT>();
            try
            {
                Country_MT Country = CountryService.GetCountryByCountryIDSys(CountryIDSys);
                response.SetData(Country);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Country_MT Country)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = CountryService.CreateCountry(Country);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{CountryIDSys}")]
        public HttpResponseMessage Put(int CountryIDSys, [FromBody]Country_MT Country)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = CountryService.UpdateCountry(CountryIDSys, Country);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("{CountryIDSys}")]
        public HttpResponseMessage Delete(int CountryIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpdated = CountryService.DeleteCountry(CountryIDSys);
                response.SetData(isUpdated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        private ICountryService GetCountryService()
        {
            return CountryService;
        }

    }

}
