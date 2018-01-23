using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Report
{
    public class ReportDetail
    {
        public string FormatType { get; set; }
        public string FileExtention { get; set; }
        public string Delimiter { get; set; }
        public string TextGualifier { get; set; }
        public string Encoding { get; set; }
        public Nullable<int> StartExportRow { get; set; }
        public Nullable<bool> IncludeHeader { get; set; }
        public Nullable<bool> AddHeaderLayout { get; set; }
        public string HeaderLayout { get; set; }
        public List<TableDetail> Detail { get; set; }
        public List<string> Where { get; set; }
        public List<FilterDetail> Filter { get; set; }
        public List<OperationDetail> Operator { get; set; }

    }
}
