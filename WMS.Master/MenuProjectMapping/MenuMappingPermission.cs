﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master
{
    class MenuMappingPermission
    {
        public int MenuIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public int MenuIDSysParent { get; set; }
        public byte Sort { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
    }
}
