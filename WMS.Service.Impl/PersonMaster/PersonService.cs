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
using WIM.Core.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Security.Context;
using WMS.Repository.Impl;

namespace WMS.Service
{ 
    public class PersonService : IPersonService
    {
        private PersonRepository repo;

        public PersonService()
        {
            repo = new PersonRepository();
        }        

        public IEnumerable<Person_MT> GetPersons()
        {           
            return repo.Get();
        }

        public Person_MT GetPersonByPersonIDSys(string id)
        {
            Person_MT Person = repo.GetByUserID(id);
            return Person;            
        }

        public PersonDto GetPersonByPersonID(int id)
        {
            var data = repo.GetByID(id);
            PersonDto Person = new PersonDto()
            {
                PersonIDSys = data.PersonIDSys,
                PersonID = data.PersonID,
                Age = data.Age,
                BirthDate = data.BirthDate,
                CreateDate = data.CreateDate,
                Email = data.Email,
                Name = data.Name,
                Surname = data.Surname,
                Religion = data.Religion,
                Nationality = data.Nationality,
                Gender = data.Gender,
                UpdateDate = data.UpdateDate,
                UserUpdate = data.UserUpdate,
                Mobile = data.Mobile
                
            };
            return Person;
        }

        public int CreatePerson(Person_MT Person)
        {
            using (var scope = new TransactionScope())
            {      
                try
                {
                    repo.Insert(Person);
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
                return Person.PersonIDSys;
            }
        }

        public bool UpdatePerson(string id, Person_MT Person)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(id, Person);
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
                try
                {
                    repo.Update(Person);
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
                              
                try
                {
                //#Oil Comment
                //Wait for Command Delete
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
