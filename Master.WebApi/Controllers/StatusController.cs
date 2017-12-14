using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Validation = WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Status;
using WIM.Core.Common.ValueObject;
using WIM.Core.Service.StatusManagement;
using WIM.Core.Service.Impl.StatusManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Extensions;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/Status")]
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
        public HttpResponseMessage Put([FromBody]StatusDto statusUpdate)
        {
            ResponseData<Status_MT> response = new ResponseData<Status_MT>();
            try
            {
                Status_MT isUpdated = StatusService.UpdateStatus(statusUpdate);
                response.SetData(isUpdated);
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
            ResponseData<IEnumerable<StatusSubModuleDto>> response = new ResponseData<IEnumerable<StatusSubModuleDto>>();
            try
            {
                IEnumerable<StatusSubModuleDto> status = StatusService.GetStatus();
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