using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Web.Http.Cors;
using WMS.Service;
using WMS.Service.Inspect;
using WMS.Entity.InspectionManagement;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Inspects")]
    public class InspectsController : ApiController
    {
        private IInspectService InspectService;

        public InspectsController(IInspectService InspectService)
        {
            this.InspectService = InspectService;
        }

        // GET: api/InspectTypes
        [HttpGet]
        [Route("types")]
        public HttpResponseMessage GetInspectTypes()
        {
            ResponseData<IEnumerable<InspectType>> response = new ResponseData<IEnumerable<InspectType>>();
            try
            {
                IEnumerable<InspectType> InspectTypes = InspectService.GetInspectTypes();
                response.SetData(InspectTypes);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Inspects
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Inspect_MT>> response = new ResponseData<IEnumerable<Inspect_MT>>();
            try
            {
                IEnumerable<Inspect_MT> Inspects = InspectService.GetInspects();
                response.SetData(Inspects);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Inspects/1
        [HttpGet]
        [Route("{inspectIDSys}")]
        public HttpResponseMessage Get(int inspectIDSys)
        {
            IResponseData<Inspect_MT> response = new ResponseData<Inspect_MT>();
            try
            {
                Inspect_MT Inspect = InspectService.GetInspectBySupIDSys(inspectIDSys);
                response.SetData(Inspect);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Inspects
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Inspect_MT Inspect)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Inspect.UpdateBy = User.Identity.Name;
                int id = InspectService.CreateInspect(Inspect);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Inspects/5
        [HttpPut]
        [Route("{inspectIDSys}")]
        public HttpResponseMessage Put(int inspectIDSys, [FromBody]Inspect_MT Inspect)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = InspectService.UpdateInspect(Inspect);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Inspects/5
        [HttpDelete]
        [Route("{inspectIDSys}")]
        public HttpResponseMessage Delete(int inspectIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = InspectService.DeleteInspect(inspectIDSys);
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
