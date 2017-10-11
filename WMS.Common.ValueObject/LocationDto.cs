using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Common
{
    class LocationDto
    {
        public int LocIDSys { get; set; }
        public string LineID { get; set; }
        public string WHID { get; set; }
        public string LocNo { get; set; }
        public string QualityType { get; set; }
        public string RackType { get; set; }
        public string Tempature { get; set; }
        public string CateID { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public byte Active { get; set; }
    }
}
