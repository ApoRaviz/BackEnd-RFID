using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity;

namespace WIM.Core.Service.Impl
{

    public class EmployeeService : IEmployeeService
    {
        private EmployeeRepository repo;

        public EmployeeService()
        {
            repo = new EmployeeRepository();
        }

        public IEnumerable<Employee_MT> GetEmployees()
        {
            return repo.Get();
        }

        public Employee_MT GetEmployeeByEmployeeIDSys(string id)
        {
            Employee_MT Employee = repo.GetByID(id);
            return Employee;
        }

        public Employee_MT GetEmployeeByPerson(int id)
        {
            Employee_MT Employee = repo.GetByPersonID(id);
            return Employee;
        }

        public string CreateEmployee(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Insert(Employee);
                    scope.Complete();
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
                return Employee.EmID;
            }
        }

        public bool UpdateEmployee(string id, Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(Employee);
                    scope.Complete();
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

        public bool UpdateEmployeeByID(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(Employee);
                    scope.Complete();
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

        public bool DeleteEmployee(string id)
        {
            using (var scope = new TransactionScope())
            {
                var existedEmployee = repo.GetByID(id);
                existedEmployee.Active = 0;
                existedEmployee.UpdateDate = DateTime.Now;
                existedEmployee.UserUpdate = "1";

                try
                {
                    repo.Update(existedEmployee);
                    scope.Complete();
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
