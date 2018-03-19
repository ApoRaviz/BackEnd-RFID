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


        public bool ImportCheckStock()
        {
            DateTime d = DateTime.Now;
            if (!Directory.Exists(STOCK_DIRECTORY))
                return false;

            FujiStockHead stockHead = new FujiStockHead();
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
                            stockHead.StockID = Guid.NewGuid().ToString();
                            stockHead.CheckedBy = obj.CheckedBy;
                            stockHead.CheckedAt = obj.CheckedAt;
                        }
                    }
                    else
                    {
                        items = FileHelper.ReadTextFileBySplit(files[i]);
                    }

                }
            }


            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                        IEnumerable<ImportSerialDetail> itemStocks = SerialDetailRepo.GetMany(m => items.Contains(m.ItemGroup));

                        scope.Complete();
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

        public CheckStockHead GetStockHeadByID(string StockID)
        {
            return new CheckStockHead();
            //IEnumerable<ImportSerialHead> items;
            //using (FujiDbContext Db = new FujiDbContext())
            //{
            //    items = (from h in Db.ImportSerialHead
            //             where !h.HeadID.Equals("0") && !h.Status.Equals(statusDeleted)
            //             orderby h.CreateAt descending
            //             select h).Take(50).ToList();
            //}
            //return items;
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
                        new List<CheckStockHead>() {
                            new CheckStockHead(){CheckStockDate=DateTime.Now,ActualQTY=2220,SystemQTY=10000,CreateBy="JEY"}
                        };

                        //items = Db.ProcPagingImportSerialHead(pageIndex, pageSize, out totalRecord);
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
                        for(int i = 0;i < cnt;i++)
                        {
                            if(parameterSearch.Columns[i].ToUpper() == "STARTDATE")
                                DateTime.TryParse(parameterSearch.Keywords[i],out startDate);
                            if (parameterSearch.Columns[i].ToUpper() == "ENDDATE")
                                DateTime.TryParse(parameterSearch.Keywords[i],out endDate);
                        }
                        items = checkStockRepo.GetMany(f => (f.CheckStockDate >= startDate.Date && f.CheckStockDate <= endDate));

                    }
 
                }

            }

            return items;
        }

        #endregion

    }
}
