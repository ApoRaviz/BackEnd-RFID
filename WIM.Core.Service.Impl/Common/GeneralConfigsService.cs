using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Common;
using WIM.Core.Repository.Common;
using WIM.Core.Repository.Impl.Common;
using WIM.Core.Service.Common;

namespace WIM.Core.Service.Impl.Common
{
    public class GeneralConfigsService : Service, IGeneralConfigsService
    {
        public GeneralConfigs CreateGeneralConfigs(GeneralConfigs config)
        {
            using (var scope = new TransactionScope())
            {
                GeneralConfigs confignew = new GeneralConfigs();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                        confignew = repo.Insert(config);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
                    throw ex;
                }
                return confignew;
            }
        }

        public bool UpdateGeneralConfigs(GeneralConfigs config)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    { 
                        IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                        repo.Update(config);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
                    throw ex;
                }
                return true;
            }
        }
    }
}
