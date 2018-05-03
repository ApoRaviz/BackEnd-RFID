using HRMS.Context;
using HRMS.Entity.Probation;
using HRMS.Repository;
using HRMS.Repository.Impl;
using HRMS.Service.Probation;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.PositionConfigManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace HRMS.Service.Impl
{
    public class VEmployeeInfoService : WIM.Core.Service.Impl.Service, IVEmployeeInfoService
    {

        public VEmployeeInfoService()
        {

        }

        public IEnumerable<VEmployeeInfo> GetProbation()
        {
            IEnumerable<VEmployeeInfo> Probation;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IVEmployeeInfoRepository repo = new VEmployeeInfoRepository(Db);
                Probation = repo.GetList();
            }
            return Probation;
        }
        
        public IEnumerable<VEmployeeInfo> GetEmployeetoEvaluate() 
        {
            IEnumerable<VEmployeeInfo> Probation;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IVEmployeeInfoRepository repo = new VEmployeeInfoRepository(Db);
                Probation = repo.GetList2();
            }
            return Probation;
        }

        public VEmployeeInfo GetEmployeeByEmployeeIDSys(string id)
        {
            VEmployeeInfo Employee;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IVEmployeeInfoRepository repo = new VEmployeeInfoRepository(Db);
                
                //string[] include = { "Person_MT" };
                Employee = repo.Get(c => c.EmID == id);
            }   
            return Employee;
        }

        public bool UpdateEmployeeByID(VEmployeeInfo Employee)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (HRMSDbContext Db = new HRMSDbContext())
                    {
                        IVEmployeeInfoRepository repo = new VEmployeeInfoRepository(Db);
                        repo.Update(Employee);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return true;
            }
        }

        //public Employee_MT SetPositionConfig2(int id, PositionConfig positionConfig)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        Employee_MT Empnew = new Employee_MT();
        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IEmployeeRepository empRepo = new EmployeeRepository(Db);
        //                Employee_MT employee = new Employee_MT();
        //                employee = empRepo.GetByID(id); ;
        //                employee.EmpConfidentialConfigs.PositionConfig = positionConfig;
        //                Empnew = empRepo.Update(employee);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
                    
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw new ValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4012);
        //            throw ex;
        //        }
        //        return Empnew;
        //    }
        //}

        //public PersonDto GetPersonByPersonID(int id)
        //{
        //    PersonDto Person;
        //    using (CoreDbContext Db = new CoreDbContext())
        //    {
        //        IPersonRepository repo = new PersonRepository(Db);
        //        var data = repo.GetByID(id);
        //        Person = new PersonDto()
        //        {
        //            PersonIDSys = data.PersonIDSys,
        //            BirthDate = data.BirthDate,
        //            Email = data.Email,
        //            Name = data.Name,
        //            NameEn = data.NameEn,
        //            SurnameEn = data.SurnameEn,
        //            Surname = data.Surname,
        //            Religion = data.Religion,
        //            Nationality = data.Nationality,
        //            Gender = data.Gender,
        //            Mobile = data.Mobile,
        //            PrefixIDSys = data.PrefixIDSys,
        //            Address = data.Address,
        //            IdentificationNo = data.IdentificationNo,
        //            PassportNo = data.PassportNo,
        //            TaxNo = data.TaxNo
        //        };
        //    }
        //    return Person;
        //}

        //public int CreatePerson(Person_MT Person)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        Person_MT Personnew = new Person_MT();
        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IPersonRepository repo = new PersonRepository(Db);
        //                Personnew = repo.Insert(Person);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw new ValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4012);
        //            throw ex;
        //        }
        //        return Personnew.PersonIDSys;
        //    }
        //}

        //public bool UpdatePerson(Person_MT Person)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IPersonRepository repo = new PersonRepository(Db);
        //                repo.Update(Person);
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw new ValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4012);
        //            throw ex;
        //        }
        //        return true;
        //    }
        //}

        //public bool UpdatePersonByID(Person_MT Person)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IPersonRepository repo = new PersonRepository(Db);
        //                repo.Update(Person);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw new ValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4012);
        //            throw ex;
        //        }

        //        return true;
        //    }
        //}

        //public bool DeletePerson(int id)
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IPersonRepository repo = new PersonRepository(Db);
        //                Person_MT person = repo.GetByID(id);
        //                repo.Update(person);
        //                //#Oil Comment
        //                //Wait for Command Delete
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4017);
        //            throw ex;
        //        }
        //        return true;
        //    }
        //}
    }
}
