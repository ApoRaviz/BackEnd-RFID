using Fuji.Service.ItemImport;
using Fuji.Service.Receive;
using Fuji.WebApi.Models;
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
        private IItemImportService _itemImportService;
        public ReceivesController(
            IReceiveService receiveService,
            IItemImportService itemImportService
        ) {
            _receiveService = receiveService;
            _itemImportService = itemImportService;
        }

        [Authorize]
        [HttpPost]
        [Route("confirm2stock")]
        public async Task<HttpResponseMessage> Confirm2Stock([FromBody]Confirm2StockRequest confirm2StockRequest)
        {
            ResponseData<bool> responseData = new ResponseData<bool>();
            try
            {
                var isConfirmedStock = await _receiveService.Confirm2Stock(confirm2StockRequest.HeadId);
                if (isConfirmedStock)
                {
                    _itemImportService.SetConfirmToStock(confirm2StockRequest.HeadId);              
                }
                responseData.SetStatus(HttpStatusCode.OK);
                responseData.SetData(true);
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
