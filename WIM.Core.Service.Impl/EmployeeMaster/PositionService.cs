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
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class PositionService : Service, IPositionService
    {
        public PositionService()
        {
        }

        public IEnumerable<Positions> GetPositions()
        {
            IEnumerable<Positions> position;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPositionRepository repo = new PositionRepository(Db);
                position = repo.Get();
            }
            return position;
        }

        public Positions GetPositionByPositionIDSys(int id)
        {
            Positions position;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPositionRepository repo = new PositionRepository(Db);
                position = repo.GetByID(id);
            }
            return position;
        }

        public int CreatePosition(Positions position)
        {
            using (var scope = new TransactionScope())
            {
                Positions Positionnew = new Positions();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        Positionnew = repo.Insert(position);
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
                return Positionnew.PositionIDSys;
            }
        }

        public bool UpdatePosition(Positions position)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        repo.Update(position);
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

        public bool DeletePosition(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        var existedPosition = repo.GetByID(id);
                        existedPosition.IsActive = false;
                        repo.Update(existedPosition);
                        scope.Complete();
                        Db.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                return true;
            }
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
