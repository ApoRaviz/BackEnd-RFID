using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Entity.InventoryManagement;
using WMS.Service.Inventories;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/Inventories")]
    public class InventoriesController : ApiController
    {
        private IInventoryService InventoryService;
        public InventoriesController(IInventoryService inventoryService)
        {
            this.InventoryService = inventoryService;
        }

        [HttpPost]
        [Route("confirmReceive")]
        public HttpResponseMessage ConfirmReceive([FromBody] ConfirmReceive confirmReceive)
        {
            ResponseData<int> response = new ResponseData<int>();
            try
            {
                InventoryService.ConfirmReceive(confirmReceive);
                response.SetData(1);
                response.SetStatus(HttpStatusCode.OK);                
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
