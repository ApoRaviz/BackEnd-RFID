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
using WMS.Master;
using WMS.Master.Common.ValueObject;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/locationGroup")]
    public class LocationGroupController : ApiController
    {
        private ILocationGroupService LocGroupService;

        public LocationGroupController(ILocationGroupService locGroupService)
        {
            this.LocGroupService = locGroupService;
        }

        #region LocationGroup

        [HttpGet]
        [Route("GetLocationGroup")]
        public HttpResponseMessage GetAllLocationGroup()
        {
            ResponseData<IEnumerable<GroupLocation>> response = new ResponseData<IEnumerable<GroupLocation>>();
            try
            {
                IEnumerable<GroupLocation> detail = LocGroupService.GetLocationGroup();
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
        [Route("GetUnassignLocationGroup")]
        public HttpResponseMessage GetUnassignLocationGroup()
        {
            ResponseData<IEnumerable<GroupLocation>> response = new ResponseData<IEnumerable<GroupLocation>>();
            try
            {
                IEnumerable<GroupLocation> detail = LocGroupService.GetUnassignLocationGroup();
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
        [Route("GetLocationGroupByID/{GroupLocIDSys}")]
        public HttpResponseMessage GetLocationGroupID(int GroupLocIDSys)
        {
            ResponseData<GroupLocation> response = new ResponseData<GroupLocation>();
            try
            {
               GroupLocation detail = LocGroupService.GetLocationGroupByGroupLocIDSys(GroupLocIDSys);
                response.SetData(detail);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpPost]
        [Route("GetLocationGroupBy")]
        public HttpResponseMessage GetLocationGroupBy([FromBody] GroupLocation item)
        {
            ResponseData<IEnumerable<GroupLocation>> response = new ResponseData<IEnumerable<GroupLocation>>();
            try
            {
                if(item != null)
                { 
                    
                    IEnumerable<GroupLocation> detail = LocGroupService.GetLocationGroupByZoneInfo(item);
                    response.SetData(detail);
                }
                else
                {
                    response.SetData(null);
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetLocationGroupByZoneIDSys/{zoneIdSys}")]
        public HttpResponseMessage GetLocationGroupByZoneIDSys(int zoneIdSys)
        {
            ResponseData<IEnumerable<GroupLocation>> response = new ResponseData<IEnumerable<GroupLocation>>();
            try
            {
                IEnumerable<GroupLocation> detail = LocGroupService.GetLocationGroupByZoneID(zoneIdSys);
                if (detail != null)
                {
                    response.SetData(detail);
                }
                else
                {
                    response.SetData(null);
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpPut]
        [Route("UpdateLocationGroupBy/{GroupLocationIDSys}")]
        public HttpResponseMessage UpdateLocationGroupBy(int GroupLocationIDSys, [FromBody] GroupLocation item)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                if (item != null)
                {
                    bool updated = LocGroupService.UpdateLocationGroup(GroupLocationIDSys, item);
                    response.SetData(updated);
                }
                else
                {
                    response.SetData(false);
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        #endregion

    }
}
