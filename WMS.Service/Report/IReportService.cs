using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.Report;

namespace WMS.Service.Report
{
    public interface IReportService : IService
    {
        List<ReportLayout_MT> GetAllReportHeader(int ProjectIDSys);
        ReportLayout_MT GetReportLayoutByReportIDSys(int id);
        int CreateReportForItemMaster(ReportLayout_MT data);
        bool UpdateReportForItemMaster(int ReportIDSys, ReportLayout_MT data);
        DataTable GetReportData(int ReportIDSys);
    }
}
