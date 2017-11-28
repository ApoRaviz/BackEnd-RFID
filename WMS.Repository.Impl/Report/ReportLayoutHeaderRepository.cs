using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.Report;
using WMS.Repository.Report;

namespace WMS.Repository.Impl.Report
{
    public class ReportLayoutHeaderRepository : Repository<ReportLayoutHeader_MT> , IReportLayoutHeaderRepository
    {
        private WMSDbContext Db { get; set; }

        public ReportLayoutHeaderRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
