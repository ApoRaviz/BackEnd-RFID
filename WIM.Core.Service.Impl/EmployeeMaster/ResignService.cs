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
using WIM.Core.Entity.Person;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class ResignService : Service, IResignService
    {
        

        public IEnumerable<Resign> GetResign()
        {
            IEnumerable<Resign> resign;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IResignRepository repo = new ResignRepository(Db);
                resign = repo.Get();
            }
            return resign;
        }

        public Resign GetResignByEmID(string id)
        {
            Resign resign;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IResignRepository repo = new ResignRepository(Db);
                resign = repo.Get(a => a.EmID == id);
            }
            return resign;
        }

        public string CreateResign(Resign resign)
        {
            using (var scope = new TransactionScope())
            {
                Resign newResign = new Resign();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IResignRepository repo = new ResignRepository(Db);
                        newResign = repo.Insert(resign);
                        IEmployeeRepository repoEmployee = new EmployeeRepository(Db);
                        IPersonRepository repoPerson = new PersonRepository(Db);
                        string[] include = { "Person_MT" };
                        Person_MT person = repoEmployee.GetWithInclude(a => a.EmID == resign.EmID, include).Select(a => a.Person_MT).SingleOrDefault();
                        person.IsActive = false;
                        repoPerson.Update(person);
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
                return newResign.EmID;
            }
        }

        public bool UpdateResign(Resign resign)
        {
            throw new NotImplementedException();
        }

        public bool DeleteResign(int id)
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
