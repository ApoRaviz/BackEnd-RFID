using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Master.WebApi.Report
{
    public class ReportUtil
    {
        public static HttpResponseMessage CreateRDLCReport(String ReportName, String ReportTitle, System.Data.DataSet ds)
        {
            return CreateRDLCReportByPDF(ReportName,ReportTitle,ds);
        }

        /// <summary>
        /// image = '~/path/to/image'
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="ReportTitle"></param>
        /// <param name="ds"></param>
        /// <param name="imageLogo"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateRDLCReportByPDF(String ReportName, String ReportTitle, System.Data.DataSet ds, string imageLogo = "")
        {

            

            ReportBuilder reportBuilder = new ReportBuilder();
            reportBuilder.DataSource = ds;

            reportBuilder.Page = new ReportPage();

            int allCols = 1;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                allCols += ds.Tables[i].Columns.Count;
            }

            #region ReportSettings
            allCols = (7 - allCols);//Auto Center Table
            reportBuilder.TableDimension = new ReportDimensions() { Left = allCols < 1 ? 0 : allCols };
            #endregion

            #region Footer
            ReportSections reportFooter = new ReportSections();
            ReportItems reportFooterItems = new ReportItems();
            ReportTextBoxControl[] footerTxt = new ReportTextBoxControl[3];
            string footer = string.Format("Copyright  {0}         Report Generated On  {1}          Page  {2}  of {3} ", DateTime.Now.Year, DateTime.Now, ReportGlobalParameters.CurrentPageNumber, ReportGlobalParameters.TotalPages);
            footerTxt[0] = new ReportTextBoxControl() { Name = "txtCopyright", ValueOrExpression = new string[] { footer } };
            reportFooterItems.TextBoxControls = footerTxt;
            reportFooter.ReportControlItems = reportFooterItems;
            reportBuilder.Page.ReportFooter = reportFooter;
            #endregion

            #region Header 
            ReportSections reportHeader = new ReportSections();
            reportHeader.Size = new ReportScale() { Height = 0.56849 };

            ReportItems reportHeaderItems = new ReportItems();

            ReportTextBoxControl[] headerTxt = new ReportTextBoxControl[]{
                new ReportTextBoxControl() { Name = "txtReportTitle"
                , ValueOrExpression = new string[] { "Report Name: " + ReportTitle }
                , Position = new ReportDimensions(){ Left = 2} }
                //new ReportTextBoxControl()
                //{
                //    Name = "txttesttitle",
                //    ValueOrExpression = new string[] { "ทดสอบ: ฟหกดฟหกด" }
                //,
                //    Position = new ReportDimensions() { Left = 3, Top = 0.7 }
                //,
                //    Size = new ReportScale() { Height = -1, Width = -1 }
                //}
            };


            reportHeaderItems.TextBoxControls = headerTxt;


            reportHeader.ReportControlItems = reportHeaderItems;
            reportBuilder.Page.ReportHeader = reportHeader;

                #region ImageLogo
                if(!string.IsNullOrEmpty(imageLogo))
                {
                    //"~/img/logo.png"
                    ReportImage reportItem = new ReportImage() { ImagePath = imageLogo, fullMIMEType = "image/png", ImageFrom = ReportImageSource.Embedded };
                    reportItem.Position = new ReportDimensions() { Top = 0, Left = 5 };
                    reportItem.Size = new ReportScale() { Height = 0.67, Width = 2 };

                    reportBuilder.Logo = reportItem;
                }
                #endregion
            #endregion

            return DownLoadByReportViewer(reportBuilder,ds,"Dynamic",ReportType.PDF);


        }
        public static HttpResponseMessage CreateRDLCReportByEXCEL(String ReportName, String ReportTitle, System.Data.DataSet ds, string imageLogo = "")
        {

            ReportBuilder reportBuilder = new ReportBuilder();
            reportBuilder.DataSource = ds;

            reportBuilder.Page = new ReportPage();

            #region ReportSettings
            reportBuilder.TableDimension = new ReportDimensions() { Left = 0, Top = 0 };
            #endregion

            #region Footer
            ReportSections reportFooter = new ReportSections();
            ReportItems reportFooterItems = new ReportItems();
            ReportTextBoxControl[] footerTxt = new ReportTextBoxControl[3];
            string footer = string.Format("Copyright  {0}         Report Generated On  {1}          Page  {2}  of {3} ", DateTime.Now.Year, DateTime.Now, ReportGlobalParameters.CurrentPageNumber, ReportGlobalParameters.TotalPages);
            footerTxt[0] = new ReportTextBoxControl() { Name = "txtCopyright", ValueOrExpression = new string[] { footer } };
            reportFooterItems.TextBoxControls = footerTxt;
            reportFooter.ReportControlItems = reportFooterItems;
            reportBuilder.Page.ReportFooter = reportFooter;
            #endregion

            #region Header 
            //ReportSections reportHeader = new ReportSections();
            //reportHeader.Size = new ReportScale() { Height = 0.56849 };

            //ReportItems reportHeaderItems = new ReportItems();

            //ReportTextBoxControl[] headerTxt = new ReportTextBoxControl[]{
            //    new ReportTextBoxControl() { Name = "txtReportTitle"
            //    , ValueOrExpression = new string[] { "Report Name: " + ReportTitle }
            //    , Position = new ReportDimensions(){ Left = 2} }
            //    //new ReportTextBoxControl()
            //    //{
            //    //    Name = "txttesttitle",
            //    //    ValueOrExpression = new string[] { "ทดสอบ: ฟหกดฟหกด" }
            //    //,
            //    //    Position = new ReportDimensions() { Left = 3, Top = 0.7 }
            //    //,
            //    //    Size = new ReportScale() { Height = -1, Width = -1 }
            //    //}
            //};


            //reportHeaderItems.TextBoxControls = headerTxt;


            //reportHeader.ReportControlItems = reportHeaderItems;
            //reportBuilder.Page.ReportHeader = reportHeader;

            #region ImageLogo
            if (!string.IsNullOrEmpty(imageLogo))
            {
                //"~/img/logo.png"
                ReportImage reportItem = new ReportImage() { ImagePath = imageLogo, fullMIMEType = "image/png", ImageFrom = ReportImageSource.Embedded };
                reportItem.Position = new ReportDimensions() { Top = 0, Left = 5 };
                reportItem.Size = new ReportScale() { Height = 0.67, Width = 2 };

                reportBuilder.Logo = reportItem;
            }
            #endregion
            #endregion

            return DownLoadByReportViewer(reportBuilder, ds, "Dynamic", ReportType.EXCEL);


        }
        public static HttpResponseMessage DownLoadByReportViewer(ReportBuilder reportBuilder, System.Data.DataSet ds,string reportName, ReportType type)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            int count = 0;
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            foreach (DataTable dt in ds.Tables)
            {
                count++;
                var report_name = "Report" + count;
                DataTable dt1 = new DataTable(report_name.ToString());
                dt1 = ds.Tables[count - 1];
                dt1.TableName = report_name.ToString();
            }

            using (var reportViewer = new ReportViewer())
            {
                //Report Viewer, Builder and Engine 
                reportViewer.Reset();
                for (int i = 0; i < ds.Tables.Count; i++)
                { 
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(ds.Tables[i].TableName, ds.Tables[i]));
                }

                reportViewer.LocalReport.LoadReportDefinition(ReportEngine.GenerateReport(reportBuilder));
                reportViewer.LocalReport.DisplayName = reportName;
                bytes = reportViewer.LocalReport.Render(type.ToString(), null, out mimeType, out encoding, out extension, out streamids, out warnings);
            }

            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            switch (type)
            {
                default:
                case ReportType.PDF:
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    break;
                case ReportType.EXCEL:
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                    break;
            }
            return result;

        }
    }

    public enum ReportType
    {
        PDF,
        EXCEL
    }
}