using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Helpers;
using WMS.WebApi.Report;
using System.IO;
using System.Data;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using WMS.Service;
using WMS.Entity.Report;
using WMS.Service.Report;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/report")]
    public class ReportController : ApiController
    {
        private IReportService ReportService;

        public ReportController(IReportService reportService)
        {
            this.ReportService = reportService;
        }

        [HttpGet]
        [Route("GetHeader/{forTable}")]
        public HttpResponseMessage Get(string forTable)
        {
            ResponseData<IEnumerable<ReportLayout_MT>> response = new ResponseData<IEnumerable<ReportLayout_MT>>();
            try
            {
                IEnumerable<ReportLayout_MT> header;
                if (User.IsSysAdmin())
                {
                    header = ReportService.GetAllReportHeader(0);
                }
                else
                {
                    header = ReportService.GetAllReportHeader(User.Identity.GetProjectIDSys());
                }

                header = header.Select(a => new ReportLayout_MT()
                {
                    ReportIDSys = a.ReportIDSys,
                    FormatName = a.FormatName,
                    ReportDetail = new ReportDetail()
                    {
                        FormatType = a.ReportDetail.FormatType,
                        AddHeaderLayout = a.ReportDetail.AddHeaderLayout,
                        Delimiter = a.ReportDetail.Delimiter,
                        Encoding = a.ReportDetail.Encoding,
                        FileExtention = a.ReportDetail.FileExtention,
                        TextGualifier = a.ReportDetail.TextGualifier,
                        StartExportRow = a.ReportDetail.StartExportRow,
                        IncludeHeader = a.ReportDetail.IncludeHeader
                        
                    }
                });

                response.SetData(header);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/label/1
        [HttpGet]
        [Route("{ReportIDSys}")]
        public HttpResponseMessage Get(int ReportIDSys)
        {
            IResponseData<ReportLayout_MT> response = new ResponseData<ReportLayout_MT>();
            try
            {
                ReportLayout_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys);
                response.SetData(report);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ReportLayout_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UpdateBy = User.Identity.Name;               
                int id = ReportService.CreateReportForItemMaster(data);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("{ReportIDSys}")]
        public HttpResponseMessage Put(int ReportIDSys, [FromBody]ReportLayout_MT data)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = ReportService.UpdateReportForItemMaster(ReportIDSys, data);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetItemReport/{ReportIDSys}")]
        public HttpResponseMessage GetItemReport(int ReportIDSys)
        {
            ReportLayout_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys);
            DataTable dt = this.ReportService.GetReportData(ReportIDSys);

            ReportUtils.GetExportReport("ItemMasterReport", report, dt);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

        [HttpGet]
        [Route("GetSupplierReport/{ReportIDSys}")]
        public HttpResponseMessage GetSupplierReport(int ReportIDSys)
        {
            ReportLayout_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys);
            DataTable dt = this.ReportService.GetReportData(ReportIDSys);

            ReportUtils.GetExportReport("SupplierMasterReport", report, dt);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }
    }
}
