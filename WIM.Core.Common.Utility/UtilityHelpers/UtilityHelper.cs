using System;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Transactions;
using System.Web;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Common.Utility.UtilityHelpers
{
    public class UtilityHelper
    {        
       
        public static string GetHandleErrorMessageException(ErrorEnum errorEnum, string internalMessage)
        {

            return string.Format("Error #{0}: {1}", errorEnum.GetValue(), internalMessage ?? errorEnum.GetDescription());
        }

        public static IIdentity GetIdentity()
        {
            try
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.GetOwinContext().Authentication.User.Identity;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
