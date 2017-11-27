using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ValueObject.Bookings;
using TMS.Common.ValueObject.Labels;
using SearchBookings = TMS.Common.ValueObject.Search.Bookings;

namespace TMS.Service.Reports
{
    public interface IReportTrackingBooking
    {
        List<BookingModel> GetSearchTrackingBooking(
            SearchBookings.TrackingModel param);

    }
}
