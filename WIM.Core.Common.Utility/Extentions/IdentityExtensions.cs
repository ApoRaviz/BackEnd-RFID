using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;

namespace WIM.Core.Common.Utility.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetCustomerName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CustomerName");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
