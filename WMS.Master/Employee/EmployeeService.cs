using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master { 

    public class EmployeeService : IEmployeeService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Employee_MT> repo;

        public EmployeeService()
        {
            repo = new GenericRepository<Employee_MT>(db);
        }        

        public IEnumerable<Employee_MT> GetEmployees()
        {           
            return repo.GetAll();
        }

        public Employee_MT GetEmployeeByEmployeeIDSys(string id)
        {
            Employee_MT Employee = (from i in db.Employee_MT
                                where i.EmID == id
                                select i).Include("Person_MT").SingleOrDefault();
            return Employee;            
        }

        public Employee_MT GetEmployeeByPerson(int id)
        {
            Employee_MT Employee = (from i in db.Employee_MT
                                    where i.PersonIDSys == id
                                    select i).Include("Person_MT").SingleOrDefault();
            return Employee;
        }

        public string CreateEmployee(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                Employee.Active = 1;
                Employee.CreatedDate = DateTime.Now;
                Employee.UpdateDate = DateTime.Now;
                Employee.UserUpdate = "1";
                
                repo.Insert(Employee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return Employee.EmID;
            }
        }

        public bool UpdateEmployee(string id, Employee_MT Employee)
        {           
            using (var scope = new TransactionScope())
            {
                var existedEmployee = (from i in db.Employee_MT
                                     where i.EmID == id
                                     select i).SingleOrDefault();
                existedEmployee.EmID = Employee.EmID;
                existedEmployee.Area = Employee.Area;
                existedEmployee.Position = Employee.Position;
                existedEmployee.Dept = Employee.Dept;
                existedEmployee.TelOffice = Employee.TelOffice;
                existedEmployee.TelEx = Employee.TelEx;
                existedEmployee.UpdateDate = DateTime.Now;
                existedEmployee.UserUpdate = "1";
                repo.Update(existedEmployee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }

        public bool UpdateEmployeeByID(Employee_MT Employee)
        {
            using (var scope = new TransactionScope())
            {
                var existedEmployee = (from i in db.Employee_MT
                                     where i.EmID == Employee.EmID
                                     select i).SingleOrDefault();
                existedEmployee.EmID = Employee.EmID;
                existedEmployee.Area = Employee.Area;
                existedEmployee.Position = Employee.Position;
                existedEmployee.Dept = Employee.Dept;
                existedEmployee.TelOffice = Employee.TelOffice;
                existedEmployee.TelEx = Employee.TelEx;
                existedEmployee.UpdateDate = DateTime.Now;
                existedEmployee.UserUpdate = "1";
                repo.Update(existedEmployee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
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
                repo.Update(existedEmployee);
                try
                {
                db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException e)
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
