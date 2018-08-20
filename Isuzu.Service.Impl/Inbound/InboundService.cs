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
using WIM.Core.Service.FileManagement;
using WIM.Core.Service.Impl.FileManagement;
using WIM.Core.Entity.FileManagement;
using Isuzu.Entity.InboundManagement;
using WIM.Core.Entity.Logs;

namespace Isuzu.Service.Impl.Inbound
{
    public class InboundService : IInboundService
    {

        private IFileService FileService;
        public InboundService()
        {
            this.FileService = new FileService();
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
        
        //Addition
        string statusRegistered = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.Registered.GetValueEnum());
        string statusCartonPacked = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.CartonPacked.GetValueEnum());
        string statusCasePacked = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.CasePacked.GetValueEnum());
        string statusRFIDMatched = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.RFIDMatched.GetValueEnum());

        string statusRegisteredPartial = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.RegisteredPartial.GetValueEnum());
        string statusCartonPackedPartial = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.CartonPackedPartial.GetValueEnum());
        string statusCasePackedPartial = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, IsuzuStatus.CasePackedPartial.GetValueEnum());
        


        #region =========================== HANDY ===========================
        public InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder)
        {
            InboundItemHandyDto item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
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
                             ISZJOrder = i.ISZJOrder,
                             Weight1 = i.Weight1,
                             Weight2 = i.Weight2,
                             Weight3 = i.Weight3,
                             Weight4 = i.Weight4,
                             Weight5 = i.Weight5,
                             Qty = i.Qty,
                             PartNo = i.PartNo,
                             ParrtName = i.ParrtName
                         }).SingleOrDefault();
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
                               && !new List<string> {
                                    statusShipped,
                                    statusDeleted
                                }.Contains(i.Status)
                               select i
                           ).Any();
            }
            return isRFIDNeedRepeat;
        }

        public int RegisterInboundItem_HANDY(InboundItemHandyDto item)
        {
            InboundItems itemExist;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundRepository detailRepo = new InboundRepository(db);
                    try
                    {
                        var IsUsedRFID = (from i in db.InboundItems
                                          where item.RFIDTag.EndsWith(i.RFIDTag)
                                          && !new List<string>
                                          {
                                              statusShipped,
                                              statusDeleted
                                          }.Contains(i.Status)
                                          select i).FirstOrDefault();

                        if (IsUsedRFID != null) return 0;

                        itemExist = (from i in db.InboundItems
                                     where i.ISZJOrder == item.ISZJOrder
                                     && !new List<string>
                                     {
                                         statusShipped,
                                         statusDeleted
                                     }.Contains(i.Status)
                                     select i
                                     ).SingleOrDefault();

                        itemExist.RFIDTag = item.RFIDTag;
                        itemExist.Status = statusRFIDMatched; // RFIDMATCHED 
                        itemExist.RegisterLocation = item.RegisterLocation;
                        itemExist.RegisterDate = DateTime.Now;
                        detailRepo.Update(itemExist);

                        db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
            if (itemExist != null)
            {
                UpdateHead_HANDY2(itemExist.InvNo, statusRFIDMatched);
            }
            return 1;
        }

        public InboundItemHandyDto RegisterInboundItemByOrder_HANDY(InboundItemHandyDto item)
        {
            InboundItems itemExist;
            InboundItemHandyDto itemReturn;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundRepository detailRepo = new InboundRepository(db);
                    try
                    {
                        //itemExist = (from i in db.InboundItems
                        //             where i.ISZJOrder == item.ISZJOrder &&
                        //             i.Status == statusNew
                        //             select i
                        //             ).SingleOrDefault();

                        //if (itemExist == null) return null;

                        itemExist = (from i in db.InboundItems
                                     where i.ISZJOrder == item.ISZJOrder
                                     && !new List<string>
                                        {
                                            statusShipped,
                                            statusDeleted
                                        }.Contains(i.Status)
                                     select i
                                     ).SingleOrDefault();
                        if (itemExist == null) return null;

                        if (itemExist.Status == statusNew)
                        {
                            itemReturn = new InboundItemHandyDto
                            {
                                InvNo = itemExist.InvNo,
                                ISZJOrder = itemExist.ISZJOrder,
                                Status = itemExist.Status
                            };
                            itemExist.Status = statusRegistered;
                            itemExist.RegisterLocation = item.RegisterLocation;
                            itemExist.RegisterDate = DateTime.Now;

                            detailRepo.Update(itemExist);
                            db.SaveChanges();
                        }
                        else
                        {
                            itemReturn = new InboundItemHandyDto
                            {
                                InvNo = itemExist.InvNo,
                                ISZJOrder = itemExist.ISZJOrder,
                                Status = itemExist.Status
                            };
                        }
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }
            }
            if (itemExist != null && itemExist.Status == statusRegistered)
            {
                UpdateHead_HANDY2(itemExist.InvNo, statusRegistered);
            }
            return itemReturn;
        }

        public void UpdateHead_HANDY2(string invNo, string status)
        {
            Task.Run(() =>
            {
                using (var scope = new TransactionScope())
                {
                    using (IsuzuDataContext db = new IsuzuDataContext())
                    {
                        IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                        try
                        {
                            string statusLastest = null;
                            var statusList = db.InboundItems.Where(w => w.InvNo == invNo && w.Status != statusDeleted).Select(s => s.Status).Distinct().ToList();
                            int countStatus = statusList.Count;

                            //if (statusList.Contains(statusCasePacked))
                            //{
                            //    status = statusCasePacked;
                            //}
                            //else if (statusList.Contains(statusCartonPacked))
                            //{
                            //    status = statusCartonPacked;
                            //}
                            //else if (statusList.Contains(statusRFIDMatched))
                            //{
                            //    status = statusRFIDMatched;
                            //}

                            switch (countStatus)
                            {
                                case 1:
                                    statusLastest = status;
                                    break;
                                default:
                                    if (new List<string> { statusCasePacked, statusCartonPacked, statusRFIDMatched }.Contains(status))
                                    {
                                        statusLastest = status;
                                    }
                                    else
                                    {
                                        statusLastest = status + "_PARTIAL";
                                    }
                                    break;
                            }

                            var head = headRepo.GetByID(invNo);
                            head.Status = statusLastest;
                            headRepo.Update(head);
                            db.SaveChanges();
                            scope.Complete();
                        }
                        catch (DbEntityValidationException e)
                        {
                            throw new AppValidationException(e);
                        }
                    }
                }
            });
        }

        private void UpdateHead_HANDY(InboundItems item)
        {
            List<InboundItems> inboundItems = new List<InboundItems>();
            inboundItems.Add(item);
            UpdateHead_HANDY(inboundItems);
        }

        private void UpdateHead_HANDY(IEnumerable<InboundItems> items)
        {
            Task.Run(() =>
            {
                using (var scope = new TransactionScope())
                {
                    using (IsuzuDataContext db = new IsuzuDataContext())
                    {
                        IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                        try
                        {
                            IEnumerable<string> invoiceList = items.Select(x => x.InvNo).Distinct();
                            IQueryable queryable = headRepo.GetMany(i =>
                                invoiceList.Contains(i.InvNo)
                            ).AsQueryable();

                            foreach (InboundItemsHead item in queryable)
                            {
                                item.Status = item.Status;
                                headRepo.Update(item);
                            }

                            db.SaveChanges();
                            scope.Complete();
                        }
                        catch (DbEntityValidationException e)
                        {
                            throw new AppValidationException(e);
                        }
                    }
                }
            });
        }

        private void Register()
        {
            ///////
            ///////
            ///////
          

            UpdateHead_HANDY2("V180234", "REGISTERED_ITA");
        }

        //private void UpdateHead_HANDY2(string invNo, string status)
        //{
        //    using (IsuzuDataContext db = new IsuzuDataContext())
        //    {
        //        string statusLatest = null;
        //        var statusList = db.InboundItems.Where(w => w.InvNo == invNo).Select(s => s.Status).Distinct();
        //        int countStatus = statusList.Count();
               
        //        switch (countStatus)
        //        {
        //            case 1:
        //                /*
        //                   REGISTERED_ITA
        //                   REGISTERED_ITA
        //                   REGISTERED_ITA
        //                   REGISTERED_ITA
        //                   REGISTERED_ITA
        //                */
        //                statusLatest = status;
        //                break;
        //            case 2:
        //                /*
        //                  NEW
        //                  NEW
        //                  REGISTERED_ITA
        //                  REGISTERED_ITA
        //                  REGISTERED_ITA
        //                */
        //                statusLatest = status + "_PARTIAL";
        //                break;

        //            default:
        //                break;
        //        }

        //        IInboundHeadRepository headRepo = new InboundHeadRepository(db);
        //        var head = headRepo.GetByID(invNo);
        //        head.Status = statusLatest;
        //    }
        //}

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

        public List<RegisterRemaining> GetAmountNewStatusRemaining_HANDY(string invoice)
        {
            List<RegisterRemaining> remainlist = new List<RegisterRemaining>();
            using (IsuzuDataContext db = new IsuzuDataContext())
            {
                if(invoice == "all")
                {
                    remainlist = (from i in db.InboundItems
                                  where new List<string> {
                                  statusNew,
                                  statusRegistered
                                  }.Contains(i.Status)
                                  group i by i.InvNo into g
                                  select new RegisterRemaining
                                  {
                                      InvNo = g.Key,
                                      Qty = g.Count(),
                                      Registered = g.Sum(x => x.Status != statusNew ? 1 : 0),
                                      Remaining = g.Sum(x => x.Status == statusNew ? 1 : 0)
                                  }).Where(x => x.Remaining > 0).OrderByDescending(x => x.Registered).ThenBy(u => u.Remaining).ToList();
                }
                else
                {
                    var invoicelist = invoice.Trim().Split(',');
                    remainlist = (from i in db.InboundItems
                                  where invoicelist.Contains(i.InvNo) &&
                                  new List<string> {
                                      statusNew,
                                      statusRegistered
                                  }.Contains(i.Status)
                                  group i by i.InvNo into g
                                  select new RegisterRemaining
                                  {
                                      InvNo = g.Key,
                                      Qty = g.Count(),
                                      Registered = g.Sum(x => x.Status != statusNew ? 1 : 0),
                                      Remaining = g.Sum(x => x.Status == statusNew ? 1 : 0)
                                  }).OrderByDescending(x => x.Remaining).ToList();
                }
            }
            return remainlist;
        }

        public List<InboundItemHandyDto> GetUnregisteredOrder_HANDY(string invoice)
        {
            List<InboundItemHandyDto> remainlist = new List<InboundItemHandyDto>();
            using (IsuzuDataContext db = new IsuzuDataContext())
            {
                remainlist = (from i in db.InboundItems
                              where i.InvNo == invoice &&
                                    i.Status == statusNew
                              select new InboundItemHandyDto
                              {
                                  ISZJOrder = i.ISZJOrder,
                                  PartNo = i.PartNo,
                                  ParrtName = i.ParrtName
                              }).ToList();
            }
            return remainlist;
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
                   ).FirstOrDefault();
                if (inboundItem == null)
                {
                    return 0;
                }

                cnt = (from i in Db.InboundItems
                       where i.InvNo == inboundItem.InvNo
                       && !new List<string>
                       {
                           statusShipped,
                           statusDeleted
                       }.Contains(i.Status)
                       select i ).Count();
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
                            ISZJOrder = i.ISZJOrder,
                            Qty = i.Qty,
                            Weight1 = i.Weight1,
                            Weight2 = i.Weight2,
                            Weight3 = i.Weight3,
                            Weight4 = i.Weight4,
                            Weight5 = i.Weight5,
                            PartNo = i.PartNo,
                            ParrtName = i.ParrtName
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
                       ISZJOrder = i.ISZJOrder,
                       ParrtName = i.ParrtName,
                       PartNo = i.PartNo,
                       Qty = i.Qty,
                       Vendor = i.Vendor
                   }
               ).ToList();
            }
            return items;

        }

        public IEnumerable<InboundItemHandyDto> GetInboundItemsRegisteredByInvoice_HANDY(string invNo)
        {
            IEnumerable<InboundItemHandyDto> items;

            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                items = (
                   from i in Db.InboundItems
                   where i.InvNo == invNo
                   && new List<string> {
                           statusRegisteredAtITA,
                           statusRegisteredAtYUT
                       }.Contains(i.Status)

                   select new InboundItemHandyDto
                   {
                       ID = i.ID,
                       InvNo = i.InvNo,
                       ITAOrder = i.ITAOrder,
                       RFIDTag = i.RFIDTag,
                       ISZJOrder = i.ISZJOrder,
                       ParrtName = i.ParrtName,
                       PartNo = i.PartNo,
                       Qty = i.Qty,
                       Vendor = i.Vendor
                   }
               ).ToList();
            }
            return items;

        }

        public void PerformHolding_HANDY(List<ConfirmReceiveParameter> itemsHolding)
        {
            IEnumerable<InboundItems> itemsForSave;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundHeadRepository headRepo = new InboundHeadRepository(db);
                    IInboundRepository detailRepo = new InboundRepository(db);

                    itemsForSave = detailRepo.GetMany(i =>
                    itemsHolding.Select(x => x.InvNo).Contains(i.InvNo)
                    && itemsHolding.Select(x => x.ISZJOrder).Contains(i.ISZJOrder)
                    && new List<string> {
                           statusRegisteredAtITA,
                           statusRegisteredAtYUT
                       }.Contains(i.Status)
                    );

                    foreach (InboundItems item in itemsForSave)
                    {
                        item.Status = statusReceivedAtYUT;
                        item.HoldDate = DateTime.Now;
                        detailRepo.Update(item);
                    }

                    db.SaveChanges();
                    scope.Complete();
                }
            }

            if (itemsForSave != null)
            {
                UpdateHead_HANDY(itemsForSave);
                InsertRFIDTagNotFoundLog(itemsForSave, "RECEIVING-ISUZU");
            }
        }

        public void PerformShipping_HANDY(InboundItemShippingHandyRequest itemsShipping)
        {
            List<InboundItems> items;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundRepository detailRepo = new InboundRepository(db);
                    items = (from i in db.InboundItems
                             where i.InvNo == itemsShipping.InvNo
                             && !new List<string> {
                               statusNew,
                               statusShipped,
                               statusDeleted
                               }.Contains(i.Status)
                             select i).ToList();
                    
                    foreach (InboundItems item in items)
                    {
                        item.Status = statusShipped;
                        item.ShippingDate = DateTime.Now;
                        detailRepo.Update(item);
                    }
                    db.SaveChanges();
                    scope.Complete();
                    //Pram comment : อาจจะไม่จำเป็นต้อง check tag เพราะตอน ship ต้องยก order ทั้ง Inv ที่หน้าบ้านอยู่แล้ว
                    //foreach (InboundItems item in items)
                    //{
                    //    foreach (string scan in itemsShipping.RFIDTags)
                    //    {
                    //        if (scan.EndsWith(item.RFIDTag))
                    //        {
                    //            item.Status = statusShipped;
                    //            item.ShippingDate = DateTime.Now;
                    //            detailRepo.Update(item);
                    //        }
                    //    }
                    //}
                }
            }
            if (items.Any())
            {
                UpdateHead_HANDY2(items[0].InvNo, statusShipped);
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

        public int PerformPackingCartonNew_HANDY(InboundItemCartonPackingHandyRequestNew inboundItemCartonPacking)
        {
            List<InboundItems> queryForPacking;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    if(inboundItemCartonPacking.function == "Packing")
                    {
                        var IsUsedRFID = (from i in Db.InboundItems
                                          where inboundItemCartonPacking.RFIDTag.EndsWith(i.RFIDTag)
                                          && !new List<string>
                                          {
                                              statusShipped,
                                              statusDeleted
                                          }.Contains(i.Status)
                                          select i).FirstOrDefault();

                        if (IsUsedRFID != null) return 0;
                    }

                    queryForPacking = ( from i in Db.InboundItems
                                            where inboundItemCartonPacking.OrderScannedList.Contains(i.ISZJOrder) &&
                                                 !new List<string> {
                                                   statusNew,
                                                   statusShipped,
                                                   statusDeleted
                                                 }.Contains(i.Status)
                                                 select i ).ToList();

                    foreach (InboundItems item in queryForPacking)
                    {
                        item.CartonNo = inboundItemCartonPacking.CartonNo;
                        item.RFIDTag = inboundItemCartonPacking.RFIDTag;
                        item.Status = statusCartonPacked;
                        item.PackCartonDate = DateTime.Now;
                        DetailRepo.Update(item);
                    }
                    Db.SaveChanges();
                    scope.Complete();
                }
            }
            if(queryForPacking.Any())
            {
               UpdateHead_HANDY2(queryForPacking[0].InvNo, statusCartonPacked);
            }
            return 1;
        }

        public InboundItemCartonPackingHandyRequestNew GetItemCartonByISZJOrder_HANDY(string ISZJOrder)
        {
            InboundItemCartonPackingHandyRequestNew item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
                        where i.ISZJOrder == ISZJOrder &&
                            !new List<string> {
                            statusNew,
                            statusShipped,
                            statusDeleted
                            }.Contains(i.Status)
                        select new InboundItemCartonPackingHandyRequestNew
                        {
                            InvNo = i.InvNo,
                            ISZJOrder = i.ISZJOrder,
                            CartonNo = i.CartonNo
                        }).FirstOrDefault();
            }
            return item;
        }

        public List<InboundItemCartonPackingHandyRequest> GetCartonNoByInvoice_HANDY(string InvNo)
        {
            List<InboundItemCartonPackingHandyRequest> item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
                        where i.InvNo == InvNo &&
                        i.CartonNo != null &&
                        !new List<string> {
                            statusNew,
                            statusShipped,
                            statusDeleted
                        }.Contains(i.Status)
                        select new InboundItemCartonPackingHandyRequest
                        {
                            CartonNo = i.CartonNo
                        }).Distinct().ToList();
            }
            return item;
        }

        public List<InboundItemCartonPackingHandyRequestNew> GetCartonPackedItemByRFID_HANDY(string rfid)
        {
            List<InboundItemCartonPackingHandyRequestNew> item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
                        where rfid.EndsWith(i.RFIDTag) &&
                              i.Status == statusCartonPacked
                        select new InboundItemCartonPackingHandyRequestNew
                        {
                            InvNo = i.InvNo,
                            ISZJOrder = i.ISZJOrder,
                            CartonNo = i.CartonNo,
                            CaseNo = i.CaseNo
                        }).ToList();
            }
            return item;
        }

        public void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking)
        {
            InboundItems InvStore = null;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(db);
                    var queryForPacking = (from i in db.InboundItems
                                       where !new List<string> {
                                            statusNew,
                                            statusShipped,
                                            statusDeleted
                                       }.Contains(i.Status) &&
                                       i.RFIDTag != null
                                       select i);

                    foreach (InboundItems item in queryForPacking)
                    {
                        foreach (string scan in inboundItemCasePacking.RFIDTags)
                        {
                            if (scan.EndsWith(item.RFIDTag))
                            {
                                item.CaseNo = inboundItemCasePacking.CaseNo.Trim();
                                item.PackCaseDate = DateTime.Now;
                                item.Status = statusCasePacked;
                                DetailRepo.Update(item);
                                InvStore = item;
                            }
                        }
                    }

                    db.SaveChanges();
                    scope.Complete();
                }
            }
            if(InvStore != null)
            {
                UpdateHead_HANDY2(InvStore.InvNo, statusCasePacked);
            }
        }

        public List<InboundItemCasePackingHandyRequest> GetCaseNoByInvoice_HANDY(string InvNo)
        {
            List<InboundItemCasePackingHandyRequest> item;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                item = (from i in Db.InboundItems
                        where i.InvNo == InvNo &&
                        i.CaseNo != null &&
                        !new List<string> {
                            statusNew,
                            statusShipped,
                            statusDeleted
                        }.Contains(i.Status)
                        select new InboundItemCasePackingHandyRequest
                        {
                            CaseNo = i.CaseNo
                        }).Distinct().ToList();
            }
            return item;
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
                    }.Contains(i.Status) &&
                    i.RFIDTag != null
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

        public void InsertRFIDTagNotFoundLog(IEnumerable<InboundItems> inboundItems, string functionName)
        {
            Task.Run(() =>
            {
                using (var scope = new TransactionScope())
                {
                    using (IsuzuDataContext db = new IsuzuDataContext())
                    {
                        IInboundRepository detailRepo = new InboundRepository(db);
                        try
                        {
                            switch (functionName)
                            {
                                case "RECEIVING-ISUZU":
                                    IEnumerable<string> invNoList = inboundItems.Select(x => x.InvNo).Distinct();
                                    inboundItems = detailRepo.GetMany(i =>
                                         invNoList.Contains(i.InvNo)
                                         && new List<string> {
                                            statusRegisteredAtITA,
                                            statusRegisteredAtYUT
                                         }.Contains(i.Status)
                                     );
                                    break;
                                default:
                                    break;
                            }

                            foreach (var item in inboundItems)
                            {
                                db.Set<RFIDTagNotFoundLog>().Add(new RFIDTagNotFoundLog
                                {
                                    IDRef = item.ID,
                                    FunctionName = functionName,
                                    CreateBy = "SYSTEM",
                                    UpdateBy = "SYSTEM"
                                });
                            }
                            db.SaveChanges();
                            scope.Complete();
                        }
                        catch (DbEntityValidationException e)
                        {
                            throw new AppValidationException(e);
                        }
                    }
                }
            });
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
        public List<InboundItems> ImportInboundItemList(List<InboundItems> itemList)
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
                                     && !new List<string> {
                                          statusShipped,
                                          statusDeleted
                                        }.Contains(p.Status)
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
                            if (HeadRepo.IsItemExistBy(a => a.InvNo == i.InvNo))
                            {
                                
                                var item = HeadRepo.GetItemFirstBy(f => f.InvNo == i.InvNo);
                                var allItemNew = DetailRepo.GetItemFirstBy(f => f.InvNo == i.InvNo && f.Status != statusNew);
                                if (item != null && allItemNew == null)
                                {
                                    i.GroupList.ForEach(x =>
                                    {
                                        x.ID = Guid.NewGuid().ToString();
                                        x.Status = statusNew;
                                        DetailRepo.Insert(x);
                                    });
                                    Db.SaveChanges();

                                    item = HeadRepo.GetItemFirstBy(f => f.InvNo == i.InvNo, true);
                                    item.Qty = item.InboundItems.Count;
                                    HeadRepo.Update(item);
                                    Db.SaveChanges();
                                }
                                else
                                {
                                    throw new AppValidationException(ErrorEnum.ImportStatusNotNew);
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
                                    DetailRepo.Insert(x);
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
                        throw new AppValidationException(e);
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
                    if (parameterSearch.Columns[i].ToUpper() == "RFIDTAG")
                    {
                        string dec = Convert.ToInt32(parameterSearch.Keywords[i]).ToString("X");
                        sql += string.Format(" {0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], dec);
                    }
                    else
                    {
                        sql += string.Format(" {0} LIKE '%{1}%' AND ", parameterSearch.Columns[i], parameterSearch.Keywords[i]);
                    }



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
        public IEnumerable<InboundItems> GetDataImportByKeyword(string keyword, int pageIndex, int pageSize, out int totalRecord)
        {
            IEnumerable<InboundItems> items = new List<InboundItems>() { };
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    try
                    {
                        items = Db.ProcPagingInboundItemSearching(keyword, pageIndex, pageSize, out totalRecord);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        return new List<InboundItems>() { };
                    }

                }
                return items;
            }
        }
        public IEnumerable<GeneralLog> GetOrderLogByID(string refID)
        {
            IEnumerable<GeneralLog> items = new List<GeneralLog>();
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    try
                    {
                        items = (from i in Db.GeneralLogs
                                 where i.RefID == refID
                                 select i).ToList();
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        return new List<GeneralLog>() { };
                    }

                }
                return items;
            }
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
        public IEnumerable<InboundItemsHead> GetDataGroupByKeyword(string keyword,int pageIndex, int pageSize, out int totalRecord)
        {
            IEnumerable<InboundItemsHead> items = new List<InboundItemsHead>() { };
            totalRecord = 0;
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundHeadRepository HeadRepo = new InboundHeadRepository(Db);
                    try
                    {
                        items = Db.ProcPagingInboundItemHeadSearching(keyword,pageIndex, pageSize, out totalRecord);
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
                        throw new AppValidationException(e);
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
                        throw new AppValidationException(e);
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
                        throw new AppValidationException(e);
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
                        throw new AppValidationException(e);
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
                listDuplicateInbound = ImportInboundItemList(inboundList);
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

        public string CreateDeletedFileID(string pathName)
        {
            string fileID = "";
            if (!string.IsNullOrEmpty(pathName))
            {
                File_MT fileMt = new File_MT();
                //fileMt.FileUID = Guid.NewGuid().ToString();
                fileMt.FileName = pathName;
                fileMt.LocalName = pathName;
                fileMt.PathFile = Path.GetExtension(pathName);
                fileID = FileService.CreateFile(fileMt);
            }

            return fileID;
            //FileService(files);
        }

        public void GetDeletedFileID(string fileID)
        {
            if (!string.IsNullOrEmpty(fileID))
            {
                FileService.GetFile(fileID);
            }
        }

        public InboundItemHandyDto GetBeforeAdjustWeight(InboundItemHandyDto adjustWeight)
        {
            InboundItemHandyDto adjustWeightReturn;
            using (IsuzuDataContext Db = new IsuzuDataContext())
            {
                adjustWeightReturn = (
                               from i in Db.InboundItems
                               where i.ISZJOrder == adjustWeight.ISZJOrder
                               //&& i.Weight.CompareTo((decimal)0) == 1
                               && !new List<string> {
                                    statusDeleted,
                                    statusShipped
                                }.Contains(i.Status)
                               select new InboundItemHandyDto
                               {
                                   ISZJOrder = i.ISZJOrder,
                                   PartNo = i.PartNo,
                                   ParrtName = i.ParrtName,
                                   Weight1 = i.Weight1,
                                   Weight2 = i.Weight2,
                                   Weight3 = i.Weight3,
                                   Weight4 = i.Weight4,
                                   Weight5 = i.Weight5,
                                   Qty = i.Qty,
                                   IsRepeat = 0
                                   //IsRepeat = Decimal.Compare(i.Weight, 0) == 0 ? 2 :
                                   //Decimal.Compare(i.Weight, adjustWeight.Weight) == 0 ? 3 : 4
                               }
                           ).SingleOrDefault();

                if (adjustWeightReturn == null)
                {
                    return new InboundItemHandyDto
                    {
                        ISZJOrder = adjustWeight.ISZJOrder,
                        Weight1 = adjustWeight.Weight1,
                        Weight2 = adjustWeight.Weight2,
                        Weight3 = adjustWeight.Weight3,
                        Weight4 = adjustWeight.Weight4,
                        Weight5 = adjustWeight.Weight5,
                        IsRepeat = -1,
                        WeightCursor = adjustWeight.WeightCursor
                    };
                }
                else
                {
                    adjustWeightReturn.WeightCursor = adjustWeight.WeightCursor;
                    switch (adjustWeight.WeightCursor)
                    {
                        default:
                        case 1:
                            adjustWeightReturn.IsRepeat = Decimal.Compare(adjustWeightReturn.Weight1, 0) == 0 ? 2 :
                                   Decimal.Compare(adjustWeightReturn.Weight1, adjustWeight.Weight1) == 0 ? 3 : 4;
                            break;
                        case 2:
                            adjustWeightReturn.IsRepeat = Decimal.Compare(adjustWeightReturn.Weight2, 0) == 0 ? 2 :
                                   Decimal.Compare(adjustWeightReturn.Weight2, adjustWeight.Weight2) == 0 ? 3 : 4;
                            break;
                        case 3:
                            adjustWeightReturn.IsRepeat = Decimal.Compare(adjustWeightReturn.Weight3, 0) == 0 ? 2 :
                                   Decimal.Compare(adjustWeightReturn.Weight3, adjustWeight.Weight3) == 0 ? 3 : 4;
                            break;
                        case 4:
                            adjustWeightReturn.IsRepeat = Decimal.Compare(adjustWeightReturn.Weight4, 0) == 0 ? 2 :
                                   Decimal.Compare(adjustWeightReturn.Weight4, adjustWeight.Weight4) == 0 ? 3 : 4;
                            break;
                        case 5:
                            adjustWeightReturn.IsRepeat = Decimal.Compare(adjustWeightReturn.Weight5, 0) == 0 ? 2 :
                                   Decimal.Compare(adjustWeightReturn.Weight5, adjustWeight.Weight5) == 0 ? 3 : 4;
                            break;
                    }
                }
            }
            return adjustWeightReturn;
        }

        public void AdjustWeight(InboundItemHandyDto adjustWeight)
        {
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    IInboundRepository DetailRepo = new InboundRepository(Db);
                    InboundItems inboundItems = (
                        from i in Db.InboundItems
                        where (adjustWeight.ISZJOrder == i.ISZJOrder)
                        && !new List<string> {
                           statusDeleted
                       }.Contains(i.Status)
                        select i
                    ).SingleOrDefault();

                    switch (adjustWeight.WeightCursor)
                    {
                        default:
                        case 1:
                            inboundItems.Weight1 = adjustWeight.Weight1;
                            break;
                        case 2:
                            inboundItems.Weight2 = adjustWeight.Weight2;
                            break;
                        case 3:
                            inboundItems.Weight3 = adjustWeight.Weight3;
                            break;
                        case 4:
                            inboundItems.Weight4 = adjustWeight.Weight4;
                            break;
                        case 5:
                            inboundItems.Weight5 = adjustWeight.Weight5;
                            break;
                    }
                    inboundItems.WeightDate = DateTime.Now;


                    DetailRepo.Update(inboundItems);
                    Db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public IEnumerable<InvoiceReportDetail> GetInvoiceHistory(InvHistoryFilter filter)
        {
            string sql = "";
            string startDate = "'" + filter.startDate.ToString("yyyy-MM-dd 00:00:00") + "'";
            string endDate = "'" + filter.endDate.ToString("yyyy-MM-dd 23:59:59") + "'";
            sql += "select a.InvNo,a.Status,a.CreateAt,count(*) as QtyOrder,sum(b.Qty) as QtyItem, " +
                   "min(b.RegisterDate) as RegisterStart,max(b.RegisterDate) as RegisterEnd, " +
                   "min(b.PackCartonDate) as CartonStart,max(b.PackCartonDate) as CartonEnd, " +
                   "min(b.PackCaseDate) as CaseStart,max(b.PackCaseDate) as CaseEnd, " +
                   "min(b.ShippingDate) as ShipStart,max(b.ShippingDate) as ShipEnd, " +
                   "count(distinct(b.CartonNo)) as totalCarton, count(distinct(b.CaseNo)) as totalCase " +
                   "from InboundItemsHead a inner join InboundItems b on a.InvNo = b.InvNo " +
                   "where a.CreateAt >= " + startDate +
                   "and a.CreateAt <= " + endDate;
            if (filter.status != null && filter.status != "All")
            {
                sql += "and a.Status = '" + filter.status + "'";
            }

            sql += "group by a.InvNo,a.Status,a.CreateAt order by a.CreateAt desc";

            List<InvoiceReportDetail> items = new List<InvoiceReportDetail>() { };
            using (var scope = new TransactionScope())
            {
                using (IsuzuDataContext Db = new IsuzuDataContext())
                {
                    try
                    {
                        items = Db.Database.SqlQuery<InvoiceReportDetail>(sql).ToList();
                    }
                    catch (Exception ex)
                    {
                        return new List<InvoiceReportDetail>() { };
                    }

                }
                return items;
            }
        }

        #region AsyncMethod 

        #endregion

        #endregion

    }
}