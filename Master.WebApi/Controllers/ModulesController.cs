using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Module;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace Master.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/modules")]
    public class ModulesController : ApiController
    {
        private IModuleService ModuleService;
        public ModulesController(IModuleService moduleService)
        {
            this.ModuleService = moduleService;
        }

        // GET: api/projects
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Module_MT>> response = new ResponseData<IEnumerable<Module_MT>>();
            try
            {
                IEnumerable<Module_MT> modules =  new List<Module_MT>();
                    modules = ModuleService.GetModules();

                
                response.SetData(modules);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        // GET: api/Projects/5
        [HttpGet]
        [Route("{moduleIDSys}")]
        public HttpResponseMessage Get(int moduleIDSys)
        {
            IResponseData<Module_MT> response = new ResponseData<Module_MT>();
            try
            {
                Module_MT module = ModuleService.GetProjectByModuleIDSys(moduleIDSys);
                response.SetData(module);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        // POST: api/Projects
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Module_MT module)
        {
            IResponseData<Module_MT> response = new ResponseData<Module_MT>();
            try
            {
                Module_MT newModule = ModuleService.CreateModule(module);
                response.SetData(newModule);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Projects
        [HttpPut]
        [Route("{moduleIDSys}")]
        public HttpResponseMessage Put(int moduleIDSys, [FromBody]Module_MT module)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ModuleService.UpdateModule(module);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Projects/5
        [HttpDelete]
        [Route("{projectIDSys}")]
        public HttpResponseMessage Delete(int moduleIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ModuleService.DeleteModule(moduleIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        public class HttpRequestParameter
        {
            public List<string> Includes { get; set; }
        }
    }
}