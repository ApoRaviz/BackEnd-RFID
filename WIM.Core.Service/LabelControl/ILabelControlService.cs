using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;

namespace WIM.Core.Service
{
    public interface ILabelControlService : IService
    {
        LabelControlDto GetDto(string Lang, int ProjectID);
        LabelControlDto UpdateLabelControl(LabelControl LabelDAta);
        LabelControlDto CreateLabelControl(LabelControl LabelDAta);
        bool AddLabelConfig(int ProjectIDSys, List<LabelConfig> LabelConfig);
        LabelControlDto DelLabelConfig(int ProjectIDSys,string[] LabelConfig);
    }
}
