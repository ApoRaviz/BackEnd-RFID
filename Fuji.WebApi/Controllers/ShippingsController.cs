using Fuji.Entity.Shipping;
using Fuji.Service.Shipping;
using Fuji.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WIM.Core.Common.Utility.Http;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/shippings")]
    public class ShippingsController : ApiController
    {
        private IShippingService _shippingService;
        public ShippingsController(
            IShippingService shippingService
        )
        {
            _shippingService = shippingService;
        }

        //[Authorize]
        [HttpGet]
        [Route("orders/{orderNumber}")]
        public async Task<HttpResponseMessage> GetOrderAsync(string orderNumber)
        {
            ResponseData<AllocateView> responseData = new ResponseData<AllocateView>();
            try
            {
                var order = await _shippingService.GetOrderAsync(orderNumber);               
                responseData.SetStatus(HttpStatusCode.OK);
                responseData.SetData(order);
                return Request.ReturnHttpResponseMessage(responseData);
            }
            catch (Exception ex)
            {
                responseData.SetErrors(new List<string> { ex.Message });
                responseData.SetStatus(HttpStatusCode.InternalServerError);
                return Request.ReturnHttpResponseMessage(responseData);
            }
        }
    }
}
