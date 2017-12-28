using AutoMapper;
using AutoMapper.QueryableExtensions;
using HashidsNet;
using Master.WebApi.Report;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml.Linq;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        public DemoController()
        {

        }

        [HttpPost]
        [Route("func1")]
        public HttpResponseMessage Func1([FromBody]List<LabelConfig> projectConfig)
        {
            LabelControl labelResponse = new LabelControl();
            ResponseData<LabelControl> response = new ResponseData<LabelControl>();
          

            /*var res = projectConfig.ToDictionary(x => x.Key, x => x.Value);
            response.SetData(res);
            return Request.ReturnHttpResponseMessage(response);*/

            using (CoreDbContext db = new CoreDbContext())
            {
                LabelControl label1 = db.LabelControl.SingleOrDefault(p => p.LabelIDSys == 1);

              

                /*using (StreamWriter sw = new StreamWriter(@"d:\Web\ftproot\lang\en.json"))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, projectConfig);

                    }
                }*/



                label1.LabelConfig = projectConfig;
                db.SaveChanges();

                var labels = (
                      from p in db.LabelControl
                      select p
                ).ToList();

                try
                {
                    labelResponse = labels.FirstOrDefault(p => p.LabelConfig.FirstOrDefault().Key == "EmID");
                    response.SetData(labelResponse);
                }
                catch (NullReferenceException)
                {

                }
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("func2")]
        public HttpResponseMessage Func2([FromBody]List<string> headReportConfig)
        {
            HeadReportControlDto headerReportResponse = new HeadReportControlDto();
            ResponseData<HeadReportControlDto> response = new ResponseData<HeadReportControlDto>();

            using (CoreDbContext db = new CoreDbContext())
            {
                HeadReportControl headReport1 = db.HeadReportControl.SingleOrDefault(p => p.HeadReportIDSys == 1);

                headReport1.HeadReportConfig = headReportConfig;
                db.SaveChanges();

                var _headReport = (
                      from p in db.HeadReportControl
                      where p.ReportName == "Report 1"
                      select p
                ).FirstOrDefault();

                var headReport = Mapper.Map<HeadReportControl, HeadReportControlDto>(_headReport);

                LabelControl labelControl = db.LabelControl.SingleOrDefault(i => i.ProjectIDSys == 1003 && i.Lang == "th");

                foreach (var hrl in headReport.HeadReportLabels)
                {
                    foreach (var labelConfig in labelControl.LabelConfig)
                    {
                        if (HashidsHelper.DecodeHex(hrl.Key) == labelConfig.Key)
                        {
                            hrl.Value = labelConfig.Value;
                        }
                    }
                }

                headerReportResponse = headReport;
                response.SetData(headerReportResponse);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("func3")]
        public HttpResponseMessage Func3([FromBody]List<string> Labels)
        {
            DataTable responseObj = new DataTable();
            List<string> newLabels = new List<string>();
            ResponseData<List<string>> response = new ResponseData<List<string>>();

            List<string> _label = new List<string>();
            foreach (var item in Labels)
            {
                string l = HashidsHelper.DecodeHex(item);
                _label.Add(l);
            }

            string labelSelect = String.Join(", ", _label.ToArray());

            System.Data.DataSet dataSet = new System.Data.DataSet();

            using (CoreDbContext db = new CoreDbContext())
            {
                string sql = string.Format("select {0} from Employee_MT FOR JSON AUTO", labelSelect);                
                string json = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                json = string.Format("{0} \"Employees\": {1} {2}", "{", json, "}");    

                dataSet = JsonConvert.DeserializeObject<System.Data.DataSet>(json);
                DataTable dataTable = dataSet.Tables["Employees"];


                LabelControl labelControl = db.LabelControl.SingleOrDefault(i => i.ProjectIDSys == 1003 && i.Lang == "th");
                var _headReport = (
                      from p in db.HeadReportControl
                      where p.ReportName == "Report 1"
                      select p
                ).FirstOrDefault();

                var headReport = Mapper.Map<HeadReportControl, HeadReportControlDto>(_headReport);

                //foreach (var hrl in headReport.HeadReportLabels)
                //{
                //    foreach (var labelConfig in labelControl.LabelConfig)
                //    {
                //        if (HashidsHelper.DecodeHex(hrl.Key) == labelConfig.Key)
                //        {
                //            hrl.Value = labelConfig.Value;
                //        }
                //    }
                //}

                //newLabels = headReport.HeadReportLabels.Select(h => h.Value).ToList();
                newLabels = (from p in headReport.HeadReportLabels
                            from r in labelControl.LabelConfig
                            where r.Key == HashidsHelper.DecodeHex(p.Key)
                            select r.Value).ToList();


                //newLabels = headReport.HeadReportLabels.Select(h => h.Value).ToList();
                if (newLabels.Count == dataTable.Columns.Count)
                {
                    for (int i = 0; i < newLabels.Count; i++)
                    {
                        dataTable.Columns[i].ColumnName = newLabels[i];
                    }
                    dataTable.AcceptChanges();
                }

            }


            return ReportUtil.createRDLCReport("Test", "Dynamic", dataSet);

            //response.SetData(response2);

            //return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("func4")]
        public HttpResponseMessage Func4()
        {
            byte[] bytes;
            string[] streamids;
            Warning[] warnings;
            string mimeType, encoding, extension;



            List<Customer_MT> labelControlList = new List<Customer_MT>();

            using (CoreDbContext db = new CoreDbContext())
            {
                labelControlList = db.Customer_MT.ToList();
            }


            //Data for binding to the Report
            DataTable table1 = new DataTable("DataSet1");
            table1.Columns.Add("CusName");

            table1.Rows.Add("Nadir");
            table1.Rows.Add("Lijo");
            table1.Rows.Add("Shelley");

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(table1);



            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/Report1.rdlc";

                //reportViewer.LocalReport.ReportEmbeddedResource = "Report/Report1.rdlc";                
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables["DataSet1"]));
                reportViewer.LocalReport.Refresh();

                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            MemoryStream stream = new MemoryStream(bytes);
            result.StatusCode = HttpStatusCode.OK;
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;

        }
    }
}