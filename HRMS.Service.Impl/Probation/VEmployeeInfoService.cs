using HRMS.Context;
using HRMS.Entity.Probation;
using HRMS.Repository;
using HRMS.Repository.Impl;
using HRMS.Service.Probation;
using System.Collections.Generic;

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

        //public Person_MT GetPersonByPersonIDSys(string id)
        //{
        //    Person_MT Person;
        //    using (CoreDbContext Db = new CoreDbContext())
        //    {
        //        IPersonRepository repo = new PersonRepository(Db);
        //        CoreDbContext Db2 = new CoreDbContext();
        //        Person = repo.GetSingle(b => (Db2.User.Where(a => a.UserID == id).Select(d => d.PersonIDSys).Contains(b.PersonIDSys)));
        //    }
        //    return Person;
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
