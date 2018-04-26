using HRMS.Common.ValueObject;
using HRMS.Common.ValueObject.ReportEvaluation;
using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using System.Collections.Generic;
using System.Net.Http;
using WIM.Core.Service;

namespace HRMS.Service.Form
{
    public interface IFormService : IService
    {
        IEnumerable<FormQuestion> GetFormQuestionByFormTopicID(int id);
        IEnumerable<FormDetail> GetFormDetailByEvaID(int id);
        Evaluated GetEvaluatedByEvaID(int id);       
        IEnumerable<object> GetEvaluated();
        IEnumerable<EvaluatedReport> GetEvaluatedFormByID(int id);
        IEnumerable<EvaluationTable> GetEvaluatedFormDetailByID(int id);
        StreamContent GetReportStream(IEnumerable<EvaluatedReport> item1, IEnumerable<EvaluationTable> item2);
        bool UpdateFormDetail(IEnumerable<FormDetail> formDetail);
        Evaluated UpdateEvaluated(Evaluated evaluated);


    }
}
