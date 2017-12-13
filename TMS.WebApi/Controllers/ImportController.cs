using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections.Generic;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.Helpers;

namespace TMS.WebApi.Controllers
{

    [RoutePrefix("api/v1/Import")]
    public class ImportController : ApiController
    {
        string YtdmUrl;
        public ImportController()
        {
            YtdmUrl = ConfigurationManager.AppSettings["as:YtdmUrl"];
        }

        [HttpPost]
        [Route("Booking")]
        public async Task<HttpResponseMessage> ImportBooking()
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                HttpRequestMessage request = Request;
                if (!request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("~/App_Data/uploads");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                var provider = new MultipartFormDataStreamProvider(root);
                await Request.Content.ReadAsMultipartAsync(provider);
                var rawMessage = await Request.Content.ReadAsStringAsync();
                WebClient oWeb = new WebClient();
                foreach (var file in provider.FileData)
                {
                    var filename = DateTime.Now.Ticks + "_" + file.Headers.ContentDisposition.FileName.Trim('\"');
                    string filePath = root + "\\" + filename;
                    File.Copy(file.LocalFileName, filePath);
                    File.Delete(file.LocalFileName);
                    string url = YtdmUrl+"pro_booking/proc/booking_manager.php";
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
                    parameters.Add("action", "importBookingExternal");
                    parameters.Add("dllTransportMode", provider.FormData.Get("transportMode"));
                    oWeb.QueryString = parameters;
                    var responseBytes = oWeb.UploadFile(url, "POST", filePath);
                    string res = Encoding.ASCII.GetString(responseBytes);
                    dynamic json = JObject.Parse(res);
                    if(json.Status == 200)
                    {
                        response.SetData(res);
                    }
                    else
                    {
                        List<ValidationError> ex = new List<ValidationError>();
                        ex.Add(new ValidationError(ErrorCode.E500.ToString(), (string)json.Message));
                        response.SetErrors(ex);
                        response.SetStatus(HttpStatusCode.InternalServerError);
                    }
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



 

    }
}
