using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Status;

namespace WIM.Core.Repository.StatusManagement
{
    public interface ISubModuleRepository : IRepository<SubModules>
    {
        SubModuleDto GetDto();

    }
}
