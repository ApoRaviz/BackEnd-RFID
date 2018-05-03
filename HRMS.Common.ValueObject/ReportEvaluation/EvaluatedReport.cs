using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.ValueObject.ReportEvaluation
{
    public class EvaluatedReport
    {
        public int EvaluatedIDSys { get; set; }
        public int? FormTopicIDSys { get; set; }
        public string EmID { get; set; }
        public int? ResultProbationIDSys { get; set; }
        public string ResultProbationNameEn { get; set; }
        public string EvaluateBy { get; set; }
        public DateTime? EvaluateDate { get; set; }
        public string ApproveBy { get; set; }
        public string HR { get; set; }
        public string Value { get; set; }
        public string ValueOld { get; set; }
        public string ValueNew { get; set; }
        public string[] ValueArr { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Comment { get; set; }
        public string NameEn { get; set; }
        public string SurnameEn { get; set; }
        public int? DepIDSys { get; set; }
        public string DepartmentAcronym { get; set; }
        public string DepName { get; set; }
        public string DepNameEn { get; set; }
        public string PositionAcronym { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEn { get; set; }
        public DateTime? HiredDate { get; set; }
        public DateTime? CompletionOfProbation { get; set; }



    }
}
