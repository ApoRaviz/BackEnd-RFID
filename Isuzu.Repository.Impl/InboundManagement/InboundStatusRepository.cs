using Isuzu.Context;
using Isuzu.Entity;
using Isuzu.Repository;
using Isuzu.Repository.ItemManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace Isuzu.Repository.Impl
{
    public class InboundStatusRepository : Repository<InboundItemsStatus>, IInboundStatusRepository
    {
        private IsuzuDataContext Db { get; set; }
        private DbSet<InboundItemsStatus> DbSet;

        public InboundStatusRepository(IsuzuDataContext context) :base(context)
        {
            Db = context;
            this.DbSet = context.Set<InboundItemsStatus>();
        }

    }
}
