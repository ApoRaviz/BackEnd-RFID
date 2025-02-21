﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Entity.View;
using WIM.Core.Entity.Employee;
using WIM.Core.Service.Impl.EmployeeMaster;
using WIM.Core.Service.EmployeeMaster;
using WIM.Core.Service.Impl;

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
                Person_MT Persons = PersonService.GetPersonByPersonIDSys(User.Identity.GetUserIdApp());
                response.SetData(Persons);
            }
            catch (AppValidationException ex)
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
            ResponseData<IEnumerable<VPersons>> response = new ResponseData<IEnumerable<VPersons>>();
            try
            {
                IEnumerable<VPersons> Persons = PersonService.GetPersons();
                response.SetData(Persons);
            }
            catch (AppValidationException ex)
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
            IResignService ResignService = new ResignService();
            IHistoryWarningService WarningService = new HistoryWarningService();
            try
            {
               
                PersonDto Person = PersonService.GetPersonByPersonID(PersonIDSys);
                User user = UserService.GetUserByPersonIDSys(PersonIDSys);
                Employee_MT Employee = EmployeeService.GetEmployeeByPerson(PersonIDSys);

                if (Employee != null)
                {
                    IEnumerable<HistoryWarning> Warning = WarningService.GetHistoryByEmID(Employee.EmID);
                    Resign resign = ResignService.GetResignByEmID(Employee.EmID);
                    Employee.HistoryWarnings = Warning.ToList();
                    Employee.Resign = resign;
                    Person.Employee = new CommonService().AutoMapper<EmployeeDto>(Employee);
                }
                if(user != null)
                {
                    Person.User = new CommonService().AutoMapper<UserDto>(user);
                }
                
                
                response.SetData(Person);
            }
            catch (AppValidationException ex)
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
                //this line for email add
                response.SetData(id);
            }
            catch (AppValidationException ex)
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
                bool isUpated = PersonService.UpdatePerson(Person);//User.Identity.GetUserId(),
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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

            IResponseData<List<Person_Email>> response = new ResponseData<List<Person_Email>> ();
            try
            {
                List<Person_Email> isUpated = PersonService.UpdatePersonByID(Person);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }        

    }   
}
