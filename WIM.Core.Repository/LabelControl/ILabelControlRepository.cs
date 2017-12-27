using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;

namespace WIM.Core.Repository
{

    public interface ILabelControlRepository : IRepository<LabelControl>
    {
        LabelControlDto GetDto(string Lang, int ProjectID);
        LabelControlDto EditData(string Lang, int ProjectID);
        LabelControlDto CreateData(string Lang, int ProjectID);
    }
}
