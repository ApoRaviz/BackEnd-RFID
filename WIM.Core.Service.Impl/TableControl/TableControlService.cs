using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.TableControl;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Service.Impl
{
    public class TableControlService : Service, ITableControlService
    {
        public IEnumerable<TableControl> GetTableControl()
        {
            IEnumerable<TableControl> tableControl;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ITableControlRepository repo = new TableControlRepository(Db);
                tableControl = repo.Get();
            }
            return tableControl;
        }
    }
}
