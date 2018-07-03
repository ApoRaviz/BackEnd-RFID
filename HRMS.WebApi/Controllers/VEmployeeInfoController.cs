using HRMS.Entity.Probation;
using HRMS.Service.Probation;
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
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.PositionConfigManagement;

namespace HRMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Probation")]
    public class VEmployeeInfoController : ApiController
    {
        private IVEmployeeInfoService VEmployeeInfoService;

        public VEmployeeInfoController(IVEmployeeInfoService VEmployeeInfoService)
        {
            this.VEmployeeInfoService = VEmployeeInfoService;

        }

        [HttpGet]
        [Route("list")]
        public HttpResponseMessage GetList()
        {
            ResponseData<IEnumerable<VEmployeeInfo>> response = new ResponseData<IEnumerable<VEmployeeInfo>>();
            try
            {
                IEnumerable<VEmployeeInfo> VEmployeeInfo = VEmployeeInfoService.GetProbation();
                response.SetData(VEmployeeInfo);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("list2")]
        public HttpResponseMessage GetList2() 
        {
            ResponseData<IEnumerable<VEmployeeInfo>> response = new ResponseData<IEnumerable<VEmployeeInfo>>();
            try
            {
                IEnumerable<VEmployeeInfo> VEmployeeInfo = VEmployeeInfoService.GetEmployeetoEvaluate(); 
                response.SetData(VEmployeeInfo);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Persons
        [HttpGet]
        [Route("{EmID}")]
        public HttpResponseMessage Get(string EmID)
        {
            ResponseData<VEmployeeInfo> response = new ResponseData<VEmployeeInfo>();
            try
            {
                VEmployeeInfo Employee = VEmployeeInfoService.GetEmployeeByEmployeeIDSys(EmID);
                response.SetData(Employee);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]VEmployeeInfo Employee)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = VEmployeeInfoService.UpdateEmployeeByID(Employee);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

    }
}