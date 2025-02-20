﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Service.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.InventoryManagement;
using WMS.Entity.Receiving;
using WMS.Entity.WarehouseManagement;
using WMS.Repository;
using WMS.Repository.Impl;
using WMS.Repository.Impl.InventoryManage;
using WMS.Repository.InvenoryManagement;
using WMS.Repository.Warehouse;

namespace WMS.Service.Impl
{
    public class ReceiveService : WIM.Core.Service.Impl.Service, IReceiveService
    {
        public IEnumerable<Receive> GetReceives()
        {
            IEnumerable<Receive> receives;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReceiveRepository repo = new ReceiveRepository(Db);
                receives = repo.Get();
            }
            return receives;
        }

        public ReceiveDto GetReceiveByReceiveIDSys(int id)
        {
            ReceiveDto receive;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReceiveRepository repo = new ReceiveRepository(Db);
                string[] include = { "InventoryTransaction" };
                receive = repo.GetReceiveDtoByID(id);
            }
            return receive;
        }

        public int CreateReceive(ReceiveDto receives)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead
                }))
            {
                Receive newReceive = new Receive();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        Receive receive = new CommonService().AutoMapper<Receive>(receives);
                        IReceiveRepository repo = new ReceiveRepository(Db);
                        newReceive = repo.Insert(receive);
                        Db.SaveChanges();


                        if (receives.InventoryTransactions != null)
                        {
                            IInventoryTransactionRepository repoTran = new InventoryTransactionRepository(Db);
                            IInventoryRepository repoInven = new InventoryRepository(Db);
                            IInventoryDetailRepository repoInvenDe = new InventoryDetailRepository(Db);
                            IInventoryTransactionDetailRepository repoTranDe = new InventoryTransactionDetailRepository(Db);
                            ILocationRepository repoLoc = new LocationRepository(Db);
                            ISpareFieldDetailRepository repoSparefd = new SpareFieldDetailRepository(Db);
                            List<Location> location = new List<Location>();
                            List<InventoryTransaction> inventran = new List<InventoryTransaction>();

                            var realinvengroup = receives.InventoryTransactions.GroupBy(a => new { a.ControlLevel1, a.Expire, a.Inspect, a.LocIDSys, a.ControlLevel2, a.ControlLevel3, a.ItemIDSys })
                                .Select(b => new
                                {
                                    ControlLevel1 = b.Key.ControlLevel1,
                                    Expire = b.Key.Expire.HasValue ? b.Key.Expire : null,
                                    ControlLevel2 = b.Key.ControlLevel2,
                                    Inspect = b.Key.Inspect,
                                    LocIDSys = b.Key.LocIDSys,
                                    ControlLevel3 = b.Key.ControlLevel3,
                                    ItemIDSys = b.Key.ItemIDSys,
                                    Child = b.ToList()
                                }).ToList();
                            List<int> listLocation = new List<int>();
                            for (int i = 0; i < realinvengroup.Count; i++)
                            {
                                listLocation.Add(realinvengroup[i].LocIDSys);
                            }
                            location = Db.Locations.Where(a => listLocation.Contains(a.LocIDSys)).ToList();
                            List<Inventory> realinventory = new List<Inventory>();
                            foreach (var invengroup in realinvengroup)
                            {
                                int actualQty = 0;
                                double useDimension = 0;
                                foreach (var tran in invengroup.Child)
                                {
                                    var piece = (int)tran.Qty;
                                    var tranQty = Db.ItemUnitMapping.Where(qt => qt.ItemIDSys == tran.ItemIDSys).OrderBy(b => b.Sequence).ToList();
                                    var currentsequence = tranQty.Where(sq => sq.UnitIDSys == tran.UnitIDSys).Select(sqn => sqn.Sequence).SingleOrDefault();
                                    //var lastUnit = tranQty.Last();

                                    foreach (var unit in tranQty)
                                    {
                                        if (unit.Sequence > currentsequence)
                                        {
                                            piece *= unit.QtyInParent;
                                        }
                                    }
                                    actualQty += piece;
                                    useDimension += tran.UsedDimension * (int)tran.Qty;
                                }

                                var laterInven = repoInven.Get(a => a.ControlLevel1 == invengroup.ControlLevel1 &&
                                a.Expire == invengroup.Expire && a.Inspect == invengroup.Inspect && a.ControlLevel2 == invengroup.ControlLevel2 && a.LocIDSys == invengroup.LocIDSys &&
                                a.ControlLevel3 == invengroup.ControlLevel3 && a.ItemIDSys == invengroup.ItemIDSys);
                                if (invengroup.Child[0] != null)
                                    location[location.FindIndex(a => a.LocIDSys == invengroup.Child[0].LocIDSys)].AvailableArea -= useDimension;
                                if (laterInven == null)
                                {
                                    Inventory inven = new CommonService().AutoMapper<Inventory>(invengroup.Child[0]);
                                    inven.InboundQty = actualQty;
                                    inven.AvailableQty = inven.InboundQty - inven.OutboundQty;
                                    inven.StatusIDSys = invengroup.Child[0].StatusIDSys;
                                    inven.Expire = invengroup.Expire.HasValue ? invengroup.Expire : null;
                                    realinventory.Add(repoInven.Insert(inven));
                                }
                                else
                                {
                                    laterInven.InboundQty += actualQty;
                                    laterInven.AvailableQty = laterInven.InboundQty - laterInven.OutboundQty;
                                    realinventory.Add(repoInven.Update(laterInven));
                                }

                            }
                            for (int i = 0; i < location.Count; i++)
                            {
                                repoLoc.Update(location[i]);
                            }
                            Db.SaveChanges();
                            foreach (var inven in realinventory)
                            {
                                var inventemp = realinvengroup.Where(a => a.ControlLevel1 == inven.ControlLevel1 &&
                                a.Expire == inven.Expire && a.Inspect == inven.Inspect && a.ControlLevel2 == inven.ControlLevel2 && a.LocIDSys == inven.LocIDSys &&
                                a.ControlLevel3 == inven.ControlLevel3 && a.ItemIDSys == inven.ItemIDSys).SingleOrDefault();

                                foreach (var childtran in inventemp.Child)
                                {
                                    var piece = (int)childtran.Qty;
                                    var tranQty = Db.ItemUnitMapping.Where(qt => qt.ItemIDSys == childtran.ItemIDSys).OrderBy(b => b.Sequence).ToList();
                                    var currentsequence = tranQty.Where(sq => sq.UnitIDSys == childtran.UnitIDSys).Select(sqn => sqn.Sequence).SingleOrDefault();
                                    var lastUnit = tranQty.Last();
                                    foreach (var unit in tranQty)
                                    {
                                        if (unit.Sequence > currentsequence)
                                        {
                                            piece *= unit.QtyInParent;
                                        }
                                    }

                                    InventoryTransaction tempchild = new CommonService().AutoMapper<InventoryTransaction>(childtran);
                                    tempchild.InvenIDSys = inven.InvenIDSys;
                                    tempchild.RefNO = newReceive.ReceiveIDSys.ToString();
                                    tempchild.ReceivingDate = childtran.ReceivingDate;
                                    tempchild.ConvertedQty = piece;
                                    inventran.Add(repoTran.Insert(tempchild));
                                    inventran[inventran.Count - 1].InventoryTransactionDetail = childtran.InventoryTransactionDetail.Select(a => new InventoryTransactionDetail()
                                    {
                                        SerialNumber = a.SerialNumber
                                    }).ToList();
                                }
                            }
                            Db.SaveChanges();

                            foreach (var inventra in inventran)
                            {
                                inventra.InventoryTransactionDetail = inventra.InventoryTransactionDetail != null ? inventra.InventoryTransactionDetail : new List<InventoryTransactionDetail>();
                                foreach (var detail in inventra.InventoryTransactionDetail)
                                {
                                    detail.InvenTranIDSys = inventra.InvenTranIDSys;
                                    repoTranDe.Insert(detail);
                                    InventoryDetail inventorydetail = new InventoryDetail()
                                    {
                                        InvenIDSys = inventra.InvenIDSys,
                                        ItemIDSys = inventra.ItemIDSys,
                                        SerialNumber = detail.SerialNumber,
                                        StatusIDSys = inventra.StatusIDSys
                                    };
                                    repoInvenDe.Insert(inventorydetail);
                                }
                            }

                            if (receives.SpareFields != null)
                            {
                                repoSparefd.insertByDto(receives.ReceiveIDSys, receives.SpareFields);
                            }

                        }

                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return newReceive.ReceiveIDSys;
            }
        }

        public bool UpdateReceive(ReceiveDto receives)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead
                }))
            {
                Receive newReceive = new Receive();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        Receive receive = new CommonService().AutoMapper<Receive>(receives);
                        IReceiveRepository repo = new ReceiveRepository(Db);

                        newReceive = repo.Update(receive);
                        Db.SaveChanges();
                        if (receives.InventoryTransactions != null)
                        {
                            IInventoryTransactionRepository repoTran = new InventoryTransactionRepository(Db);
                            IInventoryRepository repoInven = new InventoryRepository(Db);
                            IInventoryDetailRepository repoInvenDe = new InventoryDetailRepository(Db);
                            IInventoryTransactionDetailRepository repoTranDe = new InventoryTransactionDetailRepository(Db);
                            ILocationRepository repoLoc = new LocationRepository(Db);
                            ISpareFieldDetailRepository repoSparefd = new SpareFieldDetailRepository(Db);
                            List<Location> location = new List<Location>();
                            List<InventoryTransaction> inventran = new List<InventoryTransaction>();
                            var realinvengroup = receives.InventoryTransactions.GroupBy(a => new { a.ControlLevel1, a.Dimention, a.Expire, a.Inspect, a.LocIDSys, a.ControlLevel2, a.ControlLevel3, a.Serial, a.ItemIDSys })
                                .Select(b => new
                                {
                                    ControlLevel1 = b.Key.ControlLevel1,
                                    Expire = b.Key.Expire.HasValue ? b.Key.Expire : null,
                                    ControlLevel2 = b.Key.ControlLevel2,
                                    Inspect = b.Key.Inspect,
                                    LocIDSys = b.Key.LocIDSys,
                                    ControlLevel3 = b.Key.ControlLevel3,
                                    ItemIDSys = b.Key.ItemIDSys,
                                    Child = b.ToList()
                                }).ToList();
                            List<int> listLocation = new List<int>();
                            for (int i = 0; i < realinvengroup.Count; i++)
                            {
                                listLocation.Add(realinvengroup[i].LocIDSys);
                            }
                            location = Db.Locations.Where(a => listLocation.Contains(a.LocIDSys)).ToList();
                            List<Inventory> realinventory = new List<Inventory>();
                            foreach (var invengroup in realinvengroup)
                            {
                                int actualQty = 0;
                                double useDimension = 0;
                                foreach (var tran in invengroup.Child)
                                {
                                    actualQty += (int)tran.Qty;
                                    useDimension += tran.UsedDimension * (int)tran.Qty;
                                }

                                var laterInven = repoInven.Get(a => a.ControlLevel1 == invengroup.ControlLevel1 &&
                                a.Expire == invengroup.Expire && a.Inspect == invengroup.Inspect && a.ControlLevel2 == invengroup.ControlLevel2 && a.LocIDSys == invengroup.LocIDSys &&
                                a.ControlLevel3 == invengroup.ControlLevel3 && a.ItemIDSys == invengroup.ItemIDSys);

                                if (invengroup.Child[0] != null)
                                    location[location.FindIndex(a => a.LocIDSys == invengroup.Child[0].LocIDSys)].AvailableArea -= useDimension;

                                if (laterInven == null)
                                {
                                    Inventory inven = new CommonService().AutoMapper<Inventory>(invengroup.Child[0]);
                                    inven.InboundQty = actualQty;
                                    inven.AvailableQty = inven.InboundQty - inven.OutboundQty;
                                    inven.StatusIDSys = invengroup.Child[0].StatusIDSys;
                                    inven.Expire = invengroup.Expire.HasValue ? invengroup.Expire : null;
                                    realinventory.Add(repoInven.Insert(inven));
                                }
                                else
                                {
                                    laterInven.InboundQty += actualQty;
                                    laterInven.AvailableQty = laterInven.InboundQty - laterInven.OutboundQty;
                                    realinventory.Add(repoInven.Update(laterInven));
                                }
                            }
                            for (int i = 0; i < location.Count; i++)
                            {
                                repoLoc.Update(location[i]);
                            }
                            Db.SaveChanges();
                            foreach (var inven in realinventory)
                            {
                                var inventemp = realinvengroup.Where(a => a.ControlLevel1 == inven.ControlLevel1 &&
                                a.Expire == inven.Expire && a.Inspect == inven.Inspect && a.ControlLevel2 == inven.ControlLevel2 && a.LocIDSys == inven.LocIDSys &&
                                a.ControlLevel3 == inven.ControlLevel3 && a.ItemIDSys == inven.ItemIDSys).SingleOrDefault();

                                foreach (var childtran in inventemp.Child)
                                {
                                    InventoryTransaction tempchild = new CommonService().AutoMapper<InventoryTransaction>(childtran);
                                    tempchild.InvenIDSys = inven.InvenIDSys;
                                    tempchild.RefNO = newReceive.ReceiveIDSys.ToString();
                                    tempchild.ReceivingDate = childtran.ReceivingDate;
                                    tempchild.ConvertedQty = (int)childtran.Qty;
                                    inventran.Add(repoTran.Insert(tempchild));
                                }
                            }

                            Db.SaveChanges();
                            foreach (var inventra in inventran)
                            {
                                inventra.InventoryTransactionDetail = inventra.InventoryTransactionDetail != null ? inventra.InventoryTransactionDetail : new List<InventoryTransactionDetail>();
                                foreach (var detail in inventra.InventoryTransactionDetail)
                                {
                                    detail.InvenTranIDSys = inventra.InvenTranIDSys;
                                    repoTranDe.Insert(detail);
                                    InventoryDetail inventorydetail = new InventoryDetail()
                                    {
                                        InvenIDSys = inventra.InvenIDSys,
                                        ItemIDSys = inventra.ItemIDSys,
                                        SerialNumber = detail.SerialNumber,
                                        StatusIDSys = inventra.StatusIDSys
                                    };
                                    repoInvenDe.Insert(inventorydetail);
                                }
                            }

                            if (receives.SpareFields != null && receives.SpareFields.Any())
                            {
                                repoSparefd.insertByDto(receives.ReceiveIDSys, receives.SpareFields);
                            }
                        }
                        Db.SaveChanges();

                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException)
                {
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }

                return true;
            }
        }

        public bool DeleteReceive(int id)
        {
            throw new NotImplementedException();
        }               

        public TempInventoryTransaction SaveTempInventoryTransaction(TempInventoryTransaction tempTransaction)
        {
            List<TempInventoryTransaction> tempTransactions = new List<TempInventoryTransaction>();
            tempTransactions.Add(tempTransaction);
            return SaveTempInventoryTransactions(tempTransactions).SingleOrDefault();
        }

        public IEnumerable<TempInventoryTransaction> SaveTempInventoryTransactions(IEnumerable<TempInventoryTransaction> tempTransactions)
        {
            try
            {
                using (WMSDbContext db = new WMSDbContext())
                {
                    ITempInventoryTransactionRepository tranRepo = new TempInventoryTransactionRepository(db);
                    foreach (var transaction in tempTransactions)
                    {
                        tranRepo.Save(transaction);
                    }
                    db.SaveChanges();
                }
                return tempTransactions;
            }
            catch (DbUpdateException)
            {
                throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
            }            
        }

        public ReceiveTempInventoryTransaction SaveReceiveAndTempInventoryTransactions(ReceiveTempInventoryTransaction receiveTempTransaction)
        {
            using (var scope = Transaction.Default)
            {
                SaveReceive(receiveTempTransaction.Receive);
                SaveTempInventoryTransactions(receiveTempTransaction.TempTransactions);
                scope.Complete();
                return receiveTempTransaction;
            }
        }

        public Receive SaveReceive(Receive receive)
        {
            try
            {
                using (WMSDbContext db = new WMSDbContext())
                {
                    IReceiveRepository receiveRepo = new ReceiveRepository(db);
                    receiveRepo.Save(receive);
                    db.SaveChanges();
                    return receive;
                }
            }
            catch (DbEntityValidationException)
            {
                throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
            }
        }
    }    
}
