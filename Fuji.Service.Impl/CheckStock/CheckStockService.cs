using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Configuration;
using System.Data;
using Fuji.Common.ValueObject;
using Fuji.Service.ItemImport;
using Fuji.Context;
using Fuji.Entity.ItemManagement;
using System.IO;
using Fuji.Repository.Impl.ItemManagement;
using Fuji.Repository.ItemManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Service.Impl.StatusManagement;
using Fuji.Entity.StockManagement;
using Fuji.Repository.StockManagement;
using Fuji.Repository.Impl.StockManagement;
using System.Linq;
using System.Net.Http;
using Microsoft.Reporting.WebForms;
using Fuji.Common.ValueObject.CheckStock;

namespace Fuji.Service.Impl.ItemImport
{
    public class CheckStockService : WIM.Core.Service.Impl.Service, ICheckStockService
    {
        #region connection Settings

        private string connectionString = ConfigurationManager.ConnectionStrings["YUT_FUJI"].ConnectionString;
        #endregion

        private const int _SUBMODULE_ID = 10;

        private string statusNew = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.New.GetValueEnum());
        private string statusReceived = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Received.GetValueEnum());
        private string statusDeleted = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Deleted.GetValueEnum());
        private string statusImpPicking = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.ImpPicking.GetValueEnum());
        private string statusScanned = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Scanned.GetValueEnum());
        private string statusExported = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Exported.GetValueEnum());
        private string statusShipped = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Shipped.GetValueEnum());

        private const string STOCK_DIRECTORY = @"D:\upload\TestCheckStock";

        public CheckStockService()
        {
        }

        #region CheckStock

        public CheckStockHead CreateCheckStockHead()
        {
            DateTime d = DateTime.Now;
            if (!Directory.Exists(STOCK_DIRECTORY))
                return null;
            CheckStockHead stockHead = new CheckStockHead();
            return ReadFileFromHandheld(stockHead, true);
        }

        public bool UpdateCheckStockHead(CheckStockHead checkStockHead)
        {
            if (checkStockHead == null)
                return false;

            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ICheckStockRepository CheckStockRepo = new CheckStockRepository(Db);
                        CheckStockHead item = CheckStockRepo.GetByID(checkStockHead.CheckStockID);
                        if (item != null)
                        {
                            item.Status = checkStockHead.Status;
                            CheckStockRepo.Update(item);
                            Db.SaveChanges();
                            scope.Complete();
                        }

                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        throw new ValidationException(e);
                    }

                }
            }

            return true;
        }

        public CheckStockHead GetStockHeadByID(string checkStockID)
        {
            DateTime d = DateTime.Now;
            if (!Directory.Exists(STOCK_DIRECTORY))
                return null;

            CheckStockHead stockHead;
            using (FujiDbContext Db = new FujiDbContext())
            {
                try
                {
                    ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);
                    stockHead = checkStockRepo.Get(w => w.CheckStockID == checkStockID);
                    if (stockHead != null)
                    {
                        if (stockHead.Status == CheckStockStatus.InProgress.GetValueEnum())
                        {
                            stockHead = this.ReadFileFromHandheld(stockHead, false);
                            stockHead = SetComplete(stockHead);
                        }

                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
            }

            return stockHead;
        }


        public CheckStockHead GetStockHeadByProgress()
        {
            if (!Directory.Exists(STOCK_DIRECTORY))
                return null;

            CheckStockHead stockHead;
            using (FujiDbContext Db = new FujiDbContext())
            {
                try
                {
                    ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);
                    stockHead = checkStockRepo.GetMany(w => w.Status == CheckStockStatus.InProgress.GetValueEnum()).OrderByDescending(d => d.CreateAt).FirstOrDefault();
                    if (stockHead != null)
                    {
                        stockHead = this.ReadFileFromHandheld(stockHead, false);
                        stockHead = SetComplete(stockHead);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
            }

            return stockHead;
        }

        public IEnumerable<CheckStockHead> GetStock(int pageIndex, int pageSize, out int totalRecord)
        {
            DataSet dset = new DataSet();
            totalRecord = 0;
            List<CheckStockHead> items = new List<CheckStockHead>() { };
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    try
                    {

                        items = Db.ProcPagingCheckStock(pageIndex, pageSize, out totalRecord).ToList();
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                        return new List<CheckStockHead>() { };
                    }

                }

            }
            return items;
        }

        public IEnumerable<CheckStockHead> SearchStockBy(ParameterSearch parameterSearch)
        {
            IEnumerable<CheckStockHead> items = new List<CheckStockHead>();

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);

                    if (cnt > 0)
                    {
                        bool isByDate = false, isByStatus = false;
                        for (int i = 0; i < cnt; i++)
                        {
                            if (parameterSearch.Columns[i].ToUpper() == "STARTDATE")
                            {
                                isByDate = DateTime.TryParse(parameterSearch.Keywords[i], out startDate);
                            }
                            else if (parameterSearch.Columns[i].ToUpper() == "ENDDATE")
                            {
                                isByDate = DateTime.TryParse(parameterSearch.Keywords[i], out endDate);
                            }
                            else if (parameterSearch.Columns[i].ToUpper() == "STATUS")
                            {
                                isByStatus = true;
                            }

                        }
                        if (isByDate)
                            items = checkStockRepo.GetMany(f => (f.CreateAt.Value.Date >= startDate.Date && f.CreateAt.Value.Date <= endDate));
                        else if (isByStatus)
                            items = checkStockRepo.GetMany(w => w.Status == CheckStockStatus.InProgress.GetValueEnum()).OrderByDescending(o => o.CreateAt);


                    }

                }

            }

            return items;
        }



        private CheckStockHead ReadFileFromHandheld(CheckStockHead stockHead, bool isCreate)
        {

            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ISerialHeadRepository serialHeadRepo = new SerialHeadRepository(Db);
                        ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                        ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);


                        string[] files = Directory.GetFiles(STOCK_DIRECTORY);
                        if (files.Length > 0)
                        {
                            for (int i = 0; i < files.Length; i++)
                            {
                                List<string> items = new List<string>();
                                items.AddRange(FileHelper.ReadTextFileBySplit(files[i]));

                                List<ImportSerialDetail> itemStocks = serialDetailRepo.GetMany(m => items.Contains(m.ItemGroup)
                                && m.ItemType == "1" && m.Status == statusReceived && !m.IsCheckedStock).ToList();
                                itemStocks.ForEach(f =>
                                {
                                    if (f != null)
                                    {
                                        if (f.Location == null)// Check if location equal null and assign it !
                                        {
                                            var headItem = serialHeadRepo.Get(w => w.HeadID == f.HeadID);
                                            if (headItem != null)
                                                f.Location = headItem.Location;
                                        }

                                        f.IsCheckedStock = true;
                                    }
                                });
                                Db.SaveChanges();
                            }
                        }

                        int countCheckedStock = serialDetailRepo.GetCountItems(w => w.IsCheckedStock);
                        stockHead.SystemQTY = countCheckedStock;
                        stockHead.ActualQTY = serialDetailRepo.GetCountItems(w => w.Status == statusReceived && w.ItemType == "1");

                        if (isCreate)
                        {
                            stockHead.CheckStockID = Guid.NewGuid().ToString();
                            stockHead.CheckStockBy = "";
                            stockHead.CheckStockDate = DateTime.Now;
                            stockHead.Status = CheckStockStatus.InProgress.GetValueEnum();
                            checkStockRepo.Insert(stockHead);
                        }
                        else
                        {
                            checkStockRepo.Update(stockHead);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        throw new ValidationException(e);
                    }
                }
            }

            return stockHead;
        }

        private CheckStockHead SetComplete(CheckStockHead stockHead)
        {
            if (stockHead == null)
                return stockHead;

            if (stockHead != null)
                if (DateTime.Now.Date <= stockHead.CreateAt.Value.Date)
                    return stockHead;

            string[] files = Directory.GetFiles(STOCK_DIRECTORY);
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }

            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                        ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);

                        List<ImportSerialDetail> itemStocks = serialDetailRepo.GetMany(m => m.ItemType == "1"
                        && m.Status == statusReceived
                        && m.IsCheckedStock).ToList();
                        itemStocks.ForEach(f =>
                        {
                            if (f != null)
                            {
                                f.IsCheckedStock = false;
                            }
                        });
                        Db.SaveChanges();

                        stockHead.Status = CheckStockStatus.Completed.GetValueEnum();
                        checkStockRepo.Update(stockHead);
                        Db.SaveChanges();

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        throw new ValidationException(e);
                    }

                }
            }
            return null;
        }







        #endregion

        #region CheckStock Report

        public IEnumerable<FujiStockReportHead> GetStockReportGroup()
        {
            IEnumerable<FujiStockReportHead> items;
            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                        var list = serialDetailRepo.GetMany(s => s.IsCheckedStock);

                        items = (from p in list
                                 where p.IsCheckedStock
                                 group p
                                 by p.Location into g
                                 select new FujiStockReportHead() { Location = g.Key, Qty = g.Count() });

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        throw new ValidationException(e);
                    }
                }
            }
            return items;
        }

        public IEnumerable<ImportSerialDetail> GetStockReportList(string location)
        {
            IEnumerable<ImportSerialDetail> items;
            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        location = location == "No location" ? "" : location;
                        ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                        items = serialDetailRepo.GetMany(s => s.IsCheckedStock
                        && s.Location == location);

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        throw new ValidationException(e);
                    }
                }
            }
            return items;
        }

        public StreamContent GetReportStream(FujiStockReportHead stockReportHead)
        {
            byte[] bytes;
            string[] streamids;
            Warning[] warnings;
            string mimeType, encoding, extension;
            List<FujiStockReportDetail> details = new List<FujiStockReportDetail>();
            //List<FujiDataBarcode> barcodeList = new List<FujiDataBarcode>();
            //List<FujiDataBarcodeDetail> barcodeDetailList = new List<FujiDataBarcodeDetail>();
            //Barcode bc = new Barcode();

            //string barcodeInfo = item.HeadID;
            //byte[] barcodeImage = bc.EncodeToByte(TYPE.CODE128A, barcodeInfo, Color.Black, Color.White, 400, 200);
            //FujiDataBarcode barcode = new FujiDataBarcode(
            //    barcodeImage,
            //    barcodeInfo,
            //    item.WHID,
            //    item.ItemCode,
            //    item.InvoiceNumber,
            //    item.LotNumber,
            //    item.ReceivingDate.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US")),
            //    item.Qty.ToString(),
            //    item.Location);
            //barcodeList.Add(barcode);

            //foreach (var itemDetail in item.ImportSerialDetail)
            //{
            //    FujiDataBarcodeDetail detail = new FujiDataBarcodeDetail(itemDetail.ItemCode,
            //        itemDetail.SerialNumber,
            //        itemDetail.BoxNumber,
            //        itemDetail.ItemGroup);
            //    barcodeDetailList.Add(detail);
            //}


            if (stockReportHead != null)
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);
                        //stockReportHead = checkStockRepo.GetStockHeadByInprogress(stockReportHead);
                        stockReportHead = checkStockRepo.GetStockDetailByLocation(stockReportHead);
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }

            }



            using (var reportViewer = new ReportViewer())
            {
                List<FujiStockReportHead> stockReport = new List<FujiStockReportHead>();
                stockReport.Add(stockReportHead);

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/CheckStockReport.rdlc";

                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "CheckStockHeadDataSet";
                rds1.Value = stockReport;

                ReportDataSource rds2 = new ReportDataSource();
                rds2.Name = "CheckStockDataSet";
                rds2.Value = stockReportHead.Details;


                reportViewer.LocalReport.DataSources.Add(rds1);
                reportViewer.LocalReport.DataSources.Add(rds2);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }

            Stream stream = new MemoryStream(bytes);
            return new StreamContent(stream);

        }

        #endregion


        #region Handy

        public int HandyGetStatus()
        {
            int status = 0;
            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);
                        var item = checkStockRepo.Get(w => w.Status == CheckStockStatus.InProgress.GetValueEnum());
                        if (item != null)
                            status = 1;
                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        status = 0;
                        throw new ValidationException(e);
                    }
                }
            }
            return status;
        }

        public int UpdateCheckStockByHandy(FujiCheckStockHandy checkStock)
        {
            int status = 0;

            if (checkStock == null)
                return status;

            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                        ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);

                        var item = checkStockRepo.Get(w => w.Status == CheckStockStatus.InProgress.GetValueEnum());
                        if (item != null)
                        {

                            List<ImportSerialDetail> itemStocks = serialDetailRepo.GetMany(m => checkStock.RFIDTags.Contains(m.ItemGroup)
                                        && m.ItemType == "1"
                                        && m.Status == statusReceived
                                        && !m.IsCheckedStock
                                        && m.Location.Equals(checkStock.Location, StringComparison.InvariantCultureIgnoreCase)).ToList();
                            itemStocks.ForEach(f =>
                            {
                                if (f != null)
                                {
                                    f.IsCheckedStock = true;
                                }
                            });
                            Db.SaveChanges();

                            var stockHead = checkStockRepo.Get(g => g.Status == CheckStockStatus.InProgress.GetValueEnum());
                            if (stockHead != null)
                            {
                                int countCheckedStock = serialDetailRepo.GetCountItems(w => w.Status == statusReceived
                                                                                        && w.IsCheckedStock
                                                                                        && w.ItemType == "1");
                                stockHead.SystemQTY = countCheckedStock;
                                stockHead.ActualQTY = serialDetailRepo.GetCountItems(w => w.Status == statusReceived
                                                                                        && w.ItemType == "1");
                                checkStockRepo.Update(stockHead);
                                Db.SaveChanges();
                                status = 1;
                            }
                            scope.Complete();
                        }



                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        status = 0;
                        throw new ValidationException(e);
                    }
                }
            }

            return status;
        }


        #endregion
    }
}
