using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using TMS.Common.ValueObject.Search.Bookings;

namespace TMS.Service.Impl
{
    public class TrackingService : ITrackingService
    {
        string YtdmUrl;
        public TrackingService()
        {
            YtdmUrl = ConfigurationManager.AppSettings["as:YtdmUrl"];
        }

        public List<object> GetBooking(TrackingModel param)
        {
            WebClient oWeb = new WebClient();
            string url = YtdmUrl + "pro_booking/proc/booking_manager.json.php";
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
            parameters.Add("action", "GetReportTracking");
            parameters.Add("date_delivery", param.DateDelivery.ToString("yyyy-MM-dd"));
            parameters.Add("ref_cus", param.ReferenceCustomer);
            var responseBytes = oWeb.UploadValues(url, parameters);
            string json = Encoding.ASCII.GetString(responseBytes);
            //List<BookingModel> 
            List<object> res = JsonConvert.DeserializeObject<List<object>>(json);
            return res;
        }

        public List<object> GetBookingHistory(string BookingID)
        {
            WebClient oWeb = new WebClient();
            string url = YtdmUrl + "pro_booking/proc/booking_manager.json.php";
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
            parameters.Add("action", "GetTrackingHistory");
            parameters.Add("booking_id", BookingID);
            var responseBytes = oWeb.UploadValues(url, parameters);
            string json = Encoding.ASCII.GetString(responseBytes);
            //List<BookingModel> 
            List<object> res = JsonConvert.DeserializeObject<List<object>>(json);
            return res;
        }


    }
}
