﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Entity.WarehouseManagement;
using WMS.Service.LocationMaster;

namespace WMS.WebApi.Controller
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
            ResponseData<IEnumerable<Location>> response = new ResponseData<IEnumerable<Location>>();
            try
            {
                IEnumerable<Location> GeoupLocation = LocationService.GetList();
                response.SetData(GeoupLocation);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("search")]
        public HttpResponseMessage Get([FromUri] FromUriParamsObject paramsObject)
        {
            //public HttpResponseMessage Get(int GroupLocIDSys)
            //{
            //public HttpResponseMessage Get(int IDSys)
            //{
            ResponseData<GroupLocation> response = new ResponseData<GroupLocation>();
            try
            {
                GroupLocation GeoupLocation = LocationService.GetLocationByGroupLocIDSys(paramsObject.IDSys);
                response.SetData(GeoupLocation);
            }
            catch (AppValidationException ex)
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
            IResponseData<Location> response = new ResponseData<Location>();
            try
            {
                Location Location = LocationService.GetLocationByLocIDSys(LocIDSys);
                response.SetData(Location);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        //[HttpPost]
        //[Route("")]
        //public HttpResponseMessage Post([FromBody]Location_MT Location)
        //{
        //    IResponseData<int> response = new ResponseData<int>();
        //    try
        //    {
        //        Location.UpdateBy = User.Identity.Name;
        //        int id = LocationService.CreateLocation(Location);
        //        response.SetData(id);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

        //[HttpPost]
        //[Route("")]
        //public HttpResponseMessage Post([FromBody]GroupLocation Location)
        //{
        //    IResponseData<Location> resGroup = new ResponseData<Location>();
        //    try
        //    {
        //        Location.UpdateBy = User.Identity.Name;
        //        Location rs = LocationService.CreateLocation(Location.Location);
        //        response.SetData(Location);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{LocIDSys}")]
        public HttpResponseMessage Put(int LocIDSys, [FromBody]Location Location)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = LocationService.UpdateLocation(LocIDSys, Location);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
