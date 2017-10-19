using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Entity.WarehouseManagement;
using WMS.Master;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Warehouses")]
    public class WarehousesController : ApiController
    {
        private IWarehouseService WarehouseService;

        public WarehousesController(IWarehouseService WarehouseService)
        {
            this.WarehouseService = WarehouseService;
        }

        //get api/Warehouses
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Warehouse_MT>> response = new ResponseData<IEnumerable<Warehouse_MT>>();
            try
            {
                IEnumerable<Warehouse_MT> Warehouse = WarehouseService.GetWarehouses();
                response.SetData(Warehouse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Warehouses/id

        [HttpGet]
        [Route("{LocIDSys}")]
        public HttpResponseMessage Get(int LocIDSys)
        {
            IResponseData<Warehouse_MT> response = new ResponseData<Warehouse_MT>();
            try
            {
                Warehouse_MT Warehouse = WarehouseService.GetWarehouseByLocIDSys(LocIDSys);
                response.SetData(Warehouse);
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
        public HttpResponseMessage Post([FromBody]Warehouse_MT Warehouse)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Warehouse.UserUpdate = User.Identity.Name;
                int id = WarehouseService.CreateWarehouse(Warehouse);
                response.SetData(id);
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
        [Route("{LocIDSys}")]
        public HttpResponseMessage Put(int LocIDSys, [FromBody]Warehouse_MT Warehouse)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = WarehouseService.UpdateWarehouse(LocIDSys, Warehouse);
                response.SetData(isUpated);
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
                bool isUpated = WarehouseService.DeleteWarehouse(LocIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IWarehouseService GetWarehouseService()
        {
            return WarehouseService;
        }
    }
}