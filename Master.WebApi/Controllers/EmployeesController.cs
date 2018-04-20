using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity;
using WIM.Core.Entity.PositionConfigManagement;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Employees")]
    public class EmployeesController : ApiController
    {
        private IEmployeeService Employeeservice;

        public EmployeesController(IEmployeeService employeeservice)
        {
            this.Employeeservice = employeeservice;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Employee_MT>> response = new ResponseData<IEnumerable<Employee_MT>>();
            try
            {
                IEnumerable<Employee_MT> Employees = Employeeservice.GetEmployees();
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
        public HttpResponseMessage Get(string EmID)
        {
            IResponseData<Employee_MT> response = new ResponseData<Employee_MT>();
            try
            {
                Employee_MT Employee = Employeeservice.GetEmployeeByEmployeeIDSys(EmID);
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
        public HttpResponseMessage Post([FromBody]Employee_MT Employee)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                Employee.UpdateBy = User.Identity.Name;
                string id = Employeeservice.CreateEmployee(Employee);
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
        public HttpResponseMessage Put( [FromBody]Employee_MT Employee)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = Employeeservice.UpdateEmployeeByID(Employee);
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
        [Route("{EmID}")]
        public HttpResponseMessage Delete(string EmID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = Employeeservice.DeleteEmployee(EmID);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("positionconfig2/{id}")]
        public HttpResponseMessage setPositionConfig2(string id, [FromBody]WelfareConfig positionConfig)
        {
            IResponseData<Employee_MT> response = new ResponseData<Employee_MT>();
            try
            {
                Employee_MT rs = Employeeservice.SetPositionConfig2(id, positionConfig);
                response.SetData(rs);
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
