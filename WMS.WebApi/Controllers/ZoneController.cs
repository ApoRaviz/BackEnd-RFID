using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WMS.Service;
using WMS.Entity.WarehouseManagement;
using WMS.Service.WarehouseMaster;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace WMS.WebApi.Controller
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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
            IResponseData<object> response = new ResponseData<object>();
            try
            {
                ZoneLayoutHeader_MT zone = ZoneService.GetZoneLayoutByZoneIDSys(ZoneIDSys, "ZoneLayoutDetail_MT");
              

                response.SetData(zone);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        #region Rack

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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        #endregion

        #region ZoneType
        [HttpGet]
        [Route("GetAllZoneType")]
        public HttpResponseMessage GetAllZonType()
        {
            ResponseData<IEnumerable<ZoneType>> response = new ResponseData<IEnumerable<ZoneType>>();
            try
            {
                IEnumerable<ZoneType> detail = ZoneService.GetAllZoneType();
                if (detail != null)
                    detail = detail.OrderByDescending(d => d.Priority);
                response.SetData(detail);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetZoneTypeByID/{ZoneTypeIDSys}")]
        public HttpResponseMessage GetZonTypeByZoneTypeIDSys(int ZoneTypeIDSys)
        {
            ResponseData<ZoneType> response = new ResponseData<ZoneType>();
            try
            {
                ZoneType detail = ZoneService.GetZoneTypeByID(ZoneTypeIDSys);
                response.SetData(detail);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("CreateZoneType")]
        public HttpResponseMessage CreateZoneType([FromBody]ZoneType data)
        {
            ResponseData<int?> response = new ResponseData<int?>();
            try
            {
                int? retID = ZoneService.CreateZoneType(data);
                response.SetData(retID);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("UpdateZoneTypeByID/{ZoneTypeIDSys}")]
        public HttpResponseMessage UpdateZoneTypeByID(int ZoneTypeIDSys, [FromBody] ZoneType data)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpdate =  ZoneService.UpdateZoneType(ZoneTypeIDSys,data);
                response.SetData(isUpdate);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpDelete]
        [Route("RemoveZoneTypeByID/{ZoneTypeIDSys}")]
        public HttpResponseMessage RemoveZonTypeByZoneTypeIDSys(int ZoneTypeIDSys)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                 ZoneService.RemoveZoneTypeByID(ZoneTypeIDSys);
                response.SetData(true);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        #endregion

      

    }
}
