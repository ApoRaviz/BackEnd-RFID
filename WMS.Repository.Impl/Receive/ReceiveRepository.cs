using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.Receiving;
using System.Data.Entity;

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
                //var receives = (from i in Db.Receive
                //                join o in Db.InventoryTransaction on i.ReceiveIDSys equals o.ReceiveIDSys
                //                join j in Db.Inventory on o.InvenIDSys equals j.InvenIDSys
                //                join l in Db.Item_MT on o.ItemIDSys equals l.ItemIDSys
                //                join m in Db.Locations on j.LocIDSys equals m.LocIDSys
                //                where i.ReceiveIDSys == receiveIDSys
                //                select new { i,o,j,l,m}).ToList();
                //receive = receives.Select(new ReceiveDto()
                //{
                //    ReceiveIDSys = i.ReceiveIDSys,
                //    ReceiveNO = i.ReceiveNO,
                //    PONO = i.PONO,
                //    InvoiceNO = i.InvoiceNO,
                //    ReceiveDate = i.ReceiveDate,
                //    ReceivingType = i.ReceivingType,
                //    Remark = i.Remark,
                //    StatusIDSys = i.StatusIDSys,
                //    SupplierIDSys = i.SupplierIDSys,
                //    InventoryTransactions = new List<InventoryTransactionDto>().Select(e => new InventoryTransactionDto()
                //    {
                //        InvenTranIDSys = o.InvenTranIDSys,
                //        Box = j.Box,
                //        Dimention = j.Dimension,
                //        Expire = j.Expire,
                //        Inspect = j.Inspect,
                //        ItemCode = l.ItemCode,
                //        ItemIDSys = o.ItemIDSys,
                //        ItemName = l.ItemName,
                //        LocIDSys = j.LocIDSys,
                //        LocNo = m.LocNo,
                //        Lot = j.Lot,
                //        Pallet = j.Pallet,
                //        Qty = o.Qty,
                //        ReceivingDate = o.ReceivingDate,
                //        Serial = j.Serial,
                //        SerialNo = o.SerialNo,
                //        StatusIDSys = o.StatusIDSys,
                //        UnitIDSys = o.UnitIDSys
                //    }).ToList()
                //}).ToList();

                var receives = Db.Receive.Include("InventoryTransaction").Include("InventoryTransaction.Item_MT").Include("InventoryTransaction.Inventory").Include("InventoryTransaction.Inventory.Location")
                    .Join(Db.Supplier_MT, rece => rece.SupplierIDSys, sup => sup.SupIDSys, (rece, sup) => new { Receive = rece, Supplier = sup })
                    .Where(a => a.Receive.ReceiveIDSys == receiveIDSys);
                receive = receives.Select( a => new ReceiveDto()
                {
                    ReceiveIDSys = a.Receive.ReceiveIDSys,
                    ReceiveNO = a.Receive.ReceiveNO,
                    FileRefID = a.Receive.FileRefID,
                    PONO = a.Receive.PONO,
                    InvoiceNO = a.Receive.InvoiceNO,
                    ReceiveDate = a.Receive.ReceiveDate,
                    ReceivingType = a.Receive.ReceivingType,
                    Remark = a.Receive.Remark,
                    StatusIDSys = a.Receive.StatusIDSys,
                    SupplierIDSys = a.Receive.SupplierIDSys,
                    CompName = a.Supplier.CompName,
                    InventoryTransactions = a.Receive.InventoryTransaction.Select(e => new InventoryTransactionDto()
                    {
                        InvenTranIDSys = e.InvenTranIDSys,
                        Box = e.Inventory.Box,
                        Dimention = e.Inventory.Dimension,
                        Expire = e.Inventory.Expire,
                        Inspect = e.Inventory.Inspect,
                        ItemCode = e.Item_MT.ItemCode,
                        ItemIDSys = e.ItemIDSys,
                        ItemName = e.Item_MT.ItemName,
                        LocIDSys = e.Inventory.LocIDSys,
                        LocNo = e.Inventory.Location.LocNo,
                        Lot = e.Inventory.Lot,
                        Pallet = e.Inventory.Pallet,
                        Qty = e.Qty,
                        ReceivingDate = e.ReceivingDate,
                        Serial = e.Inventory.Serial,
                        SerialNo = e.SerialNo,
                        StatusIDSys = e.StatusIDSys,
                        UnitIDSys = e.UnitIDSys
                    }).ToList()
                }).Single();

                if(receive.FileRefID != null)
                {
                    receive.FileName = Db.File_MT.Where(a => a.FileRefID == receive.FileRefID).Select(b => b.FileName).SingleOrDefault();
                }
            }
            return receive;
        }
    }
}
