using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using BarcodeLib;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Printing;
using Fuji.Service.PrintLabel;

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

        //[Authorize]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("{type}/{running}")]
        public HttpResponseMessage Get(string type, int running)
        {
            int baseRunning = 0;
            IResponseData<int> response = new ResponseData<int>();
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);


            try
            {
                baseRunning = PrintLabelService.GetRunningByType(type, running,"System");
                response.SetData(baseRunning);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

           
            result.StatusCode = HttpStatusCode.OK; 
            result.Content = PrintLabelService.GetReportStream(running,baseRunning);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
            //return Request.ReturnHttpResponseMessage(response);


        }
 

    }
}
