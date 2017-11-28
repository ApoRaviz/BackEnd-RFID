using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.LayoutManagement;
using WMS.Repository.Label;

namespace WMS.Repository.Impl.Label
{
    public class LabelLayoutDetailRepository : Repository<LabelLayoutHeader_MT>, ILabelLayoutHeaderRepository
    {
        private WMSDbContext Db { get; set; }

        public LabelLayoutDetailRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
