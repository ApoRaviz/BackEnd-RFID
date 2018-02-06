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
using WIM.Core.Common.Utility.Helpers;
using System.Web.Script.Serialization;

namespace Isuzu.Service.Impl.Inbound
{
    public class InboundService : IInboundService
    {
        #region stringConnection
        private string connectionString = ConfigurationManager.ConnectionStrings["WIM_ISUZU"].ConnectionString;
        #endregion 
        
        public InboundService()
        {
           
        }

        #region =========================== HANDY ===========================
        public InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder)
        {
            InboundItemHandyDto item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
               item  = (
                        from i in Db.InboundItems
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
                               select i
                           ).Any();
            }
            return isRFIDNeedRepeat;
        }

        public void RegisterInboundItem_HANDY(InboundItemHandyDto inboundItem, string username)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    try
                    {
                        InboundItems itemRFIDIsDuplicatedAnother = (
                                    from i in Db.InboundItems
                                    where i.RFIDTag == inboundItem.RFIDTag
                                    && i.ISZJOrder != inboundItem.ISZJOrder
                                    select i
                                ).SingleOrDefault();

                        if (itemRFIDIsDuplicatedAnother != null)
                        {
                            ValidationException ve = new ValidationException();
                            ve.Add(new ValidationError(((int)ErrorCode.RFIDIsDuplicatedAnother).ToString(), string.Format("RFID {0} ถูก Register โดย Order {1} ไปแล้ว ", inboundItem.RFIDTag, itemRFIDIsDuplicatedAnother.ISZJOrder)));
                            throw ve;
                        }

                        var inboundItemExist = (
                                    from i in Db.InboundItems
                                    where i.ID == inboundItem.ID
                                        && i.ISZJOrder == inboundItem.ISZJOrder
                                        && i.InvNo == inboundItem.InvNo
                                    select i
                                ).SingleOrDefault();


                        /*if (!string.IsNullOrEmpty(inboundItemExist.RFIDTag))
                        {
                            ValidationException ve = new ValidationException();
                            ve.Add(new ValidationError(((int)ErrorCode.RFIDNotEmpty).ToString(), ErrorCode.RFIDNotEmpty.GetDescription()));
                            throw ve;
                        }*/

                        inboundItemExist.RFIDTag = inboundItem.RFIDTag;
                        inboundItemExist.Status = inboundItem.Status;
                        inboundItemExist.RegisterDate = DateTime.Now;
                        DetailRepo.Update(inboundItemExist);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                }
                
            }
        }

        public int GetAmountInboundItemInInvoiceByRFID_HANDY(string rfid)
        {
            int cnt;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                var inboundItem = (
                       from i in Db.InboundItems
                       where rfid.EndsWith(i.RFIDTag)
                       && !new List<string> { "SHIPPED", "DELETED" }.Contains(i.Status)
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
                        && !new List<string> { "SHIPPED", "DELETED" }.Contains(i.Status)
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
                        && !new List<string> { "SHIPPED", "DELETED" }.Contains(i.Status)
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
                    && new List<string> { "RECEIVED", "HOLD" }.Contains(i.Status)
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

        public void PerformHolding_HANDY(InboundItemHoldingHandyRequest inboundItemHolding, string username)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);

                    var queryForHolding = (
                        from i in Db.InboundItems
                        where i.InvNo == inboundItemHolding.InvNo
                        //&& inboundItemHolding.RFIDTags.Contains(i.RFIDTag)
                        select i
                    );

                    foreach (InboundItems item in queryForHolding)
                    {
                        foreach (string scan in inboundItemHolding.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.Status = IsuzuStatus.HOLD.ToString();
                                item.HoldDate = DateTime.Now;
                                DetailRepo.Update(item);
                            }
                        }
                    }
                    Db.SaveChanges();
                    scope.Complete();
                }

                
            }
        }

        public void PerformShipping_HANDY(InboundItemShippingHandyRequest inboundItemShipping, string username)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    var queryForShipping = (
                        from i in Db.InboundItems
                        where i.InvNo == inboundItemShipping.InvNo
                        //&& inboundItemShipping.RFIDTags.Contains(i.RFIDTag)
                        select i
                    );

                    foreach (InboundItems item in queryForShipping)
                    {
                        foreach (string scan in inboundItemShipping.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.Status = IsuzuStatus.SHIPPED.ToString();
                                item.ShippingDate = DateTime.Now;
                                DetailRepo.Update(item);
                            }
                        }
                    }
                    Db.SaveChanges();
                    scope.Complete();
                } 
            }
        }

        public void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking, string username)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    var queryForPacking = (
                        from i in Db.InboundItems
                        where i.RFIDTag.EndsWith(inboundItemCartonPacking.RFIDTag)
                        && new List<string> {
                                    IsuzuStatus.RECEIVE.ToString(),
                                    IsuzuStatus.HOLD.ToString()
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

        public void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking, string username)
        {

            //var item1Invoice = (
            //        from i in Db.InboundItems
            //        where i.CaseNo == inboundItemCasePacking.CaseNo
            //        && i.InvNo != inboundItemCasePacking.InvNo
            //        select i
            //    ).FirstOrDefault();

            //if (item1Invoice != null)
            //{
            //    ValidationException ve = new ValidationException();
            //    ve.Add(new ValidationError(((int)ErrorCode.RFIDIsDuplicatedAnother).ToString(), string.Format("Case No. {0} ถูก ใช้กับ Invoice {1} ไปแล้ว ", inboundItemCasePacking.CaseNo, item1Invoice.InvNo)));
            //    throw ve;
            //}

            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    var queryForPacking = (
                        from i in Db.InboundItems
                        where new List<string> {
                                    IsuzuStatus.RECEIVE.ToString(),
                                    IsuzuStatus.HOLD.ToString()
                                }.Contains(i.Status)
                        //where inboundItemCasePacking.RFIDTags.Contains(i.RFIDTag)
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

                    Db.SaveChanges();
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
                   where new List<string> {
                                    "RECEIVED",
                                    "HOLD"
                                }.Contains(i.Status)
                   //where rfids.RFIDTags.Contains(i.RFIDTag)
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
                item = (
                           from i in Db.InboundItems
                           where i.ISZJOrder == iszjOrder
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
            string sql = "SELECT * FROM [dbo].[InboundItems] WHERE Qty=@Qty ORDER BY SeqNo";
            if (isShipped)
                sql = "SELECT * FROM [dbo].[InboundItems] WHERE Qty=@Qty AND Status=@Status ORDER BY SeqNo";

            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    try
                    {
                        items = (isShipped) ? DetailRepo.SqlQuery<InboundItems>(sql
                            , new SqlParameter("@Qty", qty)
                            , new SqlParameter("@Status", IsuzuStatus.SHIPPED.ToString())).ToList()
                            : DetailRepo.SqlQuery<InboundItems>(sql
                            , new SqlParameter("@Qty", qty)).ToList();
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
            string sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo ORDER BY SeqNo";
            if (isShipped)
                sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo AND Status=@Status ORDER BY SeqNo";

            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    try
                    {
                        items = (isShipped) ? DetailRepo.SqlQuery<InboundItems>(sql
                           , new SqlParameter("@InvNo", invNo)
                           , new SqlParameter("@Status", IsuzuStatus.SHIPPED.ToString())).ToList()
                           : DetailRepo.SqlQuery<InboundItems>(sql
                           , new SqlParameter("@Qty", invNo)).ToList();
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
                                        x.Status = IsuzuStatus.NEW.ToString();
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
                                item.Status = IsuzuStatus.NEW.ToString();
                                HeadRepo.Insert(item);
                                Db.SaveChanges();

                                item = HeadRepo.GetItemFirstBy(b => b.InvNo == i.InvNo,true);
                                i.GroupList.ForEach(x =>
                                {
                                    x.ID = Guid.NewGuid().ToString();
                                    x.Status = IsuzuStatus.NEW.ToString();
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
                        HandleValidationException(e);
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
                        item.InboundItems = (from p in item.InboundItems where p.Status != IsuzuStatus.DELETED.ToString() select p).ToList();
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
                                                        && p.Status == IsuzuStatus.SHIPPED.ToString()
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
                        HandleValidationException(e);
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
                                if (f.ISZJOrder == reason.ISZJOrder)
                                {
                                    f.Status = IsuzuStatus.DELETED.ToString();
                                    f.DeleteReason = reason.Reason;
                                    f.PathDeleteReason = reason.Paths;
                                }
                            });
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != IsuzuStatus.DELETED.ToString()).ToList().Count;
                            HeadRepo.Update(queryUpdate);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
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
                                if (f.Status != IsuzuStatus.DELETED.ToString())
                                {
                                    f.Status = IsuzuStatus.DELETED.ToString();
                                    f.DeleteReason = reason.Reason;
                                    f.PathDeleteReason = reason.Paths;
                                }
                            });
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != IsuzuStatus.DELETED.ToString()).ToList().Count;
                            queryUpdate.Status = IsuzuStatus.DELETED.ToString();
                            HeadRepo.Update(queryUpdate);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                }


            }
            return true;
        }
        public bool UpdateQtyInboundHead(string invNo,string userUpdate)
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
                            queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != IsuzuStatus.DELETED.ToString()).ToList().Count;
                            HeadRepo.Update(queryUpdate);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
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
        public IEnumerable<IsuzuTagReport> GetReportByYearRang(ParameterSearch parameterSearch, out int totalRecord)
        {
            string[] ms = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
            string[] ml = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<IsuzuTagReport> items = new List<IsuzuTagReport>();
            string result = "", startDate = "", endDate = "";
            totalRecord = 0;
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
                    items.ForEach(f =>
                    {
                        f.MonthName = ml[f.MonthNumber - 1];
                    });
                    //var q = (from p in items group p by p.YearNumber into g select new FujiTagReport(){
                    //   YearNumber = g.Key
                    //   , MonthName = "Total Tags"
                    //   , MonthNumber =  0
                    //   , ReceivedNumber = g.Sum(s => s.ReceivedNumber)
                    //   , ShippedNumber = g.Sum(s => s.ShippedNumber)
                    //   , TotalNumber = g.Sum(s => s.TotalNumber)
                    //}).ToList();

                    //items.AddRange(q);

                }

            }
            return items;
        }
        #endregion

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

        #region TranslateDataSet
        private IsuzuInboundGroup translateIsuzuInboundGroup(DataRow data)
        {
            IsuzuInboundGroup newItem = new IsuzuInboundGroup(null, 0, false);
            if (data != null)
            {
                newItem.InvNo = data["InvNo"].ToString();
                newItem.Qty = Convert.ToInt32(data["Qty"]);
                newItem.IsExport = Convert.ToBoolean(data["IsExport"]);
            }

            return newItem;
        }
        private List<IsuzuInboundGroup> translateIsuzuInboundGroupList(DataSet data)
        {
            List<IsuzuInboundGroup> ret = new List<IsuzuInboundGroup>();
            if (data.Tables["DataSet1"] != null)
            {
                var collection = data.Tables["DataSet1"].Rows;
                if (collection.Count > 0)
                {
                    foreach (DataRow item in collection)
                    {
                        var result = translateIsuzuInboundGroup(item);
                        if (result != null)
                            ret.Add(result);
                    }
                }
            }

            return ret;

        }

        private IsuzuInboundGroup translateIsuzuInboundGroup(SqlDataReader reader)
        {
            IsuzuInboundGroup newItem = new IsuzuInboundGroup(null, 0, false);
            if (reader != null)
            {
                newItem.InvNo = reader["InvNo"].ToString();
                newItem.Qty = Convert.ToInt32(reader["Qty"]);
                newItem.IsExport = Convert.ToBoolean(reader["IsExport"]);
            }

            return newItem;
        }
        private List<IsuzuInboundGroup> translateIsuzuInboundGroupList(SqlDataReader reader)
        {
            List<IsuzuInboundGroup> ret = new List<IsuzuInboundGroup>();
            while (reader.Read())
            {
                var item = translateIsuzuInboundGroup(reader);
                if (item != null)
                    ret.Add(item);
            }

            return ret;

        }


        private InboundItemsHead translateIsuzuInboundHead(SqlDataReader reader)
        {
            InboundItemsHead newItem = new InboundItemsHead();
            if (reader != null)
            {
                newItem.InvNo = reader["InvNo"].ToString();
                newItem.Qty = Convert.ToInt32(reader["Qty"]);
                newItem.IsExport = Convert.ToBoolean(reader["IsExport"]);
                newItem.Status = reader["Status"].ToString();
                newItem.Remark = reader["Remark"].ToString();
                newItem.CreateAt = reader["CreateAt"] is DBNull ? new DateTime(1900, 1, 1) : Convert.ToDateTime(reader["CreateAt"]);
                newItem.CreateBy = reader["CreateBy"].ToString();
                newItem.UpdateAt = reader["UpdateAt"] is DBNull ? new DateTime(1900, 1, 1) : Convert.ToDateTime(reader["UpdateAt"]);
                newItem.UpdateBy = reader["UpdateBy"].ToString();
                newItem.IsActive = Convert.ToBoolean(reader["IsActive"]);
            }

            return newItem;
        }
        private List<InboundItemsHead> translateIsuzuInboundHeadList(SqlDataReader reader)
        {
            List<InboundItemsHead> ret = new List<InboundItemsHead>();
            while (reader.Read())
            {
                var item = translateIsuzuInboundHead(reader);
                if (item != null)
                    ret.Add(item);
            }

            return ret;

        }

        private InboundItems translateIsuzuInboundItems(SqlDataReader reader)
        {
            InboundItems newItem = new InboundItems();
            if (reader != null)
            {
                newItem.ID = reader["ID"].ToString();
                newItem.InvNo = reader["InvNo"].ToString();
                newItem.SeqNo = Convert.ToInt32(reader["SeqNo"]);
                newItem.ITAOrder = reader["ITAOrder"].ToString();
                newItem.RFIDTag = reader["RFIDTag"].ToString();
                newItem.ISZJOrder = reader["ISZJOrder"].ToString();
                newItem.PartNo = reader["PartNo"].ToString();
                newItem.ParrtName = reader["ParrtName"].ToString();
                newItem.Qty = Convert.ToInt32(reader["Qty"]);
                newItem.Vendor = reader["Vendor"].ToString();
                newItem.Shelf = reader["Shelf"].ToString();
                newItem.Destination = reader["Destination"].ToString();
                newItem.Status = reader["Status"].ToString();
                newItem.CreateBy = reader["CreateBy"].ToString();
                newItem.CreateAt = reader["CreateAt"] is DBNull ? new DateTime(1900, 1, 1) : Convert.ToDateTime(reader["CreateAt"]);
                newItem.UpdateBy = reader["UpdateBy"].ToString();
                newItem.UpdateAt = reader["UpdateAt"] is DBNull ? new DateTime(1900, 1, 1) : Convert.ToDateTime(reader["UpdateAt"]);
                newItem.CaseNo = reader["CaseNo"].ToString();
                newItem.CartonNo = reader["CartonNo"].ToString();
                newItem.IsActive = Convert.ToBoolean(reader["IsActive"]);
                //newItem.IsExport = Convert.ToBoolean(reader["IsExport"]);
            }

            return newItem;
        }
        private List<InboundItems> translateIsuzuInboundItemsList(SqlDataReader reader)
        {
            List<InboundItems> ret = new List<InboundItems>();
            while (reader.Read())
            {
                var item = translateIsuzuInboundItems(reader);
                if (item != null)
                    ret.Add(item);
            }

            return ret;

        }

        #endregion 

    }
}
