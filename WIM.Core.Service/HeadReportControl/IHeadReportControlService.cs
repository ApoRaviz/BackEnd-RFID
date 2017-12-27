using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.LabelManagement;

namespace WIM.Core.Service
{
    public interface IHeadReportControlService : IService
    {
        IEnumerable<HeadReportControl> GetHeadReportControls();
        HeadReportControl GetHeadReportControlByID(int headreportidsys);
        IEnumerable<HeadReportControl> GetHeadReportControlsByModuleID( int SubModuleIDSys);
        int CreateHeadReportControl(HeadReportControl headreport);
        bool UpdateHeadReportControl(HeadReportControl headreport);
        bool DeleteHeadReportControl(int headreportidsys);
    }
}
