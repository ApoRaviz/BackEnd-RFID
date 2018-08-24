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

        public static DataTable getExportInvHistoryDataTable(List<InvoiceReportDetail> items)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (items != null)
            {
                dataTable.Columns.Add(new DataColumn("Invoice No."));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Total Order No."));
                dataTable.Columns.Add(new DataColumn("Total (Item) Qty"));
                dataTable.Columns.Add(new DataColumn("First Register (Status)"));
                dataTable.Columns.Add(new DataColumn("Last Register (Status)"));
                dataTable.Columns.Add(new DataColumn("First Pack Carton (Status)"));
                dataTable.Columns.Add(new DataColumn("Last Pack Carton (Status)"));
                dataTable.Columns.Add(new DataColumn("First Pack Case (Status)"));
                dataTable.Columns.Add(new DataColumn("Last Pack Case (Status)"));
                dataTable.Columns.Add(new DataColumn("First Shipping (Status)"));
                dataTable.Columns.Add(new DataColumn("Last Shipping (Status)"));
                dataTable.Columns.Add(new DataColumn("Total Carton No."));
                dataTable.Columns.Add(new DataColumn("Total Case No."));

                for (int i = 0; i < items.Count(); i++)
                {
                    object[] obj = new object[14];
                    obj[0] = items[i].InvNo;
                    obj[1] = items[i].Status;
                    obj[2] = items[i].QtyOrder;
                    obj[3] = items[i].QtyItem;
                    obj[4] = (items[i].RegisterStart != null) ? Convert.ToDateTime(items[i].RegisterStart).ToString("dd/MM/yyyy HH:mm") : items[i].RegisterStart.ToString();
                    obj[5] = (items[i].RegisterEnd != null) ? Convert.ToDateTime(items[i].RegisterEnd).ToString("dd/MM/yyyy HH:mm") : items[i].RegisterEnd.ToString();
                    obj[6] = (items[i].CartonStart != null) ? Convert.ToDateTime(items[i].CartonStart).ToString("dd/MM/yyyy HH:mm") : items[i].CartonStart.ToString(); 
                    obj[7] = (items[i].CartonEnd != null) ? Convert.ToDateTime(items[i].CartonEnd).ToString("dd/MM/yyyy HH:mm") : items[i].CartonEnd.ToString();
                    obj[8] = (items[i].CaseStart != null) ? Convert.ToDateTime(items[i].CaseStart).ToString("dd/MM/yyyy HH:mm") : items[i].CaseStart.ToString();
                    obj[9] = (items[i].CaseEnd != null) ? Convert.ToDateTime(items[i].CaseEnd).ToString("dd/MM/yyyy HH:mm") : items[i].CaseEnd.ToString();
                    obj[10] = (items[i].ShipStart != null) ? Convert.ToDateTime(items[i].ShipStart).ToString("dd/MM/yyyy HH:mm") : items[i].ShipStart.ToString();
                    obj[11] = (items[i].ShipEnd != null) ? Convert.ToDateTime(items[i].ShipEnd).ToString("dd/MM/yyyy HH:mm") : items[i].ShipEnd.ToString();
                    obj[12] = items[i].totalCarton;
                    obj[13] = items[i].totalCase;
                    dataTable.Rows.Add(obj);
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
