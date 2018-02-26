using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Service.Impl
{

    public class EmployeeService : Service, IEmployeeService
    {
        public EmployeeService()
        {
        }

        public IEnumerable<Employee_MT> GetEmployees()
        {
            IEnumerable<Employee_MT> employee;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IEmployeeRepository repo = new EmployeeRepository(Db);
                employee = repo.Get();
            }
            return employee;
        }

        public Employee_MT GetEmployeeByEmployeeIDSys(string id)
        {
            Employee_MT Employee;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IEmployeeRepository repo = new EmployeeRepository(Db);
                string[] include = { "Person_MT" };
                Employee = repo.GetWithInclude((c => c.EmID == id),include).SingleOrDefault();
            }
            return Employee;
        }

        public Employee_MT GetEmployeeByPerson(int id)
        {
            Employee_MT Employee;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IEmployeeRepository repo = new EmployeeRepository(Db);
                string[] include = { "Person_MT" };
                Employee = repo.GetWithInclude((c => c.PersonIDSys == id), include).SingleOrDefault();
            }
            return Employee;
        }

        public string CreateEmployee(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                Employee_MT Employeenew = new Employee_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IEmployeeRepository repo = new EmployeeRepository(Db);
                        if(Employee.EmID == null)
                        {
                            Employee.EmID = (repo.GetMaxEMID(Employee.DepIDSys) + 1).ToString();
                        }
                        Employeenew = repo.Insert(Employee);
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
                return Employeenew.EmID;
            }
        }

        public bool UpdateEmployee(Employee_MT Employee )
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IEmployeeRepository repo = new EmployeeRepository(Db);
                        repo.Update(Employee);
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

        public bool UpdateEmployeeByID(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IEmployeeRepository repo = new EmployeeRepository(Db);
                        repo.Update(Employee);
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

        public bool DeleteEmployee(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IEmployeeRepository repo = new EmployeeRepository(Db);
                        var existedEmployee = repo.GetByID(id);
                        //IsActive = False;



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
