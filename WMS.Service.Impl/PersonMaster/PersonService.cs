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
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{ 
    public class PersonService : IPersonService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Person_MT> repo;

        public PersonService()
        {
            repo = new GenericRepository<Person_MT>(db);
        }        

        public IEnumerable<Person_MT> GetPersons()
        {           
            return repo.GetAll();
        }

        public Person_MT GetPersonByPersonIDSys(string id)
        {
            Person_MT Person = (from i in db.Person_MT
                                where (from o in db.Users
                                       where o.UserID == id
                                       select o.PersonIDSys)
                                     .Contains(i.PersonIDSys)
                                select i).SingleOrDefault();
            return Person;            
        }

        public PersonDto GetPersonByPersonID(int id)
        {
            var Person = (from i in db.Person_MT
                                where i.PersonIDSys == id
                                select i).Select(b => new PersonDto(){
                                Name = b.Name,
                                PersonIDSys = b.PersonIDSys,
                                Surname = b.Surname,Religion = b.Religion,
                                Age = b.Age,BirthDate = b.BirthDate,Gender = b.Gender,
                                Email = b.Email,CreateDate = b.CreateDate,Mobile = b.Mobile,Nationality = b.Nationality
                                }).SingleOrDefault();
            return Person;
        }

        public int CreatePerson(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {
                
                Person.CreateDate = DateTime.Now;
                Person.UpdateDate = DateTime.Now;
                Person.UserUpdate = "1";
                
                repo.Insert(Person);
                try
                {
                    db.SaveChanges();
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
                scope.Complete();
                return Person.PersonIDSys;
            }
        }

        public bool UpdatePerson(string id, Person_MT Person)
        {           
            using (var scope = new TransactionScope())
            {
                var existedPerson = (from i in db.Person_MT
                                     where (from o in db.Users
                                            where o.UserID == id
                                            select o.PersonIDSys)
                                          .Contains(i.PersonIDSys)
                                     select i).SingleOrDefault();
                existedPerson.PersonIDSys = Person.PersonIDSys;
                existedPerson.Name = Person.Name;
                existedPerson.Gender = Person.Gender;
                existedPerson.Age = Person.Age;
                existedPerson.BirthDate = Person.BirthDate;
                existedPerson.Religion = Person.Religion;
                existedPerson.Nationality = Person.Nationality;
                existedPerson.Surname = Person.Surname;
                existedPerson.Email = Person.Email;
                existedPerson.Mobile = Person.Mobile;
                existedPerson.UpdateDate = DateTime.Now;
                existedPerson.UserUpdate = "1";
                repo.Update(existedPerson);
                try
                {
                    db.SaveChanges();
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

        public bool UpdatePersonByID(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {
                var existedPerson = (from i in db.Person_MT
                                     where i.PersonIDSys == Person.PersonIDSys
                                     select i).SingleOrDefault();
                existedPerson.PersonIDSys = Person.PersonIDSys;
                existedPerson.Name = Person.Name;
                existedPerson.Gender = Person.Gender;
                existedPerson.Age = Person.Age;
                existedPerson.BirthDate = Person.BirthDate;
                existedPerson.Religion = Person.Religion;
                existedPerson.Nationality = Person.Nationality;
                existedPerson.Surname = Person.Surname;
                existedPerson.Email = Person.Email;
                existedPerson.Mobile = Person.Mobile;
                existedPerson.UpdateDate = DateTime.Now;
                existedPerson.UserUpdate = "1";
                repo.Update(existedPerson);
                try
                {
                    db.SaveChanges();
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

        public bool DeletePerson(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedPerson = repo.GetByID(id);
                existedPerson.UpdateDate = DateTime.Now;
                existedPerson.UserUpdate = "1";
                repo.Update(existedPerson);
                try
                {
                db.SaveChanges();
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
