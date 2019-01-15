using Fuji.Entity.Shipping;
using Fuji.Service.Shipping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Service.Impl.StatusManagement;

namespace Fuji.Service.Impl.Shiping
{
    public class ShippingService: IShippingService
    {
        public async Task<AllocateView> GetOrderAsync(string orderNumber)
        {
            var url = $"{ConfigurationManager.AppSettings["outboundServiceUrl"]}/interfaces/orders/{orderNumber}";
            var result = await UtilityHelper.GetAsync<AllocateView>(url, ConfigurationManager.AppSettings["identity"]);
            return result;
        }

    }
}
