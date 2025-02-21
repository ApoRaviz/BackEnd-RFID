﻿using AutoMapper;
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
using WIM.Core.Entity.PositionConfigManagement;

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
                Employee = repo.GetWithInclude((c => c.EmID == id), include).SingleOrDefault();
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
                        if (Employee.EmID == null)
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return Employeenew.EmID;
            }
        }

        public bool UpdateEmployee(Employee_MT Employee)
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }
                return true;
            }
        }

        public Employee_MT SetPositionConfig2(string id, WelfareConfig positionConfig)
        {
            using (var scope = new TransactionScope())
            {
                Employee_MT employeeNew = new Employee_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IEmployeeRepository repo = new EmployeeRepository(Db);
                        Employee_MT employee = new Employee_MT();
                        employee = repo.GetByID(id);

                        if (employee.EmpConfidentialConfigs != null)
                        {
                            var x = employee.EmpConfidentialConfigs;
                            x.PositionConfig.WelfareConfig = positionConfig;
                            employee.ConfidentialConfigs = null;
                            employee.EmpConfidentialConfigs = x;
                            //employee.EmpConfidentialConfigs.PositionConfig.WelfareConfig = positionConfig;
                        }
                        else
                        {
                            employee.EmpConfidentialConfigs = new EmployeeConfidentialConfig
                            {
                                PositionConfig = new PositionConfig
                                {
                                    WelfareConfig = positionConfig
                                }
                            };
                        }

                        employeeNew = repo.Update(employee);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return employeeNew;
            }
        }
    }
}
