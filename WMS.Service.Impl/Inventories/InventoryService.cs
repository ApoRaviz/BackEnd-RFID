using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity;
using WIM.Core.Service.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.InventoryManagement;
using WMS.Entity.ItemManagement;
using WMS.Entity.Receiving;
using WMS.Entity.WarehouseManagement;
using WMS.Repository;
using WMS.Repository.Impl;
using WMS.Repository.Impl.InventoryManage;
using WMS.Repository.InvenoryManagement;
using WMS.Repository.Warehouse;
using WMS.Service.Inventories;

namespace WMS.Service.Impl.Inventories
{
    public class InventoryService : WIM.Core.Service.Impl.Service, IInventoryService
    {
        public InventoryService()
        {

        }

        public void ConfirmReceive(ConfirmReceive confirmReceive)
        {
            Receive receive;
            using (WMSDbContext db = new WMSDbContext())
            {
                IReceiveRepository receiveRepo = new ReceiveRepository(db);
                ITempInventoryTransactionRepository tempTranRepo = new TempInventoryTransactionRepository(db);
                receive = receiveRepo.GetByID(confirmReceive.ReceiveIDSys, true);
                IEnumerable<TempInventoryTransaction> tempInventoryTransactions = tempTranRepo.GetMany(x => x.RefNO == receive.ReceiveIDSys.ToString(), true);

            }
        }

        private IEnumerable<InventoryTransactionGroup> GetInventoryTransactionGroups(IEnumerable<TempInventoryTransaction> tempInventoryTransactions)
        {
            IEnumerable<InventoryTransactionGroup> tempInventoryTransactionKeyValueGroups = tempInventoryTransactions.GroupBy(
                        grp => new InventoryTransactionGroupKey
                        {
                            ControlLevel1 = grp.ControlLevel1,
                            ControlLevel2 = grp.ControlLevel2,
                            ControlLevel3 = grp.ControlLevel3,
                            Expire = grp.Expire,
                            Inspect = grp.Inspect,
                            LocIDSys = grp.LocIDSys,
                            ItemIDSys = grp.ItemIDSys
                        },
                        (keyGroup, valueGroup) => new InventoryTransactionGroup
                        {
                            GroupKey = keyGroup,
                            InventoryTransactions = Mapper.Map<IEnumerable<InventoryTransaction>>(valueGroup)
                        });
            return tempInventoryTransactionKeyValueGroups;
        }

        private decimal GetSumInventoryInboundQty(InventoryTransactionGroup invetoryTransactionGroup)
        {
            decimal inboundQty = 0M;
            foreach (InventoryTransaction inTran in invetoryTransactionGroup.InventoryTransactions)
            {
                IEnumerable<ItemUnitMapping> itemUnitMappings = GetItemUnitMappingByInventoryTransaction(inTran);
                itemUnitMappings.Each(i =>
                    inTran.Qty *= i.QtyInParent
                );
                inboundQty += inTran.Qty;
            }
            if (inboundQty == 0M)
            {
                throw new AppValidationException(ErrorEnum.INBOUND_QTY_EQUAL_0);
            }
            return inboundQty;
        }

        private IEnumerable<ItemUnitMapping> GetItemUnitMappingByInventoryTransaction(InventoryTransaction transaction)
        {
            using (WMSDbContext db = new WMSDbContext())
            {
                IQueryable<ItemUnitMapping> query = db.ItemUnitMapping.Where(a =>
                        a.ItemIDSys == transaction.ItemIDSys
                        && a.Sequence > (
                            db.ItemUnitMapping.Where(b =>
                                b.ItemIDSys == transaction.ItemIDSys
                                && b.UnitIDSys == transaction.UnitIDSys
                            ).FirstOrDefault().Sequence
                        )
                    );
                IEnumerable<ItemUnitMapping> itemUnitMappings = query.ToList();
                return itemUnitMappings;
            }
        }

        private IEnumerable<Location> GetListLocations(IEnumerable<int> locIDSysList)
        {
            locIDSysList = locIDSysList.Distinct();
            using (WMSDbContext db = new WMSDbContext())
            {
                ILocationRepository locationRepo = new LocationRepository(db);
                IEnumerable<Location> locations = locationRepo.GetMany(x => locIDSysList.Contains(x.LocIDSys));
                return locations;
            }
        }

        private Func<Inventory, bool> GetCondition(InventoryTransactionGroupKey inTranGroup)
        {
            Func<Inventory, bool> func = (a) => (
                    a.ItemIDSys == a.ItemIDSys
                    && a.ControlLevel1 == inTranGroup.ControlLevel1
                    && a.ControlLevel2 == inTranGroup.ControlLevel2
                    && a.ControlLevel3 == inTranGroup.ControlLevel3
                    && a.Inspect == inTranGroup.Inspect
                    && a.Expire == inTranGroup.Expire
                    && a.LocIDSys == inTranGroup.LocIDSys);
            return func;
        }

        private Inventory GetInventory(InventoryTransactionGroupKey inTranGroup)
        {
            using (WMSDbContext db = new WMSDbContext())
            {
                IInventoryRepository inventoryRepo = new InventoryRepository(db);
                Inventory inventory = inventoryRepo.GetSingle(GetCondition(inTranGroup));
                return inventory;
            }
        }

        private Inventory GetInventory(InventoryTransactionGroup inTranGroup)
        {
            inTranGroup.ValidateCommonInventoryTransactionGroup();

            IEnumerable<int> invenIDSysList = inTranGroup.InventoryTransactions.Select(i => i.InvenIDSys).Distinct();

            if (invenIDSysList.Count() > 1)
            {
                throw new AppValidationException(ErrorEnum.INVALID_INVENTORY_ID);
            }

            int invenIDSys = invenIDSysList.SingleOrDefault();
            Inventory inventory = GetInventory(inTranGroup.GroupKey);
            
            return inventory;
        }

        private int PerformConfirmReceive(Receive receive, IEnumerable<TempInventoryTransaction> tempInventoryTransactions)
        {
            using (var scope = Transaction.Default)
            {
                try
                {
                    using (WMSDbContext db = new WMSDbContext())
                    {
                        IEnumerable<InventoryTransactionGroup> inTranGroups = GetInventoryTransactionGroups(tempInventoryTransactions);

                        foreach (InventoryTransactionGroup inTranGroup in inTranGroups)
                        {
                            decimal inboundQty = GetSumInventoryInboundQty(inTranGroup);                            
                            Inventory inventory = GetInventory(inTranGroup);
                        }

                        IEnumerable<int> locIDSysList = inTranGroups.Select(x => x.GroupKey.LocIDSys);
                        IEnumerable<Location> locations = GetListLocations(locIDSysList);

                        foreach (var invengroup in realinvengroup)
                        {





                            var laterInven = repoInven.Get(a => a.ControlLevel1 == invengroup.ControlLevel1 &&
                            a.Expire == invengroup.Expire && a.Inspect == invengroup.Inspect && a.ControlLevel2 == invengroup.ControlLevel2 && a.LocIDSys == invengroup.LocIDSys &&
                            a.ControlLevel3 == invengroup.ControlLevel3 && a.ItemIDSys == invengroup.ItemIDSys);

                            if (laterInven == null)
                            {
                                Inventory inven = new CommonService().AutoMapper<Inventory>(invengroup.Child[0]);
                                inven.InboundQty = actualQty;
                                inven.AvailableQty = inven.InboundQty - inven.OutboundQty;
                                inven.StatusIDSys = invengroup.Child[0].StatusIDSys;
                                inven.Expire = invengroup.Expire;
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
                            locationRepo.Update(location[i]);
                        }
                        db.SaveChanges();
                        foreach (var inven in realinventory)
                        {
                            var inventemp = realinvengroup.Where(a => a.ControlLevel1 == inven.ControlLevel1 &&
                            a.Expire == inven.Expire && a.Inspect == inven.Inspect && a.ControlLevel2 == inven.ControlLevel2 && a.LocIDSys == inven.LocIDSys &&
                            a.ControlLevel3 == inven.ControlLevel3 && a.ItemIDSys == inven.ItemIDSys).SingleOrDefault();

                            foreach (var childtran in inventemp.Child)
                            {
                                var piece = (int)childtran.Qty;
                                var tranQty = db.ItemUnitMapping.Where(qt => qt.ItemIDSys == childtran.ItemIDSys).OrderBy(b => b.Sequence).ToList();
                                var currentsequence = tranQty.Where(sq => sq.UnitIDSys == childtran.UnitIDSys).Select(sqn => sqn.Sequence).SingleOrDefault();
                                var lastUnit = tranQty.Last();
                                foreach (var unit in tranQty)
                                {
                                    if (unit.Sequence > currentsequence)
                                    {
                                        piece *= unit.QtyInParent;
                                    }
                                }

                                //InventoryTransaction tempchild = new CommonService().AutoMapper<InventoryTransaction>(childtran);
                                //tempchild.InvenIDSys = inven.InvenIDSys;
                                //tempchild.RefNO = newReceive.ReceiveIDSys.ToString();
                                //tempchild.ReceivingDate = childtran.ReceivingDate;
                                //tempchild.ConvertedQty = piece;
                                //inventran.Add(repoTran.Insert(tempchild));
                                //inventran[inventran.Count - 1].InventoryTransactionDetail = childtran.InventoryTransactionDetail.Select(a => new InventoryTransactionDetail()
                                //{
                                //    SerialNumber = a.SerialNumber
                                //}).ToList();
                            }
                        }

                        db.SaveChanges();

                        //foreach (var inventra in inventran)
                        //{
                        //    inventra.InventoryTransactionDetail = inventra.InventoryTransactionDetail != null ? inventra.InventoryTransactionDetail : new List<InventoryTransactionDetail>();
                        //    foreach (var detail in inventra.InventoryTransactionDetail)
                        //    {
                        //        detail.InvenTranIDSys = inventra.InvenTranIDSys;
                        //        repoTranDe.Insert(detail);
                        //        InventoryDetail inventorydetail = new InventoryDetail()
                        //        {
                        //            InvenIDSys = inventra.InvenIDSys,
                        //            ItemIDSys = inventra.ItemIDSys,
                        //            SerialNumber = detail.SerialNumber,
                        //            StatusIDSys = inventra.StatusIDSys
                        //        };
                        //        repoInvenDe.Insert(inventorydetail);
                        //    }
                        //}

                        db.SaveChanges();
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

    }
}
