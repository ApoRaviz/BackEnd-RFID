using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.TableControl;

namespace WIM.Core.Repository.Impl
{
 public class TableControlRepository : Repository<TableControl>, ITableControlRepository
    {
        private CoreDbContext Db;
        public TableControlRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
