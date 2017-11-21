using System.Collections.Generic;
using System.Net.Http;
using BarcodeLib;
using QRCoder;
using System.IO;
using TMS.Common.ValueObject.Labels;
using TMS.Service.Label;
using Microsoft.Reporting.WebForms;
using System.Drawing;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace TMS.Service.Impl.Label
{
    public class LabelService : WIM.Core.Service.Impl.Service, ILabelService
    {

        string YtdmUrl;
        public LabelService()
        {
            YtdmUrl = ConfigurationManager.AppSettings["as:YtdmUrl"];
        }

        public List<GroupDateImportBookingModel> GetGroupImportBooking()
        {

            WebClient oWeb = new WebClient();
            string url = YtdmUrl + "pro_booking/proc/booking_manager.json.php";
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
            parameters.Add("action", "GetGroupDateImportBooking");
            var responseBytes = oWeb.UploadValues(url, parameters);
            string json = Encoding.ASCII.GetString(responseBytes);
            List<GroupDateImportBookingModel> res = JsonConvert.DeserializeObject<List<GroupDateImportBookingModel>>(json);
            return res;

        }


        public List<BoxLabelBookingModel> GetDataImportBookingByDate(GroupDateImportBookingModel param)
        {
            WebClient oWeb = new WebClient();
            string url = YtdmUrl + "pro_booking/proc/booking_manager.json.php";
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
            parameters.Add("date_import", param.DateImport.ToString("yyyy-MM-dd hh:mm:ss"));
            parameters.Add("user_id", param.UserID);
            parameters.Add("action", "GetBookingByCustomer");
            var responseBytes = oWeb.UploadValues(url, parameters);
            string json = Encoding.ASCII.GetString(responseBytes);
            List<BoxLabelBookingModel> res = JsonConvert.DeserializeObject<List<BoxLabelBookingModel>>(json);
            return res;
        }

        public StreamContent GetBoxLabelBookingStream(string[] BookingIDs)
        {
            byte[] bytes;
            string[] streamids;
            Warning[] warnings;
            string mimeType, encoding, extension;
            List<BoxLabelBookingModel> DataList = new List<BoxLabelBookingModel>();
            Barcode bc = new Barcode();

            WebClient oWeb = new WebClient();
            string url = YtdmUrl + "pro_booking/proc/booking_manager.json.php";
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("auth", "asAdfreytwasdSSwersdf00SSd98632er");
            parameters.Add("action", "GetBookingByID");
            string StrBooking = string.Join(",", BookingIDs);
            parameters.Add("booking", StrBooking);
            var responseBytes = oWeb.UploadValues(url, parameters);
            string json = Encoding.ASCII.GetString(responseBytes);
            List<BoxLabelBookingModel> Bookings = JsonConvert.DeserializeObject<List<BoxLabelBookingModel>>(json);

            string barcodeStr = "";

            foreach (BoxLabelBookingModel Booking in Bookings)
            {
                int j = 0;
                int total = Booking.TotalBox;
                BoxLabelBookingModel b = new BoxLabelBookingModel();
                for (int i = 0; i < total; i++)
                {
                    BoxLabelBookingModel BookingNew = ;
                    barcodeStr = Booking.BookingID + " " + (i) + " " + Booking.TotalBox + " " + Booking.DelRoute;
                    BookingNew.BarcodeInfo = barcodeStr;
                    BookingNew.BoxNumber = i;
                    BookingNew.Barcode = bc.EncodeToByte(TYPE.CODE128, barcodeStr, Color.Black, Color.White, 900, 96);
                    BookingNew.QRcode = GenerateQrCodeToByte(barcodeStr);// bc.EncodeToByte(TYPE.Q, BoxLabelBooking.BookingID, Color.Black, Color.White, 587, 96);
                    DataList.CopyTo(BookingNew);




                }
                j++;
            }

            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Labels/BoxLabelBooking.rdlc";
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                //reportViewer.Width = 384;
                //reportViewer.Height = 384;
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataList";
                rds.Value = DataList;
                reportViewer.LocalReport.DataSources.Add(rds);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }

            MemoryStream stream = new MemoryStream(bytes);
            return new StreamContent(stream);
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

    }
}
