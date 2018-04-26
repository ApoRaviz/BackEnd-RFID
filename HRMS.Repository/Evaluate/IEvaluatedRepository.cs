using HRMS.Common.ValueObject;
using HRMS.Common.ValueObject.ReportEvaluation;
using HRMS.Entity.Evaluate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace HRMS.Repository.Evaluate
{
    public interface IEvaluatedRepository : IRepository<Evaluated>
    {
        IEnumerable<object> GetList();
        IEnumerable<EvaluationTable> GetFormDetailList(int id);
        IEnumerable<EvaluatedReport> GetFormReportList(int id);

    }
}

