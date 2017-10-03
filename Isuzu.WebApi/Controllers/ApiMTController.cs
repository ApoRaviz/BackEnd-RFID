using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Common.Extensions;
using WIM.Master;
using WIM.Common.Http;
using WIM.Common.Validation;

namespace WIM.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/apimt")]
    public class ApiMTController : ApiController
    {
        private IApiMTService ApiMTService;

        public ApiMTController(IApiMTService ApiMTService)
        {
            this.ApiMTService = ApiMTService;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCategories()
        {
            ResponseData<IEnumerable<Api_MT>> response = new ResponseData<IEnumerable<Api_MT>>();
            try
            {
                IEnumerable<Api_MT> categories = ApiMTService.GetCategories();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(categories);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/categories/1
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetApiMT([FromUri]string id)
        {
            IResponseData<ApiMTDto> response = new ResponseData<ApiMTDto>();
            try
            {
                ApiMTDto ApiMT = ApiMTService.GetApiMT(id);
                response.SetData(ApiMT);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Categories
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Api_MT ApiMT)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string id = ApiMTService.CreateApiMT(ApiMT);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Categories/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(string id, [FromBody]Api_MT ApiMT)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ApiMTService.UpdateApiMT(id, ApiMT);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Categories/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(string id)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ApiMTService.DeleteApiMT(id);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}