using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.Report
{
    [Table("ReportLayout_MT")]
    public class ReportLayout_MT : BaseEntity
    {
        [Key]
        public int ReportIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string ReportID { get; set; }
        public string FormatName { get; set; }
        public string Detail { get; private set; }

        [NotMapped]
        public ReportDetail ReportDetail
        {
            get
            {
                if (!string.IsNullOrEmpty(Detail))
                {
                    return JsonConvert.DeserializeObject<ReportDetail>(StringHelper.Decompress(Detail));
                }
                return null;
            }
            set
            {
                Detail = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }
    }
}
