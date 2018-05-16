using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.importManagement;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository.ImportDefinition;

namespace WIM.Core.Repository.Impl.ImportDefinition
{
    public class ImportDefinitionRepository : Repository<ImportDefinitionHeader_MT>, IImportDefinitionRepository
    {
        private CoreDbContext Db { get; set; }
        public ImportDefinitionRepository(CoreDbContext context):base(context)
        {
            Db = context;
        }
    }
}
