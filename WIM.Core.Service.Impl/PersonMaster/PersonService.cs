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
using WIM.Core.Entity.Person;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity.View;

namespace WIM.Core.Service.Impl
{ 
    public class PersonService : Service, IPersonService
    {

        public PersonService()
        {

        }        

        public IEnumerable<VPersons> GetPersons()
        {
            IEnumerable<VPersons> Person;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPersonRepository repo = new PersonRepository(Db);
                Person = repo.GetList();
            }
            return Person;
        }

        public Person_MT GetPersonByPersonIDSys(string id)
        {
            Person_MT Person;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPersonRepository repo = new PersonRepository(Db);
                CoreDbContext Db2 = new CoreDbContext();
                Person = repo.GetSingle(b => (Db2.User.Where(a => a.UserID == id).Select(d => d.PersonIDSys).Contains(b.PersonIDSys)));
            }
            return Person;            
        }

        public PersonDto GetPersonByPersonID(int id)
        {
            PersonDto Person;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPersonRepository repo = new PersonRepository(Db);
                var data = repo.GetByID(id);
                Person = new PersonDto()
                {
                    PersonIDSys = data.PersonIDSys,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Name = data.Name,
                    NameEn = data.NameEn,
                    SurnameEn = data.SurnameEn,
                    Surname = data.Surname,
                    Religion = data.Religion,
                    Nationality = data.Nationality,
                    Gender = data.Gender,
                    Mobile = data.Mobile,
                    PrefixIDSys = data.PrefixIDSys,
                    Address = data.Address,
                    IdentificationNo = data.IdentificationNo,
                    PassportNo = data.PassportNo,
                    TaxNo = data.TaxNo
                };
            }
            return Person;
        }

        public int CreatePerson(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {
                Person_MT Personnew = new Person_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPersonRepository repo = new PersonRepository(Db);
                        Personnew = repo.Insert(Person);
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
                return Personnew.PersonIDSys;
            }
        }

        public bool UpdatePerson(Person_MT Person)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPersonRepository repo = new PersonRepository(Db);
                        repo.Update(Person);
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

        public bool UpdatePersonByID(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPersonRepository repo = new PersonRepository(Db);
                        repo.Update(Person);
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

        public bool DeletePerson(int id)
        {
            using (var scope = new TransactionScope())
            {
                              
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPersonRepository repo = new PersonRepository(Db);
                        Person_MT person = repo.GetByID(id);
                        repo.Update(person);
                        //#Oil Comment
                        //Wait for Command Delete
                        Db.SaveChanges();
                        scope.Complete();
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
