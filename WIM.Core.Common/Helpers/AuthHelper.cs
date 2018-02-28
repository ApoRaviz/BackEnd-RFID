using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WIM.Core.Common.Helpers
{
    public class AuthHelper
    {
        public static IIdentity GetIdentity()
        {
            if (HttpContext.Current == null)
                return null;
            return HttpContext.Current.GetOwinContext().Authentication.User.Identity;
        }
    }
}
