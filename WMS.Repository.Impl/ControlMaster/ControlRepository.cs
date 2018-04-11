using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ControlMaster;
using WMS.Repository.ControlMaster;

namespace WMS.Repository.Impl.ControlMaster
{
    public class ControlRepository : Repository<Control_MT>, IControlRepository
    {
        private WMSDbContext Db { get; set; }

        public ControlRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
