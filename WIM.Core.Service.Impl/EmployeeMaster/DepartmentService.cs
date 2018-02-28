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
using WIM.Core.Repository.Impl.Personalize;
using WIM.Core.Repository.Personalize;
using WIM.Core.Service.EmployeeMaster;

namespace WIM.Core.Service.Impl.EmployeeMaster
{
    public class DepartmentService : Service, IDepartmentService
    {
        public DepartmentService()
        {
        }

        public IEnumerable<Departments> GetDepartments()
        {
            IEnumerable<Departments> department;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IDepartmentRepository repo = new DepartmentRepository(Db);
                department = repo.Get();
            }
            return department;
        }

        public Departments GetDepartmentByDepIDSys(int id)
        {
            Departments Employee;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IDepartmentRepository repo = new DepartmentRepository(Db);
                Employee = repo.GetByID(id);
            }
            return Employee;
        }

        public int CreateDepartment(Departments Department)
        {
            using (var scope = new TransactionScope())
            {
                Departments Departmentnew = new Departments();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IDepartmentRepository repo = new DepartmentRepository(Db);
                        Departmentnew = repo.Insert(Department);
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
                return Departmentnew.DepIDSys;
            }
        }

        public bool UpdateDepartment(Departments Department)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IDepartmentRepository repo = new DepartmentRepository(Db);
                        repo.Update(Department);
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

        public bool UpdateDepartmentByID(Departments Department)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IDepartmentRepository repo = new DepartmentRepository(Db);
                        repo.Update(Department);
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

        public bool DeleteDepartment(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IDepartmentRepository repo = new DepartmentRepository(Db);
                        var existedEmployee = repo.GetByID(id);
                        existedEmployee.IsActive = false;
                        repo.Update(existedEmployee);
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
    }
}
