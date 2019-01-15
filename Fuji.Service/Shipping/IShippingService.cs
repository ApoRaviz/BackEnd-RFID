using Fuji.Entity.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Service.Shipping
{
    public interface IShippingService
    {
        Task<AllocateView> GetOrderAsync(string orderNumber);
    }
}
