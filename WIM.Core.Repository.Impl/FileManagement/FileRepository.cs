using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.FileManagement;
using WIM.Core.Repository.FileManagement;

namespace WIM.Core.Repository.Impl.FileManagement
{
    public class FileRepository : Repository<File_MT>, IFileRepository
    {
        private CoreDbContext Db { get; set; }

        public FileRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }
    }
}
