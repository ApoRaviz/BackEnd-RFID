using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Employee;
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class HistoryWarningService : Service, IHistoryWarningService
    {
        public HistoryWarningService()
        {

        }

        public IEnumerable<HistoryWarning> GetHistories()
        {
            IEnumerable<HistoryWarning> history;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IHistoryWarningRepository repo = new HistoryWarningRepository(Db);
                history = repo.Get();
            }
            return history;
        }

        public IEnumerable<HistoryWarning >GetHistoryByEmID(string id)
        {
            IEnumerable<HistoryWarning> history;
            using (CoreDbContext Db = new CoreDbContext())
            {
                CoreDbContext db = new CoreDbContext();
                IHistoryWarningRepository repo = new HistoryWarningRepository(Db);
                history = repo.GetMany(a => a.EmID == id).Select(s => new HistoryWarning() {
                    EmID = s.EmID,
                    Description = s.Description,
                    FileRefID = s.FileRefID,
                    StatusIDSys = s.StatusIDSys,
                    WarnIDSys = s.WarnIDSys,
                    WarningDate = s.WarningDate,
                    File_MT = db.File_MT.Where(d => d.FileUID == s.FileRefID).SingleOrDefault()
                    
                }).ToList();
            }
            return history;
        }

        public int CreateHistory(HistoryWarning warning)
        {
            using (var scope = new TransactionScope())
            {
                HistoryWarning history = new HistoryWarning();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IHistoryWarningRepository repo = new HistoryWarningRepository(Db);
                        history = repo.Insert(warning);
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
                return history.WarnIDSys;
            }
        }

        public bool UpdateHistory(HistoryWarning warning)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IHistoryWarningRepository repo = new HistoryWarningRepository(Db);
                        repo.Update(warning);
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

        public bool DeleteHistory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
