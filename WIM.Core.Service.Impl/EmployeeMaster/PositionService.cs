using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.PositionConfigManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
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
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
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

        public bool DeletePosition(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        IEmployeeRepository repoem = new EmployeeRepository(Db);
                        var employee = repoem.GetMany(a => a.PositionIDSys == id && a.IsActive == true).ToList();
                        if (employee.Count > 0)
                        {
                            return false;
                        }
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
                    ValidationException ex = new ValidationException(ErrorEnum.E4017);
                    throw ex;
                }
                return true;
            }
        }


        public Positions SetPositionConfig(int id, List<PositionConfig<List<PositionConfig<List<PositionConfig<string>>>>>> positionConfig)
        {
            using (var scope = new TransactionScope())
            {
                Positions Positionnew = new Positions();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        Positions position = new Positions();
                        position = repo.GetByID(id); ;
                        position.PositionsConfig = new List<PositionConfig<List<PositionConfig<List<PositionConfig<string>>>>>>();
                        position.PositionsConfig = positionConfig;
                        Positionnew = repo.Update(position);
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
                return Positionnew;
            }
        }
        public Positions SetPositionConfig2(int id, WelfareConfig positionConfig)
        {
            using (var scope = new TransactionScope())
            {
                Positions Positionnew = new Positions();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPositionRepository repo = new PositionRepository(Db);
                        Positions position = new Positions();
                        position = repo.GetByID(id); ;
                        position.PositionsConfig2 = positionConfig;
                        Positionnew = repo.Update(position);
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
                return Positionnew;
            }
        }
    }
}
