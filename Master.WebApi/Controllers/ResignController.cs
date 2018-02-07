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
using WIM.Core.Service.EmployeeMaster;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/Resign")]
    public class ResignController : ApiController
    {
        private IResignService ResignService;

        public ResignController(IResignService resignservice)
        {
            this.ResignService = resignservice;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Resign>> response = new ResponseData<IEnumerable<Resign>>();
            try
            {
                IEnumerable<Resign> resign = ResignService.GetResign();
                response.SetData(resign);
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
        [Route("employee/{EmID}")]
        public HttpResponseMessage Get(string EmID)
        {
            IResponseData<Resign> response = new ResponseData<Resign>();
            try
            {
                Resign Employee = ResignService.GetResignByEmID(EmID);
                response.SetData(Employee);
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
        public HttpResponseMessage Post([FromBody]Resign resign)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string id = ResignService.CreateResign(resign);
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
        public HttpResponseMessage Put([FromBody]Resign warning)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = ResignService.UpdateResign(warning);
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
        //[HttpDelete]
        //[Route("{DepID}")]
        //public HttpResponseMessage Delete(int DepID)
        //{
        //    IResponseData<bool> response = new ResponseData<bool>();
        //    try
        //    {
        //        bool isUpated = DepartmentService.DeleteDepartment(DepID);
        //        response.SetData(isUpated);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

    }
}