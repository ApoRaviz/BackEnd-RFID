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

namespace Master.WebApi
{
    [RoutePrefix("api/v1/Probation")]
    public class ProbationController : ApiController
    {
        private IProbationService ProbationService;

        public ProbationController(IProbationService ProbationService)
        {
            this.ProbationService = ProbationService;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Probation_MT>> response = new ResponseData<IEnumerable<Probation_MT>>();
            try
            {
                IEnumerable<Probation_MT> Employees = ProbationService.GetProbation();
                response.SetData(Employees);
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
        [Route("{EmID}")]
        public HttpResponseMessage Get(int EmID)
        {
            IResponseData<Probation_MT> response = new ResponseData<Probation_MT>();
            try
            {
                Probation_MT Employee = ProbationService.GetProbationByEmID(EmID);
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
        public HttpResponseMessage Post([FromBody]Probation_MT Probation)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Probation.UpdateBy = User.Identity.Name;
                int id = ProbationService.CreateProbation(Probation);
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
        public HttpResponseMessage Put([FromBody]Probation_MT Probation)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = ProbationService.UpdateProbation(Probation);
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
        //        bool isUpated = ProbationService.DeleteProbation(DepID);
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