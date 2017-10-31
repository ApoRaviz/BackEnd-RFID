using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.Report
{
    [Table("ReportLayoutHeader_MT")]
    public class ReportLayoutHeader_MT : BaseEntity
    {        
        public ReportLayoutHeader_MT()
        {
            this.ReportLayoutDetail_MT = new HashSet<ReportLayoutDetail_MT>();
        }

        [Key]
        public int ReportIDSys { get; set; }
        public string ReportID { get; set; }
        public string FormatName { get; set; }
        public string FormatType { get; set; }
        public string FileExtention { get; set; }
        public string Delimiter { get; set; }
        public string TextGualifier { get; set; }
        public string Encoding { get; set; }
        public Nullable<int> StartExportRow { get; set; }
        public Nullable<bool> IncludeHeader { get; set; }
        public Nullable<bool> AddHeaderLayout { get; set; }
        public string HeaderLayout { get; set; }
        public string ForTable { get; set; }

        public virtual ICollection<ReportLayoutDetail_MT> ReportLayoutDetail_MT { get; set; }
        public List<ReportLayoutDetail_MT> detail;
    }
}
