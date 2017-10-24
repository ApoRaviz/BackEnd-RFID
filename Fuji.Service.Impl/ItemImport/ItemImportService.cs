using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
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

namespace Fuji.Service.Impl.ItemImport
{
    public class ItemImportService : IItemImportService
    {
        #region connection Settings

        private string connectionString = ConfigurationManager.ConnectionStrings["WIM_FUJI"].ConnectionString;
        #endregion
        
        //private FujiDbContext Db { get; set; }
        //private IGenericRepository<ImportSerialHead> Repo;

        private SerialRepository SerialDetailRepo;
        private SerialHeadRepository SerialHeadRepo;
        public ItemImportService()
        {
            //Db = FujiDbContext.Create();
            //Repo = new GenericRepository<ImportSerialHead>(Db);
            SerialDetailRepo = new SerialRepository();
            SerialHeadRepo = new SerialHeadRepository();
        }

        public IEnumerable<ImportSerialHead> GetItems()
        {
            //return (from h in Db.ImportSerialHead
            //                    where !h.HeadID.Equals("0") && !h.Status.Equals(FujiStatus.DELETED.ToString())
            //                    orderby h.CreatedDate descending
            //                    select h).Take(50);

            return SerialHeadRepo.GetItemAll(50).OrderByDescending(o => o.CreatedDate);

        }

        public IEnumerable<ImportSerialHead> GetItems(int pageIndex, int pageSize,out int totalRecord)
        {
            DataSet dset = new DataSet();
            totalRecord = 0;
            List<ImportSerialHead> items = new List<ImportSerialHead>() { };
            using (var scope = new TransactionScope())
            {
                try
                {
                    var output = new SqlParameter("@totalrow", SqlDbType.Int, 30);
                    output.Direction = ParameterDirection.Output;

                    items =  SerialHeadRepo.SqlQuery<ImportSerialHead>("ProcPagingImportSerialHead @page,@size,@sort,@sortdecending,@totalrow out"
                        , new SqlParameter("@page",pageIndex)
                        , new SqlParameter("@size",pageSize)
                        , new SqlParameter("@sort", "CreatedDate")
                        , new SqlParameter("@sortdecending", "DESC")
                        , output).ToList();

                    totalRecord = Convert.ToInt32(output.Value);


                }
                catch (Exception)
                {
                    return new List<ImportSerialHead>() { };
                }

                scope.Complete();
                return items;
            }
        }
      

        public void DeleteItem(string id)
        {
            using (var scope = new TransactionScope())
            {
                //Db.ProcDeleteImportSerial(id);
                //ImportSerialHead importSerialHead = (
                //    from h in Db.ImportSerialHead
                //    where h.HeadID == id
                //    select h
                //).SingleOrDefault();

                ImportSerialHead importSerialHead = SerialHeadRepo.GetItemSingleBy(h => h.HeadID == id);
                try
                {
                    if (importSerialHead != null)
                    {
                        importSerialHead.Status = "DELETED";
                        SerialHeadRepo.Update(importSerialHead);
                    }
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
            }               
        }

        public ImportSerialHead GetItemByDocID(string id,bool isIncludeChild = false)
        {
            /*var item = Repo.GetByID(id);
            return item;*/
            //return Db.ProcGetImportSerialHeadByHeadID(docId).FirstOrDefault();

            //var headItem = (from h in Db.ImportSerialHead
            // where h.HeadID == id
            // select h).FirstOrDefault();

            var headItem = SerialHeadRepo.GetItemFirstBy(h => h.HeadID == id, isIncludeChild);


            return headItem;
        }

        public ItemImportDto GetItemByDocID_Handy(string id)
        {
            //ItemImportDto itemHead = (from h in Db.ImportSerialHead
            //        where h.HeadID == id
            //        select new ItemImportDto
            //        {
            //            HeadID = h.HeadID,
            //            ItemCode = h.ItemCode,
            //            Qty = h.Qty
            //        }).SingleOrDefault();
           ItemImportDto itemHead = null;
           var item = SerialHeadRepo.GetItemSingleBy(h => h.HeadID == id);
            if (item != null)
            {
                itemHead = 
                new ItemImportDto {
                    HeadID = item.HeadID,
                    ItemCode = item.ItemCode,
                    Qty = item.Qty
                }; 
            }

            if (itemHead == null)
            {
                throw new WIM.Core.Common.Validation.ValidationException(new ValidationError(((int)ErrorCode.DataNotFound).ToString(), ErrorCode.DataNotFound.GetDescription()));
            }

            return itemHead;
        }

        public ImportSerialHead CreateItem(ImportSerialHead item)
        {

            using (var scope = new TransactionScope())
            {
                //item.HeadID = Db.ProcGetNewID("IS").FirstOrDefault();
                item.HeadID = SerialHeadRepo.SqlQuerySingle<string>("ProcGetNewID @Prefixes", new SqlParameter("@Prefixes", "IS"));
                item.Status = FujiStatus.NEW.ToString();
                item.Location = "";
                item.ReceivingDate = item.ReceivingDate;
                item.CreatedDate = DateTime.Now;
                item.UpdateDate = DateTime.Now;
                //Repo.Insert(item);
                //Db.Entry(item).Collection(c => c.ImportSerialDetail).Load();
                SerialHeadRepo.InsertItem(item);
                item = SerialHeadRepo.IncludeChild(item);


                if (item.ImportSerialDetail.Any())
                    {

                        foreach (var detail in item.ImportSerialDetail)
                            {
                                //IGenericRepository<ImportSerialDetail> detailRepo = new GenericRepository<ImportSerialDetail>(Db);
                                detail.HeadID = item.HeadID;
                                detail.DetailID = Guid.NewGuid().ToString();
                                detail.CreatedDate = DateTime.Now;
                                detail.UpdateDate = DateTime.Now;
                                detail.UserUpdate = item.UserUpdate;
                                SerialDetailRepo.Update(detail);
                                //detailRepo.Insert(detail);
                                
                            }
                    }
                try
                {
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return item;
            }
        }


        public bool UpdateItem(string id, ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {
                //var existedItem = Repo.GetByID(id);
                //IQueryable queryUpdateHead = (from p in Db.ImportSerialHead
                //                              where p.HeadID.Equals(id)
                //                              select p);
                item = SerialHeadRepo.GetItemBy(f => f.HeadID == item.HeadID, true);
                IEnumerable<ImportSerialHead> queryUpdateHead = SerialHeadRepo.GetItemsBy(p => p.HeadID==id);
                try
                {
                    foreach (ImportSerialHead existedItem in queryUpdateHead)
                    {
                        existedItem.ItemCode = item.ItemCode;
                        existedItem.WHID = item.WHID;
                        existedItem.LotNumber = item.LotNumber;
                        existedItem.InvoiceNumber = item.InvoiceNumber;
                        existedItem.ReceivingDate = item.ReceivingDate;
                        existedItem.DeliveryNote = item.DeliveryNote;
                        existedItem.SerialFormat1 = item.SerialFormat1;
                        existedItem.SerialFormat2 = item.SerialFormat2;
                        existedItem.SerialName1 = item.SerialName1;
                        existedItem.SerialName2 = item.SerialName2;
                        existedItem.Remark = item.Remark;
                        existedItem.UpdateDate = DateTime.Now;
                        existedItem.Qty = item.Qty;

                        existedItem.Spare1 = item.Spare1;
                        existedItem.Spare2 = item.Spare2;
                        //existedItem.Spare3 = item.Spare3;
                        existedItem.Spare4 = item.Spare4;
                        existedItem.Spare5 = item.Spare5;
                        existedItem.Spare6 = item.Spare6;
                        existedItem.Spare7 = item.Spare7;
                        existedItem.Spare8 = item.Spare8;
                        existedItem.Spare9 = item.Spare9;
                        existedItem.Spare10 = item.Spare10;
                        //Repo.Update(existedItem);
                        SerialHeadRepo.Update(existedItem);
                    }

                    //Db.Entry(item).Collection(c => c.ImportSerialDetail).Load();

                    if (item.ImportSerialDetail.Any())
                    {
                        //IGenericRepository<ImportSerialDetail> detailRepo = new GenericRepository<ImportSerialDetail>(Db);
                        //Db.ProcDeleteImportSerialDetail(item.HeadID);

                        //IEnumerable<ImportSerialDetail> _existDetails = (from d in Db.ImportSerialDetail
                        //         where d.HeadID == item.HeadID
                        //         select d
                        //         ).ToList();

                        SerialDetailRepo.DeleteItems(d => d.HeadID == item.HeadID);

                        //Db.ImportSerialDetail.RemoveRange(_existDetails);

                        foreach (var detail in item.ImportSerialDetail)
                        {
                            detail.HeadID = item.HeadID;
                            detail.ItemCode = item.ItemCode;
                            detail.DetailID = Guid.NewGuid().ToString();
                            detail.CreatedDate = DateTime.Now;
                            detail.UpdateDate = DateTime.Now;
                            detail.UserUpdate = item.UserUpdate;
                            //detailRepo.Insert(detail);
                            //Db.ImportSerialDetail.Add(detail);
                            SerialDetailRepo.Insert(detail);
                        }
                    }

              
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
        }

        public bool UpdateStausExport(ImportSerialHead item)
        {
            using (var scope = new TransactionScope())
            {
                //List<ImportSerialHead> queryUpdateHead = (from p in Db.ImportSerialHead
                //                              where p.HeadID.Equals(item.HeadID)
                //                              select p).ToList();

                List<ImportSerialHead> queryUpdateHead = SerialHeadRepo.GetItemsBy(p => p.HeadID == item.HeadID).ToList();

                try
                {
                    queryUpdateHead.ForEach(f => {
                        f.UserUpdate = item.UserUpdate;
                        f.UpdateDate = DateTime.Now;
                        f.IsRFID = 1;
                        SerialHeadRepo.Update(f);
                    });
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
        }

        public IEnumerable<ImportSerialDetail> UpdateStatus(List<PickingRequest> pickingList, string userUpdate)
        {
            List<string> itemCodes = pickingList.Select(x => x.ItemCode).ToList();
            List<string> serialNumbers = pickingList.Select(x => x.SerialNumber).ToList();
            List<ImportSerialDetail> returnValue = new List<ImportSerialDetail>();

            using (var scope = new TransactionScope())
            {
                List<ImportSerialDetail> queryDetailsList = new List<ImportSerialDetail>() { };
                for (int i = 0;i < itemCodes.Count();i++)
                {
                    string nItemCode = itemCodes[i];
                    string nSerialNumber = serialNumbers[i];
                    //var qDetailItem = (from d in Db.ImportSerailDetail
                    //                   where d.ItemCode.Equals(nItemCode)
                    //                   && d.SerialNumber.Equals(nSerialNumber)
                    //                   select d).FirstOrDefault();

                    var qDetailItem = SerialDetailRepo.GetItemFirstBy(d => d.ItemCode == nItemCode
                                      && d.SerialNumber == nSerialNumber);

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
                //var updateSerialDetailStatus = Db.ImportSerialDetail.Where(x => itemGroupsList.Contains(x.ItemGroup) 
                //&& x.Status == FujiStatus.RECEIVED.ToString()).ToList();
                var updateSerialDetailStatus = SerialDetailRepo.GetItemsBy(x => itemGroupsList.Contains(x.ItemGroup)
                && x.Status == FujiStatus.RECEIVED.ToString()).ToList();
                try
                {
                    updateSerialDetailStatus.ForEach(a =>
                    {
                        a.Status = FujiStatus.IMP_PICKING.ToString();
                        a.UpdateDate = DateTime.Now;
                        a.OrderNo = pickingList.First().OrderNumber;
                        a.UserUpdate = userUpdate;
                        SerialDetailRepo.Update(a);
                    });
                
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
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
                        HandleValidationException(e);
                    }
                }*/

                scope.Complete();
                return returnValue;
            }
        }



        public IEnumerable<FujiPickingGroup> GetPickingGroup(int max = 50)
        {
            List<FujiPickingGroup> items = new List<FujiPickingGroup>();

            var itemGroups = (from p in SerialDetailRepo.GetAll()
                              orderby p.CreatedDate descending
                              group p
                              by p.OrderNo into g
                              select new { GroupID = g.Key, GroupList = g.ToList() }).Take(max).ToList();

            itemGroups.ForEach(f =>
            {
                FujiPickingGroup item = new FujiPickingGroup(f.GroupID, f.GroupList.Count(), new List<ImportSerialDetail>() { });

                if (!string.IsNullOrEmpty(f.GroupID))
                    items.Add(item);
            });

            return items;
        }

        public IEnumerable<ImportSerialDetail> GetImportSerialDetailByHeadID(string headID)
        {
            List<ImportSerialDetail> ret = new List<ImportSerialDetail>() { };
            string sql = "SELECT * FROM [dbo].[ImportSerialDetail] WHERE HeadID=@HeadID";
            using (var scope = new TransactionScope())
            {
                try
                {
                    SerialDetailRepo.SqlQuery<ImportSerialDetail>(sql,new SqlParameter("@HeadID",headID));
                }
                catch (Exception)
                {
                    return new List<ImportSerialDetail>() { };
                }

                scope.Complete();
                return ret;
            }
        }

        public FujiPickingGroup GetPickingByOrderNo(string orderNo,bool isAddItem = false)
        {
            var itemGroups = (from p in SerialDetailRepo.GetItemsBy(p => p.OrderNo.Contains(orderNo))
                              orderby p.CreatedDate descending
                              group p
                              by p.OrderNo into g
                              select new { GroupID = g.Key, GroupList = g.ToList() }).ToList();

            var item = itemGroups.SingleOrDefault();
            if (item == null)
            {
                throw new ValidationException(new ValidationError(((int)ErrorCode.DataNotFound).ToString(), ErrorCode.DataNotFound.GetDescription()));
            }
            if (isAddItem)
            {
                return new FujiPickingGroup(item.GroupID, item.GroupList.Count(), item.GroupList);
            }
            else
            {
                return new FujiPickingGroup(item.GroupID, item.GroupList.Count(), new List<ImportSerialDetail>() { });
            }
        }

        public bool ClearPickingGroup(string orderID)
        {
            using (var scope = new TransactionScope())
            {

                //var itemGroups = (from p in Db.ImportSerialDetail
                //                  where p.OrderNo.Equals(orderID)
                //                  select p).ToList();
                var itemGroups = SerialDetailRepo.GetItemsBy(p => p.OrderNo==orderID).ToList();

                try
                {
                    itemGroups.ForEach(f =>
                    {
                        f.OrderNo = null;
                        f.Status = FujiStatus.RECEIVED.ToString();
                        SerialDetailRepo.Update(f);
                    });
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                    return false;
                }
                scope.Complete();
                return true;
            }
        }

        public string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            conditionColumnNames = conditionColumnNames.Replace("ReceivingDate", "convert(varchar(50),ReceivingDate,121)");
            //return Db.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword).FirstOrDefault();
            return "";
        }

        public IEnumerable<ImportSerialHead> GetDataByColumn(string column, string keyword)
        {
            string sql = "";
            DataSet dset = new DataSet();
            List<ImportSerialHead> items = new List<ImportSerialHead>() { };
            switch (column.Trim().ToUpper())
            {
                default:
                case "HEADID":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [HeadID] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
                case "ITEMCODE":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [ItemCode] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
                case "WHID":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [WHID] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
                case "LOTNUMBER":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [LotNumber] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
                case "INVOICENUMBER":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [InvoiceNumber] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
                case "LOCATION":
                    sql += "SELECT * FROM [dbo].[ImportSerialHead] WHERE [Location] LIKE '%' + @keyword + '%' AND [Status] <> 'DELETED'";
                    break;
            }

           

            using (var scope = new TransactionScope())
            {
                try
                {
                    items = SerialHeadRepo.SqlQuery<ImportSerialHead>(sql, new SqlParameter("@keyword", keyword)).ToList();
                }
                catch (Exception)
                {
                    return new List<ImportSerialHead>() { };
                }
                scope.Complete();
                return items;

            }

        }



        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
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

        private void RemoveRegisterDuplicate()
        {
            using (var scope = new TransactionScope())
            {
                //, ItemGroup, BoxNumber, ItemType
                SerialDetailRepo.ExceuteSql(@"
                    delete from dbo.ImportSerialDetail where DetailID in (
	                    select DetailID from (
		                    select 
		                    ROW_NUMBER() OVER(PARTITION BY SerialNumber ORDER BY CreatedDate DESC) AS Row  
		                    ,DetailID
		                    from dbo.ImportSerialDetail
		                    where HeadID = '0' and Status = 'NEW'
		                    ) a
	                    where Row > 1
                    )
                ");

                //Db.SaveChanges();
                scope.Complete();
            }
        }

        public bool SetScanned(SetScannedRequest receive)
        {
            RemoveRegisterDuplicate();

            using (var scope = new TransactionScope())
            {
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

                var resultGroup = (from p in query
                                   group p by p.ItemGroup into g
                                   select new { ItemGroup = g.Key, Items = g.ToList() }).ToList();


                ImportSerialHead importHead = (from h in Db.ImportSerialHead
                                               where h.HeadID == receive.HeadID
                                               select h
                       ).SingleOrDefault();

                if (importHead == null)
                {
                    throw new ValidationException(new ValidationError(((int)ErrorCode.DataNotFound).ToString(), ErrorCode.DataNotFound.GetDescription()));
                }

                if (resultGroup.Count() != importHead.Qty)
                {
                    throw new ValidationException(new ValidationError("48888", "Head ไม่เท่ากับที่ Scan รับ"));
                }

                importHead.Status = FujiStatus.SCANNED.ToString();
                importHead.UpdateDate = DateTime.Now;
                importHead.UserUpdate = receive.UserUpdate;

                foreach (ImportSerialDetail detail in query)
                {
                    detail.HeadID = receive.HeadID;
                    detail.ItemCode = receive.ItemCode;
                    detail.Status = FujiStatus.SCANNED.ToString();
                    detail.UpdateDate = DateTime.Now;
                    detail.UserUpdate = receive.UserUpdate;
                }

                try
                {
                    Db.SaveChanges();
                    scope.Complete();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }

                return false;
            }
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

            var serialsRemainInStock = (from a in SerialDetailRepo.GetAll()
                                        where SerialDetailRepo.IsAnyItemBy(b =>
                                               receive.ItemGroups.Contains(b.ItemGroup)
                                               && b.HeadID != "0"
                                               && b.ItemCode == a.ItemCode
                                               && b.SerialNumber == a.SerialNumber
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
                    ve.Add(new ValidationError(((int)ErrorCode.ReceiveSerialRemainInStock).ToString(), string.Format("Serial {0}, ItemCode {1} already exists! ", item.SerialNumber, item.ItemCode)));
                }
                throw ve;
            }

            // Test ValidationError
            //ValidationError ve = new ValidationError("1001", "Item Code 1111, Serials 2222 was exists!");
            //throw new ValidationException(ve);

            using (var scope = new TransactionScope())
            {
                //var query = (from d in Db.ImportSerialDetail
                //             where receive.ItemGroups.Contains(d.ItemGroup)
                //                && d.HeadID == receive.HeadID
                //                && d.Status == FujiStatus.SCANNED.ToString()
                //             select d
                //         );
                var query = SerialDetailRepo.GetItemsBy(d => receive.ItemGroups.Contains(d.ItemGroup)
                                && d.HeadID == receive.HeadID
                                && d.Status == FujiStatus.SCANNED.ToString());

                try
                {
                    foreach (ImportSerialDetail detail in query)
                    {
                        detail.HeadID = receive.HeadID;
                        detail.ItemCode = receive.ItemCode;
                        detail.Status = FujiStatus.RECEIVED.ToString();
                        detail.UpdateDate = DateTime.Now;
                        detail.UserUpdate = receive.UserUpdate;
                    }

                    //ImportSerialHead importHead = (from h in Db.ImportSerialHead
                    //                               where h.HeadID == receive.HeadID
                    //                               select h
                    //         ).SingleOrDefault
                    ImportSerialHead importHead = SerialHeadRepo.GetItemSingleBy(h => h.HeadID == receive.HeadID);

                    importHead.Status = FujiStatus.RECEIVED.ToString();
                    importHead.Location = receive.LocationID;
                    importHead.UpdateDate = DateTime.Now;
                    importHead.UserUpdate = receive.UserUpdate;

                    //Db.SaveChanges();
                    scope.Complete();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }

                return false;
            }
        }

        public List<string> GetItemGroupByOrderNo_Handy(string orderNo)
        {
            var items = SerialDetailRepo.GetItemsBy(d => d.OrderNo == orderNo && d.Status == FujiStatus.IMP_PICKING.ToString());

            var itemGroups = (from d in items
                              group d by d.ItemGroup into g
                              select g.Key
                     ).ToList();

            

            if (!itemGroups.Any())
            {
                throw new ValidationException(new ValidationError(((int)ErrorCode.DataNotFound).ToString(), ErrorCode.DataNotFound.GetDescription()));
            }

            return itemGroups;
        }

        public bool ConfirmPicking(ConfirmPickingRequest confirmRequest, string userUpdate)
        {            

            using (var scope = new TransactionScope())
            {
                //IQueryable queryDetailsList = (from d in Db.ImportSerialDetail
                //                               where confirmRequest.ItemGroups.Contains(d.ItemGroup)
                //                               && d.OrderNo == confirmRequest.OrderNumber
                //                               && d.Status == FujiStatus.IMP_PICKING.ToString()
                //                               select d);

                IQueryable queryDetailsList = SerialDetailRepo.GetItemsBy(d => confirmRequest.ItemGroups.Contains(d.ItemGroup)
                                               && d.OrderNo == confirmRequest.OrderNumber
                                               && d.Status == FujiStatus.IMP_PICKING.ToString()).AsQueryable();

                try
                {
                    foreach (ImportSerialDetail item in queryDetailsList)
                    {
                        item.Status = FujiStatus.SHIPPED.ToString();
                        item.UpdateDate = DateTime.Now;
                        item.UserUpdate = userUpdate;
                        SerialDetailRepo.Update(item);
                    }               
               
                    //Db.SaveChanges();
                    scope.Complete();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                return false;
            }
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

        #region TranslateDataSet
        private ImportSerialHead translateImportSerialHead(DataRow data)
        {
            ImportSerialHead newItem = new ImportSerialHead();
            if (data != null)
            {
                newItem.HeadID = data["HeadID"].ToString();
                newItem.ItemCode = data["ItemCode"].ToString();
                newItem.WHID = data["WHID"].ToString();
                newItem.LotNumber = data["LotNumber"].ToString();
                newItem.InvoiceNumber = data["InvoiceNumber"].ToString();
                newItem.ReceivingDate = Convert.ToDateTime(data["ReceivingDate"]);
                newItem.DeliveryNote = data["DeliveryNote"].ToString();
                newItem.Remark = data["Remark"].ToString();
                newItem.Location = data["Location"].ToString();
                newItem.Status = data["Status"].ToString();
                newItem.SerialFormat1 = data["SerialFormat1"].ToString();
                newItem.SerialFormat2 = data["SerialFormat2"].ToString();
                newItem.SerialName1 = data["SerialName1"].ToString();
                newItem.SerialName2 = data["SerialName2"].ToString();
                newItem.IsRFID = Convert.ToByte(data["IsRFID"]);
                newItem.Qty = Convert.ToInt32(data["Qty"]);
                newItem.Spare1 = data["Spare1"].ToString();
                newItem.Spare2 = data["Spare2"].ToString();
                newItem.Spare3 = data["Spare3"].ToString();
                newItem.Spare4 = data["Spare4"].ToString();
                newItem.Spare5 = data["Spare5"].ToString();
                newItem.Spare6 = data["Spare6"].ToString();
                newItem.Spare7 = data["Spare7"].ToString();
                newItem.Spare8 = data["Spare8"].ToString();
                newItem.Spare9 = data["Spare9"].ToString();
                newItem.Spare10 = data["Spare10"].ToString();
                newItem.CreatedDate = Convert.ToDateTime(data["CreatedDate"]);
                newItem.UpdateDate = Convert.ToDateTime(data["UpdateDate"]);
                newItem.UserUpdate = data["UserUpdate"].ToString();
            }

            return newItem;
        }
        private List<ImportSerialHead> translateImportSerialHeadList(DataSet data)
        {
            List<ImportSerialHead> ret = new List<ImportSerialHead>();
            if (data.Tables["DataSet1"] != null)
            {
                var collection = data.Tables["DataSet1"].Rows;
                if (collection.Count > 0)
                {
                    foreach (DataRow item in collection)
                    {
                        var result = translateImportSerialHead(item);
                        if (result != null)
                            ret.Add(result);
                    }
                }
            }

            return ret;

        }

        private ImportSerialHead translateImportSerialHead(SqlDataReader data)
        {
            ImportSerialHead newItem = new ImportSerialHead();
            if (data != null)
            {
                newItem.HeadID = data["HeadID"].ToString();
                newItem.ItemCode = data["ItemCode"].ToString();
                newItem.WHID = data["WHID"].ToString();
                newItem.LotNumber = data["LotNumber"].ToString();
                newItem.InvoiceNumber = data["InvoiceNumber"].ToString();
                newItem.ReceivingDate = Convert.ToDateTime(data["ReceivingDate"]);
                newItem.DeliveryNote = data["DeliveryNote"].ToString();
                newItem.Remark = data["Remark"].ToString();
                newItem.Location = data["Location"].ToString();
                newItem.Status = data["Status"].ToString();
                newItem.SerialFormat1 = data["SerialFormat1"].ToString();
                newItem.SerialFormat2 = data["SerialFormat2"].ToString();
                newItem.SerialName1 = data["SerialName1"].ToString();
                newItem.SerialName2 = data["SerialName2"].ToString();
                newItem.IsRFID = Convert.ToByte(data["IsRFID"]);
                newItem.Qty = Convert.ToInt32(data["Qty"]);
                newItem.Spare1 = data["Spare1"].ToString();
                newItem.Spare2 = data["Spare2"].ToString();
                newItem.Spare3 = data["Spare3"].ToString();
                newItem.Spare4 = data["Spare4"].ToString();
                newItem.Spare5 = data["Spare5"].ToString();
                newItem.Spare6 = data["Spare6"].ToString();
                newItem.Spare7 = data["Spare7"].ToString();
                newItem.Spare8 = data["Spare8"].ToString();
                newItem.Spare9 = data["Spare9"].ToString();
                newItem.Spare10 = data["Spare10"].ToString();
                newItem.CreatedDate = Convert.ToDateTime(data["CreatedDate"]);
                newItem.UpdateDate = Convert.ToDateTime(data["UpdateDate"]);
                newItem.UserUpdate = data["UserUpdate"].ToString();
            }

            return newItem;
        }
        private List<ImportSerialHead> translateImportSerialHeadList(SqlDataReader reader)
        {
            List<ImportSerialHead> ret = new List<ImportSerialHead>();
            while (reader.Read())
            {
                var result = translateImportSerialHead(reader);
                if (result != null)
                    ret.Add(result);
            }

            return ret;

        }

        private ImportSerialDetail translateImportSerialDetail(SqlDataReader data)
        {
            ImportSerialDetail newItem = new ImportSerialDetail();
            if (data != null)
            {
                newItem.DetailID = data["DetailID"].ToString();
                newItem.HeadID  = data["HeadID"].ToString();
                newItem.ItemCode = data["ItemCode"].ToString();
                newItem.SerialNumber = data["SerialNumber"].ToString();
                newItem.BoxNumber = data["BoxNumber"].ToString();
                newItem.ItemGroup = data["ItemGroup"].ToString();
                newItem.ItemType = data["ItemType"].ToString();
                newItem.Status = data["Status"].ToString();
                newItem.OrderNo = data["OrderNo"].ToString();
                newItem.CreatedDate = Convert.ToDateTime(data["CreatedDate"]);
                newItem.UpdateDate = Convert.ToDateTime(data["UpdateDate"]);
                newItem.UserUpdate = data["UserUpdate"].ToString();
            }

            return newItem;
        }
        private List<ImportSerialDetail> translateImportSerialDetailList(SqlDataReader reader)
        {
            List<ImportSerialDetail> ret = new List<ImportSerialDetail>();
            while (reader.Read())
            {
                var result = translateImportSerialDetail(reader);
                if (result != null)
                    ret.Add(result);
            }

            return ret;

        }

        private FujiPickingGroup translateFujiPickingGroup(SqlDataReader data,bool isAddItem)
        {
            FujiPickingGroup newItem = null;
            if (data != null)
            {
                var itemDetails = new List<ImportSerialDetail>() { };
                if (isAddItem)
                    itemDetails = GetImportSerialDetailByHeadID(data["HeadID"].ToString()).ToList();

                newItem = new FujiPickingGroup(data["HeadID"].ToString()
                    , Convert.ToInt32(data["Qty"])
                    , itemDetails);

            }

            return newItem;
        }
        private List<FujiPickingGroup> translateFujiPickingGroupList(SqlDataReader reader, bool isAddItem = false)
        {
            List<FujiPickingGroup> ret = new List<FujiPickingGroup>();
            while (reader.Read())
            {
                var result = translateFujiPickingGroup(reader, isAddItem);
                if (result != null)
                    ret.Add(result);
            }

            return ret;

        }
        #endregion

        #region =============== FUJI phase 3 Register RFID ================
        public bool RegisterRFID_HANDY(RegisterRFIDRequest registerRequest, string username)
        {
            using (var scope = new TransactionScope())
            {               

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
                        item.CreatedDate = DateTime.Now;
                        item.UpdateDate = DateTime.Now;
                        item.UserUpdate = username;
                        //Db.ImportSerialDetail.Add(item);
                        SerialDetailRepo.InsertItem(item);
                    }

                    //Db.SaveChanges();
                    scope.Complete();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                return false;
            }
        }

        public IEnumerable<ImportSerialDetail> FindImportSerialDetailByCriteria(ParameterSearch parameterSearch, out int totalRecord)
        {
            totalRecord = 0;
            string sql = "SELECT * FROM [dbo].[ImportSerialDetail]";

            IEnumerable<ImportSerialDetail> items = new List<ImportSerialDetail>();

            int cnt = parameterSearch != null && parameterSearch.Columns != null ? parameterSearch.Columns.Count : 0;

            if (cnt > 0)
            {
                if (parameterSearch.Columns.Any(a => a.Contains("InvoiceNumber")))
                {
                    sql = "SELECT B.* FROM [WIM_FUJI_DEV].[dbo].[ImportSerialHead] AS A INNER JOIN [WIM_FUJI_DEV].[dbo].[ImportSerialDetail] AS B ON A.HeadID = B.HeadID WHERE ";
                    for (int i = 0; i < cnt; i++)
                    {
                        if (parameterSearch.Columns[i] == "InvoiceNumber")
                            sql += string.Format(" A.{0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                        else if (parameterSearch.Columns[i] == "CreatedDate")
                            sql += string.Format(" {0} LIKE '%{1}%' AND ", "CONVERT(DATE,B." + parameterSearch.Columns[i] + ")", parameterSearch.Keywords[i]);
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
                        if (parameterSearch.Columns[i] == "CreatedDate")
                            sql += string.Format(" {0} LIKE '%{1}%' AND ", "CONVERT(DATE," + parameterSearch.Columns[i] + ")", parameterSearch.Keywords[i]);
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
                var output = new SqlParameter("@totalrow", SqlDbType.Int, 30);
                output.Direction = ParameterDirection.Output;

                
                items = SerialDetailRepo.SqlQuery<ImportSerialDetail>("ProcPagingImportSerialDetail @page,@size,@totalrow out"
                    , new SqlParameter("@page", parameterSearch.PageIndex)
                    , new SqlParameter("@size", parameterSearch.PageSize)
                    , output).ToList();
                totalRecord = Convert.ToInt32(output.Value);
            }

            return items;
        }


        #endregion =============== // FUJI phase 3 Register RFID ================
    }
}
