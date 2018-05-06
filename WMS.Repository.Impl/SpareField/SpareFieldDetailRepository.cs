using System.Collections.Generic;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.SpareField;

namespace WMS.Repository.Impl
{
    public class SpareFieldDetailRepository : Repository<SpareFieldDetail>, ISpareFieldDetailRepository
    {
        private WMSDbContext Db { get; set; }
        public SpareFieldDetailRepository(Context.WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public void insertByDto(int refID,IEnumerable<SpareFieldsDto> spdDto)
        {

            List<SpareFieldDetail> spdList = new List<SpareFieldDetail>();
            SpareFieldDetail spd;
            foreach (SpareFieldsDto obj in spdDto)
            {
                if(obj.Value == null)
                {
                    continue;
                }


                spd = new SpareFieldDetail();
                spd.SpfIDSys = obj.SpfIDSys;
                spd.SpfdRefID = refID;
                spd.Value = obj.Value;
                spd.Type = obj.Type;
                Insert(spd);
            }
        }
    }
    
}
