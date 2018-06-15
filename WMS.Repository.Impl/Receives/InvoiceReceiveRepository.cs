
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.Receiving;
using System.Linq;

namespace WMS.Repository.Impl
{
    public class InvoiceReceiveRepository : Repository<InvoiceReceive>, IInvoiceReceiveRepository
    {
        private WMSDbContext Db { get; set; }
        public InvoiceReceiveRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<InvoiceReceivesDto> GetInvReceivesDto(int receiveID)
        {
            IEnumerable<InvoiceReceivesDto> rs = new List<InvoiceReceivesDto>();
            using (WMSDbContext Db = new WMSDbContext())
            {
               rs = (from inv in Db.InvoiceReceives
                          join rc in Db.Receive on inv.ReceiveIDSys equals rc.ReceiveIDSys
                          where inv.ReceiveIDSys == receiveID
                          select new InvoiceReceivesDto
                          {
                              InvIDSys = inv.InvIDSys,
                              ReceiveNO = rc.ReceiveNO,
                              InvNO = inv.InvNO,
                              IsUnknownQty = inv.IsUnknownQty,
                              Qty = inv.Qty,
                              ReceiveIDSys = inv.ReceiveIDSys,
                              Remark =  inv.Remark
                          }
                          ).ToList();
            }
            return rs;
        }
    }
}
