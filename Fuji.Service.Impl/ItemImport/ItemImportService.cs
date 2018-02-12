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

namespace Fuji.Service.Impl.ItemImport
{
    public class ItemImportService : WIM.Core.Service.Impl.Service, IItemImportService
    {
        #region connection Settings

        private string connectionString = ConfigurationManager.ConnectionStrings["WIM_FUJI"].ConnectionString;
        #endregion

        //private FujiDbContext Db { get; set; }

        //private SerialRepository SerialDetailRepo;
        //private SerialHeadRepository SerialHeadRepo;

        public ItemImportService()
        {
            //Db = FujiDbContext.Create();
            //SerialDetailRepo = new SerialRepository(new FujiDbContext());
            //SerialHeadRepo = new SerialHeadRepository(new FujiDbContext());
        }

        public IEnumerable<ImportSerialHead> GetItems()
        {
            IEnumerable<ImportSerialHead> items;
            using (FujiDbContext Db = new FujiDbContext())
            {
                items = (from h in Db.ImportSerialHead
                         where !h.HeadID.Equals("0") && !h.Status.Equals(FujiStatus.DELETED.ToString())
                         orderby h.CreateAt descending
                         select h).Take(50);
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
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
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
                            importSerialHead.Status = "DELETED";
                            SerialHeadRepo.Update(importSerialHead);
                            Db.SaveChanges();
                        }

                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }
            }
        }
        /*public void CheckReceive(CheckReceiveRequest receive)
       {
           var x = (from d in Db.ImportSerialDetails
                    where receive.ItemGroups.Contains(d.ItemGroup)
                               && d.HeadID == "0"
                               &&  d.Status == "NEW"

                    );
       }*/

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
                    throw new ValidationException(ErrorEnum.DataNotFound);
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
        public void ReGenerateRFID(List<string> itemGroupsFromScan)
        {
            using (var scope = new TransactionScope())
            {
                using (var Db = new FujiDbContext())
                {
                    var query = (from d in Db.ImportSerialDetail
                                 where d.HeadID == "0"
                                    && new List<string> {
                                    FujiStatus.NEW.ToString(),
                                    FujiStatus.SCANNED.ToString()
                                    }.Contains(d.Status)
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
                        throw new ValidationException(e);
                    }
                }
            }

        }
        public bool SetScanned(SetScannedRequest receive)
        {
            ReGenerateRFID(receive.ItemGroups);

            RemoveRegisterDuplicate();

            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext("SetScanned(SetScannedRequest)"))
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);

                    var query = (from d in Db.ImportSerialDetail
                                 where receive.ItemGroups.Contains(d.ItemGroup)
                                    && d.HeadID == "0"
                                    //&& d.Status == FujiStatus.NEW.ToString()
                                    && new List<string> {
                                    FujiStatus.NEW.ToString(),
                                    FujiStatus.SCANNED.ToString()
                                    }.Contains(d.Status)
                                 select d
                         );

                    if (query != null)
                    {
                        ObjectCache cache = MemoryCache.Default;
                        cache.Set("Cache_SelectSQL_SetScanned", query.ToString(), DateTimeOffset.Now.AddHours(2));
                        cache.Set("Cache_DateTimeStamp_SetScanned", DateTime.Now, DateTimeOffset.Now.AddHours(2));
                    }

                    var resultGroup = (from p in query
                                       group p by p.ItemGroup into g
                                       select new { ItemGroup = g.Key, Items = g.ToList() }).ToList();


                    ImportSerialHead importHead = (from h in Db.ImportSerialHead
                                                   where h.HeadID == receive.HeadID
                                                   select h
                                                    ).SingleOrDefault();

                    if (importHead == null)
                    {
                        throw new ValidationException(ErrorEnum.DataNotFound);
                    }

                    if (resultGroup.Count() != importHead.Qty)
                    {
                        throw new ValidationException(new ValidationError("48888", "Head ไม่เท่ากับที่ Scan รับ"));
                    }

                    importHead.Status = FujiStatus.SCANNED.ToString();


                    try
                    {
                        SerialHeadRepo.Update(importHead);

                        foreach (ImportSerialDetail detail in query)
                        {
                            detail.HeadID = receive.HeadID;
                            detail.ItemCode = receive.ItemCode;
                            detail.Status = FujiStatus.SCANNED.ToString();
                            SerialDetailRepo.Update(detail);
                        }


                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }


            }

            return false;
        }
        public bool Receive(ReceiveRequest receive)
        {

            /*                
                select a.ItemCode, a.SerialNumber
                from ImportSerialDetail a
                where exists 
                (
	                select b.*
	                from ImportSerialDetail b
	                where b.ItemGroup in ('A120170809000001', 'A120170809000002', 'A120170809000003', 'A120170809000004', 'A120170809000005')
	                and b.ItemCode = a.ItemCode
	                and b.SerialNumber = a.SerialNumber
	                and a.Status != 'SHIPPED'
                )
                group by a.SerialNumber, a.ItemCode
                having count(*) > 1
             */

            // Test ValidationError
            //ValidationError ve = new ValidationError("1001", "Item Code 1111, Serials 2222 was exists!");
            //throw new ValidationException(ve);

            /*ValidationException ve = new ValidationException();
            for (int i = 0; i < 3; i++)
            {
                ve.Add(new ValidationError(i.ToString(), i.ToString()));
            }

            throw ve;*/



            // Test ValidationError
            //ValidationError ve = new ValidationError("1001", "Item Code 1111, Serials 2222 was exists!");
            //throw new ValidationException(ve);

            using (var scope = new TransactionScope())
            {
                using (FujiDbContext Db = new FujiDbContext("Receive(ReceiveRequest)"))
                {
                    ISerialDetailRepository SerialDetailRepo = new SerialDetailRepository(Db);
                    ISerialHeadRepository SerialHeadRepo = new SerialHeadRepository(Db);
                    ISerialDetailTempRepository SerialDetailTempRepo = new SerialDetailTempRepository(Db);

                    var serialsRemainInStock = (from a in SerialDetailRepo.GetAll()
                                                where SerialDetailRepo.IsAnyItemBy(b =>
                                                       receive.ItemGroups.Contains(b.ItemGroup)
                                                       && b.HeadID != "0"
                                                       && b.ItemCode == a.ItemCode
                                                       && b.SerialNumber == a.SerialNumber
                                                       && b.ItemType == a.ItemType
                                                       && a.Status != FujiStatus.SHIPPED.ToString()
                                                       && a.Status != FujiStatus.DELETED.ToString()
                                                   )
                                                group a by new
                                                {
                                                    a.ItemCode,
                                                    a.SerialNumber
                                                } into g
                                                where g.Count() > 1
                                                select new SerialsRemainInStock
                                                {
                                                    ItemCode = g.Key.ItemCode,
                                                    SerialNumber = g.Key.SerialNumber
                                                }
                        ).ToList();

                    if (serialsRemainInStock.Any())
                    {
                        ValidationException ve = new ValidationException();
                        foreach (SerialsRemainInStock item in serialsRemainInStock)
                        {
                            throw new ValidationException(ErrorEnum.ReceiveSerialRemainInStock);
                        }
                        throw ve;
                    }

                    var query = (from d in Db.ImportSerialDetail
                                 where receive.ItemGroups.Contains(d.ItemGroup)
                                 && d.HeadID == receive.HeadID
                                 && d.Status == FujiStatus.SCANNED.ToString()
                                 select d
                            ).ToList();

                    query.ForEach(f =>
                    {
                        var item = TranslateDetailTemp(f);
                        if (item != null)
                            SerialDetailTempRepo.Insert(item);
                    });

                    try
                    {
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationException(new ValidationError("5000", "เกิดข้อผิดพลาด ติดต่อ IT\n\n\n" + ex.Message));
                    }

                    foreach (ImportSerialDetail detail in query)
                    {
                        detail.HeadID = receive.HeadID;
                        detail.ItemCode = receive.ItemCode;
                        detail.Status = FujiStatus.RECEIVED.ToString();
                        detail.Location = receive.LocationID;
                        SerialDetailRepo.Update(detail);
                    }

                    try
                    {
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationException(new ValidationError("5001", "เกิดข้อผิดพลาด ติดต่อ IT\n\n\n" + ex.Message));
                    }


                    ImportSerialHead importHead = (from h in Db.ImportSerialHead
                                                   where h.HeadID == receive.HeadID
                                                   select h
                                ).SingleOrDefault();

                    importHead.Status = FujiStatus.RECEIVED.ToString();
                    importHead.Location = receive.LocationID;
                    SerialHeadRepo.Update(importHead);

                    try
                    {
                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationException(new ValidationError("5002", "เกิดข้อผิดพลาด ติดต่อ IT\n\n\n" + ex.Message));
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
                var items = SerialDetailRepo.GetItemsBy(d => d.OrderNo == orderNo && d.Status == FujiStatus.IMP_PICKING.ToString());

                itemGroups = (from d in items
                              group d by d.ItemGroup into g
                              select g.Key
                         ).ToList();

                if (!itemGroups.Any())
                {
                    throw new ValidationException(ErrorEnum.DataNotFound);
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
                    IQueryable queryDetailsList = (from d in Db.ImportSerialDetail
                                                   where confirmRequest.ItemGroups.Contains(d.ItemGroup)
                                                   && d.OrderNo == confirmRequest.OrderNumber
                                                   && d.Status == FujiStatus.IMP_PICKING.ToString()
                                                   select d);

                    try
                    {
                        foreach (ImportSerialDetail item in queryDetailsList)
                        {
                            item.Status = FujiStatus.SHIPPED.ToString();
                            SerialDetailRepo.Update(item);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }

            }
            return false;
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
                            ImportSerialDetail item = new ImportSerialDetail();
                            item.DetailID = Guid.NewGuid().ToString();
                            item.HeadID = "0";
                            item.SerialNumber = serial;
                            item.BoxNumber = registerRequest.BoxNumber;
                            item.ItemGroup = registerRequest.RFIDTag;
                            item.ItemType = (++itemType).ToString();
                            item.Status = FujiStatus.NEW.ToString();
                            //Db.ImportSerialDetail.Add(item);
                            SerialDetailRepo.Insert(item);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }
                return false;
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
                            //GeneralLog log = new GeneralLog(emID)
                            //{
                            //    TableName = "ImportSerialDetail",
                            //    ColumnName = "BoxNumber",
                            //    RefID = detail.DetailID,
                            //    Value = boxNumberTo,
                            //    Remark = "SetSerial2Box"
                            //};
                            //Db.GeneralLogs.Add(log);
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
                    //GeneralLog log = new GeneralLog(emID)
                    //{
                    //    TableName = "ImportSerialDetail",
                    //    ColumnName = "Location",
                    //    RefID = detail.DetailID,
                    //    Value = locationTo,
                    //    Remark = "SetBox2Location"
                    //};
                    //db.GeneralLogs.Add(log);

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
            /*var item = Repo.GetByID(id);
            return item;*/
            //return Db.ProcGetImportSerialHeadByHeadID(docId).FirstOrDefault();
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
                        item.Status = FujiStatus.NEW.ToString();
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
                        throw new ValidationException(e);
                    }

                }

            }
            return item;
        }
        public bool UpdateItem(string id, ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {
                //var existedItem = Repo.GetByID(id);
                //IQueryable queryUpdateHead = (from p in Db.ImportSerialHead
                //                              where p.HeadID.Equals(id)
                //    
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
                            Db.SaveChanges();
                        });

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

                                SerialDetailRepo.DeleteItems(d => d.HeadID == item.HeadID);

                                //Db.ImportSerialDetail.RemoveRange(_existDetails);

                                foreach (var detail in updatedItem.ImportSerialDetail)
                                {
                                    detail.HeadID = updatedItem.HeadID;
                                    detail.ItemCode = updatedItem.ItemCode;
                                    detail.DetailID = Guid.NewGuid().ToString();
                                    //Db.ImportSerialDetail.Add(detail);
                                    SerialDetailRepo.Insert(detail);
                                    Db.SaveChanges();
                                }
                            }
                        }


                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
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
                        throw new ValidationException(e);
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
                    && x.Status == FujiStatus.RECEIVED.ToString()).ToList();

                    try
                    {
                        updateSerialDetailStatus.ForEach(a =>
                        {
                            a.Status = FujiStatus.IMP_PICKING.ToString();
                            a.OrderNo = pickingList.First().OrderNumber;
                            SerialDetailRepo.Update(a);
                        });

                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
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
                var selectedData = (from p in Db.ImportSerialDetail where p.Status == FujiStatus.IMP_PICKING.ToString() select new { OrderNo = p.OrderNo, UpdateAt = p.UpdateAt }).ToList();


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
                    throw new ValidationException(ErrorEnum.DataNotFound);
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
                            f.Status = FujiStatus.RECEIVED.ToString();
                            SerialDetailRepo.Update(f);
                        });

                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                        return false;
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
            string[] streamids;
            Warning[] warnings;
            string mimeType, encoding, extension;
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

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = barcodeList;

                ReportDataSource rds2 = new ReportDataSource();
                rds2.Name = "DataSet2";
                rds2.Value = barcodeDetailList;


                reportViewer.LocalReport.DataSources.Add(rds1);
                reportViewer.LocalReport.DataSources.Add(rds2);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

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
                items = (from p in Db.ImportSerialDetail
                         where p.BoxNumber == boxNumber
                         orderby p.SerialNumber
                        select new FujiSerialAndRFIDModel()
                        {
                            SerialNumber = p.SerialNumber
                            ,
                            IsValid = (p.ItemGroup.Length > 4)
                            ,
                            RFIDTag = (p.ItemGroup.Length > 4
                            ? Convert.ToInt32(p.ItemGroup.Substring(p.ItemGroup.Length - 4, 4), 16)
                            : Convert.ToInt32(p.ItemGroup, 16)).ToString()
                        }).ToList();

            }



            return items;
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
        public IEnumerable<ImportSerialHead> GetHeadDataTopten(ParameterSearch parameterSearch)
        {
            int top = parameterSearch.PageSize > 0 ? parameterSearch.PageSize : 10;
            string sql = string.Format("SELECT TOP({0}) * FROM [dbo].[ImportSerialHead] WHERE Status = '{1}' ORDER BY CreateAt DESC", top, FujiStatus.RECEIVED.ToString());

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

        #endregion

        #region Async

      

        #endregion

    }
}
