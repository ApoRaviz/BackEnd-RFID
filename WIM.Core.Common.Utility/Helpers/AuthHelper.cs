using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WIM.Core.Common.Utility.Helpers
{
    public class AuthHelper
    {
        public static IIdentity GetIdentity()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User.Identity;
        }
    }
}
