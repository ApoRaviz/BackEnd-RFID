using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using QRCoder;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml;
using System.Text;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using WIM.Core.Common.Helpers;
using WMS.Entity.LayoutManagement;
using WMS.Entity.Report;
using WIM.Core.Common.Constants;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WMS.WebApi.Report
{
    public class ReportUtils
    {
        public static HttpResponseMessage ViewReport(string reportPath, IList dataList)
        {
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = reportPath;

                reportViewer.LocalReport.Refresh();

                string imagePath = "file:///D:/test.png";
                ReportParameter[] parameter = new ReportParameter[2];
                parameter[0]=new ReportParameter("Img", imagePath, true);
                parameter[1] = new ReportParameter("ItemCode", "อยู่ในน้ำ การันย์", true);
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.SetParameters(parameter);


                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = dataList;
                reportViewer.LocalReport.DataSources.Add(rds1);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
        }

        public static HttpResponseMessage ViewReport(string reportPath, System.Data.DataTable dataList)
        {
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = reportPath;        

                reportViewer.LocalReport.Refresh();

                //string imagePath = "file:///D:/test.png";
                //ReportParameter parameter = new ReportParameter("Img", imagePath, true);
                //reportViewer.LocalReport.EnableExternalImages = true;
                //reportViewer.LocalReport.SetParameters(parameter);

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = dataList;
                reportViewer.LocalReport.DataSources.Add(rds1);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
        }

        public static HttpResponseMessage ViewReportFromStream(MemoryStream r, System.Data.DataTable dataList)
        {
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                r.Position = 0;
                reportViewer.LocalReport.LoadReportDefinition(r);

                reportViewer.LocalReport.Refresh();

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = dataList;
                reportViewer.LocalReport.DataSources.Add(rds1);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
        }

        public static Image GenerateQrCode(string code)
        {
            Image img;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                                byte[] byteImage = ms.ToArray();
                                MemoryStream ms1 = new MemoryStream(byteImage);
                                img = Image.FromStream(ms1);
                            }

                            img = ResizeImage(img, 300, 300);

                            RectangleF rectangleF = new RectangleF(10f, 0.0f, 0.0f, 0.0f);

                            Image MyImage = new Bitmap(img);

                            Graphics g2 = Graphics.FromImage(MyImage);

                            IntPtr dc2 = g2.GetHdc();

                            g2.ReleaseHdc(dc2);
                            //Image save here...     
                            MyImage.Save("D:\\test.png", System.Drawing.Imaging.ImageFormat.Jpeg);
                        }                        
                    }
                }
            }

            return img;
        }

        public static byte[] GenerateQrCodeToByte(string code)
        {
            byte[] byteImage;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                                byteImage = ms.ToArray();                                
                            }                            
                        }
                    }
                }
            }

            return byteImage;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void CreateReport(LabelLayoutHeader_MT data)
        {
            string[] array = data.detail.Where(x => x.Label_From == Constant.FromMaster_Text).Select(x => x.Label_Item).Distinct().ToArray();
            if (array.Length == 0)
                array = new string[] { "test" };

            string ReportName = string.Format("~/Report/{0}.rdlc", data.FormatName.Replace(" ","_"));

            FileStream stream;
            stream = new FileStream(HttpContext.Current.Server.MapPath(ReportName), FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

            // Causes child elements to be indented
            writer.Formatting = Formatting.Indented;

            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");
            writer.WriteAttributeString("xmlns", null, "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            writer.WriteAttributeString("xmlns:rd", null, "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            writer.WriteStartElement("Body");
            writer.WriteStartElement("ReportItems");

            foreach (LabelLayoutDetail_MT detail in data.detail)
            {
                if (detail.Label_Type == Constant.Word_Text)
                {
                    writer.WriteStartElement("Textbox");
                    if (detail.Label_From == Constant.Newword_Text)
                        writer.WriteAttributeString("Name", null, detail.Label_Text.Replace(" ",""));
                    else
                        writer.WriteAttributeString("Name", null, detail.Label_Item.Replace(" ", ""));

                    writer.WriteElementString("CanGrow", "true");
                    writer.WriteElementString("KeepTogether", "true");
                    writer.WriteStartElement("Paragraphs");
                    writer.WriteStartElement("Paragraph");
                    writer.WriteStartElement("TextRuns");
                    writer.WriteStartElement("TextRun");
                    if (detail.Label_From == Constant.Newword_Text)
                        writer.WriteElementString("Value", detail.Label_Text);
                    else
                        writer.WriteElementString("Value", string.Format("=First(Fields!{0}.Value, \"DataSet1\")", detail.Label_Item));

                    writer.WriteStartElement("Style");
                    writer.WriteElementString("FontFamily", detail.Font_Name);
                    writer.WriteElementString("FontSize", detail.Font_Size + "pt");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // TextRun
                    writer.WriteEndElement(); // TextRuns
                    writer.WriteStartElement("Style");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Paragraph
                    writer.WriteEndElement(); // Paragraphs

                    writer.WriteElementString("rd:DefaultName", detail.Label_Item);
                    writer.WriteElementString("Top", (float.Parse(detail.Label_Top.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Left", (float.Parse(detail.Label_Left.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Height", (float.Parse(detail.Label_Height.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("Width", (float.Parse(detail.Label_Width.ToString()) * 0.01075) + "in");

                    writer.WriteStartElement("Style");
                    writer.WriteStartElement("Border");
                    writer.WriteElementString("Style", "None");
                    writer.WriteEndElement(); // Border

                    writer.WriteElementString("PaddingLeft", "2pt");
                    writer.WriteElementString("PaddingRight", "2pt");
                    writer.WriteElementString("PaddingTop", "2pt");
                    writer.WriteElementString("PaddingBottom", "2pt");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Textbox
                }
                else if(detail.Label_Type == Constant.BarCode_Text)
                {
                    writer.WriteStartElement("Image");
                    writer.WriteAttributeString("Name", null, detail.Label_Code);
                    writer.WriteElementString("Source", "Database");

                    var colName = "";

                    if (detail.Label_From == Constant.Newword_Text)
                    {
                        colName = detail.Label_BarcodeType.ToString().Replace(" ", "") + detail.Label_Text.Replace(" ", "");
                    }
                    else if(detail.Label_From == Constant.FromMaster_Text)
                    {
                        colName = detail.Label_BarcodeType.ToString().Replace(" ", "") + detail.Label_Item;
                    }

                    writer.WriteElementString("Value", string.Format("=First(Fields!{0}.Value, \"DataSet1\")", colName));
                    writer.WriteElementString("MIMEType", "image/jpeg");
                    writer.WriteElementString("Sizing", "FitProportional");
                    writer.WriteElementString("Top", (float.Parse(detail.Label_Top.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Left", (float.Parse(detail.Label_Left.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Height", (float.Parse(detail.Label_Height.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("Width", (float.Parse(detail.Label_Width.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("ZIndex", "2");

                    writer.WriteStartElement("Style");
                    writer.WriteStartElement("Border");
                    writer.WriteElementString("Style", "None");
                    writer.WriteEndElement(); // Border
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Image
                }
            }

            writer.WriteEndElement(); // ReportItems
            writer.WriteElementString("Height", data.Height + data.HeightUnit);
            writer.WriteStartElement("Style");
            writer.WriteEndElement(); // Style
            writer.WriteEndElement(); // Body

            writer.WriteElementString("Width", data.Width + data.WidthUnit);

            writer.WriteStartElement("Page");
            writer.WriteElementString("LeftMargin", "1pt");
            writer.WriteElementString("RightMargin", "1pt");
            writer.WriteElementString("TopMargin", "1pt");
            writer.WriteElementString("BottomMargin", "1pt");
            writer.WriteStartElement("Style");
            writer.WriteEndElement(); // Style
            writer.WriteEndElement(); // Page

            writer.WriteElementString("AutoRefresh", "0");

            // DataSource element
            writer.WriteStartElement("DataSources");
            writer.WriteStartElement("DataSource");
            writer.WriteAttributeString("Name", null, "Item_MT");
            writer.WriteStartElement("ConnectionProperties");
            writer.WriteElementString("DataProvider", "System.Data.DataSet");
            writer.WriteElementString("ConnectString", "/* Local Connection */");
            writer.WriteEndElement(); // ConnectionProperties
            writer.WriteElementString("rd:DataSourceID", "be13729b-d0f2-41b1-aeb0-13aa56182e89");

            writer.WriteEndElement(); // DataSource
            writer.WriteEndElement(); // DataSources

            // DataSet element
            writer.WriteStartElement("DataSets");
            writer.WriteStartElement("DataSet");
            writer.WriteAttributeString("Name", null, "DataSet1");

            // Query element
            writer.WriteStartElement("Query");
            writer.WriteElementString("DataSourceName", "Item_MT");

            writer.WriteElementString("CommandType", "Text");
            writer.WriteElementString("CommandText", "/* Local Query */");
            writer.WriteEndElement(); // Query

            // Fields elements
            writer.WriteStartElement("Fields");
            foreach (string fieldName in array)
            {
                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name", null, fieldName);
                writer.WriteElementString("DataField", null, fieldName);
                writer.WriteElementString("rd:TypeName", null, "System.String");
                writer.WriteEndElement(); // Field
            }

            // Add Bar Code

            List<LabelLayoutDetail_MT> field = data.detail.Where(x => x.Label_Type == Constant.BarCode_Text).ToList();

            foreach (LabelLayoutDetail_MT f in field)
            {
                var colName = "";

                if (f.Label_From == Constant.Newword_Text)
                {
                    colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Text.Replace(" ", "");
                }
                else if (f.Label_From == Constant.FromMaster_Text)
                {
                    colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Item;
                }

                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name", null, colName);
                writer.WriteElementString("DataField", null, colName);
                writer.WriteElementString("rd:TypeName", null, "System.Byte[]");
                writer.WriteEndElement(); // Field
            }            

            // End previous elements
            writer.WriteEndElement(); // Fields

            writer.WriteStartElement("rd:DataSetInfo");
            writer.WriteElementString("rd:DataSetName", null, "Item_MT");
            writer.WriteElementString("rd:SchemaPath", null, "DataSet/Item_MT.xsd");
            writer.WriteElementString("rd:TableName", null, "Item_MT");
            writer.WriteElementString("rd:TableAdapterFillMethod", null, "Fill");
            writer.WriteElementString("rd:TableAdapterGetDataMethod", null, "GetData");
            writer.WriteElementString("rd:TableAdapterName", null, "Item_MTTableAdapter");
            writer.WriteEndElement(); // rd:DataSetInf

            writer.WriteEndElement(); // DataSet
            writer.WriteEndElement(); // DataSets

            //writer.WriteStartElement("ReportParameters");
            //writer.WriteStartElement("ReportParameter");
            //writer.WriteAttributeString("Name", null, "Img");
            //writer.WriteElementString("DataType", null, "String");
            //writer.WriteElementString("Prompt", null, "ReportParameter1");
            //writer.WriteEndElement(); // ReportParameter
            //writer.WriteEndElement(); // ReportParameters

            writer.WriteEndElement(); // Report

            // Flush the writer and close the stream
            writer.Flush();
            stream.Close();
        }

        public static MemoryStream CreateReportToStream(LabelLayoutHeader_MT data)
        {
            string[] array = data.detail.Where(x => x.Label_From == Constant.FromMaster_Text).Select(x => x.Label_Item).Distinct().ToArray();
            if (array.Length == 0)
                array = new string[] { "test" };

            MemoryStream stream;
            stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

            // Causes child elements to be indented
            writer.Formatting = Formatting.Indented;

            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");
            writer.WriteAttributeString("xmlns", null, "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition");
            writer.WriteAttributeString("xmlns:rd", null, "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            writer.WriteStartElement("ReportSections");
            writer.WriteStartElement("ReportSection");
            writer.WriteStartElement("Body");
            writer.WriteStartElement("ReportItems");

            foreach (LabelLayoutDetail_MT detail in data.detail)
            {
                if (detail.Label_Type == Constant.Word_Text)
                {
                    writer.WriteStartElement("Textbox");
                    if (detail.Label_From == Constant.Newword_Text)
                        writer.WriteAttributeString("Name", null, detail.Label_Text.Replace(" ", ""));
                    else
                        writer.WriteAttributeString("Name", null, detail.Label_Item.Replace(" ", ""));

                    writer.WriteElementString("CanGrow", "true");
                    writer.WriteElementString("KeepTogether", "true");
                    writer.WriteStartElement("Paragraphs");
                    writer.WriteStartElement("Paragraph");
                    writer.WriteStartElement("TextRuns");
                    writer.WriteStartElement("TextRun");
                    if (detail.Label_From == Constant.Newword_Text)
                        writer.WriteElementString("Value", detail.Label_Text);
                    else
                        writer.WriteElementString("Value", string.Format("=First(Fields!{0}.Value, \"DataSet1\")", detail.Label_Item));

                    writer.WriteStartElement("Style");
                    writer.WriteElementString("FontFamily", detail.Font_Name);
                    writer.WriteElementString("FontSize", detail.Font_Size + "pt");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // TextRun
                    writer.WriteEndElement(); // TextRuns
                    writer.WriteStartElement("Style");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Paragraph
                    writer.WriteEndElement(); // Paragraphs

                    writer.WriteElementString("rd:DefaultName", detail.Label_Item);
                    writer.WriteElementString("Top", (float.Parse(detail.Label_Top.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Left", (float.Parse(detail.Label_Left.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Height", (float.Parse(detail.Label_Height.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("Width", (float.Parse(detail.Label_Width.ToString()) * 0.01075) + "in");

                    writer.WriteStartElement("Style");
                    writer.WriteStartElement("Border");
                    writer.WriteElementString("Style", "None");
                    writer.WriteEndElement(); // Border

                    writer.WriteElementString("PaddingLeft", "2pt");
                    writer.WriteElementString("PaddingRight", "2pt");
                    writer.WriteElementString("PaddingTop", "2pt");
                    writer.WriteElementString("PaddingBottom", "2pt");
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Textbox
                }
                else if (detail.Label_Type == Constant.BarCode_Text)
                {
                    writer.WriteStartElement("Image");
                    writer.WriteAttributeString("Name", null, detail.Label_Code);
                    writer.WriteElementString("Source", "Database");

                    var colName = "";

                    if (detail.Label_From == Constant.Newword_Text)
                    {
                        colName = detail.Label_BarcodeType.ToString().Replace(" ", "") + detail.Label_Text.Replace(" ", "");
                    }
                    else if (detail.Label_From == Constant.FromMaster_Text)
                    {
                        colName = detail.Label_BarcodeType.ToString().Replace(" ", "") + detail.Label_Item;
                    }

                    writer.WriteElementString("Value", string.Format("=First(Fields!{0}.Value, \"DataSet1\")", colName));
                    writer.WriteElementString("MIMEType", "image/jpeg");
                    writer.WriteElementString("Sizing", "FitProportional");
                    writer.WriteElementString("Top", (float.Parse(detail.Label_Top.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Left", (float.Parse(detail.Label_Left.ToString()) / detail.PxPerInch_Ratio) + "in");
                    writer.WriteElementString("Height", (float.Parse(detail.Label_Height.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("Width", (float.Parse(detail.Label_Width.ToString()) * 0.01075) + "in");
                    writer.WriteElementString("ZIndex", "2");

                    writer.WriteStartElement("Style");
                    writer.WriteStartElement("Border");
                    writer.WriteElementString("Style", "None");
                    writer.WriteEndElement(); // Border
                    writer.WriteEndElement(); // Style
                    writer.WriteEndElement(); // Image
                }
            }

            writer.WriteEndElement(); // ReportItems
            writer.WriteElementString("Height", data.Height + data.HeightUnit);
            writer.WriteStartElement("Style");
            writer.WriteEndElement(); // Style
            writer.WriteEndElement(); // Body

            writer.WriteElementString("Width", data.Width + data.WidthUnit);

            writer.WriteStartElement("Page");
            writer.WriteElementString("LeftMargin", "1pt");
            writer.WriteElementString("RightMargin", "1pt");
            writer.WriteElementString("TopMargin", "1pt");
            writer.WriteElementString("BottomMargin", "1pt");
            writer.WriteStartElement("Style");
            writer.WriteEndElement(); // Style
            writer.WriteEndElement(); // Page
            writer.WriteEndElement(); // ReportSection
            writer.WriteEndElement(); // ReportSections

            writer.WriteElementString("AutoRefresh", "0");

            // DataSource element
            writer.WriteStartElement("DataSources");
            writer.WriteStartElement("DataSource");
            writer.WriteAttributeString("Name", null, "MasterDataSet");
            writer.WriteStartElement("ConnectionProperties");
            writer.WriteElementString("DataProvider", "System.Data.DataSet");
            writer.WriteElementString("ConnectString", "/* Local Connection */");
            writer.WriteEndElement(); // ConnectionProperties
            writer.WriteElementString("rd:DataSourceID", "be13729b-d0f2-41b1-aeb0-13aa56182e89");

            writer.WriteEndElement(); // DataSource
            writer.WriteEndElement(); // DataSources

            // DataSet element
            writer.WriteStartElement("DataSets");
            writer.WriteStartElement("DataSet");
            writer.WriteAttributeString("Name", null, "DataSet1");

            // Query element
            writer.WriteStartElement("Query");
            writer.WriteElementString("DataSourceName", "MasterDataSet");

            writer.WriteElementString("CommandType", "Text");
            writer.WriteElementString("CommandText", "/* Local Query */");
            writer.WriteEndElement(); // Query

            // Fields elements
            writer.WriteStartElement("Fields");
            foreach (string fieldName in array)
            {
                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name", null, fieldName);
                writer.WriteElementString("DataField", null, fieldName);
                writer.WriteElementString("rd:TypeName", null, "System.String");
                writer.WriteEndElement(); // Field
            }

            // Add Bar Code

            List<LabelLayoutDetail_MT> field = data.detail.Where(x => x.Label_Type == Constant.BarCode_Text).ToList();

            foreach (LabelLayoutDetail_MT f in field)
            {
                var colName = "";

                if (f.Label_From == Constant.Newword_Text)
                {
                    colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Text.Replace(" ", "");
                }
                else if (f.Label_From == Constant.FromMaster_Text)
                {
                    colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Item;
                }

                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name", null, colName);
                writer.WriteElementString("DataField", null, colName);
                writer.WriteElementString("rd:TypeName", null, "System.Byte[]");
                writer.WriteEndElement(); // Field
            }

            // End previous elements
            writer.WriteEndElement(); // Fields

            writer.WriteStartElement("rd:DataSetInfo");
            writer.WriteElementString("rd:DataSetName", null, "MasterDataSet");
            writer.WriteElementString("rd:SchemaPath", null, "DataSet/MasterDataSet.xsd");
            writer.WriteElementString("rd:TableName", null, "MasterDataSet");
            writer.WriteElementString("rd:TableAdapterFillMethod", null, "Fill");
            writer.WriteElementString("rd:TableAdapterGetDataMethod", null, "GetData");
            writer.WriteElementString("rd:TableAdapterName", null, "TableAdapter");
            writer.WriteEndElement(); // rd:DataSetInf

            writer.WriteEndElement(); // DataSet
            writer.WriteEndElement(); // DataSets

            //writer.WriteStartElement("ReportParameters");
            //writer.WriteStartElement("ReportParameter");
            //writer.WriteAttributeString("Name", null, "Img");
            //writer.WriteElementString("DataType", null, "String");
            //writer.WriteElementString("Prompt", null, "ReportParameter1");
            //writer.WriteEndElement(); // ReportParameter
            //writer.WriteEndElement(); // ReportParameters

            writer.WriteEndElement(); // Report
            // Flush the writer and close the stream
            writer.Flush();   
            

            return stream;
        }

        public static void GetExportReport(string ReportName, ReportLayoutHeader_MT report, DataTable dt)
        {
            string filePath = Path.GetTempFileName();
            string fileName = "{0}_{1}.{2}";
            bool includeHeader = report.IncludeHeader != null && report.IncludeHeader.Value == true;
            string delimiter = ";";
            string gualifier = "";
            Encoding encoding = Encoding.Default;

            switch (report.Delimiter)
            {
                case "Tab": delimiter = "\t"; break;
                case "Semicolon (;)": delimiter = ";"; break;
                case "Colon (:)": delimiter = ":"; break;
                case "Comma (,)": delimiter = ","; break;
            }

            switch(report.TextGualifier)
            {
                case "Double quote (\")": gualifier = "\""; break;
                case "Single quote (')": gualifier = "'"; break;
                default: gualifier = ""; break;
            }

            switch (report.Encoding)
            {
                case "Unicode": encoding = Encoding.Unicode; break;
                case "Unicode big endian": encoding = Encoding.BigEndianUnicode; break;
                case "UTF-8": encoding = Encoding.UTF8; break;
                default: encoding = Encoding.Default; break;
            }

            if (report.FormatType == "XLSX")
            {
                filePath = Path.GetTempFileName().Replace(".tmp", ".xlsx");
                WriteReportExcel(dt, filePath, report.StartExportRow.Value, includeHeader);
                fileName = String.Format(fileName, ReportName, DateTime.Now.ToString("ddMMyyyy"), "xlsx");
            }
            else
            {
                string fileType = "";

                if (report.FormatType == "CSV")
                    fileType = "csv";
                else if (report.FormatType == "TXT")
                    fileType = "txt";
                else if (report.FormatType == "Other")
                    fileType = report.FileExtention.Replace(".", "");

                WriteReportFile(dt, filePath, report.StartExportRow.Value, includeHeader, delimiter, gualifier, encoding);
                fileName = String.Format(fileName, ReportName, DateTime.Now.ToString("ddMMyyyy"), fileType);
            }

            DownloadHelper.DownloadFile(filePath, fileName);            
        }

        public static void GetExportReport(string ReportName, ReportLayout_MT report, DataTable dt)
        {
            string filePath = Path.GetTempFileName();
            string fileName = "{0}_{1}.{2}";
            bool includeHeader = report.ReportDetail.IncludeHeader != null && report.ReportDetail.IncludeHeader.Value == true;
            string delimiter = ";";
            string gualifier = "";
            Encoding encoding = Encoding.Default;

            switch (report.ReportDetail.Delimiter)
            {
                case "Tab": delimiter = "\t"; break;
                case "Semicolon (;)": delimiter = ";"; break;
                case "Colon (:)": delimiter = ":"; break;
                case "Comma (,)": delimiter = ","; break;
            }

            switch (report.ReportDetail.TextGualifier)
            {
                case "Double quote (\")": gualifier = "\""; break;
                case "Single quote (')": gualifier = "'"; break;
                default: gualifier = ""; break;
            }

            switch (report.ReportDetail.Encoding)
            {
                case "Unicode": encoding = Encoding.Unicode; break;
                case "Unicode big endian": encoding = Encoding.BigEndianUnicode; break;
                case "UTF-8": encoding = Encoding.UTF8; break;
                default: encoding = Encoding.Default; break;
            }

            if (report.ReportDetail.FormatType == "XLSX")
            {
                filePath = Path.GetTempFileName().Replace(".tmp", ".xlsx");
                WriteReportExcel(dt, filePath, report.ReportDetail.StartExportRow.Value, includeHeader);
                fileName = String.Format(fileName, ReportName, DateTime.Now.ToString("ddMMyyyy"), "xlsx");
            }
            else
            {
                string fileType = "";

                if (report.ReportDetail.FormatType == "CSV")
                    fileType = "csv";
                else if (report.ReportDetail.FormatType == "TXT")
                    fileType = "txt";
                else if (report.ReportDetail.FormatType == "Other")
                    fileType = report.ReportDetail.FileExtention.Replace(".", "");

                WriteReportFile(dt, filePath, report.ReportDetail.StartExportRow.Value, includeHeader, delimiter, gualifier, encoding);
                fileName = String.Format(fileName, ReportName, DateTime.Now.ToString("ddMMyyyy"), fileType);
            }
            
            DownloadHelper.DownloadFile(filePath, fileName , report.ReportDetail.FormatType);
        }

        public static void WriteReportFile<T>(IEnumerable<T> items, string path, bool includeHeader = false, string delimeter = ",")
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var writer = new StreamWriter(path, false, Encoding.Unicode))
            {
                if(includeHeader)
                    writer.WriteLine(string.Join(delimeter + " ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(delimeter + " ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }

        public static void WriteReportExcel<T>(IEnumerable<T> items, string path, bool includeHeader = false)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");
                //sheet.Cells["A1"].LoadFromCollection(items, includeHeader, OfficeOpenXml.Table.TableStyles.None);

                var col = 1;
                var row = 0;

                //Write Header
                foreach (var header in props)
                {
                    if (includeHeader)
                    {
                        row = 1;
                        var cell = sheet.Cells[row, col];
                        var border = cell.Style.Border;
                        cell.Value = header.Name;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style =
                            ExcelBorderStyle.Thin;
                    }

                    if (header.PropertyType == typeof(Int32) || header.PropertyType == typeof(Int32?))
                        sheet.Column(col).Style.Numberformat.Format = "#,##0";
                    else if (header.PropertyType == typeof(DateTime) || header.PropertyType == typeof(DateTime?))
                        sheet.Column(col).Style.Numberformat.Format = "dd/mm/yyyy";
                    else if (header.PropertyType == typeof(decimal) || header.PropertyType == typeof(decimal?))
                        sheet.Column(col).Style.Numberformat.Format = "#,##0.00";

                    col = col + 1;
                }                

                //Write Detail
                foreach (var item in items)
                {
                    col = 1;
                    row++;
                    foreach (var detail in props.Select(p => p.GetValue(item, null)))
                    {
                        var cell = sheet.Cells[row, col++];
                        var border = cell.Style.Border;
                        cell.Value = detail;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style =
                            ExcelBorderStyle.Thin;
                    }                    
                }

                sheet.Cells.AutoFitColumns();

                package.Save();
            }
        }

        public static void WriteReportFile(DataTable dt, string path, int startRow, bool includeHeader = false, string delimeter = ",", string gualifier = "", Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            using (var writer = new StreamWriter(path, false, encoding))
            {
                string text = "";

                if(startRow > 1)
                {
                    int idx = 1;

                    for (int i = idx; i < startRow; i++)
                        writer.WriteLine("");
                }

                if (includeHeader)
                {
                    writer.WriteLine(string.Join(delimeter, dt.Columns.Cast<DataColumn>().Select(x => gualifier + x.ColumnName + gualifier).ToArray()));
                }                

                for (int row = 0;row < dt.Rows.Count; row++)
                {
                    text = "";

                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        string val = "";

                        if (dt.Columns[col].DataType == typeof(Int32) || dt.Columns[col].DataType == typeof(Int32?))
                            val = Int32.Parse(dt.Rows[row][col].ToString()).ToString("#,##0");
                        else if (dt.Columns[col].DataType == typeof(DateTime) || dt.Columns[col].DataType == typeof(DateTime?))
                            val = DateTime.Parse(dt.Rows[row][col].ToString()).ToString("dd/MM/yyyy");
                        else if (dt.Columns[col].DataType == typeof(decimal) || dt.Columns[col].DataType == typeof(decimal?))
                            val = Decimal.Parse(dt.Rows[row][col].ToString()).ToString("#,##0.00");
                        else
                            val = dt.Rows[row][col].ToString();

                        if (col == 0)
                            text = gualifier + val + gualifier;
                        else
                            text = text + delimeter + gualifier + val + gualifier;
                    }

                    writer.WriteLine(text);
                }                
            }
        }

        public static void WriteReportExcel(DataTable dt, string path, int startRow, bool includeHeader = false)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("report");

                int idxRow = startRow;
                int idxCol = 1;

                //Write Header
                for(int col = 0; col < dt.Columns.Count; col ++)
                {
                    if (includeHeader)
                    {
                        var cell = sheet.Cells[idxRow, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Columns[col].ColumnName;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style =
                            ExcelBorderStyle.Thin;                        
                    }

                    if (dt.Columns[col].DataType == typeof(Int32) || dt.Columns[col].DataType == typeof(Int32?))
                        sheet.Column(idxCol + col).Style.Numberformat.Format = "#,##0";
                    else if (dt.Columns[col].DataType == typeof(DateTime) || dt.Columns[col].DataType == typeof(DateTime?))
                        sheet.Column(idxCol + col).Style.Numberformat.Format = "dd/MM/yyyy";
                    else if (dt.Columns[col].DataType == typeof(decimal) || dt.Columns[col].DataType == typeof(decimal?))
                        sheet.Column(idxCol + col).Style.Numberformat.Format = "#,##0.00";
                }     
                
                if (includeHeader)
                    idxRow = idxRow + 1;

                //Write Detail
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for(int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = sheet.Cells[idxRow + row, idxCol + col];
                        var border = cell.Style.Border;
                        cell.Value = dt.Rows[row][col];
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style =
                            ExcelBorderStyle.Thin;
                    }
                }

                sheet.Cells.AutoFitColumns();

                package.Save();
            }
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true, int startRow = 1)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[startRow, 1, startRow, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                startRow = hasHeader ? (startRow + 1) : startRow;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        public static DataTable GetDataTableFromFile(string path, Encoding encoding = null, bool hasHeader = true, string delimiter = ",", string gualifier = "")
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
                    dt.Columns.Add(hasHeader ? text : string.Format("Column {0}", colIdx++));
                }

                if(!hasHeader)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < firstLine.Length; i++)
                    {
                        dr[i] = firstLine[i].Replace(gualifier, "");
                    }
                    dt.Rows.Add(dr);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(delimiter.ToCharArray());
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < firstLine.Length; i++)
                    {
                        dr[i] = rows[i].Replace(gualifier, "");
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
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
                        row[cell.Start.Column - 1] = cell.Text.Replace("\"",string.Empty);
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
                    if(rows.Length >= firstLine.Length)
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
}