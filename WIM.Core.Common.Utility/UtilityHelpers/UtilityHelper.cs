using System;
using System.Net;
using System.Reflection;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Common.Utility.UtilityHelpers
{
    public class UtilityHelper
    {
        public static string GetHandleErrorMessageException(ErrorEnum errorEnum, string internalMessage)
        {

            return string.Format("Error #{0}: {1}", errorEnum.GetValue(), internalMessage ?? errorEnum.GetDescription());
        }

        public static ValidationError GetHandleErrorMessageException(ErrorEnum errorEnum)
        {
            return new ValidationError(errorEnum);
        }
    }
}
