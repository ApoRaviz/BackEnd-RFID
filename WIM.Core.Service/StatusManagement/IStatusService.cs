using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Status;


namespace WIM.Core.Service.StatusManagement
{
    public interface IStatusService : IService
    {
        StatusDto GetStatusByID(int id);
        IEnumerable<StatusSubModuleDto> GetStatus();
        Status_MT CreateStatus(Status_MT status);
        Status_MT CreateStatus(Status_MT status, IEnumerable<SubModuleDto> submodule);
        Status_MT UpdateStatus(StatusDto status);
    }
}
