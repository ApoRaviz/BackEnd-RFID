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
    public class FormTopicRepository : Repository<FormTopic>, IFormTopicRepository
    {
        private HRMSDbContext Db { get; set; }

        public FormTopicRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
