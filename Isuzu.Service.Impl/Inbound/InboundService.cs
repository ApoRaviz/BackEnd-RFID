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
using WIM.Core.Common.Validation;
using WIM.Core.Common.Helpers;
using Isuzu.Service;
using Isuzu.Common.ValueObject;
using System.IO;
using OfficeOpenXml;
using Isuzu.Context;
using Isuzu.Entity;
using Isuzu.Repository.Impl;

namespace Isuzu.Service.Impl
{
    public class InboundService : IInboundService
    {
        //private IsuzuDataContext Db;
        private ItemsRepository DetailRepo;
        private ItemsHeadRepository HeadRepo;

        private string connectionString = ConfigurationManager.ConnectionStrings["WIM_ISUZU"].ConnectionString;
        public InboundService()
        {
            //Db = IsuzuDataContext.Create();
            DetailRepo = new ItemsRepository();
            HeadRepo = new ItemsHeadRepository();
        }

        #region =========================== HANDY ===========================
        public InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder)
        {
            var item = DetailRepo.GetItemSingleBy(s => s.ISZJOrder == iszjOrder);
            if (item != null)
            {
                return new InboundItemHandyDto
                {
                    ID = item.ID,
                    InvNo = item.InvNo,
                    ITAOrder = item.ITAOrder,
                    RFIDTag = item.RFIDTag,
                    ISZJOrder = item.ISZJOrder
                };
            }
            return new InboundItemHandyDto();
            //return (
            //            from i in Db.InboundItems
            //            select new InboundItemHandyDto
            //            {
            //                ID = i.ID,
            //                InvNo = i.InvNo,
            //                ITAOrder = i.ITAOrder,
            //                RFIDTag = i.RFIDTag,
            //                ISZJOrder = i.ISZJOrder
            //            }
            //        ).SingleOrDefault();
        }

        public bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem)
        {
            //bool isRFIDNeedRepeat = (
            //                   from i in Db.InboundItems
            //                   where i.ISZJOrder == inboundItem.ISZJOrder
            //                   && !string.IsNullOrEmpty(i.RFIDTag)
            //                   select i
            //               ).Any();

            bool isRFIDNeedRepeat = DetailRepo.IsItemExistBy(i => i.ISZJOrder == inboundItem.ISZJOrder 
                && !string.IsNullOrEmpty(i.RFIDTag));
            return isRFIDNeedRepeat;
        }

        public void RegisterInboundItem_HANDY(InboundItemHandyDto inboundItem, string username)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    //InboundItems itemRFIDIsDuplicatedAnother = (
                    //            from i in Db.InboundItems
                    //            where i.RFIDTag == inboundItem.RFIDTag
                    //            && i.ISZJOrder != inboundItem.ISZJOrder
                    //            select i
                    //        ).SingleOrDefault();

                    InboundItems itemRFIDIsDuplicatedAnother = DetailRepo.GetItemSingleBy(i => i.RFIDTag == inboundItem.RFIDTag
                                                            && i.ISZJOrder != inboundItem.ISZJOrder);

                    if (itemRFIDIsDuplicatedAnother != null)
                    {
                        ValidationException ve = new ValidationException();
                        ve.Add(new ValidationError(((int)ErrorCode.RFIDIsDuplicatedAnother).ToString(), string.Format("RFID {0} ถูก Register โดย Order {1} ไปแล้ว ", inboundItem.RFIDTag, itemRFIDIsDuplicatedAnother.ISZJOrder)));
                        throw ve;
                    }

                    //var inboundItemExist = (
                    //            from i in Db.InboundItems
                    //            where i.ID == inboundItem.ID
                    //                && i.ISZJOrder == inboundItem.ISZJOrder
                    //                && i.InvNo == inboundItem.InvNo
                    //            select i
                    //        ).SingleOrDefault();
                    var inboundItemExist = DetailRepo.GetItemSingleBy(i => i.ID == inboundItem.ID
                                    && i.ISZJOrder == inboundItem.ISZJOrder
                                    && i.InvNo == inboundItem.InvNo);


                    /*if (!string.IsNullOrEmpty(inboundItemExist.RFIDTag))
                    {
                        ValidationException ve = new ValidationException();
                        ve.Add(new ValidationError(((int)ErrorCode.RFIDNotEmpty).ToString(), ErrorCode.RFIDNotEmpty.GetDescription()));
                        throw ve;
                      
                    }*/

                    inboundItemExist.RFIDTag = inboundItem.RFIDTag;
                    inboundItemExist.Status = "RECEIVED";
                    inboundItemExist.CreateBy = username;
                    inboundItemExist.CreateAt = DateTime.Now;
                    inboundItemExist.UpdateBy = username;
                    inboundItemExist.UpdateAt = DateTime.Now;
                    DetailRepo.Update(inboundItemExist);
                    //Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
            }
        }

        public int GetAmountInboundItemInInvoiceByRFID_HANDY(string rfid)
        {
            //var inboundItem = (
            //           from i in Db.InboundItems
            //           where i.RFIDTag == rfid
            //           select i
            //       ).SingleOrDefault();
            var inboundItem = DetailRepo.GetItemSingleBy(i => i.RFIDTag == rfid);
            if (inboundItem == null)
            {
                return 0;
            }

            //var cnt = (
            //           from i in Db.InboundItems
            //           where i.InvNo == inboundItem.InvNo
            //           select i
            //       ).Count();

            var cnt = DetailRepo.GetItemsBy(i => i.InvNo == inboundItem.InvNo).Count();
            return cnt;
        }

        public InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid)
        {
            //return (
            //            from i in Db.InboundItems
            //            where i.RFIDTag == rfid
            //            select new InboundItemHandyDto
            //            {
            //                ID = i.ID,
            //                InvNo = i.InvNo,
            //                ITAOrder = i.ITAOrder,
            //                RFIDTag = i.RFIDTag,
            //                ISZJOrder = i.ISZJOrder
            //            }
            //        ).SingleOrDefault();

            var item = DetailRepo.GetItemSingleBy(i => i.RFIDTag == rfid);
            if (item != null)
            {
                return new InboundItemHandyDto
                {
                    ID = item.ID,
                    InvNo = item.InvNo,
                    ITAOrder = item.ITAOrder,
                    RFIDTag = item.RFIDTag,
                    ISZJOrder = item.ISZJOrder
                };
            }
            return new InboundItemHandyDto();
        }

        public InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid)
        {
            //return (
            //            from i in Db.InboundItems
            //            where i.RFIDTag == rfid
            //            select new InboundItemCartonHandyDto
            //            {
            //                InvNo = i.InvNo,
            //                CartonNo = i.CartonNo,
            //                RFIDTag = i.RFIDTag
            //            }
            //        ).SingleOrDefault();
            var item = DetailRepo.GetItemSingleBy(i => i.RFIDTag == rfid);
            if (item != null)
            {
                return new InboundItemCartonHandyDto
                {
                    InvNo = item.InvNo,
                    CartonNo = item.CartonNo,
                    RFIDTag = item.RFIDTag
                };
            }
            return new InboundItemCartonHandyDto();
        }

        public IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string invNo)
        {
            //var items = (
            //        from i in Db.InboundItems
            //        where i.InvNo == invNo
            //        && new List<string> { "RECEIVED", "HOLD" }.Contains(i.Status)
            //        select new InboundItemHandyDto
            //        {
            //            ID = i.ID,
            //            InvNo = i.InvNo,
            //            ITAOrder = i.ITAOrder,
            //            RFIDTag = i.RFIDTag,
            //            ISZJOrder = i.ISZJOrder
            //        }
            //    ).ToList();
            //return items;

            var items = DetailRepo.GetItemsBy(i => i.InvNo == invNo 
                                && new List<string> { "RECEIVED", "HOLD" }.Contains(i.Status)).ToList();
            if (items != null)
            {
                return (from i in items
                 select new InboundItemHandyDto
                 {
                     ID = i.ID,
                     InvNo = i.InvNo,
                     ITAOrder = i.ITAOrder,
                     RFIDTag = i.RFIDTag,
                     ISZJOrder = i.ISZJOrder
                 }).ToList();

            }
            return new List<InboundItemHandyDto>();
        }

        public void PerformHolding_HANDY(InboundItemHoldingHandyRequest inboundItemHolding, string username)
        {
            using (var scope = new TransactionScope())
            {
                //var queryForHolding = (
                //        from i in Db.InboundItems
                //        where i.InvNo == inboundItemHolding.InvNo
                //        && inboundItemHolding.RFIDTags.Contains(i.RFIDTag)
                //        select i
                //    );

                var queryForHolding = DetailRepo.GetItemsBy(i => i.InvNo == inboundItemHolding.InvNo
                        && inboundItemHolding.RFIDTags.Contains(i.RFIDTag));

                foreach (InboundItems item in queryForHolding)
                {
                    item.Status = "HOLD";
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                    DetailRepo.Update(item);
                }

                //Db.SaveChanges();
                scope.Complete();
            }
        }

        public void PerformShipping_HANDY(InboundItemShippingHandyRequest inboundItemShipping, string username)
        {
            using (var scope = new TransactionScope())
            {
                //var queryForShipping = (
                //        from i in Db.InboundItems
                //        where i.InvNo == inboundItemShipping.InvNo
                //        && inboundItemShipping.RFIDTags.Contains(i.RFIDTag)
                //        select i
                //    );

                var queryForShipping = DetailRepo.GetItemsBy(i => i.InvNo == inboundItemShipping.InvNo
                     && inboundItemShipping.RFIDTags.Contains(i.RFIDTag));

                foreach (InboundItems item in queryForShipping)
                {
                    item.Status = "SHIPPED";
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                    DetailRepo.Update(item);
                }

                //Db.SaveChanges();
                scope.Complete();
            }
        }

        public void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking, string username)
        {
            using (var scope = new TransactionScope())
            {
                //var queryForPacking = (
                //        from i in Db.InboundItems
                //        where i.RFIDTag == inboundItemCartonPacking.RFIDTag
                //        select i
                //    );

                var queryForPacking = DetailRepo.GetItemsBy(i => i.RFIDTag == inboundItemCartonPacking.RFIDTag);

                foreach (InboundItems item in queryForPacking)
                {
                    item.CartonNo = inboundItemCartonPacking.CartonNo;
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                    DetailRepo.Update(item);
                }

                //Db.SaveChanges();
                scope.Complete();
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
                //var queryForPacking = (
                //        from i in Db.InboundItems
                //        where inboundItemCasePacking.RFIDTags.Contains(i.RFIDTag)
                //        select i
                //    );

                var queryForPacking = DetailRepo.GetItemsBy(i => inboundItemCasePacking.RFIDTags.Contains(i.RFIDTag));

                foreach (InboundItems item in queryForPacking)
                {
                    item.CaseNo = inboundItemCasePacking.CaseNo.Trim();
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                    DetailRepo.Update(item);
                }
                

                //Db.SaveChanges();
                scope.Complete();
            }
        }

        public IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids)
        {
            //return (
            //        from i in Db.InboundItems
            //        where rfids.RFIDTags.Contains(i.RFIDTag)
            //        select i
            //    ).ToList();
            return DetailRepo.GetItemsBy(i => rfids.RFIDTags.Contains(i.RFIDTag));

        }
        #endregion

        #region =========================== DEFAULT ===========================
        public InboundItems GetInboundItemByISZJOrder(string iszjOrder)
        {
            return DetailRepo.GetItemSingleBy(s => s.ISZJOrder==iszjOrder);
            //return (
            //            from i in Db.InboundItems
            //            where i.ISZJOrder == iszjOrder
            //            select i
            //        ).SingleOrDefault();
        }
        public IEnumerable<InboundItems> GetInboundItemPaging(int pageIndex, int pageSize, out int totalRecord)
        {
            List<InboundItems> items = new List<InboundItems>();
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                try
                {

                    var output = new SqlParameter("@totalRecord", SqlDbType.Int, 30);
                    output.Direction = ParameterDirection.Output;

                    items = DetailRepo.SqlQuery<InboundItems>("ProcPagingInboundItems @page,@size,@totalRecord out"
                        , new SqlParameter("@page", pageIndex)
                        , new SqlParameter("@size", pageSize)
                        , output).ToList();

                    totalRecord = Convert.ToInt32(output.Value);
                }
                catch (Exception)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
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
                try
                {
                    items = (isShipped) ? DetailRepo.SqlQuery<InboundItems>(sql
                        , new SqlParameter("@Qty", qty)
                        , new SqlParameter("@Status", IsuzuStatus.SHIPPED.ToString())).ToList()
                        : DetailRepo.SqlQuery<InboundItems>(sql
                        , new SqlParameter("@Qty",qty)).ToList();
                }
                catch (Exception)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }

        }
        public IEnumerable<InboundItems> GetInboundItemByInvoiceNumber(string invNo, bool isShipped = false)
        {
            List<InboundItems> items = new List<InboundItems>() { };
            string sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo ORDER BY SeqNo";
            if (isShipped)
                sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo AND Status=@Status ORDER BY SeqNo";

            using (var scope = new TransactionScope())
            {
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

                scope.Complete();
                return items;
            }

        }
        public List<InboundItems> ImportInboundItemList(List<InboundItems> itemList, string userName)
        {
            List<InboundItems> duplicateList = new List<InboundItems>();
            List<string> isuzuOrders = itemList.Select(x => x.ISZJOrder).ToList();

            //duplicateList = (from p in Db.InboundItems
            //                 where isuzuOrders.Contains(p.ISZJOrder)
            //                 select p).ToList();

            duplicateList = DetailRepo.GetItemsBy(m => isuzuOrders.Contains(m.ISZJOrder)).ToList();
            if (duplicateList.Count > 0)
                return duplicateList;

            var itemGroups = (from p in itemList
                              group p
                              by p.InvNo into g
                              select new { InvNo = g.Key, GroupList = g.ToList() }).ToList();



            using (var scope = new TransactionScope())
            {

                try
                {
                    itemGroups.ForEach(i =>
                    {
                        
                        //if (Db.InboundItemsHead.Any(a => a.InvNo.Equals(i.InvNo)))
                        if(HeadRepo.IsItemExistBy(a => a.InvNo==i.InvNo))
                        {
                            i.GroupList.ForEach(x =>
                            {
                                x.ID = Guid.NewGuid().ToString();
                                x.CreateBy = userName;
                                x.CreateAt = DateTime.Now;
                                x.UpdateBy = userName;
                                x.UpdateAt = DateTime.Now;
                                x.Status = IsuzuStatus.NEW.ToString();
                                DetailRepo.Insert(x);
                            });
                            //var item = (from p in Db.InboundItemsHead where p.InvNo.Equals(i.InvNo) select p).FirstOrDefault();
                            var item = HeadRepo.GetItemFirstBy(f => f.InvNo==i.InvNo,true);
                            if (item != null)
                            {
                                item.Qty = item.InboundItems.Count;
                                HeadRepo.Update(item);
                            }
                                
                        }
                        else
                        {
                            InboundItemsHead item = new InboundItemsHead();
                            item.InvNo = i.InvNo;
                            i.GroupList.ForEach(x =>
                            {
                                x.ID = Guid.NewGuid().ToString();
                                x.CreateBy = userName;
                                x.CreateAt = DateTime.Now;
                                x.UpdateBy = userName;
                                x.UpdateAt = DateTime.Now;
                                x.Status = IsuzuStatus.NEW.ToString();
                                item.InboundItems.Add(x);
                            });
                            item.CreateBy = userName;
                            item.CreateAt = DateTime.Now;
                            item.UpdateBy = userName;
                            item.UpdateAt = DateTime.Now;
                            item.Status = IsuzuStatus.NEW.ToString();
                            item.Qty = i.GroupList.Count;
                            HeadRepo.Insert(item);
                        }
                    });

                    //Db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
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
                try
                {
                    items = DetailRepo.SqlQuery<InboundItems>(sql).ToList();
                }
                catch (Exception)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }

        }
        public IEnumerable<InboundItemsHead> GetInboundGroup(int max = 50)
        {
            List<InboundItemsHead> items = new List<InboundItemsHead>();

            items = HeadRepo.GetItemAll(max).ToList();

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
            InboundItemsHead item = HeadRepo.GetItemFirstBy(f => f.InvNo==invNo, isAddItems);
            //InboundItemsHead item = (from p in Db.InboundItemsHead
            //                          where p.InvNo.Equals(invNo)
            //                          select p).FirstOrDefault();
            if (item != null)
            {
                if(isAddItems)
                {
                    item.InboundItems = (from p in item.InboundItems where p.Status != IsuzuStatus.DELETED.ToString() select p).ToList();
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
            List<InboundItemsHead> items = new List<InboundItemsHead>() { };
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var output = new SqlParameter("@totalRecord", SqlDbType.Int, 30);
                    output.Direction = ParameterDirection.Output;

                    items = HeadRepo.SqlQuery<InboundItemsHead>("ProcPagingInboundItemHead @page,@size,@totalRecord out"
                        , new SqlParameter("@page", pageIndex)
                        , new SqlParameter("@size", pageSize)
                        , output).ToList();

                    totalRecord = Convert.ToInt32(output.Value);
                }
                catch (Exception)
                {
                    return new List<InboundItemsHead>() { };
                }

                scope.Complete();
                return items;
            }

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
                try
                {
                    items = HeadRepo.SqlQuery<InboundItemsHead>(sql,new SqlParameter("@keyword", keyword)).ToList();
                }
                catch (Exception)
                {
                    return new List<InboundItemsHead>() { };
                }

                scope.Complete();
                return items;
            }

        }
        public bool UpdateStausExport(InboundItemsHead item)
        {
            using (var scope = new TransactionScope())
            {
                //InboundItemsHead queryUpdateHead = (from p in Db.InboundItemsHead
                //                                    where p.InvNo.Equals(item.InvNo)
                //                                    && p.Status == IsuzuStatus.SHIPPED.ToString()
                //                                    select p).FirstOrDefault();
               
                try
                {
                    InboundItemsHead queryUpdateHead = HeadRepo.GetItemFirstBy(f => f.InvNo==item.InvNo && f.Status == IsuzuStatus.SHIPPED.ToString());
                    if (queryUpdateHead != null)
                    {
                        queryUpdateHead.IsExport = true;
                        queryUpdateHead.UpdateBy = item.UpdateBy;
                        queryUpdateHead.UpdateAt = DateTime.Now;
                        HeadRepo.Update(queryUpdateHead);
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
        public bool UpdateDeleteReason(IsuzuDeleteReason reason)
        {
            using (var scope = new TransactionScope())
            {
                //InboundItems queryUpdate = (from p in Db.InboundItems
                //                            where p.ISZJOrder.Equals(reason.ISZJOrder)
                //                            select p).FirstOrDefault();
              
                try
                {
                    InboundItems queryUpdate = DetailRepo.GetItemFirstBy(f => f.ISZJOrder==reason.ISZJOrder);
                    if (queryUpdate != null)
                    {
                        queryUpdate.Status = IsuzuStatus.DELETED.ToString();
                        queryUpdate.DeleteReason = reason.Reason;
                        queryUpdate.PathDeleteReason = reason.Paths;
                        queryUpdate.UpdateAt = DateTime.Now;
                        queryUpdate.UpdateBy = reason.UserName;
                        DetailRepo.Update(queryUpdate);
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
        public bool UpdateQtyInboundHead(string invNo)
        {
            using (var scope = new TransactionScope())
            {
                //InboundItemsHead queryUpdate = (from p in Db.InboundItemsHead
                //                                where p.InvNo.Equals(invNo)
                //                                select p).FirstOrDefault();
 
                try
                {
                    InboundItemsHead queryUpdate = HeadRepo.GetItemFirstBy(f => f.InvNo==invNo, true);
                    if (queryUpdate != null)
                    {
                        queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != IsuzuStatus.DELETED.ToString()).ToList().Count;
                        HeadRepo.Update(queryUpdate);
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
