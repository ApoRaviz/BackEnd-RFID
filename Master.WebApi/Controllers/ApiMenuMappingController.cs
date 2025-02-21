﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;
using WIM.Core.Common.ValueObject;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace Master.WebApi.Controllers
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
                IEnumerable<ApiMenuMappingDto> categories = ApiMenuMappingService.GetApiMenuMapping();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(categories);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
            IResponseData<IEnumerable<ApiMenuMapping>> response = new ResponseData<IEnumerable<ApiMenuMapping>>();
            try
            {
                IEnumerable<ApiMenuMapping> ApiMT = ApiMenuMappingService.GetListApiMenuMapping(MenuIDSys);
                response.SetData(ApiMT);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Categories/
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]List<ApiMenuMappingDto> ApiMenuMapping)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ApiMenuMappingService.UpdateApiInMenu(ApiMenuMapping);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
                bool isUpated = ApiMenuMappingService.UpdateApiMenuMapping(ApiMenuMapping);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}