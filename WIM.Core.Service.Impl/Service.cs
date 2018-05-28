using System.Security.Principal;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Service.Impl
{
    public class Service : IService
    {
        public IIdentity Identity
        {
            get
            { 
                return UtilityHelper.GetIdentity();
            }
        }
    }
}
