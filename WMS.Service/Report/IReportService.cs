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
        List<ReportLayoutHeader_MT> GetAllReportHeader(string forTable);
        ReportLayoutHeader_MT GetReportLayoutByReportIDSys(int id, string include);
        int? CreateReportForItemMaster(ReportLayoutHeader_MT data);
        bool UpdateReportForItemMaster(int ReportIDSys, ReportLayoutHeader_MT data);
        DataTable GetReportData(int ReportIDSys);
    }
}
