using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Entity.SpareField;
using WMS.Service;

namespace WMS.WebApi.Controller
{
    [RoutePrefix("api/v1/SpareField")]
    public class SpareFieldController : ApiController
    {
        private ISpareFieldService SpareFieldService;

        public SpareFieldController(ISpareFieldService SpareFieldservice)
        {
            this.SpareFieldService = SpareFieldservice;
        }

        // GET: api/SpareField
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<SpareField>> response = new ResponseData<IEnumerable<SpareField>>();
            try
            {
                IEnumerable<SpareField> SpareField = SpareFieldService.GetSpareField();
                response.SetData(SpareField);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Employees/1
        [HttpGet]
        [Route("{SpfIDSys}")]
        public HttpResponseMessage Get(int SpfIDSys)
        {
            IResponseData<SpareField> response = new ResponseData<SpareField>();
            try
            {
                SpareField SpareField = SpareFieldService.GetSpareFieldBySpfIDSys(SpfIDSys);
                response.SetData(SpareField);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Employees
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]IEnumerable<SpareField> SpareField)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = SpareFieldService.CreateSpareField(SpareField);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Employees/5

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]SpareField SpareField)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = SpareFieldService.UpdateSpareField(SpareField);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Employees/5
        [HttpDelete]
        [Route("{SpfID}")]
        public HttpResponseMessage Delete(int SpfID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = SpareFieldService.DeleteSpareField(SpfID);
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