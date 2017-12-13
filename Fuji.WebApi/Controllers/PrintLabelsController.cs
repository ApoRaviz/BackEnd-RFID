using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BarcodeLib;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Printing;
using Fuji.Service.PrintLabel;
using Fuji.Common.ValueObject;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

namespace Fuji.WebApi.Controllers.ExternalInterface
{
    [RoutePrefix("api/v1/external/printLabel")]
    public class PrintLabelsController : ApiController
    {
        private IPrintLabelService PrintLabelService;
        public PrintLabelsController(IPrintLabelService printLabelService)
        {
            this.PrintLabelService = printLabelService;
        }

        [Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Get([FromBody]ParameterBoxLabel item)
        {
            int baseRunning = 0;
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage();


            try
            {
                baseRunning = PrintLabelService.GetRunningByType(item.Type, item.Running);
                result.StatusCode = HttpStatusCode.OK;
                result.Content = PrintLabelService.GetReportStream(item.Running, baseRunning);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            catch (ValidationException ex)
            {
                result = Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }

            return result;
            //return Request.ReturnHttpResponseMessage(response);


        }
 

    }
}
