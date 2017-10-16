using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using WIM.Core.Common.Extensions;
using WMS.Master;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Web.Http.Cors;
using WMS.Common;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/currency")]
    public class CurrencyController : ApiController
    {

        private ICustomerService CustomerService;

        public CurrencyController(ICustomerService customerService)
        {
            this.CustomerService = customerService;
        }

        //// GET: api/Customers
        //[HttpGet]
        //[Route("")]
        //public HttpResponseMessage Get()
        //{
        //    ResponseData<IEnumerable<ProcGetCustomers_Result>> response = new ResponseData<IEnumerable<ProcGetCustomers_Result>>();
        //    try
        //    {
        //        IEnumerable<ProcGetCustomers_Result> customers = CustomerService.GetCustomers();
        //        response.SetData(customers);
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
