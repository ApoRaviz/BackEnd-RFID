﻿namespace TMS.Common.ValueObject.Bookings
{
    public class BookingModel
    {
        public byte[] Barcode { get; set; }
        public string BarcodeInfo { get; set; }
        public byte[] QRcode { get; set; }
        public string CusRef { get; set; }
        public string BookingID { get; set; }
        public string PeyerID { get; set; }
        public string DelRoute { get; set; }
        public string Name { get; set; }
        public string SendCompanyName { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string ProvinceCode { get; set; }
        //public virtual  BookingDetail { get; set; }
        public int BoxNumber { get; set; }
        public int TotalBox { get; set; }

    }
}
