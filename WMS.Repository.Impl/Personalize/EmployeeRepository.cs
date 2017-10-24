using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WIM.Core.Context;
using WIM.Core.Entity.Employee;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class EmployeeRepository : IGenericRepository<Employee_MT>
    {
        private CoreDbContext Db { get; set; }

        public EmployeeRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Employee_MT> Get()
        {
            var employee = from c in Db.Employee_MT
                           select c;
            return employee.ToList();
        }

        public Employee_MT GetByID(object id)
        {
            Employee_MT Employee = (from i in Db.Employee_MT
                                    where i.EmID == id
                                    select i).Include("Person_MT").SingleOrDefault();
            return Employee;
        }

        public Employee_MT GetByPersonID(int id)
        {
            Employee_MT Employee = (from i in Db.Employee_MT
                                    where i.PersonIDSys == id
                                    select i).Include(b => b.Person_MT).SingleOrDefault();
            return Employee;
        }

        public void Insert(Employee_MT entity)
        {
            entity.Active = 1;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            Db.Employee_MT.Add(entity);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Employee_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee_MT entityToUpdate)
        {
            var existedEmployee = (from i in Db.Employee_MT
                                   where i.EmID == entityToUpdate.EmID
                                   select i).SingleOrDefault();
            existedEmployee.EmID = entityToUpdate.EmID;
            existedEmployee.Area = entityToUpdate.Area;
            existedEmployee.Position = entityToUpdate.Position;
            existedEmployee.Dept = entityToUpdate.Dept;
            existedEmployee.TelOffice = entityToUpdate.TelOffice;
            existedEmployee.TelEx = entityToUpdate.TelEx;
            existedEmployee.UpdateDate = DateTime.Now;
            existedEmployee.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Employee_MT> GetMany(Func<Employee_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee_MT> GetManyQueryable(Func<Employee_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Employee_MT Get(Func<Employee_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Employee_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee_MT> GetWithInclude(Expression<Func<Employee_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Employee_MT GetSingle(Func<Employee_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Employee_MT GetFirst(Func<Employee_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
