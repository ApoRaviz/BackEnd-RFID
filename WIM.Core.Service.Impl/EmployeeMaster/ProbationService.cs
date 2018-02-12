using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Employee;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class ProbationService : Service, IProbationService
    {
        public IEnumerable<Probation_MT> GetProbation()
        {
            IEnumerable<Probation_MT> probation;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IProbationRepository repo = new ProbationRepository(Db);
                probation = repo.Get();
            }
            return probation;
        }

        public Probation_MT GetProbationByEmID(int id)
        {
            Probation_MT position;
            using (CoreDbContext Db = new CoreDbContext())
            {
                    IEmployeeRepository repo = new EmployeeRepository(Db);
                string[] include = { "Probation_MT" };
                position = repo.GetWithInclude(a => a.ProbationIDSys == id,include).Select(s => s.Probation_MT).SingleOrDefault();
            }
            return position;
        }

        public int CreateProbation(Probation_MT probation)
        {
            using (var scope = new TransactionScope())
            {
                Probation_MT probationNew = new Probation_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProbationRepository repo = new ProbationRepository(Db);
                        probationNew = repo.Insert(probation);
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return probationNew.ProbationIDSys;
            }
        }


        public bool UpdateProbation(Probation_MT probation)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IProbationRepository repo = new ProbationRepository(Db);
                        repo.Update(probation);
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return true;
            }
        }

        public bool Delete(Probation_MT id)
        {
            throw new NotImplementedException();
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
