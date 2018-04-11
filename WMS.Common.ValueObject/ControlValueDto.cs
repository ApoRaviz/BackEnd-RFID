﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class ControlValueDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Condition { get; set; }
    }
}
