using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WMS.Master;
using System.Web.Http.Cors;
using WMS.Service;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Persons")]
    public class PersonsController : ApiController
    {
        private IPersonService PersonService;
        private IUserService UserService;
        private IEmployeeService EmployeeService;

        public PersonsController(IPersonService PersonService, IUserService UserService, IEmployeeService EmployeeService)
        {
            this.PersonService = PersonService;
            this.UserService = UserService;
            this.EmployeeService = EmployeeService;
        }

        // GET: api/Persons
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<Person_MT> response = new ResponseData<Person_MT>();
            try
            {
                Person_MT Persons = PersonService.GetPersonByPersonIDSys(User.Identity.GetUserId());
                response.SetData(Persons);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("list")]
        public HttpResponseMessage GetList()
        {
            ResponseData<IEnumerable<Person_MT>> response = new ResponseData<IEnumerable<Person_MT>>();
            try
            {
                IEnumerable<Person_MT> Persons = PersonService.GetPersons();
                response.SetData(Persons);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Persons/1
        [HttpGet]
        [Route("{PersonIDSys}")]
        public HttpResponseMessage Get(int PersonIDSys)
        {
            IResponseData<PersonDto> response = new ResponseData<PersonDto>();
            try
            {
                PersonDto Person = PersonService.GetPersonByPersonID(PersonIDSys);
                User user = UserService.GetUserByPersonIDSys(PersonIDSys);
                Employee_MT Employee = EmployeeService.GetEmployeeByPerson(PersonIDSys);
                Person.User = user;
                Person.Employee = Employee;
                response.SetData(Person);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }        

        // POST: api/Persons
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Person_MT Person)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Person.UpdateBy = User.Identity.Name;
                int id = PersonService.CreatePerson(Person);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Persons/5

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put( [FromBody]Person_MT Person)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = PersonService.UpdatePerson( Person);//User.Identity.GetUserId(),
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("person")]
        public HttpResponseMessage PutByID([FromBody]Person_MT Person)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = PersonService.UpdatePersonByID(Person);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Persons/5
        [HttpDelete]
        [Route("{PersonIDSys}")]
        public HttpResponseMessage Delete(int PersonIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = PersonService.DeletePerson(PersonIDSys);
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
