using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.ValueObject
{
    public class PermissionTree
    {
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public int? MenuIDSys { get; set; }
        public string Method { get; set; }
        public List<PermissionTree> Group { get; set; }
    }
}
