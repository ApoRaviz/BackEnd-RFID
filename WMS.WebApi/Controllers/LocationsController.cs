using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Entity.WarehouseManagement;
using WMS.Service;
using WMS.Service.LocationMaster;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Locations")]
    public class LocationsController : ApiController
    {
        private ILocationService LocationService;

        public LocationsController(ILocationService LocationService)
        {
            this.LocationService = LocationService;
        }

        //get api/Locations
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Location_MT>> response = new ResponseData<IEnumerable<Location_MT>>();
            try
            {
                IEnumerable<Location_MT> Location = LocationService.GetLocations();
                response.SetData(Location);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Locations/id

        [HttpGet]
        [Route("{LocIDSys}")]
        public HttpResponseMessage Get(int LocIDSys)
        {
            IResponseData<Location_MT> response = new ResponseData<Location_MT>();
            try
            {
                Location_MT Location = LocationService.GetLocationByLocIDSys(LocIDSys);
                response.SetData(Location);
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
        public HttpResponseMessage Post([FromBody]Location_MT Location)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Location.UpdateBy = User.Identity.Name;
                int id = LocationService.CreateLocation(Location);
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
        public HttpResponseMessage Put(int LocIDSys, [FromBody]Location_MT Location)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = LocationService.UpdateLocation(LocIDSys, Location);
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
                bool isUpated = LocationService.DeleteLocation(LocIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private ILocationService GetLocationService()
        {
            return LocationService;
        }
    }
}
