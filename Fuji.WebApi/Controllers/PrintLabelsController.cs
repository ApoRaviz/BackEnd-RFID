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
                baseRunning = PrintLabelService.GetRunningByType(type, running);
                response.SetData(baseRunning);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension;
            List<DataBarcode> barcodeList = new List<DataBarcode>();
            Barcode bc = new Barcode();
            type = "BXFJ";

            for (int i = 0; i < running; i++)
            {
                string barcodeInfoImage = type
                    + DateTime.Now.ToString("yyMMdd", new System.Globalization.CultureInfo("en-US"))
                    + (baseRunning + i).ToString("0000");
                string barcodeInfo = type
                    + "\n"
                    + DateTime.Now.ToString("yyMMdd", new System.Globalization.CultureInfo("en-US"))
                    + (baseRunning + i).ToString("0000");

                byte[] barcodeImage = bc.EncodeToByte(TYPE.CODE128, barcodeInfoImage, Color.Black, Color.White, 287, 96);

                DataBarcode barcode = new DataBarcode(barcodeImage, barcodeInfo);
                barcodeList.Add(barcode);
            }

            using (var reportViewer = new ReportViewer())
            {
                //reportViewer.Width = 384;
                //reportViewer.Height = 384;
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/BoxLabelReport.rdlc";
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = barcodeList;
                reportViewer.LocalReport.DataSources.Add(rds1);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }
            result.StatusCode = HttpStatusCode.OK;
            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
            //return Request.ReturnHttpResponseMessage(response);


        }

        public class DataBarcode
        {

            public byte[] Barcode { get; private set; }
            public string BarcodeInfo { get; private set; }
            public DataBarcode(byte[] barcode, string barcodeInfo)
            {
                this.Barcode = barcode;
                this.BarcodeInfo = barcodeInfo;
            }
        }

    }
}
