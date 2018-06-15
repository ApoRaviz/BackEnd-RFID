using System.Linq;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.Receiving;

namespace WMS.Repository.Impl
{
    public class ReceiveRepository : Repository<Receive>, IReceiveRepository
    {
        private WMSDbContext Db { get; set; }
        public ReceiveRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public ReceiveDto GetReceiveDtoByID(int receiveIDSys)
        {
            ReceiveDto receive = new ReceiveDto();
            //Receive receives;

            using (WMSDbContext Db = new WMSDbContext())
            {
                var query = (from i in Db.Receive
                             join o in Db.InventoryTransaction on i.ReceiveIDSys.ToString() equals o.RefNO into rece
                             from receiv in rece.DefaultIfEmpty()
                             join p in Db.Item_MT on receiv.ItemIDSys equals p.ItemIDSys into pi
                             from leftItem in pi.DefaultIfEmpty()
                             join j in Db.Inventory on receiv.InvenIDSys equals j.InvenIDSys into ji
                             from leftInventory in ji.DefaultIfEmpty()
                             join k in Db.Locations on leftInventory.LocIDSys equals k.LocIDSys into kl
                             from leftLocation in kl.DefaultIfEmpty()
                             join l in Db.InventoryTransactionDetail on receiv.InvenTranIDSys equals l.InvenTranIDSys into inven
                             from leftTranDetail in inven.DefaultIfEmpty()
                             join n in Db.Supplier_MT on i.SupplierIDSys equals n.SupIDSys into ns
                             from leftSup in ns
                             where i.ReceiveIDSys == receiveIDSys
                             select new { i, receiv, leftItem, leftInventory, leftLocation, leftSup, leftTranDetail }).ToList();
                if (query != null)
                {
                    receive = new ReceiveDto()
                    {
                        ReceiveIDSys = query[0].i.ReceiveIDSys,
                        ReceiveNO = query[0].i.ReceiveNO,
                        FileRefID = query[0].i.FileRefID,
                        PONO = query[0].i.PONO,
                        InvoiceNO = query[0].i.InvoiceNO,
                        ReceiveDate = query[0].i.ReceiveDate,
                        ReceivingType = query[0].i.ReceivingType,
                        Remark = query[0].i.Remark,
                        StatusIDSys = query[0].i.StatusIDSys,
                        SupplierIDSys = query[0].i.SupplierIDSys,
                        CompName = query[0].leftSup.CompName,
                    };


                }

                receive.InventoryTransactions = query.GroupBy(a => a.receiv).Select(e => new { Tran = e.Key, Data = e.ToList() })
                    .Select(e => new InventoryTransactionDto()
                    {
                        InvenTranIDSys = e.Tran.InvenTranIDSys,
                        ControlLevel1 = e.Data[0].leftInventory.ControlLevel1,
                        Dimention = e.Data[0].leftInventory.Dimension,
                        Expire = e.Data[0].leftInventory.Expire,
                        Inspect = e.Data[0].leftInventory.Inspect,
                        ItemCode = e.Data[0].leftItem.ItemCode,
                        ItemIDSys = e.Tran.ItemIDSys,
                        ItemName = e.Data[0].leftItem.ItemName,
                        LocIDSys = e.Data[0].leftInventory.LocIDSys,
                        LocNo = e.Data[0].leftInventory.Location.LocNo,
                        ControlLevel2 = e.Data[0].leftInventory.ControlLevel2,
                        ControlLevel3 = e.Data[0].leftInventory.ControlLevel3,
                        Qty = e.Tran.Qty,
                        ReceivingDate = e.Tran.ReceivingDate,
                        StatusIDSys = e.Tran.StatusIDSys,
                        Price = e.Tran.Price,
                        Cost = e.Tran.Cost,
                        UnitIDSys = e.Tran.UnitIDSys,
                        InventoryTransactionDetail = e.Data.Where(a => e.Tran.InvenTranIDSys == (a.leftTranDetail != null ? a.leftTranDetail.InvenTranIDSys : 0)).Select(r => new InventoryTransactionDetailDto()
                        {
                            InvenTranDetailIDSys = r.leftTranDetail.InvenTranDetailIDSys,
                            InvenTranIDSys = r.leftTranDetail.InvenTranIDSys,
                            SerialNumber = r.leftTranDetail.SerialNumber,
                            ItemCode = e.Data[0].leftItem.ItemCode,
                            ItemName = e.Data[0].leftItem.ItemName,
                            LocIDSys = e.Data[0].leftInventory.LocIDSys,
                            LocNo = e.Data[0].leftInventory.Location.LocNo,
                            UnitIDSys = e.Data[0].receiv.UnitIDSys,
                            ControlLevel1 = e.Data[0].leftInventory.ControlLevel1,
                            ControlLevel2 = e.Data[0].leftInventory.ControlLevel2,
                            ControlLevel3 = e.Data[0].leftInventory.ControlLevel3
                        }).ToList()
                    }).ToList();

                receive.SpareFields = Db.ProcGetSpareFieldsByTableAndRefID(Identity.GetProjectIDSys(), "Receives", receive.ReceiveIDSys).ToList();

                if (receive.FileRefID != null)
                {
                    receive.FileName = Db.File_MT.Where(a => a.FileRefID == receive.FileRefID).Select(b => b.FileName).SingleOrDefault();
                }
            }
            return receive;
        }
    }
}
