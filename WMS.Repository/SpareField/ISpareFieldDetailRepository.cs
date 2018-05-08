
using System.Collections.Generic;
using WIM.Core.Repository;
using WMS.Common.ValueObject;
using WMS.Entity.SpareField;

namespace WMS.Repository
{
    public interface ISpareFieldDetailRepository : IRepository<SpareFieldDetail>
    {
        void insertByDto(int refID,IEnumerable<SpareFieldsDto> spdDto);
    }
}
