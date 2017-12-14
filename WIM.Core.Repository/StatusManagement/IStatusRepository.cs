using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Status;

namespace WIM.Core.Repository.StatusManagement
{
    public interface IStatusRepository : IRepository<Status_MT>
    {
        StatusDto GetDto(int id);
        IEnumerable<StatusSubModuleDto> GetDto();
    }
}
