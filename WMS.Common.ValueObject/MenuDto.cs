using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common
{
    public class MenuDto
    {
        public int MenuIDSys { get; set; }
        public int MenuParentID {get; set;}
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string Api { get; set; }
        public byte? Sort { get; set; }
        public string Icon { get; set; }
        public byte IsPermission { get; set; }
        public int have { get; set; }
        public List<MenuDto> ParentMenu { get; set; }
    }
}
