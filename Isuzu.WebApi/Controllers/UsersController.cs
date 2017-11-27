using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;

namespace Isuzu.WebApi.Controllers
{
    [RoutePrefix("api/v1/Users")]
    public class UsersController : ApiController
    {
        private IUserService UserService;
        private ICustomerService CustomerService;


        public UsersController(IUserService UserService, ICustomerService customerService)
        {
            this.UserService = UserService;
            this.CustomerService = customerService;
        }

        //get api/Users
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<User>> response = new ResponseData<IEnumerable<User>>();
            try
            {
                IEnumerable<User> User = UserService.GetUsers();
                response.SetData(User);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Users/id

        [HttpGet]
        [Route("{UserID}")]
        public HttpResponseMessage Get(string UserID)
        {
            IResponseData<User> response = new ResponseData<User>();
            try
            {

                User User = UserService.GetUserByUserID(UserID);
                response.SetData(User);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("customers")]
        public HttpResponseMessage GetCustonersByUserID()
        {
            ResponseData<object> response = new ResponseData<object>();
            try
            {
                string userid = User.Identity.GetUserId();
                object customer;
                if (User.IsSysAdmin())
                {
                    customer = CustomerService.GetCustomerAll();
                }
                else
                {
                    customer = UserService.GetCustonersByUserID(userid);
                }

                response.SetData(customer);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]User User)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string id = "";
                PasswordHasher ph = new PasswordHasher();
                User.PasswordHash = ph.HashPassword("1234!");
                id = UserService.CreateUser(User);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Suppliers
        [HttpGet]
        [Route("mobile/keyaccess")]
        public HttpResponseMessage GetKeyRegisterMobile()
        {
            object val = Request.GetHeaderValue("Accept");
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string userid = User.Identity.GetUserId();
                Random rnd = new Random();
                String str = "", sreRs = "", key = "";
                for (int i = 0; i < 3; i++)
                {
                    key = rnd.Next(1000, 9999).ToString();
                    sreRs += key;
                    str += key;
                    if (i < 2)
                        sreRs += " - ";
                }

                UserService.GetKeyRegisterMobile(userid, key);
                response.SetData(sreRs);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("mobile/keyaccess")]
        public HttpResponseMessage PostRegisterFirebaseTokenMobile([FromBody]KeyAccessModel param)
        {
            IResponseData<Dictionary<string, string>> response = new ResponseData<Dictionary<string, string>>();
            try
            {
                string userid = User.Identity.GetUserId();
                bool key = UserService.RegisterTokenMobile(param);
                Dictionary<string, string> Json = new Dictionary<string, string>();
                if (key)
                {
                    Json.Add("message", "Success");
                    Json.Add("status", "200");
                }
                else
                {
                    Json.Add("message", "Error");
                    Json.Add("status", "500");
                }
                response.SetData(Json);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("mobile/uodatefirebasetoken")]
        public HttpResponseMessage PostUpdateFirebaseTokenMobile([FromBody]FirebaseTokenModel param)
        {
            IResponseData<Dictionary<string, string>> response = new ResponseData<Dictionary<string, string>>();
            try
            {
                string userid = User.Identity.GetUserId();
                bool key = UserService.UodateTokenMobile(param);
                Dictionary<string, string> Json = new Dictionary<string, string>();
                if (key)
                {
                    Json.Add("message", "Success");
                    Json.Add("status", "200");
                }
                else
                {
                    Json.Add("message", "Error");
                    Json.Add("status", "500");
                }
                response.SetData(Json);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]User User)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = UserService.UpdateUser(User);
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
        [Route("reset")]
        public HttpResponseMessage PutResetPassword([FromBody]User User)
        {

            IResponseData<string> response = new ResponseData<string>();

            try
            {
                PasswordHasher ph = new PasswordHasher();
                User.PasswordHash = ph.HashPassword("1234!");
                bool isUpated = UserService.UpdateUser(User);
                response.SetData(User.PasswordHash);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpDelete]
        [Route("{LocIDSys}")]
        public HttpResponseMessage Delete(int LocIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = UserService.DeleteUser(LocIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IUserService GetUserService()
        {
            return UserService;
        }
    }
}
