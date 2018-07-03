using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;

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
        public HttpResponseMessage GetApiMT([FromUri]string id)
        {
            IResponseData<ApiMTDto> response = new ResponseData<ApiMTDto>();
            try
            {
                ApiMTDto ApiMT = ApiMTService.GetApiMT(id);
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
        public HttpResponseMessage Post([FromBody]List<Api_MT> ApiMT)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string id = ApiMTService.CreateApiMT(ApiMT);
                response.SetData(id);
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
        public HttpResponseMessage Put(string id, [FromBody]Api_MT ApiMT)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ApiMTService.UpdateApiMT(ApiMT);
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
                bool isUpated = ApiMTService.DeleteApiMT(id);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("description")]
        public HttpResponseMessage GetApiDescription()
        {
            ResponseData<List<ApiDesc>> response = new ResponseData<List<ApiDesc>>();

            Collection<ApiDescription> apis = Configuration.Services.GetApiExplorer().ApiDescriptions;
            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = apis.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            List<ApiDesc> apiDescList = new List<ApiDesc>();
            foreach (IGrouping<HttpControllerDescriptor, ApiDescription> group in apiGroups)
            {
                foreach (ApiDescription api in group)
                {
                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    foreach (ApiParameterDescription parameter in api.ParameterDescriptions)
                    {
                        if (parameter.ParameterDescriptor == null)
                        {
                            continue;
                        }

                        Type type = parameter.ParameterDescriptor.ParameterType;
                        if (type == typeof(Int32) || type == typeof(Int64) || type == typeof(Boolean))
                        {
                            paramDic.Add(parameter.Name, "1");
                        }
                        else if (type == typeof(String) || type == typeof(DateTime))
                        {
                            paramDic.Add(parameter.Name, "@");
                        }
                    }

                    string path = "";
                    string[] pathSplit = api.RelativePath.Split('?')[0].Split('/');
                    for (int i = 0; i < pathSplit.Length; i++)
                    {
                        if (i > 2 && pathSplit[i][0] == '{' && pathSplit[i][pathSplit[i].Length - 1] == '}')
                        {
                            foreach (KeyValuePair<string, string> paramKeyVal in paramDic)
                            {
                                string x = pathSplit[i].Substring(1, pathSplit[i].Length - 2);

                                if (x.Equals(paramKeyVal.Key, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    path += "/" + paramKeyVal.Value;
                                    continue;
                                }
                            }
                            continue;
                        }
                        path += "/" + pathSplit[i];
                    }

                    apiDescList.Add(new ApiDesc
                    {
                        ID = api.ID,
                        ControllerName = group.Key.ControllerName,
                        RelativePath = api.RelativePath,
                        ApiPath = path,
                        Method = api.HttpMethod.Method
                    });
                }
            }

            response.Data = apiDescList;
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}