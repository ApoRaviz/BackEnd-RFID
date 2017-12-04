using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Service;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/customers")]
    public class CustomersController : ApiController
    {
        private ICustomerService CustomerService;
        private IProjectService ProjectService;


        public CustomersController(ICustomerService customerService, IProjectService projectService)
        {
            this.CustomerService = customerService;
            this.ProjectService = projectService;
        }

        //// GET: api/Customers
        //[HttpGet]
        //[Route("")]
        //public HttpResponseMessage Get([FromUri]UriCustomersModel param)
        //{
        //    ResponseData<object> response = new ResponseData<object>();
        //    object res = null;
        //    try
        //    {
        //        switch (param.action)
        //        {
        //            case "getcustomerbyuserid":
        //                res = this.CustomerService.GetCustonersByUserID(User.Identity.GetUserId());
        //                break;

        //        }


        //        response.SetData(res);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

        [HttpGet]
        [Route("projects/{cusIDSys}")]
        public HttpResponseMessage GetIncludeProjects(int cusIDSys)
        {
            ResponseData<CustomerDto> response = new ResponseData<CustomerDto>();
            try
            {
                CustomerDto customer = CustomerService.GetCustomerByCusIDSysIncludeProjects(cusIDSys);
                response.SetData(customer);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        // GET: api/Customers
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
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
                    customer = CustomerService.GetCustomers(userid);
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



        [HttpGet]
        [Route("projects")]
        public object GetIncludeProjects([FromUri]UriCustomersModel param)
        {
            ResponseData<object> response = new ResponseData<object>();
            try
            {
                string userid = User.Identity.GetUserId();
                object customer;
                if (User.IsSysAdmin())
                {
                    customer = ProjectService.ProjectCustomer(param.cusIDSys);
                }
                else
                {
                    customer = CustomerService.GetProjectByCustomer(userid, param.cusIDSys);
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

        // GET: api/customers/1/projects
        [HttpGet]
        [Route("{cusIDSys}")]
        public HttpResponseMessage Get(int cusIDSys, string include = null)
        {
            ResponseData<CustomerDto> response = new ResponseData<CustomerDto>();
            try
            {
                string[] tableNames = null;
                if (!string.IsNullOrEmpty(include))
                {
                    tableNames = include.Split(',');
                }
                CustomerDto customer = CustomerService.GetCustomersInclude(cusIDSys, tableNames);
                response.SetData(customer);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        // POST: api/Customers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Customer_MT customer)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                customer.UpdateBy = User.Identity.Name;
                int id = CustomerService.CreateCustomer(customer);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Customers/5

        [HttpPut]
        [Route("{cusIDSys}")]
        public HttpResponseMessage Put(int cusIDSys, [FromBody]Customer_MT customer)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = CustomerService.UpdateCustomer(customer);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Customers/5
        [HttpDelete]
        [Route("{cusIDSys}")]
        public HttpResponseMessage Delete(int cusIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = CustomerService.DeleteCustomer(cusIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetReport")]
        public HttpResponseMessage GetReport()
        {
            //var list = CustomerService.GetCustomers("");
            //return ReportUtils.ViewReport("Report/CustomerReport.rdlc", list.ToList<CustomerDto>());
            ////return ReportUtils.ViewReport("D:\\Projects\\CustomerReport.rdlc", list.ToList<ProcGetCustomers_Result>());
            return null;
        }
    }

    public class UriCustomersModel
    {
        public string action { get; set; }
        public int cusIDSys { get; set; }
    }
}
