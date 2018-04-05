using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Common
{
    public class LocationFormat
    {
        public LocationFormat()
        {
            Format = new List<Format>();
        }
        public string Key { get; set; }
        public string Separator { get; set; }
        public List<Format> Format { get; set; }
    }

    public class Format
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public Int16 Digit { get; set; }
    }
}



