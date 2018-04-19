using WIM.Core.Repository.Impl;
using Fuji.Context;
using Fuji.Entity.StockManagement;
using Fuji.Repository.StockManagement;
using Fuji.Common.ValueObject.CheckStock;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fuji.Entity.ItemManagement;

namespace Fuji.Repository.Impl.StockManagement
{
    public class CheckStockRepository : Repository<CheckStockHead>, ICheckStockRepository
    {
        FujiDbContext Db { get; set; }
        private DbSet<CheckStockHead> DbSet { get; set; }

        public CheckStockRepository(FujiDbContext context) : base(context)
        {
            Db = context;
            this.DbSet = context.Set<CheckStockHead>();
        }

        public FujiStockReportHead GetStockDetailByLocation(FujiStockReportHead checkStockHead)
        {
            //IEnumerable<FujiStockReportDetail> items = new List<FujiStockReportDetail>() ;
            CheckStockHead head = DbSet
                .Where(w => w.Status == CheckStockStatus.InProgress.ToString())
                .FirstOrDefault();

            if(head != null)
            {
                checkStockHead.StockID = head.CheckStockID;
                checkStockHead.CreateAt = head.CheckStockDate.ToString("dd-MM-yyyy");
                checkStockHead.CreateBy = head.CreateBy;
                // checkStockHead.WarehouseCode =

                checkStockHead.Details = (from p in Db.ImportSerialDetail
                         where p.Location == checkStockHead.Location
                         && p.Status == "RECEIVED"
                         orderby p.BoxNumber , p.ItemGroup
                         select new FujiStockReportDetail()
                         {
                             DetailID = p.DetailID
                             , Location = p.Location
                             , ItemCode = p.ItemCode
                             , SerialNumber = p.SerialNumber
                             , BoxNumber = p.BoxNumber
                             , ItemGroup = p.ItemGroup
                             , SystemQty = 1
                             , CountQty = (p.IsCheckedStock == true) ? 1 : 0
                             , CheckedAt = head.CheckStockDate
                         }).ToList();
            }
            return checkStockHead;
        }

        public FujiStockReportHead GetStockHeadByInprogress(FujiStockReportHead checkStockHead)
        {
          
            CheckStockHead head = DbSet
                .Where(w => w.Status == CheckStockStatus.InProgress.ToString())
                .FirstOrDefault();

            if (head != null)
            {
                checkStockHead.StockID = head.CheckStockID;
                checkStockHead.CreateAt = head.CheckStockDate.ToString("dd/MM/yyyy");
                checkStockHead.CreateBy = head.CreateBy;
            }
            return checkStockHead;
        }
    }
}
