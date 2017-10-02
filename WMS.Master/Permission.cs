//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Master
{
    using System;
    using System.Collections.Generic;
    
    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            this.RolePermissions = new HashSet<RolePermission>();
        }
    
        public string PermissionName { get; set; }
        public string PermissionID { get; set; }
        public Nullable<int> MenuIDSys { get; set; }
        public Nullable<int> ProjectIDSys { get; set; }
        public string Method { get; set; }
        public string ApiIDSys { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual MenuProjectMapping MenuProjectMapping { get; set; }
        public virtual Api_MT Api_MT { get; set; }
    }
}
