﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HashidsNet;
using Master.WebApi.Report;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Xml.Linq;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;
using System.Collections.ObjectModel;
using System.Web.Http.Description;
using System.Web.Http.Controllers;
using WIM.Core.Entity.ProjectManagement.ProjectConfigs;
using WIM.Core.Entity.ProjectManagement;

namespace Master.WebApi.Controllers
{
    public class ProjectGroup
    {
        public Project_MT Project_MT { get; set; }
        public ProjectConfig ProjectConfig { get; set; }
    }

    public class EntityAndConfig<TEntity, TConfig>
    {
        public TEntity Entity { get; set; }
        public TConfig Config { get; set; }

    }

    public class GeneralConfigTest<TEntity>
    {
        public string Key { get; set; }
        public string Value { get; private set; }

        [NotMapped]
        public TEntity Config
        {
            get
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    return JsonConvert.DeserializeObject<TEntity>(StringHelper.Decompress(Value));
                }
                return default(TEntity);
            }
            set
            {
                Value = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }
    }

    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        public DemoController()
        {

        }
        public class From<A, B>
        {
            public A a { get; set; }
            public B b { get; set; }
        }

        public class A
        {
            public string test1 { get; set; }
        }

        public class B
        {
            public string test2 { get; set; }
        }

        [HttpPost]
        [Route("birdfunc3")]
        public HttpResponseMessage birdFunc3([FromBody]From<A, B> a)
        {
            ResponseData<string> response = new ResponseData<string>();
            try
            {
                object xa = a;
                //B xb = b;
                response.SetData("");
            }
            catch (NullReferenceException)
            {
                response.SetData("");
            }

            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpPost]
        [Route("func1")]
        public HttpResponseMessage Func1([FromBody]EntityAndConfig<Project_MT, List<LabelConfig>> ec)
        {
            LabelControl labelResponse = new LabelControl();
            ResponseData<List<LabelConfig>> response = new ResponseData<List<LabelConfig>>();

            GeneralConfigTest<List<LabelConfig>> gc = new GeneralConfigTest<List<LabelConfig>>();

            gc.Key = "LabelConfig";
            gc.Config = ec.Config;





            response.SetData(gc.Config);
            return Request.ReturnHttpResponseMessage(response);

            /* using (CoreDbContext db = new CoreDbContext())
             {
                 LabelControl label1 = db.LabelControl.SingleOrDefault(p => p.LabelIDSys == 1);



                 using (StreamWriter sw = new StreamWriter(@"d:\Web\ftproot\lang\en.json"))
                 {
                     using (JsonWriter writer = new JsonTextWriter(sw))
                     {
                         serializer.Serialize(writer, labelConfig);

                     }
                 }



                 label1.LabelConfig = labelConfig;
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
             return Request.ReturnHttpResponseMessage(response);*/
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
                        //if (HashidsHelper.DecodeHex(hrl.Key) == labelConfig.Key)
                        //{
                        //    hrl.Value = labelConfig.Value;
                        //}
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
                string sql = string.Format("select TOP(10) {0} from Employee_MT FOR JSON AUTO", labelSelect);
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
                //newLabels = (from p in headReport.HeadReportLabels
                //             from r in labelControl.LabelConfig
                //             where r.Key == HashidsHelper.DecodeHex(p.Key)
                //             select r.Value).ToList();


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

            return ReportUtil.CreateRDLCReportByEXCEL("Test", "Dynamic", dataSet);

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

        [HttpGet]
        [Route("func5")]
        public HttpResponseMessage Func5()
        {
            ResponseData<Hashtable> response = new ResponseData<Hashtable>();
            List<Type> tEntities = new List<Type>
            {
                typeof(Customer_MT),
                typeof(LabelControl)
            };



            response.Data = ApiHashTableHelper.apiTable;
            //response.Data = DemoHelper.GetAttributeEntities(tEntities);
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("func6")]
        public HttpResponseMessage Func6()
        {
            ResponseData<List<string>> response = new ResponseData<List<string>>();
            List<Type> tEntities = new List<Type>
            {
                typeof(Customer_MT),
                typeof(LabelControl)
            };

            response.Data = DemoHelper.GetAttributeEntities(tEntities);
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("func7")]
        public HttpResponseMessage Func7()
        {
            ResponseData<List<ApiDesc>> response = new ResponseData<List<ApiDesc>>();

            Collection<ApiDescription> apis = Configuration.Services.GetApiExplorer().ApiDescriptions;

            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = apis.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            List<ApiDesc> apiDescList = new List<ApiDesc>();
            foreach (IGrouping<HttpControllerDescriptor, ApiDescription> group in apiGroups)
            {
                foreach (ApiDescription api in group)
                {
                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    foreach (ApiParameterDescription parameter in api.ParameterDescriptions)
                    {
                        if (parameter.ParameterDescriptor == null)
                        {
                            continue;
                        }

                        Type type = parameter.ParameterDescriptor.ParameterType;
                        if (type == typeof(Int32) || type == typeof(Int64) || type == typeof(Boolean))
                        {
                            paramDic.Add(parameter.Name, "1");
                        }
                        else if (type == typeof(String) || type == typeof(DateTime))
                        {
                            paramDic.Add(parameter.Name, "@");
                        }
                    }

                    string path = "";
                    string[] pathSplit = api.RelativePath.Split('?')[0].Split('/');
                    for (int i = 0; i < pathSplit.Length; i++)
                    {
                        if (i > 2 && pathSplit[i][0] == '{' && pathSplit[i][pathSplit[i].Length - 1] == '}')
                        {
                            foreach (KeyValuePair<string, string> paramKeyVal in paramDic)
                            {
                                string x = pathSplit[i].Substring(1, pathSplit[i].Length - 2);

                                if (x.Equals(paramKeyVal.Key, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    path += "/" + paramKeyVal.Value;
                                    continue;
                                }
                            }
                            continue;
                        }
                        path += "/" + pathSplit[i];
                    }

                    apiDescList.Add(new ApiDesc
                    {
                        ID = api.ID,
                        ControllerName = group.Key.ControllerName,
                        RelativePath = api.RelativePath,
                        ApiPath = path,
                        Method = api.HttpMethod.Method
                    });
                }
            }

            response.Data = apiDescList;
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("func8")]
        public HttpResponseMessage Func8([FromBody]object x)
        {
            ResponseData<WarehouseTest> response = new ResponseData<WarehouseTest>();

            WarehouseTest w;
            using (CoreDbContext db = new CoreDbContext())
            {
                w = db.WarehouseTests.First();
                w.CreateBy = "Job";
            }

            response.Data = w;
            return Request.ReturnHttpResponseMessage(response);
        }

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class AddressAttribute : Attribute
    {
        public AddressAttribute()
        {

        }
    }



  








    public class ApiDesc
    {
        public string ID { get; set; }
        public string ControllerName { get; set; }
        public string RelativePath { get; set; }
        public string ApiPath { get; set; }
        public string Method { get; set; }

    }

    public class DemoHelper
    {
        public static List<string> GetAttributeEntities(List<Type> tEntities)
        {
            List<string> columns = new List<string>();
            foreach (Type tEntity in tEntities)
            {
                TableAttribute tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(tEntity, typeof(TableAttribute));
                columns.Add("TableName ==== " + tableAttribute.Name + "====");

                PropertyInfo[] properties = tEntity.GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    {
                        columns.Add("PrimaryKey ==== " + prop.Name + "====");
                    }
                    else if (prop.GetCustomAttribute<ForeignKeyCustomAttribute>() != null)
                    {
                        ForeignKeyCustomAttribute fkAttribute = (ForeignKeyCustomAttribute)Attribute.GetCustomAttribute(prop, typeof(ForeignKeyCustomAttribute));
                        columns.Add("ForeignKey ==== " + fkAttribute.TableName + "." + prop.Name + "====");
                    }
                    else
                    {
                        columns.Add(prop.Name);
                    }
                }
            }
            return columns;
        }
    }
}

