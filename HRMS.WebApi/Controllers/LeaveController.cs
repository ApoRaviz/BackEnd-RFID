using AutoMapper;
using HRMS.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using Validation = WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Linq.Expressions;
using HRMS.Entity.LeaveManagement;
using HRMS.Context;
using HRMS.Repository.Impl.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using HRMS.Service.LeaveManagement;
using HRMS.Service.Impl.LeaveManagement;
using HRMS.Common.ValueObject.LeaveManagement;

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/leave")]
    public class LeaveController : ApiController
    {
        private ILeaveService LeaveService;
        public LeaveController(ILeaveService leaveService)
        {
            LeaveService = new LeaveService(User.Identity);
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
        [Route("")]
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

    }
}