﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Common
{
    [NotMapped]
    public class ExtendedConfig : GeneralConfigs
    {
        public string Test { get; set; }
    }
}
