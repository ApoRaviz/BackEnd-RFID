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
    
    public partial class ZoneLayoutDetail_MT
    {
        public int ZoneIDSys { get; set; }
        public int Floor { get; set; }
        public int ZoneID { get; set; }
        public Nullable<int> ZoneParentID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Left { get; set; }
        public Nullable<int> Top { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Length { get; set; }
        public string Use { get; set; }
    
        public virtual ZoneLayoutHeader_MT ZoneLayoutHeader_MT { get; set; }
    }
}
