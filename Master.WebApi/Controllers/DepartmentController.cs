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
    [RoutePrefix("api/v1/Department")]
    public class DepartmentController : ApiController
    {
        private IDepartmentService DepartmentService;

        public DepartmentController(IDepartmentService departmentservice)
        {
            this.DepartmentService = departmentservice;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Departments>> response = new ResponseData<IEnumerable<Departments>>();
            try
            {
                IEnumerable<Departments> Employees = DepartmentService.GetDepartments();
                response.SetData(Employees);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Employees/1
        [HttpGet]
        [Route("{DepIDSys}")]
        public HttpResponseMessage Get(int DepIDSys)
        {
            IResponseData<Departments> response = new ResponseData<Departments>();
            try
            {
                Departments Employee = DepartmentService.GetDepartmentByDepIDSys(DepIDSys);
                response.SetData(Employee);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Employees
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Departments Department)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Department.UpdateBy = User.Identity.Name;
                int id = DepartmentService.CreateDepartment(Department);
                response.SetData(id);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Employees/5

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]Departments Department)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = DepartmentService.UpdateDepartment(Department);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Employees/5
        [HttpDelete]
        [Route("{DepID}")]
        public HttpResponseMessage Delete(int DepID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = DepartmentService.DeleteDepartment(DepID);
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