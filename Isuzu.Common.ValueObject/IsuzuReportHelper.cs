using Isuzu.Entity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuReportHelper
    {

        public static MemoryStream parseExcelToDownload(DataTable dt, string filePath, string fileName, HttpResponse response)
        {
            MemoryStream ms = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                int idxRow = 1;
                int idxCol = 1;
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    var cell = sheet.Cells[idxRow, idxCol + col];
                    var border = cell.Style.Border;
                    cell.Value = dt.Columns[col].ColumnName;
                }

                idxRow = idxRow + 1;
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow + row, idxCol + col];
                        if (dt.Rows[row][col] is DateTime)
                        {
                            DateTime ddt = (DateTime)dt.Rows[row][col];
                            cell.Value = ddt.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US"));
                        }
                        else
                            cell.Value = dt.Rows[row][col].ToString();

                    }
                }

                sheet.Cells.AutoFitColumns();

                package.SaveAs(ms);
            }

            return ms;
            //DownloadFile(ms, fileName, response);

        }

        public static DataTable getExportInboundDataTable(List<InboundItems> items)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (items != null)
            {
                dataTable.Columns.Add(new DataColumn("No."));
                dataTable.Columns.Add(new DataColumn("ITA Order"));
                dataTable.Columns.Add(new DataColumn("ISZJ Order"));
                dataTable.Columns.Add(new DataColumn("Part No."));
                dataTable.Columns.Add(new DataColumn("Part Name"));
                dataTable.Columns.Add(new DataColumn("Q'ty"));
                dataTable.Columns.Add(new DataColumn("Vendor"));
                dataTable.Columns.Add(new DataColumn("Shelf"));
                dataTable.Columns.Add(new DataColumn("Destination"));
                dataTable.Columns.Add(new DataColumn("Carton No."));
                dataTable.Columns.Add(new DataColumn("Case No."));

                for (int i = 0; i < items.Count(); i++)
                {
                    //if(items[i].Status == IsuzuStatus.SHIPPED.ToString())
                    //{ 
                    object[] obj = new object[11];
                    obj[0] = items[i].SeqNo;
                    obj[1] = items[i].ITAOrder;
                    obj[2] = items[i].ISZJOrder;
                    obj[3] = items[i].PartNo;
                    obj[4] = items[i].ParrtName;
                    obj[5] = items[i].Qty.ToString();
                    obj[6] = items[i].Vendor;
                    obj[7] = items[i].Shelf;
                    obj[8] = items[i].Destination;
                    obj[9] = items[i].CartonNo;
                    obj[10] = items[i].CaseNo;
                    dataTable.Rows.Add(obj);
                    //}
                }
            }

            return dataTable;
        }

        public static string GetIsuzuAutoGenHeader(string input)
        {
            //if (input.Length == 1)
            //{
            //    switch (input.Trim().ToUpper())
            //    {
            //        case "C":
            //            input = input + DateTime.Now.ToString("yyMMdd");
            //            break;
            //        case "R":
            //            input = input + DateTime.Now.ToString("yyMMdd");
            //            break;
            //    }
            //}
            return input;
        }
        public static bool DownloadFile(MemoryStream ms, string fileName, HttpResponse respone)
        {
            //if (!File.Exists(path))
            //    return false;

            //System.Web.HttpResponse respone = System.Web.HttpContext.Current.Response;
            respone.ClearContent();
            respone.Clear();
            respone.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            respone.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            ms.WriteTo(respone.OutputStream);
            //respone.WriteFile(path);
            respone.Flush();
            respone.Close();

            return true;
        }
    }
}
