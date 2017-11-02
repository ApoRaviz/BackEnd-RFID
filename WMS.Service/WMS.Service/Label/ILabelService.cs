using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.LayoutManagement;

namespace WMS.Service.Label
{
    public interface ILabelService : IService
    {
        List<LabelLayoutHeader_MT> GetAllLabelHeader(string forTable);
        int? CreateLabelForItemMaster(LabelLayoutHeader_MT data);
        LabelLayoutHeader_MT GetLabelLayoutByReportIDSys(int id, string include);
        bool UpdateLabelForItemMaster(int LabelIDSys, LabelLayoutHeader_MT data);
    }
}
