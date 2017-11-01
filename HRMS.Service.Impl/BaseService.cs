using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRMS.Service.Impl
{
    public class BaseService : IBaseService
    {
        public IIdentity Identity
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication.User.Identity;
            }
        }
        
    }
}
