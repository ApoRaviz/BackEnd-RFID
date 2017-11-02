using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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
