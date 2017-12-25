using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;

namespace WIM.Core.Service
{
    public interface ILabelControlService : IService
    {
        LabelControlDto GetDto(string Lang, int ProjectID);
        LabelControlDto UpdateLabelControl(LabelControl LabelDAta);
        LabelControlDto CreateLabelControl(LabelControl LabelDAta);
    }
}
