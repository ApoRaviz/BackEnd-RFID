using Fuji.Context;
using Fuji.Entity.ItemManagement;
using Fuji.Repository.ItemManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace Fuji.Repository.Impl.ItemManagement
{
    public class SerialDetailTempRepository : Repository<ImportSerialDetailTemp>, ISerialDetailTempRepository
    {
        //private FujiDbContext Context { get; set; }
        private FujiDbContext Db { get; set; }
        private DbSet<ImportSerialDetailTemp> DbSet { get; set; }

        public SerialDetailTempRepository(FujiDbContext context) : base(context)
        {
            Db = context;
            this.DbSet = context.Set<ImportSerialDetailTemp>();
        }

    }
}
