using System.Collections.Generic;
using System.Net.Http;
using TMS.Common.ValueObject.Labels;

namespace TMS.Service.Label
{
    public interface ILabelService
    {        
        StreamContent GetBoxLabelBookingStream(string[] BookingID);
        List<GroupDateImportBookingModel> GetGroupImportBooking();
        List<BoxLabelBookingModel> GetDataImportBookingByDate(GroupDateImportBookingModel param);

    }
}
