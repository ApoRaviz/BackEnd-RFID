using WIM.Core.Context;
using WIM.Core.Entity.importManagement;

namespace WIM.Core.Repository.Impl
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
