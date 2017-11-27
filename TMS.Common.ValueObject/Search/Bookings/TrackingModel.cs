using System;

namespace TMS.Common.ValueObject.Search.Bookings
{
    public class TrackingModel
    {
        public DateTime DateReceive { get; set; }
        public DateTime DateDelivery { get; set; }
        public string ReferenceCustomer { get; set; }
    }
}
