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
            return ReadFileFromHandheld(stockHead,true);
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
                    stockHead = checkStockRepo.GetSingle(w => w.CheckStockID == checkStockID);
                    if (stockHead != null)
                    {
                        if(stockHead.Status == CheckStockStatus.InProgress.GetValueEnum())
                            stockHead = this.ReadFileFromHandheld(stockHead, false);
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
                        stockHead = SetComplete(stockHead);
                        stockHead = this.ReadFileFromHandheld(stockHead, false);
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
                        bool isByDate = false,isByStatus = false;
                        for(int i = 0;i < cnt;i++)
                        {
                            if(parameterSearch.Columns[i].ToUpper() == "STARTDATE")
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
                        if(isByDate)
                            items = checkStockRepo.GetMany(f => (f.CreateAt.Value.Date >= startDate.Date && f.CreateAt.Value.Date <= endDate));
                        else if(isByStatus)
                            items = checkStockRepo.GetMany(w => w.Status == CheckStockStatus.InProgress.GetValueEnum()).OrderByDescending(o => o.CreateAt);


                    }
 
                }

            }

            return items;
        }

        private CheckStockHead ReadFileFromHandheld(CheckStockHead stockHead,bool isCreate)
        {
            List<string> items = new List<string>();
            string[] files = Directory.GetFiles(STOCK_DIRECTORY);
            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string extens = Path.GetExtension(files[i]);
                    if (extens.ToUpper() == ".JSON")
                    {
                        var obj = FileHelper.ReadJsonFileToJsonObj<FujiStockHead>(files[i]);
                        if (obj != null)
                        {
                            if(isCreate)
                            {
                                stockHead.CheckStockID = Guid.NewGuid().ToString();
                                stockHead.CheckStockBy = obj.CheckedBy;
                                stockHead.CheckStockDate = obj.CheckedAt;
                            }
                        }
                    }
                    else
                    {
                        items.AddRange(FileHelper.ReadTextFileBySplit(files[i]));
                    }

                }
            }

            if (items.Count > 0)
            {
                using (var scope = new TransactionScope())
                {

                    using (FujiDbContext Db = new FujiDbContext())
                    {
                        try
                        {
                            ISerialDetailRepository serialDetailRepo = new SerialDetailRepository(Db);
                            ICheckStockRepository checkStockRepo = new CheckStockRepository(Db);

                            List<ImportSerialDetail> itemStocks = serialDetailRepo.GetMany(m => items.Contains(m.ItemGroup)
                            && m.ItemType == "1" && m.Status == statusReceived && !m.IsCheckedStock).ToList();
                            itemStocks.ForEach(f =>
                            {
                                if (f != null)
                                {
                                    f.IsCheckedStock = true;
                                }
                            });
                            Db.SaveChanges();

                            int countCheckedStock= itemStocks.Where(w => w.IsCheckedStock).Count();
                            stockHead.SystemQTY = countCheckedStock;
                            stockHead.ActualQTY = serialDetailRepo.GetCountItems(w => w.Status == statusReceived && w.ItemType == "1");
                            
                            if(isCreate)
                            {
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
            }
            return stockHead;
        }

        private CheckStockHead SetComplete(CheckStockHead stockHead)
        {
            if (stockHead == null)
                return stockHead;

            if (stockHead != null)
                if (DateTime.Now.Date <= stockHead.CreateAt)
                    return stockHead;

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





        #endregion

    }
}
