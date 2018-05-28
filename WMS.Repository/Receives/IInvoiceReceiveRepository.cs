
using System.Collections.Generic;
using WIM.Core.Repository;
using WMS.Common.ValueObject;
using WMS.Entity.Receiving;

namespace WMS.Repository
{
    public interface IInvoiceReceiveRepository : IRepository<InvoiceReceive>
    {
        IEnumerable<InvoiceReceivesDto> GetInvReceivesDto(int receiveID);
    }
}
