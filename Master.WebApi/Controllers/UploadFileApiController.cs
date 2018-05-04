using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.importManagement;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/upload")]
    public class UploadFileApiController : ApiController
    {
        private IImportDataService ImportService;
        private const string DEFAULT_UPLOAD_PATH = "D:/Uploads/WIM/";
        public UploadFileApiController(IImportDataService importService)
        {
            this.ImportService = importService;
        }

        [HttpPost]
        [Route("UploadForSample")]
        public HttpResponseMessage UploadForSample(UploadSample uploadItem)
        {
            ResponseData<ImportDefinition> response = new ResponseData<ImportDefinition>();
            try
            {
                string path = "";
                string fileName = "";

                if (uploadItem != null)
                {
                    path = DEFAULT_UPLOAD_PATH + uploadItem.FilePath;
                    FileInfo f = new FileInfo(path);
                    fileName = f.Name;
                    string delimet = uploadItem.Delimiter;
                    string encode = uploadItem.Encoding;
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
                        FileInfo fi = new FileInfo(fileName);
                        if(fi.Extension == ".xlsx")
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
                        dt = GetDataTableFromExcelWithFHeader(path);
                    }
                    else
                    {
                        dt = GetDataTableFromFileWithFHeader(path, encoding, delimiter,"\"");
                    }

                    ImportDefinition imp = new ImportDefinition();
                    imp.data = dt;                  
                    imp.header = dt.Columns.Cast<DataColumn>().Select(x => new tbColumn { name = x.ColumnName.ToUpper() }).ToList();
                    imp.headerSelect = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToUpper()).ToArray();

                    File.Delete(path);

                    response.SetData(imp);
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
        public HttpResponseMessage UploadForImport(UploadItem uploadItem)
        {
            ResponseData<string> response = new ResponseData<string>();
            string result = string.Empty;
            try
            {
                //var httpRequest = HttpContext.Current.Request;
                //if (httpRequest.Files.Count > 0)
                //{
                    string path = "";
                    string fileName = "";

                    //foreach (string file in httpRequest.Files)
                    //{
                    //    var postedFile = httpRequest.Files[file];
                    //    var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + postedFile.FileName);
                    //    postedFile.SaveAs(filePath);
                    //    path = filePath;
                    //    fileName = postedFile.FileName;
                    //}

                    if (uploadItem != null)
                    {
                        path = DEFAULT_UPLOAD_PATH + uploadItem.FilePath;
                        FileInfo f = new FileInfo(path);
                        fileName = f.Name;
                        string FormatName = uploadItem.FormatName;

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
                                    dt = GetDataTableFromExcelWithFHeader(path);
                                }
                                else
                                {
                                    dt = GetDataTableFromFileWithFHeader(path, encoding, delimiter);
                                }

                                if (data.SkipFirstRecode != null && data.SkipFirstRecode.Value)
                                    dt.Rows.RemoveAt(0);

                                result = CheckImportData(dt, data, data.ImportDefinitionDetail_MT.ToList());

                                if (string.IsNullOrEmpty(result))
                                {
                                    string xml = PrepareData(dt, data.ImportDefinitionDetail_MT.ToList());
                                    result = this.ImportService.ImportDataToTable(int.Parse(FormatName), xml, User.Identity.GetUserName());

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
                //}
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

                    sb.AppendFormat(pXmlDetail, d.ColumnName, value.Replace("\"",string.Empty));
                }

                sb.Append("</row>");
            }

            return sb.ToString();
        }

        public static DataTable GetDataTableFromExcelWithFHeader(string path)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(string.Format("f{0}", firstRowCell.Start.Column));
                }
                for (int rowNum = 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text.Replace("\"", string.Empty);
                    }
                }
                return tbl;
            }
        }

        public static DataTable GetDataTableFromFileWithFHeader(string path, Encoding encoding = null, string delimiter = ",", string gualifier = "")
        {
            DataTable dt = new DataTable();

            if (encoding == null)
                encoding = Encoding.Default;
            using (StreamReader sr = new StreamReader(path, encoding))
            {
                string[] firstLine = sr.ReadLine().Split(delimiter.ToCharArray());
                int colIdx = 1;
                foreach (string text in firstLine)
                {
                    dt.Columns.Add(string.Format("f{0}", colIdx++));
                }

                DataRow drH = dt.NewRow();
                for (int i = 0; i < firstLine.Length; i++)
                {
                    if (!string.IsNullOrEmpty(gualifier))
                        drH[i] = firstLine[i].Replace(gualifier, "");
                    else
                        drH[i] = firstLine[i];
                }
                dt.Rows.Add(drH);

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(delimiter.ToCharArray());
                    if (rows.Length >= firstLine.Length)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < firstLine.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(gualifier))
                                dr[i] = rows[i].Replace(gualifier, "");
                            else
                                dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
    }

    public class ImportDefinition
    {
        public List<tbColumn> header;
        public string[] headerSelect;
        public DataTable data;
    }

    public class UploadItem
    {
        public string FilePath { get;set;}
        public string FormatName { get; set; }
    }

    public class UploadSample
    {
        public string FilePath { get; set; }
        public string Delimiter { get; set; }
        public string Encoding { get; set; }
    }

    public class tbColumn
    {
        public string name { get; set; }
    }
}
