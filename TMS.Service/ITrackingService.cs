using System;
using System.Collections.Generic;
using TMS.Common.ValueObject.Bookings;
using SearchBookings = TMS.Common.ValueObject.Search.Bookings;

namespace TMS.Service
{
    public interface ITrackingService
    {
        List<object> GetBooking(SearchBookings.TrackingModel param);
        List<object> GetBookingHistory(string BookingID);
    }
}
