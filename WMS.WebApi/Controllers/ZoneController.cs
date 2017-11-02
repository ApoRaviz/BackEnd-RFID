using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Extensions;
using WMS.Service;
using WMS.Entity.WarehouseManagement;
using WMS.Service.WarehouseMaster;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/zone")]
    public class ZoneController : ApiController
    {
        private IZoneService ZoneService;

        public ZoneController(IZoneService zoneService)
        {
            this.ZoneService = zoneService;
        }

        [HttpGet]
        [Route("GetZone")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<ZoneLayoutHeader_MT>> response = new ResponseData<IEnumerable<ZoneLayoutHeader_MT>>();
            try
            {
                IEnumerable<ZoneLayoutHeader_MT> header = ZoneService.GetAllZoneHeader();
                response.SetData(header);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetZoneDetail")]
        public HttpResponseMessage GetZoneDetail()
        {
            ResponseData<IEnumerable<ZoneLayoutDetail_MT>> response = new ResponseData<IEnumerable<ZoneLayoutDetail_MT>>();
            try
            {
                IEnumerable<ZoneLayoutDetail_MT> detail = ZoneService.GetAllZoneDetail();
                response.SetData(detail);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/zone/1
        [HttpGet]
        [Route("{ZoneIDSys}")]
        public HttpResponseMessage Get(int ZoneIDSys)
        {
            IResponseData<ZoneLayoutHeader_MT> response = new ResponseData<ZoneLayoutHeader_MT>();
            try
            {
                ZoneLayoutHeader_MT zone = ZoneService.GetZoneLayoutByZoneIDSys(ZoneIDSys, "ZoneLayoutDetail_MT");
                response.SetData(zone);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ZoneLayoutHeader_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UpdateBy = User.Identity.Name;
                int id = ZoneService.CreateZoneLayout(data).Value;
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("{ZoneIDSys}")]
        public HttpResponseMessage Put(int ZoneIDSys, [FromBody]ZoneLayoutHeader_MT data)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = ZoneService.UpdateZoneLayout(ZoneIDSys, data);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("CreateRack")]
        public HttpResponseMessage CreateRack([FromBody]List<RackLayout_MT> data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = ZoneService.CreateRackLayout(data).Value;
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetRackDetail")]
        public HttpResponseMessage GetRackDetail(int ZoneIDSys, int ZoneID)
        {
            ResponseData<IEnumerable<RackLayout>> response = new ResponseData<IEnumerable<RackLayout>>();
            try
            {
                IEnumerable<RackLayout> detail = ZoneService.GetAllRackDetail(ZoneIDSys, ZoneID);
                response.SetData(detail);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetRackDetailByZoneIDSys/{ZoneIDSys}")]
        public HttpResponseMessage GetRackDetailByZoneIDSys(int ZoneIDSys)
        {
            ResponseData<IEnumerable<RackLayout>> response = new ResponseData<IEnumerable<RackLayout>>();
            try
            {
                IEnumerable<RackLayout> detail = ZoneService.GetRackDetailByZoneIDSys(ZoneIDSys);
                response.SetData(detail);
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
