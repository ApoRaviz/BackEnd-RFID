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
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Extensions;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Service;
using WMS.Entity.Report;
using WMS.Service.Report;

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
        [Route("GetHeader/{ForTable}")]
        public HttpResponseMessage Get(string forTable)
        {
            ResponseData<IEnumerable<ReportLayoutHeader_MT>> response = new ResponseData<IEnumerable<ReportLayoutHeader_MT>>();
            try
            {
                IEnumerable<ReportLayoutHeader_MT> header = ReportService.GetAllReportHeader(forTable);
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
            IResponseData<ReportLayoutHeader_MT> response = new ResponseData<ReportLayoutHeader_MT>();
            try
            {
                ReportLayoutHeader_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys, "ReportLayoutDetail_MT");
                report.ReportLayoutDetail_MT = report.ReportLayoutDetail_MT.OrderBy(x => x.ColumnOrder).ToList();
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
        public HttpResponseMessage Post([FromBody]ReportLayoutHeader_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UpdateBy = User.Identity.Name;               
                int id = ReportService.CreateReportForItemMaster(data).Value;
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
        public HttpResponseMessage Put(int ReportIDSys, [FromBody]ReportLayoutHeader_MT data)
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
            ReportLayoutHeader_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys, "ReportLayoutDetail_MT");
            DataTable dt = this.ReportService.GetReportData(ReportIDSys);

            ReportUtils.GetExportReport("ItemMasterReport", report, dt);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

        [HttpGet]
        [Route("GetSupplierReport/{ReportIDSys}")]
        public HttpResponseMessage GetSupplierReport(int ReportIDSys)
        {
            ReportLayoutHeader_MT report = ReportService.GetReportLayoutByReportIDSys(ReportIDSys, "ReportLayoutDetail_MT");
            DataTable dt = this.ReportService.GetReportData(ReportIDSys);

            ReportUtils.GetExportReport("SupplierMasterReport", report, dt);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }
    }
}
