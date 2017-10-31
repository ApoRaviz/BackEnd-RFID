using Fuji.Entity.ProgramVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Data.Entity;
using Fuji.Context;

namespace Fuji.Repository.Impl.ProgramVersion
{
    public class ProgramVersionRepository : Repository<ProgramVersionHistory>
    {
        FujiDbContext Db { get; set; }
        public ProgramVersionRepository(FujiDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
