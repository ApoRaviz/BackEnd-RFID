using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Status;
using WIM.Core.Repository;


namespace HRMS.Repository.StatusManagement
{
    public interface ISubModuleRepository : IRepository<SubModules>
    {
        SubModuleDto GetDto();

    }
}
