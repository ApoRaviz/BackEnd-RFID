using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
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


        // GET: api/SpareField
        [HttpGet]
        [Route("Project/{ProjectIDSys}")]
        public HttpResponseMessage GetByProjectIDSys(int ProjectIDSys)
        {
            ResponseData<IEnumerable<SpareField>> response = new ResponseData<IEnumerable<SpareField>>();
            try
            {
                IEnumerable<SpareField> SpareField = SpareFieldService.GetSpareFieldByProjectIDSys(ProjectIDSys);
                response.SetData(SpareField);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("table/{TableName}")]
        public HttpResponseMessage GetSpareFieldByTableName(string TableName)
        {
            ResponseData<IEnumerable<SpareField>> response = new ResponseData<IEnumerable<SpareField>>();
            try
            {
                IEnumerable<SpareField> SpareField = SpareFieldService.GetSpareFieldByTableName(TableName);
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
        public HttpResponseMessage Post([FromBody]List<SpareField> SpareField)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                if (SpareField != null)
                {
                    ILabelControlService LabelService = new LabelControlService();
                    LabelControlDto labelResponse = new LabelControlDto();
                    labelResponse = LabelService.GetDto("th", SpareField[0].ProjectIDSys);
                    int id = SpareFieldService.CreateSpareField(SpareField);
                    response.SetData(id);
                }
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
        [Route("{ProjectIDSys}")]
        public HttpResponseMessage Put(int ProjectIDSys, [FromBody]IEnumerable<SpareField> SpareField)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                if (SpareField != null)
                {
                    bool isUpated = SpareFieldService.UpdateSpareField(SpareField);
                    response.SetData(isUpated);
                }
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
        [Route("{SpfIDSys}")]
        public HttpResponseMessage Delete(int SpfIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = SpareFieldService.DeleteSpareField(SpfIDSys);
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