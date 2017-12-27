using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Status;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/Submodule")]
    public class SubModulesController : ApiController
    {
        private ISubModuleService ModuleService;
        public SubModulesController(ISubModuleService moduleService)
        {
            this.ModuleService = moduleService;
        }

        // GET: api/projects
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<SubModules>> response = new ResponseData<IEnumerable<SubModules>>();
            try
            {
                IEnumerable<SubModules> modules = new List<SubModules>();
                if (User.IsSysAdmin())
                {
                    modules = ModuleService.GetSubModules();
                }

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
        [Route("{submoduleIDSys}")]
        public HttpResponseMessage GetSubModule(int submoduleIDSys)
        {
            IResponseData<SubModules> response = new ResponseData<SubModules>();
            try
            {
                SubModules module = ModuleService.GetSubModulesByID(submoduleIDSys);
                response.SetData(module);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        [HttpGet]
        [Route("module/{moduleIDSys}")]
        public HttpResponseMessage GetModule(int moduleIDSys)
        {
            IResponseData<IEnumerable<SubModules>> response = new ResponseData<IEnumerable<SubModules>>();
            try
            {
                IEnumerable<SubModules> module = ModuleService.GetSubModulesByModuleID(moduleIDSys);
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
        public HttpResponseMessage Post([FromBody]SubModules module)
        {
            IResponseData<SubModules> response = new ResponseData<SubModules>();
            try
            {
                SubModules newModule = ModuleService.CreateModule(module);
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
        public HttpResponseMessage Put(int moduleIDSys, [FromBody]SubModules module)
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
        [Route("{moduleIDSys}")]
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