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
//using System.Web.Mvc;
//using Newtonsoft.Json;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.ValueObject;

namespace WIM.Core.Service.Impl
{
    public class CustomerService : ICustomerService
    {
        public CustomerService()
        {
        }

        public object GetCustomerAll()
        {
            object query;
            using(CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                query = repo.GetMany((x => x.IsActive == true));
            }
          
            return query;
        }

        //public ICollection<CustomerDto> GetCustomers(string userid)
        //{

        //    var query = (from ctm in db.Customer_MT
        //                 join c in db.UserCustomerMapping on ctm.CusIDSys equals c.CusIDSys 
        //                 where c.UserID ==  userid
        //                 select ctm 
        //                  );
        //    object y = query.ToList();
        //    string x = JsonConvert.SerializeObject(y); // query.ToDictionary<string, object>;
        //    ICollection<CustomerDto> userDto = Mapper.Map<ICollection<Customer_MT>, ICollection<CustomerDto>>(query.ToList());
        //    //JsonResult userDto = query.ToList();
        //    return userDto;
        //}

        public CustomerDto GetCustomerByCusIDSysIncludeProjects(int id)
        {
            Customer_MT customer;
            using(CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                string[] include = {"Project_MT" };
                customer = repo.GetWithInclude((c => c.IsActive == true && c.CusIDSys == id),include).SingleOrDefault();
            }
            if (customer != null)
            {
                CustomerDto customerDto = Mapper.Map<Customer_MT, CustomerDto>(customer);

                return customerDto;
            }
            return null;
        }

        public object GetCustomers(string userid)
        {
            object query;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                query = repo.GetByUserID(userid);
            }
            return query;
        }

        public object GetProjectByCustomer(string userid, int cusIDSys)
        {
            object query;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                query = repo.GetProjectByUserIDCusID(userid, cusIDSys);
            }
            return query;
        }

        public Customer_MT GetCustomerByCusIDSys(int id)
        {
            Customer_MT customer;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                string[] include = { "Project_MT" };
                customer = repo.GetWithInclude((c => c.IsActive == true && c.CusIDSys == id), include).SingleOrDefault();
            }
            return customer;
        }

        public CustomerDto GetCustomersInclude(int id, string[] tableNames)
        {
            Customer_MT customer , query;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                customer = repo.Get(c => c.CusIDSys == id && c.IsActive == true);
                if (customer != null)
                {
                    string[] include = { "Project_MT" };
                    query = repo.GetWithInclude((c => c.IsActive == true && c.CusIDSys == id), include).SingleOrDefault();
                    if (tableNames != null)
                    {
                        foreach (string tableName in tableNames)
                        {
                            switch (tableName)
                            {
                                case "ProjectMT":
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    CustomerDto customerDto = Mapper.Map<Customer_MT, CustomerDto>(query);
                    return customerDto;
                }
            }
            return null;
        }



        //public CustomerDto GetCustomerByCusIDSysIncludeProjects(int id)
        //{
        //    var customer = GetCustomerByCusIDSys(id);           
        //    if (customer != null)
        //    {
        //        Mapper.Initialize(cfg => cfg.CreateMap<ProcGetCustomerByCusIDSys_Result, CustomerDto>());
        //        CustomerDto customerDto = Mapper.Map<ProcGetCustomerByCusIDSys_Result, CustomerDto>(customer);

        //        var projects = db.ProcGetProjectsByCusIDSys(customerDto.CusIDSys).ToList();                
        //        Mapper.Initialize(cfg => cfg.CreateMap<ProcGetProjectsByCusIDSys_Result, ProjectDto>());
        //        IEnumerable<ProjectDto> projectsDto = Mapper.Map<IEnumerable<ProcGetProjectsByCusIDSys_Result>, IEnumerable<ProjectDto>>(projects);
        //        customerDto.Projects = projectsDto;

        //        return customerDto;
        //    }
        //    return null;
        //}

        public int CreateCustomer(Customer_MT customer , string username)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICustomerRepository repo = new CustomerRepository(Db);
                        repo.Insert(customer,username);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }

                return customer.CusIDSys;
            }
        }

        public bool UpdateCustomer(Customer_MT customer,string username)
        {
            using (var scope = new TransactionScope())
            {
                using (CoreDbContext Db = new CoreDbContext())
                {
                    ICustomerRepository repo = new CustomerRepository(Db);
                    
                    try
                    {
                        repo.Update(customer, username);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                }
                return true;
            }
        }

        public bool DeleteCustomer(int id)
        {
            using (var scope = new TransactionScope())
            {
                using (CoreDbContext Db = new CoreDbContext())
                {
                    ICustomerRepository repo = new CustomerRepository(Db);
                    var existedCustomer = repo.GetByID(id);
                    repo.Delete(existedCustomer);
                    // repo.Update(existedCustomer);
                    Db.SaveChanges();
                    scope.Complete();
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
