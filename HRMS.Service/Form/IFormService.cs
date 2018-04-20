using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using System.Collections.Generic;
using WIM.Core.Service;

namespace HRMS.Service.Form
{
    public interface IFormService : IService
    {
        IEnumerable<FormQuestion> GetFormQuestion();
        IEnumerable<FormDetail> GetFormDetailByEvaID(int id);
        Evaluated GetEvaluatedByEvaID(int id);       
        IEnumerable<object> GetEvaluated();
        bool UpdateFormDetail(IEnumerable<FormDetail> formDetail);
        Evaluated UpdateEvaluated(Evaluated evaluated);


    }
}
