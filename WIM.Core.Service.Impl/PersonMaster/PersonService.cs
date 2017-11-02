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
using WIM.Core.Entity.Person;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository;
using System.Security.Principal;

namespace WIM.Core.Service.Impl
{ 
    public class PersonService : IPersonService
    {
        private IIdentity user { get; set; }
        public PersonService(IIdentity identity)
        {
            user = identity;
        }        

        public IEnumerable<Person_MT> GetPersons()
        {
            IEnumerable<Person_MT> Person;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPersonRepository repo = new PersonRepository(Db);
                Person = repo.Get();
            }
            return Person;
        }

        public Person_MT GetPersonByPersonIDSys(string id)
        {
            Person_MT Person;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPersonRepository repo = new PersonRepository(Db);
                Person = repo.GetSingle(b => (Db.User.Where(a => a.UserID == id).Select(d => d.PersonIDSys).Contains(b.PersonIDSys)));
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
                    PersonID = data.PersonID,
                    Age = data.Age,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Name = data.Name,
                    Surname = data.Surname,
                    Religion = data.Religion,
                    Nationality = data.Nationality,
                    Gender = data.Gender,
                    Mobile = data.Mobile
                };
            }
            return Person;
        }

        public int CreatePerson(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {      
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPersonRepository repo = new PersonRepository(Db);
                        repo.Insert(Person);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                return Person.PersonIDSys;
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
