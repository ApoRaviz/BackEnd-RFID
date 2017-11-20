using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using Validation = WIM.Core.Common.Validation;
using HRMS.Service.StatusManagement;
using HRMS.Service.Impl.StatusManagement;
using WIM.Core.Entity.Status;
using WIM.Core.Common.ValueObject;

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("Master/api/v1/Status")]
    public class StatusController : ApiController
    {
        private IStatusService StatusService;
        public StatusController(IStatusService statusService)
        {
            StatusService = new StatusService();
        }

        //Create Status
        [HttpPost]
        [Route("new")]
        public HttpResponseMessage Post([FromBody]StatusDto status)
        {
            ResponseData<Status_MT> response = new ResponseData<Status_MT>();
            try
            {
                Status_MT Status = new Status_MT();
                Status.Title = status.Title;
                
                Status_MT id = StatusService.CreateStatus(Status,status.StatusSubModule);
                response.SetData(id);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Update Status
        [HttpPut]
        [Route("{StatusIDSys}")]
        public HttpResponseMessage Put([FromBody]Status_MT statusUpdate)
        {
            ResponseData<Status_MT> response = new ResponseData<Status_MT>();
            try
            {
                Status_MT isUpdated = StatusService.UpdateStatus(statusUpdate);
                response.SetData(statusUpdate);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get Status By ID
        [HttpGet]
        [Route("{StatusIDSys}")]
        public HttpResponseMessage Get(int StatusIDSys)
        {
            ResponseData<StatusDto> response = new ResponseData<StatusDto>();
            try
            {
                StatusDto statusByID = StatusService.GetStatusByID(StatusIDSys);
                response.SetData(statusByID);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get Data
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Status_MT>> response = new ResponseData<IEnumerable<Status_MT>>();
            try
            {
                IEnumerable<Status_MT> status = StatusService.GetStatus();
                response.SetData(status);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }
}