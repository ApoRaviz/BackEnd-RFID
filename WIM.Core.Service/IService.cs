using System.Security.Principal;

namespace WIM.Core.Service
{
    public interface IService
    {
        IIdentity Identity { get; }
    }
}
