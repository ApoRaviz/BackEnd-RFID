using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Validation = WIM.Core.Common.Utility.Validation;
using HRMS.Entity.LeaveManagement;
using HRMS.Service.LeaveManagement;
using HRMS.Service.Impl.LeaveManagement;
using HRMS.Common.ValueObject.LeaveManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Extensions;

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("HRMS/api/v1/leave")]
    public class LeaveController : ApiController
    {
        private ILeaveService LeaveService;
        public LeaveController(ILeaveService leaveService)
        {
            LeaveService = new LeaveService();
        }

        //Create LeaveReq
        [HttpPost]
        [Route("new")]
        public HttpResponseMessage Post([FromBody]Leave leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {
                leaveRequest.CreateBy = User.Identity.Name;
                Leave id = LeaveService.CreateLeave(leaveRequest);
                response.SetData(id);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Update LeaveReq
        [HttpPut]
        [Route("{LeaveIDSys}")]
        public HttpResponseMessage Put([FromBody]LeaveDto leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {
                Leave isUpdated = LeaveService.UpdateLeave(leaveRequest);
                response.SetData(isUpdated);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get Dto By ID
        [HttpGet]
        [Route("{LeaveIDSys}")]
        public HttpResponseMessage Get(int LeaveIDSys)
        {
            ResponseData<LeaveDto> response = new ResponseData<LeaveDto>();
            try
            {
                LeaveDto leaveReqByID = LeaveService.GetLeaveByID(LeaveIDSys);
                response.SetData(leaveReqByID);
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
            ResponseData<IEnumerable<Leave>> response = new ResponseData<IEnumerable<Leave>>();
            try
            {
                IEnumerable<Leave> leaveReq = LeaveService.GetLeaves();
                response.SetData(leaveReq);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get LeaveType Data
        [HttpGet]
        [Route("leaveType")]
        public HttpResponseMessage GetLT()
        {
            ResponseData<IEnumerable<LeaveTypeDto>> response = new ResponseData<IEnumerable<LeaveTypeDto>>();
            try
            {
                IEnumerable<LeaveTypeDto> leaveType = LeaveService.GetLeaveType();
                response.SetData(leaveType);
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