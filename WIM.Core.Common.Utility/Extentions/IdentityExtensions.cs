using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace WIM.Core.Common.Utility.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserIdApp(this IIdentity _identity)
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                return _identity.GetUserId();

                // #Job Comment
                //var ci = _identity as ClaimsIdentity;
                //string _userId = ci?.FindFirstValue(ClaimTypes.NameIdentifier);

                //if (!string.IsNullOrEmpty(_userId))
                //{
                //    return _userId;
                //}
            }
            return null;
        }

        public static string GetUserNameApp(this IIdentity _identity)
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                return _identity.GetUserName();

                // #Job Comment
                //var ci = _identity as ClaimsIdentity;
                //if (!string.IsNullOrEmpty(ci.Name))
                //{
                //    return ci.Name;
                //}
            }
            return null;
        }

        public static string GetCustomerName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CustomerName");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static int GetProjectIDSys(this IIdentity _identity)
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                var ci = _identity as ClaimsIdentity;
                var exp = (from c in ci.Claims
                           where c.Type == "ProjectIDSys"
                           select c).SingleOrDefault();

                if (exp != null)
                {
                    return Convert.ToInt32(exp.Value);
                }
            }
            return 0;
        }
    }
}
