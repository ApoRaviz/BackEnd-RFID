using System.Collections.Generic;

namespace WMS.Master
{
    public interface IPermission
    {
        string Id { get; set; }
        string PermissionName { get; set; }
        ICollection<Role> Roles { get; set; }
    }
}