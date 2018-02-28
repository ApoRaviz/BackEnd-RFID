using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Isuzu.Service;
using Isuzu.Common.ValueObject;
using System.IO;
using OfficeOpenXml;
using Isuzu.Context;
using Isuzu.Entity;
using Isuzu.Repository.Impl;
using System.Security.Principal;
using Isuzu.Repository.ItemManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using System.Web.Script.Serialization;
using System.Data.Entity;
using WIM.Core.Service.Impl.StatusManagement;

namespace Isuzu.Service.Impl.Inbound
{
    public class InboundService : IInboundService
    {
        public InboundService()
        {
        }

        private const int _SUBMODULE_ID = 11;

        string statusNew = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.New.GetValueEnum());
        string statusDeleted = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Deleted.GetValueEnum());
        string statusHold = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Hold.GetValueEnum());
        string statusImported = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Imported.GetValueEnum());
        string statusReceived = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Received.GetValueEnum());
        string statusReceivedAtITA = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.ReceivedAtITA.GetValueEnum());
        string statusReceivedAtYUT = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.ReceivedAtYUT.GetValueEnum());
        string statusRegisteredAtITA = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.RegisteredAtITA.GetValueEnum());
        string statusRegisteredAtYUT = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.RegisteredAtYUT.GetValueEnum());
        string statusShipped = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Shipped.GetValueEnum());
        #region =========================== HANDY ===========================
        public InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder)
        {
            InboundItemHandyDto item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (
                         from i in Db.InboundItems
                         where i.ISZJOrder == iszjOrder
                         && !new List<string> {
                                    statusShipped,
                                    statusDeleted
                                }.Contains(i.Status)
                         select new InboundItemHandyDto
                         {
                             ID = i.ID,
                             InvNo = i.InvNo,
                             ITAOrder = i.ITAOrder,
                             RFIDTag = i.RFIDTag,
                             ISZJOrder = i.ISZJOrder
                         }
                     ).SingleOrDefault();
            }
            return item;
        }

        public bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem)
        {
            bool isRFIDNeedRepeat;
          

            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                isRFIDNeedRepeat = (
                               from i in Db.InboundItems
                               where i.ISZJOrder == inboundItem.ISZJOrder
                               && !string.IsNullOrEmpty(i.RFIDTag)
                               && i.Status != statusDeleted
                               select i
                           ).Any();
            }
            return isRFIDNeedRepeat;
        }

        public void RegisterInboundItem_HANDY(InboundItemHandyDto item)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                    IInboundRepository detailRepo = new InboundRepository(db);
                    db.InboundItems.AsParallel();
                    try
                    {
                        bool isDupAnother = detailRepo.Exists(i =>
                            i.RFIDTag == item.RFIDTag
                            && i.ISZJOrder != item.ISZJOrder
                            && !new List<string> {
                                    statusShipped,
                                    statusDeleted
                                }.Contains(i.Status)
                        );

                        if (isDupAnother)
                        {
                            throw new ValidationException(ErrorEnum.RFIDIsDuplicatedAnother);
                        }

                        InboundItems itemExist = detailRepo.GetItemSingleBy(i => i.ISZJOrder == item.ISZJOrder);

                        itemExist.RFIDTag = item.RFIDTag;
                        itemExist.Status = item.Status;
                        itemExist.RegisterDate = DateTime.Now;
                        detailRepo.Update(itemExist);

                        // #Update Head
                        InboundItemsHead itemHeadExist = headRepo.GetByID(item.InvNo);

                        itemHeadExist.Status = item.Status;
                        headRepo.Update(itemHeadExist);

                        db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                }
            }
        }

        public int GetAmountRegistered_HANDY()
        {
            int cnt = 0;

            using (var Db = new IsuzuDataContext())
            {
                cnt = (
                       from i in Db.InboundItems
                       where new List<string> {
                           statusRegisteredAtYUT,
                           statusRegisteredAtITA
                       }.Contains(i.Status)
                       select i
                   ).Count();
            }
            return cnt;
        }

        public int GetAmountInboundItemInInvoiceByRFID_HANDY(string rfid)
        {
            int cnt;

            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                var inboundItem = (
                       from i in Db.InboundItems
                       where rfid.EndsWith(i.RFIDTag)
                       && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                       select i
                   ).SingleOrDefault();
                if (inboundItem == null)
                {
                    return 0;
                }

                cnt = (
                          from i in Db.InboundItems
                          where i.InvNo == inboundItem.InvNo
                          select i
                      ).Count();
            }
            return cnt;
        }

        public InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid)
        {
            InboundItemHandyDto item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (
                        from i in Db.InboundItems
                        where rfid.EndsWith(i.RFIDTag)
                        && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                        select new InboundItemHandyDto
                        {
                            ID = i.ID,
                            InvNo = i.InvNo,
                            ITAOrder = i.ITAOrder,
                            RFIDTag = i.RFIDTag,
                            ISZJOrder = i.ISZJOrder
                        }
                    ).SingleOrDefault();
            }
            return item;

        }

        public InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid)
        {
            InboundItemCartonHandyDto item;
          
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (
                        from i in Db.InboundItems
                        where rfid.EndsWith(i.RFIDTag)
                        && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                        select new InboundItemCartonHandyDto
                        {
                            InvNo = i.InvNo,
                            CartonNo = i.CartonNo,
                            RFIDTag = i.RFIDTag
                        }
                    ).SingleOrDefault();
            }
            return item;
        }

        public IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string invNo)
        {
            IEnumerable<InboundItemHandyDto> items;
           
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                items = (
                   from i in Db.InboundItems
                   where i.InvNo == invNo
                   && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                   select new InboundItemHandyDto
                   {
                       ID = i.ID,
                       InvNo = i.InvNo,
                       ITAOrder = i.ITAOrder,
                       RFIDTag = i.RFIDTag,
                       ISZJOrder = i.ISZJOrder
                   }
               ).ToList();
            }
            return items;

        }

        public void PerformHolding_HANDY(InboundItemHoldingHandyRequest itemsHolding)
        {
         
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                    IInboundRepository detailRepo = new InboundRepository(db);

                    IEnumerable<InboundItems> items = detailRepo.GetMany(i =>
                        new List<string> {
                           statusRegisteredAtITA,
                           statusRegisteredAtYUT
                        }.Contains(i.Status)
                    );

                    List<string> invNoList = new List<string>(); // #For Update Head
                    foreach (InboundItems item in items)
                    {
                        foreach (string scan in itemsHolding.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.Status = statusReceivedAtYUT;
                                item.HoldDate = DateTime.Now;
                                detailRepo.Update(item);
                                invNoList.Add(item.InvNo); // #For Update Head
                            }
                        }
                    }

                    // #Update Head
                    invNoList = invNoList.Distinct().ToList();
                    foreach (var invNo in invNoList)
                    {
                        InboundItemsHead itemHeadExist = headRepo.Get(i =>
                            i.InvNo == invNo
                        );
                        itemHeadExist.Status = statusReceivedAtYUT;
                        headRepo.Update(itemHeadExist);
                    }

                    db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void PerformShipping_HANDY(InboundItemShippingHandyRequest itemsShipping)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                    IInboundRepository detailRepo = new InboundRepository(db);

                    IEnumerable<InboundItems> items = detailRepo.GetMany(i =>
                        i.InvNo == itemsShipping.InvNo
                        && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                    );
                    foreach (InboundItems item in items)
                    {
                        foreach (string scan in itemsShipping.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.Status = statusShipped;
                                item.ShippingDate = DateTime.Now;
                                detailRepo.Update(item);
                            }
                        }
                    }

                    // #Update Head
                    InboundItemsHead itemHeadExist = headRepo.Get(i =>
                            i.InvNo == itemsShipping.InvNo
                        );
                    itemHeadExist.Status = statusShipped;
                    headRepo.Update(itemHeadExist);

                    db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    var queryForPacking = (
                        from i in Db.InboundItems
                        where (inboundItemCartonPacking.RFIDTag == i.ISZJOrder || i.RFIDTag.EndsWith(inboundItemCartonPacking.RFIDTag))
                        && !new List<string> {
                           statusNew,
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                        select i
                    );

                    foreach (InboundItems item in queryForPacking)
                    {
                        item.CartonNo = inboundItemCartonPacking.CartonNo;
                        item.PackCartonDate = DateTime.Now;
                        DetailRepo.Update(item);
                    }
                    Db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(db);
                    var queryForPacking = (
                        from i in db.InboundItems
                        where !new List<string> {
                            statusNew,
                            statusShipped,
                           statusDeleted
                    }.Contains(i.Status)
                        select i
                    );

                    foreach (InboundItems item in queryForPacking)
                    {
                        foreach (string scan in inboundItemCasePacking.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.CaseNo = inboundItemCasePacking.CaseNo.Trim();
                                item.PackCaseDate = DateTime.Now;
                                DetailRepo.Update(item);
                            }
                        }
                    }

                    db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids)
        {
            List<InboundItems> inboundItems = new List<InboundItems>();
         
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                var query = (
                   from i in Db.InboundItems
                   where !new List<string> {
                        statusNew,
                        statusShipped,
                        statusDeleted
                    }.Contains(i.Status)
                   select i
               );

                foreach (InboundItems item in query)
                {
                    foreach (string scan in rfids.RFIDTags)
                    {
                        if (scan.EndsWith(item.RFIDTag))
                        {
                            inboundItems.Add(item);
                        }
                    }
                }
            }
            return inboundItems;

        }
        #endregion

        #region =========================== DEFAULT ===========================
        public InboundItems GetInboundItemByISZJOrder(string iszjOrder)
        {
            InboundItems item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
                        where i.ISZJOrder == iszjOrder
                        && !new List<string> {
                                    //statusShipped,
                                    statusDeleted
                                }.Contains(i.Status)
                        select i).SingleOrDefault();
            }
            return item;
        }
        public IEnumerable<InboundItems> GetInboundItemPaging(int pageIndex, int pageSize, out int totalRecord)
        {
            IEnumerable<InboundItems> items = new List<InboundItems>();
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    try
                    {
                        items = Db.ProcPagingInboundItems(pageIndex, pageSize, out totalRecord);
                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        return new List<InboundItems>() { };
                    }
                }

            }
            return items;

        }
        public IEnumerable<InboundItems> GetInboundItemDeletedPaging(int pageIndex, int pageSize, out int totalRecord)
        {
            IEnumerable<InboundItems> items = new List<InboundItems>();
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    try
                    {
                        items = Db.ProcPagingInboundItemsDeleted(pageIndex, pageSize, out totalRecord);
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        return new List<InboundItems>() { };
                    }
                }


                return items;
            }

        }
        public IEnumerable<InboundItems> GetInboundItemByQty(int qty, bool isShipped = false)
         {
            List<InboundItems> items = new List<InboundItems>() { };

            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    try
                    {
                        items = (from p in Db.InboundItems
                                 where p.Qty == qty
                                 orderby p.SeqNo
                                 select p).ToList();

                        if (isShipped)
                            items = items.Where(x => x.Status == statusShipped).ToList();

                    }
                    catch (Exception)
                    {
                        return new List<InboundItems>() { };
                    }
                }

                scope.Complete();

            }
            return items;

        }
        public IEnumerable<InboundItems> GetInboundItemByInvoiceNumber(string invNo, bool isShipped = false)
        {
            List<InboundItems> items = new List<InboundItems>() { };
           
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    try
                    {
                        items = (from p in Db.InboundItems
                                 where p.InvNo == invNo
                                 orderby p.SeqNo
                                 select p).ToList();
                        if (isShipped)
                            items = items.Where(w => w.Status == statusShipped).ToList();
                    }
                    catch (Exception)
                    {
                        return new List<InboundItems>() { };
                    }
                }

                scope.Complete();
            }
            return items;
        }
        public List<InboundItems> ImportInboundItemList(List<InboundItems> itemList, string userName)
        {
            List<InboundItems> duplicateList = new List<InboundItems>();
            List<string> isuzuOrders = itemList.Select(x => x.ISZJOrder).ToList();
            
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    duplicateList = (from p in Db.InboundItems
                                     where isuzuOrders.Contains(p.ISZJOrder)
                                     select p).ToList();

                    if (duplicateList.Count > 0)
                        return duplicateList;

                    var itemGroups = (from p in itemList
                                      group p
                                      by p.InvNo into g
                                      select new { InvNo = g.Key, GroupList = g.ToList() }).ToList();

                    try
                    {
                        itemGroups.ForEach(i =>
                        {

                            //if (Db.InboundItemsHead.Any(a => a.InvNo.Equals(i.InvNo)))
                            if (HeadRepo.IsItemExistBy(a => a.InvNo == i.InvNo))
                            {

                                //var item = (from p in Db.InboundItemsHead where p.InvNo.Equals(i.InvNo) select p).FirstOrDefault();
                                var item = HeadRepo.GetItemFirstBy(f => f.InvNo == i.InvNo, true);
                                if (item != null)
                                {
                                    i.GroupList.ForEach(x =>
                                    {
                                        x.ID = Guid.NewGuid().ToString();
                                        x.Status = statusNew;
                                        item.InboundItems.Add(x);
                                    });

                                    item.Qty = item.InboundItems.Count;
                                    HeadRepo.Update(item);
                                    Db.SaveChanges();
                                }

                            }
                            else
                            {
                                InboundItemsHead item = new InboundItemsHead();
                                item.InvNo = i.InvNo;
                                item.Status = statusNew;
                                HeadRepo.Insert(item);
                                Db.SaveChanges();

                                item = HeadRepo.GetItemFirstBy(b => b.InvNo == i.InvNo, true);
                                i.GroupList.ForEach(x =>
                                {
                                    x.ID = Guid.NewGuid().ToString();
                                    x.Status = statusNew;
                                    item.InboundItems.Add(x);
                                });
                                item.Qty = i.GroupList.Count;
                                item.IsExport = false;
                                HeadRepo.Update(item);
                                Db.SaveChanges();
                            }
                        });


                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }

                }
            }

            return new List<InboundItems>();
        }
        public IEnumerable<InboundItems> GetDataByColumn(ParameterSearch parameterSearch)
        {
            List<InboundItems> items = new List<InboundItems>() { };
            string sql = "SELECT * FROM [dbo].[InboundItems]";

            int cnt = parameterSearch.Columns.Any() ? parameterSearch.Columns.Count : 0;

            if (cnt > 0)
            {
                sql += " WHERE ";
                for (int i = 0; i < cnt; i++)
                {
                    sql += string.Format(" {0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                }
                sql = sql.Substring(0, sql.Length - 4);
                sql += " AND [Status] <> 'DELETED'";
            }


            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    try
                    {
                        items = DetailRepo.SqlQuery<InboundItems>(sql).ToList();
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        return new List<InboundItems>() { };
                    }
                }

            }
            return items;

        }
        public IEnumerable<InboundItemsHead> GetInboundGroup(int max = 50)
        {
            List<InboundItemsHead> items = new List<InboundItemsHead>();
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                items = HeadRepo.GetItemAll(max).ToList();
            }

            //var itemGroups = (from p in Db.InboundItems
            //                orderby p.CreateAt descending
            //                group p
            //                by p into g
            //                select new { GroupID = g.Key.InvNo, GroupList = g.ToList() ,IsExport = g.Key.IsExport}).Take(max).ToList();

            //itemGroups.ForEach(f => {
            //    IsuzuInboundGroup item = new IsuzuInboundGroup(f.GroupID, f.GroupList.Count(),f.IsExport);
            //    items.Add(item);
            //});

            return items;
        }
        public InboundItemsHead GetInboundGroupByInvoiceNumber(string invNo, bool isAddItems = false)
        {
            InboundItemsHead item;
          
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);

                item = (from p in Db.InboundItemsHead
                        where p.InvNo.Equals(invNo)
                        select p).FirstOrDefault();
                if (item != null)
                {
                    if (isAddItems)
                    {
                        Db.Entry(item).Collection(c => c.InboundItems).Load();
                        item.InboundItems = (from p in item.InboundItems where p.Status != statusDeleted select p).ToList();
                    }
                }
            }
            //items = items.ForEach(f => {
            //    if(f.InboundItems.FirstOrDefault().Status == IsuzuStatus.DELETED.ToString())
            //      f.InboundItems.Remove(f)
            //    });
            //var itemGroups = (from p in Db.InboundItems
            //                  orderby p.CreateAt descending
            //                  where p.InvNo.Equals(invNo)
            //                  group p
            //                  by p into g
            //                  select new { GroupID = g.Key.InvNo, GroupList = g.ToList(),IsExport = g.Key.IsExport }
            //                  ).ToList();

            //itemGroups.ForEach(f => {
            //    IsuzuInboundGroup item = new IsuzuInboundGroup(f.GroupID, f.GroupList.Count(),f.IsExport);
            //    items.Add(item);
            //});


            return item;
        }

        public IEnumerable<InboundItemsHead> GetInboundGroupPaging(int pageIndex, int pageSize, out int totalRecord)
        {
            DataSet dset = new DataSet();
            IEnumerable<InboundItemsHead> items = new List<InboundItemsHead>() { };
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    try
                    {
                        items = Db.ProcPagingInboundItemHead(pageIndex, pageSize, out totalRecord);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                        return new List<InboundItemsHead>() { };
                    }
                }



            }
            return items;

        }
        public IEnumerable<InboundItemsHead> GetDataGroupByColumn(string column, string keyword)
        {
            string sql = "";
            switch (column.Trim().ToUpper())
            {
                default:
                case "INVNO":
                    sql += "SELECT * FROM InboundItemsHead WHERE [InvNo] LIKE '%' + @keyword + '%' ";
                    break;
            }
            List<InboundItemsHead> items = new List<InboundItemsHead>() { };
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    try
                    {
                        items = HeadRepo.SqlQuery<InboundItemsHead>(sql, new SqlParameter("@keyword", keyword)).ToList();
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        return new List<InboundItemsHead>() { };
                    }

                }

                return items;
            }

        }
        public bool UpdateStausExport(InboundItemsHead item)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    InboundItemsHead queryUpdateHead = (from p in Db.InboundItemsHead
                                                        where p.InvNo.Equals(item.InvNo)
                                                        && p.Status == statusShipped
                                                        select p).FirstOrDefault();

                    try
                    {

                        if (queryUpdateHead != null)
                        {
                            queryUpdateHead.IsExport = true;
                            HeadRepo.Update(queryUpdateHead);
                        }
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
        public bool UpdateDeleteReason(IsuzuDeleteReason reason)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    var queryUpdate = HeadRepo.GetItemFirstBy(f => f.InvNo == reason.InvNo, true);

                    try
                    {
                        if (queryUpdate != null)
                        {
                            queryUpdate.InboundItems.ToList().ForEach(f =>
                            {
                                if (reason.ISZJOrder.Contains(f.ISZJOrder))
                                {
                                    f.Status = statusDeleted;
                                    f.DeleteReason = reason.Reason;
                                    f.PathDeleteReason = reason.Paths;
                                }
                            });
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != statusDeleted).ToList().Count;
                            HeadRepo.Update(queryUpdate);
                        }
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
        public bool UpdateDeleteReasonByInvoice(string InvNo, IsuzuDeleteReason reason)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    var queryUpdate = HeadRepo.GetItemFirstBy(f => f.InvNo == InvNo, true);

                    try
                    {
                        if (queryUpdate != null)
                        {
                            queryUpdate.InboundItems.ToList().ForEach(f =>
                            {
                                if (f.Status != statusDeleted)
                                {
                                    f.Status = statusDeleted;
                                    f.DeleteReason = reason.Reason;
                                    f.PathDeleteReason = reason.Paths;
                                }
                            });
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != statusDeleted).ToList().Count;
                            queryUpdate.Status = statusDeleted;
                            HeadRepo.Update(queryUpdate);
                        }
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
        public bool UpdateQtyInboundHead(string invNo, string userUpdate)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    InboundItemsHead queryUpdate = (from p in Db.InboundItemsHead
                                                    where p.InvNo.Equals(invNo)
                                                    select p).FirstOrDefault();

                    try
                    {
                        if (queryUpdate != null)
                        {
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != statusDeleted).ToList().Count;
                            HeadRepo.Update(queryUpdate);
                        }
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
        public IsuzuDataImport OpenReadExcel(string localFileName)
        {
            List<InboundItems> listDuplicateInbound = new List<InboundItems>();
            List<InboundItems> inboundList = new List<InboundItems>();
            IsuzuDataImport ret = new IsuzuDataImport();

            int num = new Random().Next(100);
            string dir = System.IO.Path.GetDirectoryName(localFileName);
            string newFileName = dir + "\\" + "IMPORT_" + num + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            System.IO.File.Copy(localFileName, newFileName + ".xlsx");
            System.IO.File.Delete(localFileName);

            string path = newFileName + ".xlsx";

            #region ExcelPackage
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets.First();
                if (ws != null)
                {

                    for (int i = 2; i <= ws.Dimension.End.Row; i++)
                    {
                        if (!string.IsNullOrEmpty(ws.Cells[i, 1].Text))
                        {
                            InboundItems inbound = new InboundItems()
                            {
                                InvNo = IsuzuReportHelper.GetIsuzuAutoGenHeader(ws.Cells[i, 1].Text),
                                SeqNo = Convert.ToInt32(ws.Cells[i, 2].Text),
                                ITAOrder = ws.Cells[i, 3].Text,
                                ISZJOrder = ws.Cells[i, 4].Text,
                                PartNo = ws.Cells[i, 5].Text,
                                ParrtName = ws.Cells[i, 6].Text,
                                Qty = Convert.ToInt32(ws.Cells[i, 7].Text),
                                Vendor = ws.Cells[i, 8].Text,
                                Shelf = ws.Cells[i, 9].Text,
                                Destination = ws.Cells[i, 10].Text
                            };
                            if (!string.IsNullOrEmpty(inbound.ISZJOrder))
                                inboundList.Add(inbound);
                        }
                    }
                }
            }
            #endregion

            var duplicateKeys = inboundList.GroupBy(gb => gb.ISZJOrder)
                 .Where(w => w.Count() > 1)
                 .Select(s => s.FirstOrDefault());


            if (duplicateKeys.Count() > 0)
            {

                listDuplicateInbound = duplicateKeys.ToList();
                ret.isDuplicated = true;
                ret.listItem = listDuplicateInbound;
            }
            else
            {
                listDuplicateInbound = ImportInboundItemList(inboundList, "SYSTEM");
                if (listDuplicateInbound.Count > 0)
                {
                    ret.isDuplicated = true;
                    ret.listItem = listDuplicateInbound;

                }
                else
                {
                    ret.isDuplicated = false;
                    ret.listItem = inboundList;
                }
            }



            return ret;
        }
        public string GetRFIDInfo(ParameterSearch parameter)
        {
            using (var Db = new IsuzuDataContext())
            {
                return Db.Database.SqlQuery<string>("ProcGetRFIDInfo").FirstOrDefault();
            }
        }
        public IEnumerable<IsuzuTagReport> GetReportByYearRang(ParameterSearch parameterSearch)
        {
            string[] ms = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
            string[] ml = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<IsuzuTagReport> items = new List<IsuzuTagReport>();
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

            using (IsuzuDataContext db = new IsuzuDataContext())
            {
                result = db.Database.SqlQuery<string>("ProcGetRFIDInfoByCreateAt @StartDate,@EndDate"
                    , new SqlParameter("@StartDate", startDate)
                    , new SqlParameter("@EndDate", endDate)).SingleOrDefault();
            }

            if (!string.IsNullOrEmpty(result))
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                items = json_serializer.Deserialize<List<IsuzuTagReport>>(result);
                if (items != null)
                {
                    foreach (var f in items)
                    {
                        f.MonthName = ml[f.MonthNumber - 1];
                    }
                }

            }
            return items;
        }

        #region AsyncMethod 

        #endregion

        #endregion

    }
}
