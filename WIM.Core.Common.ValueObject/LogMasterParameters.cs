using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Common.ValueObject
{
    public class LogMasterParameters
    {
        public string RequestMethod { get; set; }
        public string RequestUrl { get; set; }
        public string RequestUrlFrontEnd { get; set; }
        public string RequestMenuNameFrontEnd { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public string Username { get; set; }

        public int Rows { get; set; }
        public int PageNum { get; set; }
        public int Totalrow { get; set; }


    }
}
