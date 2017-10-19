using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Extensions;
using WMS.WebApi.Report;
using WMS.Master.Import;
using System.IO;
using WMS.Master;
using System.Globalization;
using Microsoft.AspNet.Identity;
using WMS.Common;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/upload")]
    public class UploadFileApiController : ApiController
    {
        private IImportService ImportService;

        public UploadFileApiController(IImportService importService)
        {
            this.ImportService = importService;
        }

        [HttpPost]
        [Route("UploadForSample")]
        public HttpResponseMessage UploadForSample()
        {
            ResponseData<ImportDefinition> response = new ResponseData<ImportDefinition>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    string path = "";
                    string fileName = "";

                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        path = filePath;
                        fileName = postedFile.FileName;
                    }

                    if (httpRequest.Form.Count > 0)
                    {
                        string delimet = httpRequest.Form[0];
                        string encode = httpRequest.Form[1];
                        DataTable dt;
                        Encoding encoding = Encoding.Default;
                        string delimiter = ";";

                        switch (delimet)
                        {
                            case "Tab": delimiter = "\t"; break;
                            case "Semicolon (;)": delimiter = ";"; break;
                            case "Colon (:)": delimiter = ":"; break;
                            case "Comma (,)": delimiter = ","; break;
                        }

                        switch (encode)
                        {
                            case "Unicode": encoding = Encoding.Unicode; break;
                            case "Unicode big endian": encoding = Encoding.BigEndianUnicode; break;
                            case "UTF-8": encoding = Encoding.UTF8; break;
                            default: encoding = Encoding.Default; break;
                        }

                        if(delimet == "undefined")
                        {
                            FileInfo f = new FileInfo(fileName);
                            if(f.Extension == ".xlsx")
                            {
                                delimet = "Excel";
                            }
                            else
                            {
                                using (StreamReader sr = new StreamReader(path, encoding))
                                {
                                    string data = sr.ReadLine();
                                    if (data.IndexOf("\t") > -1)
                                        delimiter = "\t";
                                    else if (data.IndexOf(",") > -1)
                                        delimiter = ",";
                                }
                                    
                            }
                        }

                        if (delimet == "Excel")
                        {
                            dt = ReportUtils.GetDataTableFromExcelWithFHeader(path);
                        }
                        else
                        {
                            dt = ReportUtils.GetDataTableFromFileWithFHeader(path, encoding, delimiter);
                        }

                        ImportDefinition imp = new ImportDefinition();
                        imp.data = dt;                  
                        imp.header = dt.Columns.Cast<DataColumn>().Select(x => new tbColumn { name = x.ColumnName.ToUpper() }).ToList();
                        imp.headerSelect = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToUpper()).ToArray();

                        File.Delete(path);

                        response.SetData(imp);
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

        [HttpPost]
        [Route("UploadForImport")]
        public HttpResponseMessage UploadForImport()
        {
            ResponseData<string> response = new ResponseData<string>();
            string result = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    string path = "";
                    string fileName = "";

                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        path = filePath;
                        fileName = postedFile.FileName;
                    }

                    if (httpRequest.Form.Count > 0)
                    {
                        FileInfo f = new FileInfo(fileName);
                        string FormatName = httpRequest.Form[0];

                        ImportDefinitionHeader_MT data = this.ImportService.GetImportDefinitionByImportIDSys(int.Parse(FormatName), "ImportDefinitionDetail_MT");
         
                        if(data != null)
                        {
                            bool success = false;

                            if (data.Delimiter == "Excel" && f.Extension != ".xlsx")
                                result = "File Import incorrect format";
                            else
                            {
                                DataTable dt;
                                Encoding encoding = Encoding.Default;
                                string delimiter = ";";

                                switch (data.Delimiter)
                                {
                                    case "Tab": delimiter = "\t"; break;
                                    case "Semicolon (;)": delimiter = ";"; break;
                                    case "Colon (:)": delimiter = ":"; break;
                                    case "Comma (,)": delimiter = ","; break;
                                }

                                switch (data.Encoding)
                                {
                                    case "Unicode": encoding = Encoding.Unicode; break;
                                    case "Unicode big endian": encoding = Encoding.BigEndianUnicode; break;
                                    case "UTF-8": encoding = Encoding.UTF8; break;
                                    default: encoding = Encoding.Default; break;
                                }

                                if (data.Delimiter == "Excel")
                                {
                                    dt = ReportUtils.GetDataTableFromExcelWithFHeader(path);
                                }
                                else
                                {
                                    dt = ReportUtils.GetDataTableFromFileWithFHeader(path, encoding, delimiter);
                                }

                                if (data.SkipFirstRecode != null && data.SkipFirstRecode.Value)
                                    dt.Rows.RemoveAt(0);

                                result = CheckImportData(dt, data, data.ImportDefinitionDetail_MT.ToList());

                                if (string.IsNullOrEmpty(result))
                                {
                                    string xml = PrepareData(dt, data.ImportDefinitionDetail_MT.ToList());
                                    result = this.ImportService.ImportDataToTable(int.Parse(FormatName), xml);

                                    if (string.IsNullOrEmpty(result))
                                    {
                                        result = "Import data complete!";
                                        success = true;
                                    }
                                }
                            }

                            this.ImportService.InsertImportHistory(int.Parse(FormatName), fileName, result, success, User.Identity.GetUserName());
                        }

                        response.SetData(result);
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

        public string CheckImportData(DataTable dt, ImportDefinitionHeader_MT header, List<ImportDefinitionDetail_MT> detail)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            
            if (dt.Columns.Count < header.MaxHeading)
            {
                //Check column refer (F...) <= column in file import
                sb.AppendLine(String.Format("File have column less than max heading (Max. Heading = {0})", header.MaxHeading));
            }
            else
            {
                int row = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (ImportDefinitionDetail_MT d in detail)
                    {
                        if (d.Mandatory != "0" && !string.IsNullOrEmpty(d.Import) && (dr[int.Parse(d.Import.Replace("F", "")) - 1] == null || dr[int.Parse(d.Import.Replace("F", "")) - 1].ToString() == ""))
                        {
                            sb.AppendLine(String.Format("File at row {0} and column {1} must have data", row, int.Parse(d.Import.Replace("F", ""))));
                        }
                        else
                        {
                            if (d.DataType == "DateTime" && d.Mandatory != "0" && d.Mandatory != "1")
                            {
                                DateTime dateTime;

                                if (!DateTime.TryParseExact(dr[int.Parse(d.Import.Replace("F", "")) - 1].ToString(), d.Mandatory, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                {
                                    sb.AppendLine(String.Format("File at row {0} and column {1} have date format incorrect (Format {2})", row, int.Parse(d.Import.Replace("F", "")), d.Mandatory));
                                }
                            }
                            if (!string.IsNullOrEmpty(d.Import))
                            {
                                int digits = d.Digits;

                                string value = dr[int.Parse(d.Import.Replace("F", "")) - 1].ToString();

                                if (value.Length > digits)
                                    sb.AppendLine(String.Format("File at row {0} and column {1} have data length is over", row, int.Parse(d.Import.Replace("F", ""))));
                            }
                        }
                    }

                    row += 1;
                }
            }

            string result = sb.ToString();

            return result;
        }

        public string PrepareData(DataTable dt, List<ImportDefinitionDetail_MT> detail)
        {
            string pXmlDetail = "<{0}>{1}</{0}>";
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<row>");

                foreach (ImportDefinitionDetail_MT d in detail)
                {
                    string value = "";

                    if (!string.IsNullOrEmpty(d.FixedValue))
                        value = d.FixedValue;
                    else
                        value = dr[int.Parse(d.Import.Replace("F", "")) - 1].ToString();

                    sb.AppendFormat(pXmlDetail, d.ColumnName, value);
                }

                sb.Append("</row>");
            }

            return sb.ToString();
        }
    }

    public class ImportDefinition
    {
        public List<tbColumn> header;
        public string[] headerSelect;
        public DataTable data;
    }

    public class tbColumn
    {
        public string name { get; set; }
    }
}
