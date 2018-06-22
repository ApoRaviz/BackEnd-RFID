using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity;
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class ProbationService: Service, IProbationService
    {
        public ProbationService()
        {
        }

        public IEnumerable<Probation_MT> GetProbation()
        {
            IEnumerable<Probation_MT> Probation;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProbationRepository repo = new ProbationRepository(Db);
                Probation = repo.Get();
            }
            return Probation;
        }

        public Probation_MT GetProbationByProbationIDSys(int id)
        {
            Probation_MT Probation;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProbationRepository repo = new ProbationRepository(Db);
                Probation = repo.GetByID(id);
            }
            return Probation;
        }

        public int CreateProbation(Probation_MT Probation)
        {
            using (var scope = new TransactionScope())
            {
                Probation_MT Probationnew = new Probation_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProbationRepository repo = new ProbationRepository(Db);
                        Probationnew = repo.Insert(Probation);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return Probationnew.ProbationIDSys;
            }
        }

        public bool UpdateProbation(Probation_MT Probation)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProbationRepository repo = new ProbationRepository(Db);
                        repo.Update(Probation);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return true;
            }
        }

        public bool DeleteProbation(int id)
        {
            throw new NotImplementedException();
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new AppValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }
}
