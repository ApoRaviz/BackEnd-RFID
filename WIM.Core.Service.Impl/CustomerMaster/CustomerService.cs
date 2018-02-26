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
//using System.Web.Mvc;
//using Newtonsoft.Json;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.ValueObject;
using System.Security.Principal;
using WIM.Core.Common;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Service.Impl
{
    public class CustomerService : Service, ICustomerService
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
            IEnumerable<Project_MT> project;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICustomerRepository repo = new CustomerRepository(Db);
                IProjectRepository repo2 = new ProjectRepository(Db);
                string[] include = {"Project_MT" };
                customer = repo.GetWithInclude((c => c.IsActive == true && c.CusIDSys == id),include).SingleOrDefault();
                project = repo2.GetMany(c => c.CusIDSys == id).ToList();
            }
            if (customer != null)
            {
                CustomerDto customerDto = new CommonService().AutoMapper<CustomerDto>(customer);
                if(project != null)
                    customerDto.Project_MT = project.Select(a => new CommonService().AutoMapper<ProjectDto>(a)).ToList();
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
                IProjectRepository repo2 = new ProjectRepository(Db);
                customer = repo.Get(c => c.CusIDSys == id && c.IsActive == true);
                if (customer != null)
                {
                    string[] include = { "Project_MT" };
                    query = repo.Get((c => c.IsActive == true && c.CusIDSys == id));
                    var project = repo2.GetMany(c => c.CusIDSys == id).ToList();
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
                    CustomerDto customerDto = new CommonService().AutoMapper<CustomerDto>(query)/* Mapper.Map<Customer_MT, CustomerDto>(query)*/;
                    if(project != null)
                    customerDto.Project_MT =  project.Select(a => new CommonService().AutoMapper<ProjectDto>(a)).ToList();
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

        public int CreateCustomer(Customer_MT customer)
        {
            using (var scope = new TransactionScope())
            {
                Customer_MT customernew = new Customer_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICustomerRepository repo = new CustomerRepository(Db);
                        customernew = repo.Insert(customer);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }

                return customernew.CusIDSys;
            }
        }

        public bool UpdateCustomer(Customer_MT customer)
        {
            using (var scope = new TransactionScope())
            {
                using (CoreDbContext Db = new CoreDbContext())
                {
                    ICustomerRepository repo = new CustomerRepository(Db);
                    
                    try
                    {
                        repo.Update(customer);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
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
        

        public IEnumerable<AutocompleteCustomerDto> AutocompleteCustomer(string term)
        {
            IEnumerable<AutocompleteCustomerDto> autocompleteItemDto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                CustomerRepository repo = new CustomerRepository(Db);
                autocompleteItemDto = repo.AutocompleteItem(term);

            }
            return autocompleteItemDto;
        }
    }
}
