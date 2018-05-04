using HRMS.Context;
using HRMS.Entity.Probation;
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using System.Linq;
using WIM.Core.Context;
using HRMS.Entity.Form;
using HRMS.Repository.Form;

namespace HRMS.Repository.Impl
{
    public class FormQuestionRepository : Repository<FormQuestion>, IFormQuestionRepository
    {
        private HRMSDbContext Db { get; set; }

        public FormQuestionRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }
        public IEnumerable<FormQuestion> GetFormQByFormTopicID(int id)
        {

            var formq =
                               from fq in Db.FormQuestions
                               where fq.FormTopicIDSys == id

                               select fq;

            return formq.ToList();
        }
    }

}
