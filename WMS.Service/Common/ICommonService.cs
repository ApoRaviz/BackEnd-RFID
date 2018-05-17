using System.Collections.Generic;
using WIM.Core.Service;
using WMS.Common.ValueObject;

namespace WMS.Service.Common
{
    public interface ICommonService : IService
    {
        IEnumerable<CheckDependentPKDto> CheckDependentPK(string TableName, string ColumnName, string Value = "");

    }
}
