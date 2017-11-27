using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;

namespace Fuji.WebApi.Controllers
{
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
        public HttpResponseMessage GetAPIs()
        {
            ResponseData<IEnumerable<IEnumerable<Api_MT>>> response = new ResponseData<IEnumerable<IEnumerable<Api_MT>>>();
            try
            {
                IEnumerable<Api_MT> apis = ApiMTService.GetAPIs();
                var group = apis.GroupBy(a => a.Controller);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(group);
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
        public HttpResponseMessage Post([FromBody]List<Api_MT> ApiMT)
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
                bool isUpated = ApiMTService.UpdateApiMT(ApiMT);
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
