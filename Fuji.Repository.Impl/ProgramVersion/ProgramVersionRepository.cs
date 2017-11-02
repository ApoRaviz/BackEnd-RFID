using Fuji.Entity.ProgramVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Data.Entity;
using Fuji.Context;
using Fuji.Repository.ProgramVersion;
using System.Security.Principal;

namespace Fuji.Repository.Impl.ProgramVersion
{
    public class ProgramVersionRepository : Repository<ProgramVersionHistory>, IProgramVersionRepository
    {
        FujiDbContext Db { get; set; }
        private IIdentity Identity { get; set; }

        public ProgramVersionRepository(FujiDbContext context,IIdentity identity) : base(context, identity)
        {
            Db = context;
            Identity = identity;
        }
    }
}
