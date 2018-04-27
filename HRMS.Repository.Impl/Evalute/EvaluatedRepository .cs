using HRMS.Context;
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using System.Linq;
using WIM.Core.Context;
using HRMS.Repository.Evaluate;
using HRMS.Entity.Evaluate;
using HRMS.Common.ValueObject;
using HRMS.Common.ValueObject.ReportEvaluation;

namespace HRMS.Repository.Impl
{
    public class EvaluatedRepository : Repository<Evaluated>, IEvaluatedRepository
    {
        private HRMSDbContext Db { get; set; }

        public EvaluatedRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<object> GetList()
        {

            var evaluatelist =
                               from eva in Db.Evaluated
                               join vemp in Db.VEmployeeInfo on eva.EmID equals vemp.EmID
                               select new { eva.EvaluatedIDSys, eva.EmID, eva.EvaluateDate, vemp.Name, vemp.Surname, vemp.DepName, vemp.EmTypeName };

            return evaluatelist.ToList();
        }

        public IEnumerable<EvaluationTable> GetFormDetailList(int id)
        {

            var evaluateformlist =
                               from eva in Db.Evaluated
                               join vemp in Db.VEmployeeInfo on eva.EmID equals vemp.EmID
                               join fdt in Db.FormDetail on eva.EvaluatedIDSys equals fdt.EvaluatedIDSys

                               from fqt in Db.FormQuestion
                               where fqt.FormQIDSys == fdt.FormQIDSys

                               where eva.EvaluatedIDSys == id

                               select new EvaluationTable()
                               {
                                   FormQ = fqt.FormQ,
                                   FormQEn = fqt.FormQEn,
                                   FormAns = fdt.FormAns,
                               };

            return evaluateformlist.ToList();
        }

        public IEnumerable<EvaluatedReport> GetFormReportList(int id)
        {

            var evaluateformlist =
                               from eva in Db.Evaluated
                               join vemp in Db.VEmployeeInfo on eva.EmID equals vemp.EmID

                               where eva.EvaluatedIDSys == id

                               select new EvaluatedReport()
                               {
                                   EmID = eva.EmID,
                                   EvaluateBy = eva.EvaluateBy,
                                   EvaluateDate = eva.EvaluateDate,
                                   EvaluatedIDSys = eva.EvaluatedIDSys,
                                   FormTopicIDSys = eva.FormTopicIDSys,
                                   ApproveBy = eva.ApproveBy,
                                   Comment = eva.Comment,
                                   ResultProbationIDSys = eva.ResultProbationIDSys,
                                   Value = eva.Value,
                                   ValueOld = " ",
                                   ValueNew = " ",
                                   ValueDate = eva.ValueDate,
                                   HR = eva.HR,
                                   NameEn = vemp.NameEn + " " + vemp.SurnameEn,
                                   PositionNameEn = vemp.PositionNameEn,
                                   DepNameEn = vemp.DepNameEn,
                                   HiredDate = vemp.HiredDate,
                                   CompletionOfProbation = vemp.CompletionOfProbation,

                               };

            return evaluateformlist.ToList();
        }
    }
}
