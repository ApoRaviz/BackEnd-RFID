using Fuji.Service.Receive;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/receives")]
    public class ReceivesController : ApiController
    {

        private IReceiveService _receiveService;
        public ReceivesController(IReceiveService receiveService)
        {
            _receiveService = receiveService;
        }

        [Authorize]
        [HttpPost]
        [Route("confirm2stock")]
        public async Task<HttpResponseMessage> Confirm2Stock([FromBody]Confirm2StockRequest confirm2StockRequest)
        {
            try
            {
                await _receiveService.Confirm2Stock(confirm2StockRequest.HeadId);

                ResponseData<string> responseData = new ResponseData<string>();
                responseData.SetStatus(HttpStatusCode.OK);
                responseData.SetData("");
                return Request.ReturnHttpResponseMessage(responseData);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class Confirm2StockRequest
    {
        public string HeadId { get; set; }
    }
}
