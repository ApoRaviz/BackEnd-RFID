using Fuji.Entity.ItemManagement;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fuji.Common.ValueObject
{
    public class FujiReportHelper
    {

        public static MemoryStream parseExcelToDownload(DataTable dt,string filePath)
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
            //DownloadFile(ms, fileName,response);
        }

        public static DataTable getFujiPickingGroupDataTable(FujiPickingGroup item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("HeadID"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Serial Number"));
                dataTable.Columns.Add(new DataColumn("Box Number"));
                dataTable.Columns.Add(new DataColumn("Item Group"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Item Type"));


                var q = from p in item.SerialDetail
                        orderby p.ItemGroup, p.ItemType
                        select p;

                foreach (var itemDetail in q)
                {
                    object[] obj = new object[7];
                    obj[0] = itemDetail.HeadID;
                    obj[1] = itemDetail.ItemCode;
                    obj[2] = itemDetail.SerialNumber;
                    obj[3] = itemDetail.BoxNumber;
                    obj[4] = itemDetail.ItemGroup;
                    obj[5] = itemDetail.Status;
                    obj[6] = itemDetail.ItemType;

                    dataTable.Rows.Add(obj);
                }
            }


            return dataTable;
        }
        public static DataTable getImportSerailByStatusDataTable(ImportSerialHead item, string status = "RECEIVED")
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Warehouse Code"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Allocated Serial No. (Ref1)"));
                dataTable.Columns.Add(new DataColumn("Location No."));
                dataTable.Columns.Add(new DataColumn("Allocated Lot No. (Ref2)"));
                dataTable.Columns.Add(new DataColumn("Allocated INV.No (Ref3)"));
                dataTable.Columns.Add(new DataColumn("Shipping Date"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Ref4"));
                dataTable.Columns.Add(new DataColumn("Ref5"));
                dataTable.Columns.Add(new DataColumn("Spare 1 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 2 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 3 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 4 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 5 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 6 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 7 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 8 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 9 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 10 (Details)"));
                dataTable.Columns.Add(new DataColumn("Order No."));


                var itemGroups = from p in item.ImportSerialDetail
                                 where p.Status.Equals(status)
                                 group p
                                 by p.ItemGroup into g
                                 select new { GroupID = g.Key, GroupList = g.ToList() };

                foreach (var itemGroup in itemGroups)
                {
                    string sItemCode = "", sSerialNumber = "", sBoxNumber = "", sStatus = "", sOrderNo = "", ref5 = "", sSpare1 = "", sSpare3 = "", sSpare4 = "";

                    var groupOrdered = itemGroup.GroupList.OrderBy(d => d.ItemType).ToList();
                    for (int i = 0; i < groupOrdered.Count; i++)
                    {
                        if (i == 0)//Main serial
                        {
                            sItemCode = groupOrdered[i].ItemCode;
                            sSerialNumber = groupOrdered[i].SerialNumber;
                            sBoxNumber = groupOrdered[i].BoxNumber;
                            sStatus = groupOrdered[i].Status;
                            sOrderNo = groupOrdered[i].OrderNo;
                        }
                        else if (i == 1)//Secound serial
                            ref5 = groupOrdered[i].SerialNumber;
                        else if (i == 2)
                            sSpare1 = groupOrdered[i].SerialNumber;
                        else if (i == 3)
                            sSpare3 = groupOrdered[i].SerialNumber;
                        else if (i == 4)
                            sSpare4 = groupOrdered[i].SerialNumber;
                    }
                    object[] obj = new object[21];
                    obj[0] = item.WHID;
                    obj[1] = sItemCode;
                    obj[2] = sSerialNumber;
                    obj[3] = item.Location;
                    obj[4] = sBoxNumber;
                    obj[5] = item.InvoiceNumber;
                    obj[6] = item.ReceivingDate.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    obj[7] = sStatus;
                    obj[8] = item.Remark;
                    obj[9] = ref5;
                    obj[10] = sSpare1;
                    obj[11] = itemGroup.GroupID;
                    obj[12] = sSpare3;
                    obj[13] = sSpare4;
                    obj[14] = "";
                    obj[15] = "";
                    obj[16] = "";
                    obj[17] = "";
                    obj[18] = "";
                    obj[19] = "";
                    obj[20] = sOrderNo;
                    dataTable.Rows.Add(obj);
                }

            }



            return dataTable;
        }
        public static DataTable getImportSerailDataTable(ImportSerialHead item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Warehouse Code"));
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Allocated Serial No. (Ref1)"));
                dataTable.Columns.Add(new DataColumn("Location No."));
                dataTable.Columns.Add(new DataColumn("Allocated Lot No. (Ref2)"));
                dataTable.Columns.Add(new DataColumn("Allocated INV.No (Ref3)"));
                dataTable.Columns.Add(new DataColumn("Shipping Date"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Ref4"));
                dataTable.Columns.Add(new DataColumn("Ref5"));
                dataTable.Columns.Add(new DataColumn("Spare 1 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 2 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 3 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 4 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 5 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 6 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 7 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 8 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 9 (Details)"));
                dataTable.Columns.Add(new DataColumn("Spare 10 (Details)"));
                dataTable.Columns.Add(new DataColumn("Order No."));


                var itemGroups = from p in item.ImportSerialDetail
                                 group p
                                 by p.ItemGroup into g
                                 select new { GroupID = g.Key, GroupList = g.ToList() };

                foreach (var itemGroup in itemGroups)
                {
                    string sItemCode = "", sSerialNumber = "", sBoxNumber = "", sStatus = "", sOrderNo = "", ref5 = "", sSpare1 = "", sSpare3 = "", sSpare4 = "";

                    var groupOrdered = itemGroup.GroupList.OrderBy(d => d.ItemType).ToList();
                    for (int i = 0; i < groupOrdered.Count; i++)
                    {
                        if (i == 0)
                        {
                            sItemCode = groupOrdered[i].ItemCode;
                            sSerialNumber = groupOrdered[i].SerialNumber;
                            sBoxNumber = groupOrdered[i].BoxNumber;
                            sStatus = groupOrdered[i].Status;
                            sOrderNo = groupOrdered[i].OrderNo;
                        }
                        else if (i == 1)
                            ref5 = groupOrdered[i].SerialNumber;
                        else if (i == 2)
                            sSpare1 = groupOrdered[i].SerialNumber;
                        else if (i == 3)
                            sSpare3 = groupOrdered[i].SerialNumber;
                        else if (i == 4)
                            sSpare4 = groupOrdered[i].SerialNumber;
                    }
                    object[] obj = new object[21];
                    obj[0] = item.WHID;
                    obj[1] = sItemCode;
                    obj[2] = sSerialNumber;
                    obj[3] = item.Location;
                    obj[4] = sBoxNumber;
                    obj[5] = item.InvoiceNumber;
                    obj[6] = item.ReceivingDate.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    obj[7] = sStatus;
                    obj[8] = item.Remark;
                    obj[9] = ref5;
                    obj[10] = sSpare1;
                    obj[11] = itemGroup.GroupID;
                    obj[12] = sSpare3;
                    obj[13] = sSpare4;
                    obj[14] = "";
                    obj[15] = "";
                    obj[16] = "";
                    obj[17] = "";
                    obj[18] = "";
                    obj[19] = "";
                    obj[20] = sOrderNo;
                    dataTable.Rows.Add(obj);
                }
            }



            return dataTable;
        }
        public static DataTable getImportSerailGroupDataTable(ImportSerialHead item)
        {
            DataTable dataTable = new DataTable();
            //custom fields
            if (item != null)
            {
                dataTable.Columns.Add(new DataColumn("Item Code"));
                dataTable.Columns.Add(new DataColumn("Item Group"));
                dataTable.Columns.Add(new DataColumn("RECDATE"));
                dataTable.Columns.Add(new DataColumn("Model"));
                dataTable.Columns.Add(new DataColumn("SERIAL"));
                dataTable.Columns.Add(new DataColumn("Status"));
                dataTable.Columns.Add(new DataColumn("Box No"));


                var q = from p in item.ImportSerialDetail
                        orderby p.ItemGroup, p.ItemType
                        select p;
                foreach (var itemDetail in q)
                {
                    object[] obj = new object[7];
                    obj[0] = itemDetail.ItemCode;
                    obj[1] = itemDetail.ItemGroup;
                    obj[2] = item.ReceivingDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                    obj[3] = "";
                    obj[4] = itemDetail.SerialNumber;
                    obj[5] = itemDetail.Status;
                    obj[6] = itemDetail.BoxNumber;

                    dataTable.Rows.Add(obj);
                }
            }


            return dataTable;
        }

        public static bool DownloadFile(MemoryStream ms, string fileName,HttpResponse respone)
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
