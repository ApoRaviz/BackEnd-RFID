using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.Status;
using WIM.Core.Service;

namespace HRMS.Service.StatusManagement
{
    public interface IStatusService : IService
    {
        StatusDto GetStatusByID(int id);
        IEnumerable<Status_MT> GetStatus();
        Status_MT CreateStatus(Status_MT status);
        Status_MT CreateStatus(Status_MT status, IEnumerable<SubModuleDto> submodule);
        Status_MT UpdateStatus(Status_MT status);
    }
}
