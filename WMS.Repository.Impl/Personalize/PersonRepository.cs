using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WIM.Core.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Repository;

namespace WMS.Repository.Impl
{
    public class PersonRepository : IGenericRepository<Person_MT>
    {
        private CoreDbContext Db { get; set; }

        public PersonRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Person_MT> Get()
        {
            var person = from c in Db.Person_MT
                         select c;
            return person.ToList();
        }

        public Person_MT GetByID(object id)
        {
            var person = (from i in Db.Person_MT
                          where i.PersonIDSys== (int)id
                          select i).SingleOrDefault();
            return person;
        }

        public Person_MT GetByUserID(string id)
        {
            var person = (from i in Db.Person_MT
                          where (from o in Db.User
                                 where o.UserID == id
                                 select o.PersonIDSys)
                               .Contains(i.PersonIDSys)
                          select i).SingleOrDefault();
            return person;
        }

        public void Insert(Person_MT entity)
        {
            Db.Person_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Person_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Person_MT entityToUpdate)
        {
            var existedPerson = (from i in Db.Person_MT
                                 where i.PersonIDSys == entityToUpdate.PersonIDSys
                                 select i).SingleOrDefault();
            existedPerson.PersonIDSys = entityToUpdate.PersonIDSys;
            existedPerson.Name = entityToUpdate.Name;
            existedPerson.Gender = entityToUpdate.Gender;
            existedPerson.Age = entityToUpdate.Age;
            existedPerson.BirthDate = entityToUpdate.BirthDate;
            existedPerson.Religion = entityToUpdate.Religion;
            existedPerson.Nationality = entityToUpdate.Nationality;
            existedPerson.Surname = entityToUpdate.Surname;
            existedPerson.Email = entityToUpdate.Email;
            existedPerson.Mobile = entityToUpdate.Mobile;
            Db.Person_MT.Add(existedPerson);
            Db.SaveChanges();
        }

        public int Update(string id, Person_MT Person)
        {
            var existedPerson = (from i in Db.Person_MT
                                 where (from o in Db.User
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
            Db.Person_MT.Add(Person);
            return Person.PersonIDSys;
        }

        public IEnumerable<Person_MT> GetMany(Func<Person_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Person_MT> GetManyQueryable(Func<Person_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Person_MT Get(Func<Person_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Person_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Person_MT> GetWithInclude(Expression<Func<Person_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Person_MT GetSingle(Func<Person_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Person_MT GetFirst(Func<Person_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
