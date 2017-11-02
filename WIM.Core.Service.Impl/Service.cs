using System.Security.Principal;
using WIM.Core.Common.Helpers;

namespace WIM.Core.Service.Impl
{
    public class Service : IService
    {
        public IIdentity Identity
        {
            get
            {
                return AuthHelper.GetIdentity();
            }
        }
    }
}
