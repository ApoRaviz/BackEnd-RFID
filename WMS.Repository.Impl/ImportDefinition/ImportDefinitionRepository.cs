using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ImportManagement;
using WMS.Repository.ImportDefinition;

namespace WMS.Repository.Impl.ImportDefinition
{
    public class ImportDefinitionRepository : Repository<ImportDefinitionHeader_MT>, IImportDefinitionRepository
    {
        private WMSDbContext Db { get; set; }
        public ImportDefinitionRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
