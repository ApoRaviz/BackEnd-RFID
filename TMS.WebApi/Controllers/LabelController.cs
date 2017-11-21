using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TMS.Service.Label;
using TMS.Service.Impl.Label;
using WIM.Core.Common.Http;
using TMS.Common.ValueObject.Labels;
using WIM.Core.Common.Extensions;
using System.Collections.Generic;
using System;

namespace TMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/external/label")]
    public class LabelController : ApiController
    {
        //private ILabelService LabelService;
        //public LabelController(ILabelService LabelService)
        //{
        //    this.LabelService = LabelService;
        //}

        [HttpGet]
        [Route("groupdateimportbooking")]
        public HttpResponseMessage GetImportBookingByDate()
        {
            //string[] BookingIDs = BookingIDs;
            ILabelService LabelService = new LabelService();
            IResponseData<List<GroupDateImportBookingModel>> response = new ResponseData<List<GroupDateImportBookingModel>>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
            response.SetStatus(HttpStatusCode.OK);
            response.SetData(LabelService.GetGroupImportBooking());
            return Request.ReturnHttpResponseMessage(response);


        }


        [HttpGet]
        [Route("importbooking/{date}/{user_id}")]
        public HttpResponseMessage GetImportBookingByDate(DateTime date ,string user_id)
        {
            //string[] BookingIDs = BookingIDs;
            ILabelService LabelService = new LabelService();
            IResponseData<List<BoxLabelBookingModel>> response = new ResponseData<List<BoxLabelBookingModel>>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
            response.SetStatus(HttpStatusCode.OK);
            GroupDateImportBookingModel param = new GroupDateImportBookingModel();
            param.DateImport = date;
            param.UserID = user_id;
            response.SetData(LabelService.GetDataImportBookingByDate(param));
            return Request.ReturnHttpResponseMessage(response);


        }

        [HttpGet]
        [Route("lablebooking")]
        public HttpResponseMessage GetLabelBoking([FromUri]BookingIDArrModel BookingIDs)
        {
            //string[] BookingIDs = BookingIDs;
            ILabelService LabelService = new LabelService();
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            result.StatusCode = HttpStatusCode.OK;
            result.Content = LabelService.GetBoxLabelBookingStream(BookingIDs.BookingID);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
            //return Request.ReturnHttpResponseMessage(response);


        }


        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("print/lablebooking")]
        public HttpResponseMessage GetPrintLabelBoking([FromUri]BookingIDArrModel BookingIDs)
        {
            //string[] BookingIDs = BookingIDs;
            ILabelService LabelService = new LabelService();
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
            
            result.StatusCode = HttpStatusCode.OK;
            result.Content = LabelService.GetBoxLabelBookingStream(BookingIDs.BookingID);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
            //return Request.ReturnHttpResponseMessage(response);


        }
    }
    
    
}
