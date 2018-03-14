using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Service.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.InventoryManagement;
using WMS.Entity.Receiving;
using WMS.Repository;
using WMS.Repository.Impl;
using WMS.Repository.Impl.InventoryManage;
using WMS.Repository.InvenoryManagement;

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
            Receive unit;
            ReceiveDto receive;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReceiveRepository repo = new ReceiveRepository(Db);
                string[] include = { "InventoryTransaction" };
                receive = repo.GetReceiveDtoByID(id);
                //unit = repo.GetWithInclude(u => u.ReceiveIDSys == id,include).SingleOrDefault();
                // receive = new CommonService().AutoMapper<ReceiveDto>(unit);
                //receive.InventoryTransactions = unit.InventoryTransaction.Select(a => new CommonService().AutoMapper<InventoryTransactionDto>(a)).ToList();
            }
            return receive;
        }

        public int CreateReceive(ReceiveDto receives)
        {
            using (var scope = new TransactionScope())
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
                        if(receives.InventoryTransactions != null)
                        {
                            IInventoryTransactionRepository repoTran = new InventoryTransactionRepository(Db);
                            IInventoryRepository repoInven = new InventoryRepository(Db);
                            List<InventoryTransaction> inventran = new List<InventoryTransaction>();
                            var realinvengroup = receives.InventoryTransactions.GroupBy(a => new { a.Box, a.Dimention, a.Expire, a.Inspect, a.LocIDSys, a.Lot, a.Pallet, a.Serial, a.ItemIDSys })
                                .Select(b => new {
                                    Box = b.Key.Box,
                                    Dimension = b.Key.Dimention,
                                    Expire = b.Key.Expire.HasValue ? b.Key.Expire:null,
                                    Lot = b.Key.Lot,
                                    Inspect = b.Key.Inspect,
                                    LocIDSys = b.Key.LocIDSys,
                                    Pallet = b.Key.Pallet,
                                    Serial = b.Key.Serial,
                                    ItemIDSys = b.Key.ItemIDSys,
                                    Child = b.ToList()
                                }).ToList();
                            List<Inventory> realinventory = new List<Inventory>();
                            foreach(var invengroup in realinvengroup)
                            {
                                int actualQty = 0;
                                foreach(var tran in invengroup.Child)
                                {
                                    var piece = (int)tran.Qty;
                                    var tranQty = Db.ItemUnitMapping.Where(qt => qt.ItemIDSys == tran.ItemIDSys).OrderBy(b => b.Sequence).ToList();
                                    var currentsequence = tranQty.Where(sq => sq.UnitIDSys == tran.UnitIDSys).Select(sqn => sqn.Sequence).SingleOrDefault();
                                    var lastUnit = tranQty.Last();
                                    foreach(var unit in tranQty)
                                    {
                                        if(unit.Sequence > currentsequence )
                                        {
                                            piece *= unit.QtyInParent;
                                        }
                                    }
                                    actualQty += piece;
                                    
                                }
 
                                var laterInven = repoInven.Get(a => a.Box == invengroup.Box && a.Dimension == invengroup.Dimension &&
                                a.Expire == invengroup.Expire && a.Inspect == invengroup.Inspect && a.Lot == invengroup.Lot && a.LocIDSys == invengroup.LocIDSys &&
                                a.Pallet == invengroup.Pallet && a.Serial == invengroup.Serial && a.ItemIDSys == invengroup.ItemIDSys);

                                if(laterInven == null)
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
                            Db.SaveChanges();
                            foreach(var inven in realinventory)
                            {
                                var inventemp = realinvengroup.Where(a => a.Box == inven.Box && a.Dimension == inven.Dimension &&
                                a.Expire == inven.Expire && a.Inspect == inven.Inspect && a.Lot == inven.Lot && a.LocIDSys == inven.LocIDSys &&
                                a.Pallet == inven.Pallet && a.Serial == inven.Serial && a.ItemIDSys == inven.ItemIDSys).SingleOrDefault();

                                foreach(var childtran in inventemp.Child)
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
                                    tempchild.ReceiveIDSys = newReceive.ReceiveIDSys;
                                    tempchild.ReceivingDate = childtran.ReceivingDate;
                                    tempchild.ConvertedQty = piece;
                                    inventran.Add(repoTran.Insert(tempchild));
                                }
                            }
                            Db.SaveChanges();
                        }

                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return newReceive.ReceiveIDSys;
            }
        }

        public bool UpdateReceive(ReceiveDto receives)
        {
            using (var scope = new TransactionScope())
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
                            List<InventoryTransaction> inventran = new List<InventoryTransaction>();
                            var realinvengroup = receives.InventoryTransactions.GroupBy(a => new { a.Box, a.Dimention, a.Expire, a.Inspect, a.LocIDSys, a.Lot, a.Pallet, a.Serial, a.ItemIDSys })
                                .Select(b => new {
                                    Box = b.Key.Box,
                                    Dimension = b.Key.Dimention,
                                    Expire = b.Key.Expire.HasValue ? b.Key.Expire : null,
                                    Lot = b.Key.Lot,
                                    Inspect = b.Key.Inspect,
                                    LocIDSys = b.Key.LocIDSys,
                                    Pallet = b.Key.Pallet,
                                    Serial = b.Key.Serial,
                                    ItemIDSys = b.Key.ItemIDSys,
                                    Child = b.ToList()
                                }).ToList();
                            List<Inventory> realinventory = new List<Inventory>();
                            foreach (var invengroup in realinvengroup)
                            {
                                int actualQty = 0;
                                foreach (var tran in invengroup.Child)
                                {
                                    actualQty += (int)tran.Qty;
                                }

                                var laterInven = repoInven.Get(a => a.Box == invengroup.Box && a.Dimension == invengroup.Dimension &&
                                a.Expire == invengroup.Expire && a.Inspect == invengroup.Inspect && a.Lot == invengroup.Lot && a.LocIDSys == invengroup.LocIDSys &&
                                a.Pallet == invengroup.Pallet && a.Serial == invengroup.Serial && a.ItemIDSys == invengroup.ItemIDSys);

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
                            Db.SaveChanges();
                            foreach (var inven in realinventory)
                            {
                                var inventemp = realinvengroup.Where(a => a.Box == inven.Box && a.Dimension == inven.Dimension &&
                                a.Expire == inven.Expire && a.Inspect == inven.Inspect && a.Lot == inven.Lot && a.LocIDSys == inven.LocIDSys &&
                                a.Pallet == inven.Pallet && a.Serial == inven.Serial && a.ItemIDSys == inven.ItemIDSys).SingleOrDefault();

                                foreach (var childtran in inventemp.Child)
                                {
                                    InventoryTransaction tempchild = new CommonService().AutoMapper<InventoryTransaction>(childtran);
                                    tempchild.InvenIDSys = inven.InvenIDSys;
                                    tempchild.ReceiveIDSys = newReceive.ReceiveIDSys;
                                    tempchild.ReceivingDate = childtran.ReceivingDate;
                                    tempchild.ConvertedQty = (int)childtran.Qty;
                                    inventran.Add(repoTran.Insert(tempchild));
                                }
                            }
                            Db.SaveChanges();
                        }

                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }

                return true;
            }
        }

        public bool DeleteReceive(int id)
        {
            throw new NotImplementedException();
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
    }
}
