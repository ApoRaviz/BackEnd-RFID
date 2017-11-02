using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Service
{
    public interface IService
    {
        IIdentity Identity { get; }
    }
}
