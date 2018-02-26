using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.Receiving;

namespace WMS.Repository.Impl
{
    public class ReceiveRepository : Repository<Receive>, IReceiveRepository
    {
        private WMSDbContext Db { get; set; }
        public ReceiveRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
