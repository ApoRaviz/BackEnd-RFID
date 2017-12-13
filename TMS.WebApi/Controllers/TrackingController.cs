using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TMS.Service;
using TMS.Service.Impl;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using SearchBookings = TMS.Common.ValueObject.Search.Bookings;

namespace TMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/external/tracking")]
    public class TrackingController : ApiController
    {
        [HttpGet]
        [Route("booking")]
        public HttpResponseMessage GetBooking([FromUri]SearchBookings.TrackingModel TrackingModel)
        {
            //string[] BookingIDs = BookingIDs;
            ITrackingService TrackingService = new TrackingService();
            IResponseData<List<object>> response = new ResponseData<List<object>>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
            response.SetStatus(HttpStatusCode.OK);
            response.SetData(TrackingService.GetBooking(TrackingModel));
            return Request.ReturnHttpResponseMessage(response);


        }

        [HttpGet]
        [Route("bookinghistory/{BookingID}")]
        public HttpResponseMessage GetBookingHistory(string BookingID)
        {
            //string[] BookingIDs = BookingIDs;
            ITrackingService TrackingService = new TrackingService();
            IResponseData<List<object>> response = new ResponseData<List<object>>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
            response.SetStatus(HttpStatusCode.OK);
            response.SetData(TrackingService.GetBookingHistory(BookingID));
            return Request.ReturnHttpResponseMessage(response);


        }

    }
}
