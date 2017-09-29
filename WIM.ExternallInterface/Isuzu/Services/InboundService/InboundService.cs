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
using WIM.Common.Validation;
using WIM.ExternallInterface.Isuzu.Dtos;

namespace WIM.ExternallInterface.Isuzu.Services.InboundService
{
    public class InboundService : IInboundService
    {
        private WIM_ISUZU_DEVEntities Db = new WIM_ISUZU_DEVEntities();

        private string connectionString = ConfigurationManager.ConnectionStrings["WIM_ISUZU"].ConnectionString;
        public InboundService()
        {
            Db = new WIM_ISUZU_DEVEntities();
        }

        #region =========================== HANDY ===========================
        public InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder)
        {
            return (
                        from i in Db.InboundItems
                        where i.ISZJOrder == iszjOrder
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

        public bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem)
        {
            bool isRFIDNeedRepeat = (
                               from i in Db.InboundItems
                               where i.ISZJOrder == inboundItem.ISZJOrder
                               && !string.IsNullOrEmpty(i.RFIDTag)
                               select i
                           ).Any();
            return isRFIDNeedRepeat;
        }

        public void RegisterInboundItem_HANDY(InboundItemHandyDto inboundItem, string username)
        {
            using (var scope = new TransactionScope())
            {
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
                    inboundItemExist.Status = "RECEIVED";
                    inboundItemExist.CreateBy = username;
                    inboundItemExist.CreateAt = DateTime.Now;
                    inboundItemExist.UpdateBy = username;
                    inboundItemExist.UpdateAt = DateTime.Now;

                    Db.SaveChanges();
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
            var inboundItem = (
                       from i in Db.InboundItems
                       where i.RFIDTag == rfid
                       select i
                   ).SingleOrDefault();

            if (inboundItem == null)
            {
                return 0;
            }

            var cnt = (
                       from i in Db.InboundItems
                       where i.InvNo == inboundItem.InvNo
                       select i
                   ).Count();

            return cnt;
        }

        public InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid)
        {
            return (
                        from i in Db.InboundItems
                        where i.RFIDTag == rfid
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

        public InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid)
        {
            return (
                        from i in Db.InboundItems
                        where i.RFIDTag == rfid
                        select new InboundItemCartonHandyDto
                        {
                            InvNo = i.InvNo,
                            CartonNo = i.CartonNo,
                            RFIDTag = i.RFIDTag
                        }
                    ).SingleOrDefault();
        }

        public IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string invNo)
        {
            var items = (
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
            return items;
        }

        public void PerformHolding_HANDY(InboundItemHoldingHandyRequest inboundItemHolding, string username)
        {
            using (var scope = new TransactionScope())
            {
                var queryForHolding = (
                        from i in Db.InboundItems
                        where i.InvNo == inboundItemHolding.InvNo
                        && inboundItemHolding.RFIDTags.Contains(i.RFIDTag)
                        select i
                    );

                foreach (InboundItems item in queryForHolding)
                {
                    item.Status = "HOLD";
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                }

                Db.SaveChanges();
                scope.Complete();
            }
        }

        public void PerformShipping_HANDY(InboundItemShippingHandyRequest inboundItemShipping, string username)
        {
            using (var scope = new TransactionScope())
            {
                var queryForShipping = (
                        from i in Db.InboundItems
                        where i.InvNo == inboundItemShipping.InvNo
                        && inboundItemShipping.RFIDTags.Contains(i.RFIDTag)
                        select i
                    );

                foreach (InboundItems item in queryForShipping)
                {
                    item.Status = "SHIPPED";
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                }

                Db.SaveChanges();
                scope.Complete();
            }
        }

        public void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking, string username)
        {
            using (var scope = new TransactionScope())
            {
                var queryForPacking = (
                        from i in Db.InboundItems
                        where i.RFIDTag == inboundItemCartonPacking.RFIDTag
                        select i
                    );

                foreach (InboundItems item in queryForPacking)
                {
                    item.CartonNo = inboundItemCartonPacking.CartonNo;
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                }

                Db.SaveChanges();
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
                var queryForPacking = (
                        from i in Db.InboundItems
                        where inboundItemCasePacking.RFIDTags.Contains(i.RFIDTag)
                        select i
                    );

                foreach (InboundItems item in queryForPacking)
                {
                    item.CaseNo = inboundItemCasePacking.CaseNo.Trim();
                    item.UpdateBy = username;
                    item.UpdateAt = DateTime.Now;
                }

                Db.SaveChanges();
                scope.Complete();
            }
        }

        public IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids)
        {
            return (
                    from i in Db.InboundItems
                    where rfids.RFIDTags.Contains(i.RFIDTag)
                    select  i
                ).ToList();
        }
        #endregion

        #region =========================== DEFAULT ===========================
        public InboundItems GetInboundItemByISZJOrder(string iszjOrder)
        {
            return (
                        from i in Db.InboundItems
                        where i.ISZJOrder == iszjOrder
                        select i
                    ).SingleOrDefault();
        }
        public List<InboundItems> GetInboundItemPaging(int pageIndex, int pageSize, out int totalRecord)
        {
            List<InboundItems> items = new List<InboundItems>();
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("ProcPagingInboundItems", con))
                        {
                            cmd.Parameters.Add("@page", SqlDbType.Int).Value = pageIndex;
                            cmd.Parameters.Add("@size", SqlDbType.Int).Value = pageSize;
                            cmd.Parameters.Add("@totalRecord", SqlDbType.Int, 30);
                            cmd.Parameters["@totalRecord"].Direction = ParameterDirection.Output;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //dset = new DataSet();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if(reader.HasRows)
                                 items = translateIsuzuInboundItemsList(reader);
                            }
                            totalRecord = Convert.ToInt32(cmd.Parameters["@totalRecord"].Value);
                        }
                    }
                }
                catch (Exception e)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }

        }
        public List<InboundItems> GetInboundItemByQty(int qty, bool isShipped = false)
        {
            List<InboundItems> items = new List<InboundItems>() { };
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM [dbo].[InboundItems] WHERE Qty=@Qty ORDER BY SeqNo";
                        if (isShipped)
                            sql = "SELECT * FROM [dbo].[InboundItems] WHERE Qty=@Qty AND Status=@Status ORDER BY SeqNo";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = qty;
                            if(isShipped)
                                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = IsuzuStatus.SHIPPED.ToString();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                items = translateIsuzuInboundItemsList(reader);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }
            //return (from i in Db.InboundItems select i).Take(Qty).ToList();
        }
        public List<InboundItems> GetInboundItemByInvoiceNumber(string invNo,bool isShipped = false)
        {
            List<InboundItems> items = new List<InboundItems>() { };
            using (var scope = new TransactionScope())
            {
                try
                {
                    
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo ORDER BY SeqNo";
                        if (isShipped)
                            sql = "SELECT * FROM [dbo].[InboundItems] WHERE InvNo=@InvNo AND Status=@Status ORDER BY SeqNo";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = invNo;
                            if(isShipped)
                                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = IsuzuStatus.SHIPPED.ToString();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                items = translateIsuzuInboundItemsList(reader);
                            }
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }

            //return (from i in Db.InboundItems where i.InvNo.Equals(invNo) select i).ToList();
        }
        public List<InboundItems> ImportInboundItemList(List<InboundItems> itemList,string userName)
        {
            List<InboundItems> duplicateList = new List<InboundItems>();
            List<string> isuzuOrders = itemList.Select(x => x.ISZJOrder).ToList();

            duplicateList = (from p in Db.InboundItems
                     where isuzuOrders.Contains(p.ISZJOrder)
                     select p).ToList();
            if (duplicateList.Count > 0)
                return duplicateList;

            var itemGroups = (from p in itemList
                              group p
                              by p.InvNo into g
                              select new { InvNo = g.Key, GroupList = g.ToList()}).ToList();

           

            using (var scope = new TransactionScope())
            {

                try
                {
                    itemGroups.ForEach(i =>
                    {
                    if (Db.InboundItemsHead.Any(a => a.InvNo.Equals(i.InvNo)))
                    {
                        i.GroupList.ForEach(x =>
                        {
                            x.ID = Guid.NewGuid().ToString();
                            x.CreateBy = userName;
                            x.CreateAt = DateTime.Now;
                            x.UpdateBy = userName;
                            x.UpdateAt = DateTime.Now;
                            x.Status = IsuzuStatus.NEW.ToString();
                            Db.InboundItems.Add(x);
                        });
                        var item = (from p in Db.InboundItemsHead where p.InvNo.Equals(i.InvNo) select p).FirstOrDefault();
                            if (item != null)
                                item.Qty = item.InboundItems.Count();
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
                        item.Qty = i.GroupList.Count();
                        item.IsExport = false;
                        item.Remark = "";
                        Db.InboundItemsHead.Add(item);
                        }
                    });

                Db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
            }

            return new List<InboundItems>();
        }
        public IEnumerable<InboundItems> GetDataByColumn(string column, string keyword)
        {
            string sql = "";
            switch (column.Trim().ToUpper())
            {
                default:
                case "INVNO":
                    sql += "SELECT * FROM [dbo].[InboundItems] WHERE [InvNo] LIKE '%' + @keyword + '%' ";
                    break;
                case "ISZJORDER":
                    sql += "SELECT * FROM [dbo].[InboundItems] WHERE [ISZJOrder] LIKE '%' + @keyword + '%' ";
                    break;
                
            }

            DataSet dset = new DataSet();
            List<InboundItems> items = new List<InboundItems>() { };
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add("@keyword", SqlDbType.VarChar).Value = keyword;
                            dset = new DataSet();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                items = translateIsuzuInboundItemsList(reader);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    return new List<InboundItems>() { };
                }

                scope.Complete();
                return items;
            }

        }
        public List<InboundItemsHead> GetInboundGroup(int max = 50)
        {
            List<InboundItemsHead> items = new List<InboundItemsHead>();

            items = (from p in Db.InboundItemsHead select p).Take(max).ToList();

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
        public InboundItemsHead GetInboundGroupByInvoiceNumber(string invNo)
        {
            InboundItemsHead items = (from p in Db.InboundItemsHead where p.InvNo.Equals(invNo)
                     select  p).FirstOrDefault();
            if (items != null)
            {
                items.InboundItems = (from p in items.InboundItems where p.Status != IsuzuStatus.DELETED.ToString() select p).ToList();
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


            return items;
        }
        public List<InboundItemsHead> GetInboundGroupPaging(int pageIndex,int pageSize,out int totalRecord)
        {
            DataSet dset = new DataSet();
            List<InboundItemsHead> items = new List<InboundItemsHead>() { };
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("ProcPagingInboundItemHead", con))
                        {
                            cmd.Parameters.Add("@page", SqlDbType.Int).Value = pageIndex;
                            cmd.Parameters.Add("@size", SqlDbType.Int).Value = pageSize;
                            cmd.Parameters.Add("@totalRecord", SqlDbType.Int, 30);
                            cmd.Parameters["@totalRecord"].Direction = ParameterDirection.Output;
                            cmd.CommandType = CommandType.StoredProcedure;
                            dset = new DataSet();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                items = translateIsuzuInboundHeadList(reader);
                            }
                            totalRecord = Convert.ToInt32(cmd.Parameters["@totalRecord"].Value);
                        }
                    }
                }
                catch (Exception e)
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
                    sql += "SELECT * FROM (SELECT [InvNo],COUNT([InvNo]) AS Qty,[IsExport]  FROM [dbo].[InboundItems] GROUP BY [InvNo],[IsExport]) AS Temp WHERE Temp.[InvNo] LIKE '%' + @keyword + '%' ";
                    break;
            }

            DataSet dset = new DataSet();
            List<InboundItemsHead> items = new List<InboundItemsHead>() { };
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var con = new SqlConnection(this.connectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add("@keyword", SqlDbType.VarChar).Value = keyword;
                            dset = new DataSet();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                items = translateIsuzuInboundHeadList(reader);
                            }
                            
                        }
                    }
                }
                catch (Exception e)
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
                InboundItemsHead queryUpdateHead = (from p in Db.InboundItemsHead
                                                     where p.InvNo.Equals(item.InvNo)
                                                     && p.Status == IsuzuStatus.SHIPPED.ToString()
                                                     select p).FirstOrDefault();
                if(queryUpdateHead != null)
                {
                    queryUpdateHead.IsExport = true;
                    queryUpdateHead.UpdateBy = item.UpdateBy;
                    queryUpdateHead.UpdateAt = DateTime.Now;
                }
                   
                try
                {
                    Db.SaveChanges();
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
                InboundItems queryUpdate = (from p in Db.InboundItems
                                                    where p.ISZJOrder.Equals(reason.ISZJOrder)
                                                    select p).FirstOrDefault();
                if (queryUpdate != null)
                {
                    queryUpdate.Status = IsuzuStatus.DELETED.ToString();
                    queryUpdate.DeleteReason = reason.Reason;
                    queryUpdate.PathDeleteReason = reason.Paths;
                    queryUpdate.UpdateAt = DateTime.Now;
                    queryUpdate.UpdateBy = reason.UserName;
                    Db.SaveChanges();
                }
                scope.Complete();
                return true;
            }

        }
        public bool UpdateQtyInboundHead(string invNo)
        {
            using (var scope = new TransactionScope())
            {
                InboundItemsHead queryUpdate = (from p in Db.InboundItemsHead
                                            where p.InvNo.Equals(invNo)
                                            select p).FirstOrDefault();
                if (queryUpdate != null)
                {
                    queryUpdate.Qty = queryUpdate.InboundItems.Where(w => w.Status != IsuzuStatus.DELETED.ToString()).ToList().Count();
                    Db.SaveChanges();
                }
                scope.Complete();
                return true;
            }
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
            IsuzuInboundGroup newItem = new IsuzuInboundGroup(null,0,false);
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
            IsuzuInboundGroup newItem = new IsuzuInboundGroup(null, 0,false);
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
