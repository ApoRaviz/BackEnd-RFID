using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WMS.Master;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WMS.Common;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/ApiMenuMapping")]
    public class ApiMenuMappingController : ApiController
    {
        private IApiMenuMappingService ApiMenuMappingService;
        public ApiMenuMappingController(IApiMenuMappingService ApiMenuMappingService)
        {
            this.ApiMenuMappingService = ApiMenuMappingService;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCategories()
        {
            ResponseData<IEnumerable<ApiMenuMappingDto>> response = new ResponseData<IEnumerable<ApiMenuMappingDto>>();
            try
            {
                IEnumerable<ApiMenuMappingDto> categories = ApiMenuMappingService.GetCategories();
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
        public HttpResponseMessage GetApiMenuMapping([FromUri]string id)
        {
            IResponseData<ApiMenuMappingDto> response = new ResponseData<ApiMenuMappingDto>();
            try
            {
                ApiMenuMappingDto ApiMenuMapping = ApiMenuMappingService.GetApiMenuMapping(id);
                response.SetData(ApiMenuMapping);
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
        [Route("menu/{MenuIDSys}")]
        public HttpResponseMessage GetListApiMenuMapping(int MenuIDSys)
        {
            IResponseData<List<ApiMenuMapping>> response = new ResponseData<List<ApiMenuMapping>>();
            try
            {
                List<ApiMenuMapping> ApiMT = ApiMenuMappingService.GetListApiMenuMapping(MenuIDSys);
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
        public HttpResponseMessage Post(List<ApiMenuMappingDto> ApiMenuMapping)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string id = "Not have data";
                if(ApiMenuMapping != null)
                 id = ApiMenuMappingService.CreateApiMenuMapping(ApiMenuMapping);
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
        public HttpResponseMessage Put(string id, [FromBody]ApiMenuMapping ApiMenuMapping)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ApiMenuMappingService.UpdateApiMenuMapping(id, ApiMenuMapping);
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
                bool isUpated = ApiMenuMappingService.DeleteApiMenuMapping(id);
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