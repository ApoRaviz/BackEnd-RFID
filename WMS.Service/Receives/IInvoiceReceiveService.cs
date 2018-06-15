using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.Receiving;

namespace WMS.Service
{
    public interface IInvoiceReceiveService : IService
    {
        InvoiceReceivesDto CreateInvReceives(InvoiceReceive param);
    }
}
