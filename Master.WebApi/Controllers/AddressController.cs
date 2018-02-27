using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using System.Web.Http.Cors;
using WIM.Core.Entity.Currency;
using WIM.Core.Service;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Service.Address;
using WIM.Core.Common.ValueObject;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/address")]
    public class AddressController : ApiController
    {

        private IAddressService AddressService;

        public AddressController(IAddressService AddressService)
        {
            this.AddressService = AddressService;
        }

        //get api/Address
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<Object> response = new ResponseData<Object>();
            try
            {
                Object Address = AddressService.GetAddress();
                response.SetData(Address);
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
