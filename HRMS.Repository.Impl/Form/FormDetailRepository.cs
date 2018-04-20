using HRMS.Context;
using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using System.Linq;
using WIM.Core.Context;
using HRMS.Entity.Form;
using HRMS.Repository.Form;

namespace HRMS.Repository.Impl
{
    public class FormDetailRepository : Repository<FormDetail>, IFormDetailRepository
    {
        private HRMSDbContext Db { get; set; }

        public FormDetailRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<FormDetail> Get1(int id)
        {
             var formDetail = from fdt in Db.FormDetail
                            where fdt.EvaluatedIDSys == id
                            select fdt;
            return formDetail.ToList();
        }
    }
}
