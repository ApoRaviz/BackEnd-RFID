using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using WIM.Core.Repository;
using Fuji.Common.ValueObject;
using WIM.Core.Common.Helpers;
using Fuji.Service.ItemImport;
using WIM.Core.Repository.Impl;
using Fuji.Context;
using Fuji.Entity.ItemManagement;
using System.Net.Http;
using System.Drawing;
using BarcodeLib;
using Microsoft.Reporting.WebForms;
using System.IO;
using Fuji.Repository.Impl.ItemManagement;
using System.Security.Principal;
using Fuji.Repository.ItemManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using System.Runtime.Caching;
using WIM.Core.Entity.Logs;
using System.Web.Script.Serialization;
using WIM.Core.Service.Impl.StatusManagement;
using System.Data.Linq.SqlClient;

namespace Fuji.Service.Impl.ItemImport
{
    public class ItemImportService : WIM.Core.Service.Impl.Service, IItemImportService
    {
        #region connection Settings

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["YUT_FUJI"].ConnectionString;
        #endregion

        private const int _SUBMODULE_ID = 10;

        private readonly string statusNew = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.New.GetValueEnum());
        private readonly string statusReceived = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Received.GetValueEnum());
        private readonly string statusDeleted = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Deleted.GetValueEnum());
        private readonly string statusImpPicking = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.ImpPicking.GetValueEnum());
        private readonly string statusScanned = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Scanned.GetValueEnum());
        private readonly string statusExported = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Exported.GetValueEnum());
        private readonly string statusShipped = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Shipped.GetValueEnum());


        public ItemImportService()
        {

        }

        public IEnumerable<ImportSerialHead> GetItems()
        {
            IEnumerable<ImportSerialHead> items;
            using (FujiDbContext Db = new FujiDbContext())
            {
                items = (from h in Db.ImportSerialHead
                         where !h.HeadID.Equals("0") && !h.Status.Equals(statusDeleted)
                         orderby h.CreateAt descending
                         select h).Take(50).ToList();
            }
            return items;
        }

        public IEnumerable<ImportSerialHead> GetItems(int pageIndex, int pageSize, out int totalRecord)
        {
            DataSet dset = new DataSet();
            totalRecord = 0;
            IEnumerable<ImportSerialHead> items = new List<ImportSerialHead>() { };
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    try
                    {
                        items = Db.ProcPagingImportSerialHead(pageIndex, pageSize, out totalRecord);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                        return new List<ImportSerialHead>() { };
                    }

                }

            }
            return items;
        }

        #region Handy
        public void DeleteItem(string id)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    //Db.ProcDeleteImportSerial(id);
                    ImportSerialHead importSerialHead = (
                        from h in Db.ImportSerialHead
                        where h.HeadID == id
                        select h
                    ).SingleOrDefault();

                    try
                    {
                        if (importSerialHead != null)
                        {
                            importSerialHead.Status = statusDeleted;
                            SerialHeadRepo.Update(importSerialHead);
                            Db.SaveChanges();
                        }

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
        }

        public ItemImportDto GetItemByDocID_Handy(string id)
        {
            ItemImportDto itemHead;
            using (FujiDbContext Db = new FujiDbContext())
            {
                itemHead = (from h in Db.ImportSerialHead
                            where h.HeadID == id
                            select new ItemImportDto
                            {
                                HeadID = h.HeadID,
                                ItemCode = h.ItemCode,
                                Qty = h.Qty
                            }).SingleOrDefault();


                if (itemHead == null)
                {
                    throw new AppValidationException(ErrorEnum.DATA_NOT_FOUND);
                }
            }
            return itemHead;
        }
        private void RemoveRegisterDuplicate()
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    //, ItemGroup, BoxNumber, ItemType
                    SerialDetailRepo.ExceuteSql(@"
                    delete from dbo.ImportSerialDetail where DetailID in (
	                    select DetailID from (
		                    select 
		                    ROW_NUMBER() OVER(PARTITION BY SerialNumber ORDER BY CreateAt DESC) AS Row  
		                    ,DetailID
		                    from dbo.ImportSerialDetail
		                    where HeadID = '0' and Status = 'NEW'
		                    ) a
	                    where Row > 1
                    )
                    delete from dbo.ImportSerialDetail where DetailID in (
                         select DetailID from (
                              select 
                              ROW_NUMBER() OVER(PARTITION BY itemGroup, itemType ORDER BY CreateAt DESC) AS Row  
                              ,DetailID
                              from dbo.ImportSerialDetail
                              where HeadID = '0' and Status = 'NEW'
                              ) a
                         where Row > 1
                    )

                ");

                    scope.Complete();
                }
            }
        }
        public void ReGenerateRFID(List<string> itemGroupsFromScan, string headId)
        {

            using (var scope = new TransactionScope())
            {
                using (var Db = new FujiDbContext())
                {
                    var query = (from d in Db.ImportSerialDetail
                                 where (d.HeadID == "0" && d.Status == statusNew)
                                 || (d.HeadID == headId && d.Status == statusScanned)
                                 select d
                        );

                    foreach (ImportSerialDetail detail in query)
                    {
                        foreach (string scan in itemGroupsFromScan)
                        {
                            if (scan.EndsWith(detail.ItemGroup))
                            {
                                detail.ItemGroup = scan;
                            }
                        }
                    }

                    try
                    {
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }

        }

        public void InsertItemScanLatests(ImportSerialHead head, IEnumerable<ImportSerialDetail> items)
        {
            using (var db = new FujiDbContext())
            {
                var existItems = db.ItemScanLastests.ToList();
                db.ItemScanLastests.RemoveRange(existItems);

                var itemScans = new List<ItemScanLastest>();
                foreach (var item in items)
                {
                    var itemScanLastest = new ItemScanLastest()
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeadID = head.HeadID,
                        ItemCode = head.ItemCode,
                        BoxNumber = item.BoxNumber,
                        ItemGroup = item.ItemGroup,

                        CreateAt = DateTime.Now,
                        CreateBy = "SYSTEM",
                        UpdateAt = DateTime.Now,
                        UpdateBy = "SYSTEM"
                    };
                    itemScans.Add(itemScanLastest);
                }
                db.ItemScanLastests.AddRange(itemScans);
                db.SaveChanges();
            }
        }


        public bool SetScanned(SetScannedRequest req)
        {
            RemoveRegisterDuplicate();

            using (var db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(db);
                ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(db);

                var importHead = (from h in db.ImportSerialHead
                                  where h.HeadID == req.HeadID
                                  select h
                                       ).SingleOrDefault();

                if (importHead == null)
                {
                    throw new AppValidationException(ErrorEnum.DATA_NOT_FOUND);
                }


                var existItems = (from d in db.ImportSerialDetail
                                  where
                                     (
                                         (d.HeadID == "0" && d.Status == statusNew)
                                         || (d.HeadID == req.HeadID && d.Status == statusScanned)
                                      )
                                  select d
                ).ToList();


                var items = MapFormScan(existItems, req.ItemGroups, true);


                InsertItemScanLatests(importHead, items);


                var itemGroupCount = (from p in items
                                      group p by p.ItemGroup into g
                                      select new { ItemGroup = g.Key, Items = g.ToList() }).Count();


                if (itemGroupCount != importHead.Qty)
                {
                    throw new AppValidationException(new ValidationError("50101", "Head ไม่เท่ากับที่ Scan รับ"));
                }

                foreach (var item in items)
                {
                    var existItemDupplicated = db.ImportSerialDetail.FirstOrDefault(x =>
                        x.ItemCode == importHead.ItemCode
                        && x.SerialNumber == item.SerialNumber
                        && x.Status == statusReceived
                    );
                    if (existItemDupplicated != null)
                    {
                        throw new AppValidationException(ErrorEnum.SerialAndItemCodeAlreadyExist
                            , $"มี {existItemDupplicated.ItemCode}/{existItemDupplicated.SerialNumber}ซ้ำอยู่ที่{existItemDupplicated.Location}/{existItemDupplicated.BoxNumber}");
                    }
                }

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        importHead.Status = statusScanned;
                        SerialHeadRepo.Update(importHead);

                        foreach (var item in items)
                        {
                            item.HeadID = importHead.HeadID;
                            item.ItemCode = importHead.ItemCode;
                            item.Status = statusScanned;
                            SerialDetailRepo.Update(item);
                        }

                        db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }

        }
        public bool Receive(ReceiveRequest req)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(db);
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(db);
                    ISerialDetailTempRepository SerialDetailTempRepo = new SerialDetailTempRepository(db);


                    var query = (from d in db.ImportSerialDetail
                                 where
                                 d.HeadID == req.HeadID
                                 && d.Status == statusScanned
                                 select d
                            ).ToList();

                    // ทดลองปิด
                    //query.ForEach(f =>
                    //{
                    //    var item = TranslateDetailTemp(f);
                    //    if (item != null)
                    //        SerialDetailTempRepo.Insert(item);
                    //});

                    //try
                    //{
                    //    db.SaveChanges();
                    //}
                    //catch (DbEntityValidationException e)
                    //{
                    //    throw new AppValidationException(e);
                    //}
                    //catch (Exception)
                    //{
                    //    throw new AppValidationException(ErrorEnum.UNKNOWN_ERROR);
                    //}

                    foreach (var detail in query)
                    {
                        detail.Status = statusReceived;
                        detail.Location = req.LocationID;
                        SerialDetailRepo.Update(detail);
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                    catch (Exception)
                    {
                        throw new AppValidationException(ErrorEnum.UNKNOWN_ERROR);
                    }

                    ImportSerialHead importHead = (from h in db.ImportSerialHead
                                                   where h.HeadID == req.HeadID
                                                   select h
                                ).SingleOrDefault();

                    importHead.Status = statusReceived;
                    importHead.Location = req.LocationID;
                    SerialHeadRepo.Update(importHead);

                    try
                    {
                        db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (Exception)
                    {
                        throw new AppValidationException(ErrorEnum.UNKNOWN_ERROR);
                    }
                }
            }
        }
        public List<string> GetItemGroupByOrderNo_Handy(string orderNo)
        {
            List<string> itemGroups;
            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                var items = SerialDetailRepo.GetMany(d =>
                    d.OrderNo == orderNo
                    && d.Status == statusImpPicking
                );

                itemGroups = (from d in items
                              group d by d.ItemGroup into g
                              select g.Key
                         ).ToList();

                if (!itemGroups.Any())
                {
                    throw new AppValidationException(ErrorEnum.DATA_NOT_FOUND);
                }
            }

            return itemGroups;
        }
        public bool ConfirmPicking(ConfirmPickingRequest confirmRequest)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    var existItems = (from d in Db.ImportSerialDetail
                                      where
                                      d.OrderNo == confirmRequest.OrderNumber
                                      && d.Status == statusImpPicking
                                      select d).ToList();



                    try
                    {
                        var items = MapFormScan(existItems, confirmRequest.ItemGroups, false);
                        foreach (ImportSerialDetail item in items)
                        {
                            item.Status = statusShipped;
                            SerialDetailRepo.Update(item);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }

            }
        }

        private ImportSerialDetailTemp TranslateDetailTemp(ImportSerialDetail detail)
        {
            ImportSerialDetailTemp item = new ImportSerialDetailTemp();
            if (detail != null)
            {
                item.HeadID = detail.HeadID;
                item.BoxNumber = detail.BoxNumber;
                item.CreateAt = detail.CreateAt;
                item.CreateBy = detail.CreateBy;
                item.DetailID = detail.DetailID;
                item.IsActive = detail.IsActive;
                item.ItemCode = detail.ItemCode;
                item.ItemGroup = detail.ItemGroup;
                item.ItemType = detail.ItemType;
                item.OrderNo = detail.OrderNo;
                item.SerialNumber = detail.SerialNumber;
                item.Status = detail.Status;
                item.UpdateAt = detail.UpdateAt;
                item.UpdateBy = detail.UpdateBy;
            }
            return item;
        }
        public bool RegisterRFID_HANDY(RegisterRFIDRequest registerRequest)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    int itemType = 0;
                    try
                    {
                        foreach (var serial in registerRequest.SerialNumbers)
                        {
                            ImportSerialDetail item = new ImportSerialDetail
                            {
                                DetailID = Guid.NewGuid().ToString(),
                                HeadID = "0",
                                SerialNumber = serial,
                                BoxNumber = registerRequest.BoxNumber,
                                ItemGroup = registerRequest.RFIDTag,
                                ItemType = (++itemType).ToString(),
                                Status = statusNew
                            };
                            //Db.ImportSerialDetail.Add(item);
                            SerialDetailRepo.Insert(item);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
        }
        public int SetSerial2Box(string boxNumberFrom, string boxNumberTo, ItemGroupRequest ItemGroup, string emID)
        {
            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);

                var query = SerialDetailRepo.GetMany(p => p.BoxNumber == boxNumberFrom);

                foreach (ImportSerialDetail detail in query)
                {
                    foreach (string scan in ItemGroup.ItemGroups)
                    {
                        if (detail.ItemGroup.EndsWith(scan))
                        {
                            detail.BoxNumber = boxNumberTo;
                            SerialDetailRepo.Update(detail);
                            Db.SaveChanges();
                        }
                    }
                }
            }

            return 1;
        }

        public int SetBox2Location(string locationTo, ItemGroupRequest boxList, string emID)
        {
            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);

                var query = SerialDetailRepo.GetMany(i => boxList.ItemGroups.Contains(i.BoxNumber));

                foreach (ImportSerialDetail detail in query)
                {
                    detail.Location = locationTo;
                    SerialDetailRepo.Update(detail);
                    Db.SaveChanges();
                }
            }
            return 1;
        }
        #endregion

        #region Default
        public ImportSerialHead GetItemByDocID(string id, bool isIncludeChild = false)
        {
            ImportSerialHead headItem;
            using (FujiDbContext Db = new FujiDbContext())
            {
                headItem = (from h in Db.ImportSerialHead
                            where h.HeadID == id
                            select h).FirstOrDefault();
                if (isIncludeChild)
                    Db.Entry(headItem).Collection(c => c.ImportSerialDetail).Load();
            }
            return headItem;
        }
        public ImportSerialHead CreateItem(ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    try
                    {
                        //item.HeadID = Db.ProcGetNewID("IS").FirstOrDefault();
                        item.HeadID = Db.ProcGetNewID("IS");
                        item.Status = statusNew;
                        item.IsExport = false;
                        item.Location = "";
                        item.ReceivingDate = item.ReceivingDate;
                        SerialHeadRepo.Insert(item);
                        Db.SaveChanges();

                        var insertedItem = SerialHeadRepo.GetItemBy(b => b.HeadID == item.HeadID, true);
                        if (insertedItem != null)
                        {
                            if (item.ImportSerialDetail.Any())
                            {
                                foreach (var detail in item.ImportSerialDetail)
                                {
                                    detail.HeadID = item.HeadID;
                                    detail.DetailID = Guid.NewGuid().ToString();
                                    SerialDetailRepo.Update(detail);
                                    Db.SaveChanges();
                                    //detailRepo.Insert(detail);
                                }
                            }
                        }

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }

                }

            }
            return item;
        }
        public bool UpdateItem(string id, ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {

                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    try
                    {
                        var queryUpdateHead = SerialHeadRepo.GetItemsBy(p => p.HeadID == id).ToList();
                        queryUpdateHead.ForEach(f =>
                        {
                            f.ItemCode = item.ItemCode;
                            f.WHID = item.WHID;
                            f.LotNumber = item.LotNumber;
                            f.InvoiceNumber = item.InvoiceNumber;
                            f.ReceivingDate = item.ReceivingDate;
                            f.DeliveryNote = item.DeliveryNote;
                            f.Remark = item.Remark;
                            f.Qty = item.Qty;
                            f.Spare1 = item.Spare1;
                            f.Spare2 = item.Spare2;
                            //existedItem.Spare3 = item.Spare3;
                            f.Spare4 = item.Spare4;
                            f.Spare5 = item.Spare5;
                            f.Spare6 = item.Spare6;
                            f.Spare7 = item.Spare7;
                            f.Spare8 = item.Spare8;
                            f.Spare9 = item.Spare9;
                            f.Spare10 = item.Spare10;
                            SerialHeadRepo.Update(f);
                            Db.SaveChanges();
                        });

                        var updatedItem = SerialHeadRepo.GetItemBy(f => f.HeadID == item.HeadID, true);
                        if (updatedItem != null)
                        {
                            if (updatedItem.ImportSerialDetail.Any())
                            {
                                //IGenericRepository<ImportSerialDetail> detailRepo = new GenericRepository<ImportSerialDetail>(Db);
                                //Db.ProcDeleteImportSerialDetail(item.HeadID);

                                IEnumerable<ImportSerialDetail> _existDetails = (from d in Db.ImportSerialDetail
                                                                                 where d.HeadID == item.HeadID
                                                                                 select d).ToList();

                                Db.ImportSerialDetail.RemoveRange(_existDetails);


                                foreach (var detail in updatedItem.ImportSerialDetail)
                                {
                                    detail.HeadID = updatedItem.HeadID;
                                    detail.ItemCode = updatedItem.ItemCode;
                                    detail.DetailID = Guid.NewGuid().ToString();
                                    SerialDetailRepo.Insert(detail);
                                    Db.SaveChanges();
                                }
                            }
                        }


                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }

                }

            }
            return true;
        }
        public bool UpdateStausExport(ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext("UpdateStausExport(ImportSerialHead)"))
                {

                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);

                    List<ImportSerialHead> queryUpdateHead = (from p in Db.ImportSerialHead
                                                              where p.HeadID.Equals(item.HeadID)
                                                              select p).ToList();

                    try
                    {
                        queryUpdateHead.ForEach(f =>
                        {
                            f.IsExport = true;
                            SerialHeadRepo.Update(f);
                        });
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }

                }

            }
            return true;
        }
        public IEnumerable<ImportSerialDetail> UpdateStatus(List<PickingRequest> pickingList)
        {
            List<string> itemCodes = pickingList.Select(x => x.ItemCode).ToList();
            List<string> serialNumbers = pickingList.Select(x => x.SerialNumber).ToList();
            List<ImportSerialDetail> returnValue = new List<ImportSerialDetail>();

            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);

                    List<ImportSerialDetail> queryDetailsList = new List<ImportSerialDetail>() { };
                    for (int i = 0; i < itemCodes.Count(); i++)
                    {
                        string nItemCode = itemCodes[i];
                        string nSerialNumber = serialNumbers[i];
                        var qDetailItem = (from d in Db.ImportSerialDetail
                                           where d.ItemCode.Equals(nItemCode)
                                           && d.SerialNumber.Equals(nSerialNumber)
                                           select d).FirstOrDefault();

                        if (qDetailItem != null)
                            queryDetailsList.Add(qDetailItem);
                    }

                    returnValue = queryDetailsList;

                    List<string> itemGroupsList = new List<string>();
                    foreach (ImportSerialDetail detail in queryDetailsList)
                    {
                        itemGroupsList.Add(detail.ItemGroup);
                    }

                    //update importDetail
                    var updateSerialDetailStatus = Db.ImportSerialDetail.Where(x => itemGroupsList.Contains(x.ItemGroup)
                    && x.Status == statusReceived).ToList();

                    try
                    {
                        updateSerialDetailStatus.ForEach(a =>
                        {
                            a.Status = statusImpPicking;
                            a.OrderNo = pickingList.First().OrderNumber;
                            SerialDetailRepo.Update(a);
                        });

                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }


                    //update importHead
                    /*if (!Db.ImportSerialDetails.Any(a => a.Status != "Completed"))
                    {
                        var updateImportSerialHead = Db.ImportSerialHeads.ToList();
                        updateImportSerialHead.ForEach(a =>
                        {
                            a.Status = "Completed";
                            a.UpdateDate = DateTime.Now;
                            a.UserUpdate = userUpdate;
                        });

                        try
                        {
                            Db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            throw new ValidationException(e);
                        }
                    }*/


                }

            }
            return returnValue;
        }
        public IEnumerable<FujiPickingGroup> GetPickingGroup(int max = 50)
        {
            List<FujiPickingGroup> items = new List<FujiPickingGroup>();
            using (FujiDbContext Db = new FujiDbContext())
            {
                var selectedData = (from p in Db.ImportSerialDetail where p.Status == statusImpPicking select new { p.OrderNo, p.UpdateAt }).ToList();


                var itemGroups = (from p in selectedData
                                  orderby p.UpdateAt descending
                                  group p
                                  by p.OrderNo into g
                                  select new { GroupID = g.Key, GroupList = g.ToList() }).Take(max).ToList();


                itemGroups.ForEach(f =>
                {
                    FujiPickingGroup item = new FujiPickingGroup(f.GroupID, f.GroupList.Count(), new List<ImportSerialDetail>() { });

                    if (!string.IsNullOrEmpty(f.GroupID))
                        items.Add(item);
                });
            }

            return items;
        }
        public FujiPickingGroup GetPickingByOrderNo(string orderNo, bool isAddItem = false)
        {
            FujiPickingGroup retItem;
            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                var itemGroups = (from p in SerialDetailRepo
                                  .GetItemsBy(p => p.OrderNo != null && p.OrderNo.Contains(orderNo))
                                  orderby p.CreateAt descending
                                  group p
                                  by p.OrderNo into g
                                  select new { GroupID = g.Key, GroupList = g.ToList() }).ToList();

                var item = itemGroups.SingleOrDefault();
                if (item == null)
                {
                    throw new AppValidationException(ErrorEnum.DATA_NOT_FOUND);
                }
                if (isAddItem)
                    retItem = new FujiPickingGroup(item.GroupID, item.GroupList.Count(), item.GroupList);
                else
                    retItem = new FujiPickingGroup(item.GroupID, item.GroupList.Count(), new List<ImportSerialDetail>() { });
            }
            return retItem;
        }
        public bool ClearPickingGroup(string orderID)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    var itemGroups = (from p in Db.ImportSerialDetail
                                      where p.OrderNo.Equals(orderID)
                                      select p).ToList();

                    try
                    {
                        itemGroups.ForEach(f =>
                        {
                            f.OrderNo = null;
                            f.Status = statusReceived;
                            SerialDetailRepo.Update(f);
                        });

                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }

                }

            }
            return true;
        }
        public string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            conditionColumnNames = conditionColumnNames.Replace("ReceivingDate", "convert(varchar(50),ReceivingDate,121)");
            //return Db.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword).FirstOrDefault();
            return "";
        }
        public IEnumerable<ImportSerialHead> GetDataByColumn(ParameterSearch parameterSearch, out int totalRecord)
        {
            totalRecord = 0;
            string sql = "SELECT * FROM [dbo].[ImportSerialHead]";

            IEnumerable<ImportSerialHead> items = new List<ImportSerialHead>();

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;

            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);

                    if (cnt > 0)
                    {
                        sql += " WHERE ";
                        for (int i = 0; i < cnt; i++)
                        {
                            if (parameterSearch.Columns[i] == "CreateAt")
                                sql += string.Format(" {0} = '{1}' AND ", "CONVERT(DATE," + parameterSearch.Columns[i] + ")", parameterSearch.Keywords[i]);
                            else
                                sql += string.Format(" {0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                        }
                        sql = sql.Substring(0, sql.Length - 4);
                        sql += " AND [Status] <> 'DELETED'";

                        items = SerialHeadRepo.SqlQuery<ImportSerialHead>(sql).ToList();
                        totalRecord = items.Count();
                    }
                    else
                    {
                        items = Db.ProcPagingImportSerialHead(parameterSearch.PageIndex, parameterSearch.PageSize, out totalRecord);
                    }
                }

            }

            return items;

        }
        public StreamContent GetReportStream(ImportSerialHead item)
        {
            byte[] bytes;
            List<FujiDataBarcode> barcodeList = new List<FujiDataBarcode>();
            List<FujiDataBarcodeDetail> barcodeDetailList = new List<FujiDataBarcodeDetail>();
            Barcode bc = new Barcode();

            string barcodeInfo = item.HeadID;
            byte[] barcodeImage = bc.EncodeToByte(TYPE.CODE128A, barcodeInfo, Color.Black, Color.White, 400, 200);
            FujiDataBarcode barcode = new FujiDataBarcode(
                barcodeImage,
                barcodeInfo,
                item.WHID,
                item.ItemCode,
                item.InvoiceNumber,
                item.LotNumber,
                item.ReceivingDate.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-US")),
                item.Qty.ToString(),
                item.Location);
            barcodeList.Add(barcode);

            foreach (var itemDetail in item.ImportSerialDetail)
            {
                FujiDataBarcodeDetail detail = new FujiDataBarcodeDetail(itemDetail.ItemCode,
                    itemDetail.SerialNumber,
                    itemDetail.BoxNumber,
                    itemDetail.ItemGroup);
                barcodeDetailList.Add(detail);
            }


            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/GenerateHeaderReport.rdlc";

                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds1 = new ReportDataSource
                {
                    Name = "DataSet1",
                    Value = barcodeList
                };

                ReportDataSource rds2 = new ReportDataSource
                {
                    Name = "DataSet2",
                    Value = barcodeDetailList
                };


                reportViewer.LocalReport.DataSources.Add(rds1);
                reportViewer.LocalReport.DataSources.Add(rds2);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out string mimeType, out string encoding, out string extension, out string[] streamids, out Warning[] warnings);

            }

            Stream stream = new MemoryStream(bytes);
            return new StreamContent(stream);

        }

        public IEnumerable<ImportSerialDetail> FindImportSerialDetailByCriteria(ParameterSearch parameterSearch, out int totalRecord)
        {
            totalRecord = 0;
            string sql = "SELECT * FROM [dbo].[ImportSerialDetail]";

            IEnumerable<ImportSerialDetail> items = new List<ImportSerialDetail>();

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;

            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);

                if (cnt > 0)
                {
                    if (parameterSearch.Columns.Any(a => a.Contains("InvoiceNumber")))
                    {
                        sql = "SELECT B.* FROM [WIM_FUJI_DEV].[dbo].[ImportSerialHead] AS A INNER JOIN [WIM_FUJI_DEV].[dbo].[ImportSerialDetail] AS B ON A.HeadID = B.HeadID WHERE ";
                        for (int i = 0; i < cnt; i++)
                        {
                            if (parameterSearch.Columns[i] == "InvoiceNumber")
                                sql += string.Format(" A.{0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                            else if (parameterSearch.Columns[i] == "CreateAt")
                                sql += string.Format(" {0} = '{1}' AND ", "CONVERT(DATE,B." + parameterSearch.Columns[i] + ")", parameterSearch.Keywords[i]);
                            else
                                sql += string.Format(" B.{0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                        }
                        sql = sql.Substring(0, sql.Length - 4);
                        sql += " AND B.[Status] <> 'DELETED'";

                    }
                    else
                    {
                        sql += " WHERE ";
                        for (int i = 0; i < cnt; i++)
                        {
                            if (parameterSearch.Columns[i] == "CreateAt")
                                sql += string.Format(" {0} = '{1}' AND ", "CONVERT(DATE," + parameterSearch.Columns[i] + ")", parameterSearch.Keywords[i]);
                            else
                                sql += string.Format(" {0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                        }
                        sql = sql.Substring(0, sql.Length - 4);
                        sql += " AND [Status] <> 'DELETED'";



                    }
                    items = SerialDetailRepo.SqlQuery<ImportSerialDetail>(sql).ToList();
                    totalRecord = items.Count();
                }
                else
                {
                    items = Db.ProcPagingImportSerialDetail(parameterSearch.PageIndex, parameterSearch.PageSize, out totalRecord);
                }
            }

            return items;
        }
        public string GetRFIDInfo(ParameterSearch parameter)
        {
            string specialQuery = "";
            if (parameter != null)
                if (!string.IsNullOrEmpty(parameter.SpeacialQuery))
                    specialQuery = parameter.SpeacialQuery;

            using (FujiDbContext Db = new FujiDbContext())
            {
                return Db.ProcGetRFIDInfo(specialQuery);
            }
        }
        public IEnumerable<FujiBoxNumberAndAmountModel> GetBoxNumberAndAmountList(ParameterSearch parameterSearch)
        {
            IEnumerable<FujiBoxNumberAndAmountModel> boxes = new List<FujiBoxNumberAndAmountModel>();
            using (FujiDbContext Db = new FujiDbContext())
            {
                ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                string sql = @" select BoxNumber, count(*) AS 'Amount'
                from dbo.ImportSerialDetail
                where status = 'NEW' 
                group by BoxNumber ";

                if (parameterSearch != null && !string.IsNullOrEmpty(parameterSearch.SpeacialQuery))
                {
                    sql += " having count(*) " + parameterSearch.SpeacialQuery;
                }

                sql += " order by BoxNumber ";
                boxes = SerialDetailRepo.SqlQuery<FujiBoxNumberAndAmountModel>(sql).ToList();
            }
            return boxes;
        }
        public IEnumerable<FujiSerialAndRFIDModel> GetItemsInBoxNumber(string boxNumber)
        {
            if (string.IsNullOrEmpty(boxNumber))
            {
                return null;
            }

            IEnumerable<FujiSerialAndRFIDModel> items = new List<FujiSerialAndRFIDModel>();
            using (FujiDbContext Db = new FujiDbContext())
            {
                try
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);

                    items = SerialDetailRepo.GetMany(g => g.BoxNumber == boxNumber)
                        .OrderBy(o => o.SerialNumber)
                        .Select(s => new FujiSerialAndRFIDModel()
                        {
                            SerialNumber = s.SerialNumber,
                            IsValid = true,
                            RFIDTag = ConvertRFID(s.ItemGroup)
                        }).ToList();


                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
            }

            return items;
        }

        private string ConvertRFID(string hexRFID)
        {
            var bin18 = UtilityHelper.H2B(hexRFID);            
            return UtilityHelper.B2D(bin18.Substring(bin18.Length - 18));
        }

        public FujiCheckRegister GetLastestBoxNumberItems()
        {
            FujiCheckRegister model = new FujiCheckRegister();
            DateTime lastestDate = new DateTime(1900, 1, 1);
            ObjectCache cache = MemoryCache.Default;


            string cacheDateTime = cache["Cache_DateTimeStamp_SetScanned"] + "";
            if (string.IsNullOrEmpty(cacheDateTime))
                return null;
            if (!DateTime.TryParse(cache["Cache_DateTimeStamp_SetScanned"].ToString(), out lastestDate))
                return null;
            string cacheContent = cache["Cache_SelectSQL_SetScanned"] + "";
            if (string.IsNullOrEmpty(cacheContent))
                return null;

            IEnumerable<FujiBoxNumberAndAmountModel> items = new List<FujiBoxNumberAndAmountModel>();
            using (FujiDbContext Db = new FujiDbContext())
            {
                string sql = cacheContent;
                sql = sql.Remove(sql.IndexOf("AND"));
                var scannedItems = Db.Database.SqlQuery<ImportSerialDetail>(sql).ToList();
                if (scannedItems != null)
                {
                    model.TotalRecord = scannedItems.Count;
                    int ix = 1;
                    items = (from p in scannedItems
                             orderby p.BoxNumber
                             group p by p.BoxNumber
                            into g
                             select new FujiBoxNumberAndAmountModel
                             {
                                 ItemIndex = ix++
                  ,
                                 BoxNumber = g.Key
                  ,
                                 Amount = g.ToList().Count
                             }).ToList();

                }
            }
            model.BoxAndAmount = items;
            model.LastestDate = lastestDate;


            return model;
        }

        public FujiCheckRegister GetItemScanLastest()
        {
            FujiCheckRegister model = new FujiCheckRegister();
            IEnumerable<FujiBoxNumberAndAmountModel> items = new List<FujiBoxNumberAndAmountModel>();
            using (FujiDbContext db = new FujiDbContext())
            {
                var scannedItems = db.ItemScanLastests.ToList();
                model.TotalRecord = scannedItems.Count;
                int ix = 1;
                items = (from p in scannedItems
                         orderby p.BoxNumber
                         group p by p.BoxNumber
                        into g
                         select new FujiBoxNumberAndAmountModel
                         {
                             ItemIndex = ix++
              ,
                             BoxNumber = g.Key
              ,
                             Amount = g.ToList().Count
                         }).ToList();
                model.BoxAndAmount = items;
                model.LastestDate = scannedItems.FirstOrDefault() != null ? scannedItems.First().CreateAt : null;
                return model;

            }
        }

        public IEnumerable<ImportSerialHead> GetHeadDataTopten(ParameterSearch parameterSearch)
        {
            int top = parameterSearch.PageSize > 0 ? parameterSearch.PageSize : 10;
            string sql = string.Format("SELECT TOP({0}) * FROM [dbo].[ImportSerialHead] WHERE Status = '{1}' ORDER BY CreateAt DESC", top, statusReceived);

            IEnumerable<ImportSerialHead> items = new List<ImportSerialHead>();

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;

            if (cnt > 0)
                sql += parameterSearch.SpeacialQuery;

            using (FujiDbContext Db = new FujiDbContext())
            {
                items = Db.Database.SqlQuery<ImportSerialHead>(sql).ToList();
            }


            return items;

        }
        public IEnumerable<FujiTagReport> GetReportByYearRang(ParameterSearch parameterSearch)
        {
            string[] ms = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
            string[] ml = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<FujiTagReport> items = new List<FujiTagReport>();
            string result = "", startDate = "", endDate = "";

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;
            if (cnt != 2)
                return null;

            for (int i = 0; i < cnt; i++)
            {
                if (parameterSearch.Columns[i] == "startDate")
                    startDate = parameterSearch.Keywords[i];
                if (parameterSearch.Columns[i] == "endDate")
                    endDate = parameterSearch.Keywords[i];
            }

            using (FujiDbContext db = new FujiDbContext())
            {
                result = db.Database.SqlQuery<string>("ProcGetRFIDInfoByCreateAt @StartDate,@EndDate"
                    , new SqlParameter("@StartDate", startDate)
                    , new SqlParameter("@EndDate", endDate)).SingleOrDefault();
            }

            if (!string.IsNullOrEmpty(result))
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                items = json_serializer.Deserialize<List<FujiTagReport>>(result);
                if (items != null)
                {
                    items.ForEach(f =>
                    {
                        f.MonthName = ml[f.MonthNumber - 1];
                    });
                }

            }
            return items;
        }

        public bool SetConfirmToStock(string headId)
        {
            using (var scope = new TransactionScope())
            {
                using (FujiDbContext db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(db);
                    ImportSerialHead importSerialHead = GetItemByDocID(headId);

                    try
                    {
                        if (importSerialHead != null)
                        {
                            importSerialHead.IsConfirmedToStock = true;
                            SerialHeadRepo.Update(importSerialHead);
                            db.SaveChanges();                            
                        }
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
            return true;
        }

        private IEnumerable<ImportSerialDetail> MapFormScan(List<ImportSerialDetail> items, List<string> scans, bool isPerformMap)
        {
            var itemMappeds = new List<ImportSerialDetail>();
            foreach (var item in items)
            {
                foreach (var scan in scans)
                {
                    if (scan.EndsWith(item.ItemGroup))
                    {
                        if (isPerformMap)
                        {
                            item.ItemGroup = scan;
                        }
                        itemMappeds.Add(item);
                    }
                }
            }
            return itemMappeds;
        }

        #endregion

        #region Async
        public async Task<bool> UpdateItemAsync(string id, ImportSerialHead item)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (FujiDbContext Db = new FujiDbContext())
                {
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    try
                    {
                        var queryUpdateHead = SerialHeadRepo.GetItemsBy(p => new List<string>() { "IS1802090000002", "IS1802090000001" }.Contains(p.HeadID)).ToList();
                        queryUpdateHead.ForEach(f =>
                        {
                            f.ItemCode = "T2T";//item.ItemCode;
                            f.WHID = item.WHID;
                            f.LotNumber = item.LotNumber;
                            f.InvoiceNumber = item.InvoiceNumber;
                            f.ReceivingDate = item.ReceivingDate;
                            f.DeliveryNote = item.DeliveryNote;
                            f.Remark = item.Remark;
                            //existedItem.UpdateDate = DateTime.Now;
                            f.Qty = item.Qty;
                            f.Spare1 = item.Spare1;
                            f.Spare2 = item.Spare2;
                            //existedItem.Spare3 = item.Spare3;
                            f.Spare4 = item.Spare4;
                            f.Spare5 = item.Spare5;
                            f.Spare6 = item.Spare6;
                            f.Spare7 = item.Spare7;
                            f.Spare8 = item.Spare8;
                            f.Spare9 = item.Spare9;
                            f.Spare10 = item.Spare10;
                            //Repo.Update(existedItem);
                            SerialHeadRepo.Update(f);
                        });
                        await Db.SaveChangesAsync();

                        var updatedItem = SerialHeadRepo.GetItemBy(f => f.HeadID == item.HeadID, true);
                        if (updatedItem != null)
                        {
                            if (updatedItem.ImportSerialDetail.Any())
                            {
                                //IGenericRepository<ImportSerialDetail> detailRepo = new GenericRepository<ImportSerialDetail>(Db);
                                //Db.ProcDeleteImportSerialDetail(item.HeadID);

                                //IEnumerable<ImportSerialDetail> _existDetails = (from d in Db.ImportSerialDetail
                                //         where d.HeadID == item.HeadID
                                //         select d
                                //         ).ToList();

                                SerialDetailRepo.Delete(item.HeadID);

                                //Db.ImportSerialDetail.RemoveRange(_existDetails);

                                foreach (var detail in updatedItem.ImportSerialDetail)
                                {
                                    detail.HeadID = updatedItem.HeadID;
                                    detail.ItemCode = updatedItem.ItemCode;
                                    detail.DetailID = Guid.NewGuid().ToString();
                                    //Db.ImportSerialDetail.Add(detail);
                                    SerialDetailRepo.Insert(detail);
                                    await Db.SaveChangesAsync().ConfigureAwait(false);
                                }
                            }
                        }
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
            return true;
        }


        #endregion

    }
}
