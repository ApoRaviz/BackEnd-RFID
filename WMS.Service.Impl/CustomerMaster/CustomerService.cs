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
//using System.Web.Mvc;
//using Newtonsoft.Json;
using WMS.Common;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Context;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class CustomerService : ICustomerService
    {
        private CustomerRepository Repo;

        public CustomerService()
        {
            Repo = new CustomerRepository();
        }

        public object GetCustomerAll()
        {
            var query = Repo.Get();
            return query.ToList();
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
            var customer = Repo.GetByID(id);
            if (customer != null)
            {
                CustomerDto customerDto = Mapper.Map<Customer_MT, CustomerDto>(customer);

                return customerDto;
            }
            return null;
        }

        public object GetCustomers(string userid)
        {
            var query = Repo.GetByUserID(userid);
            return query;
        }

        public object GetProjectByCustomer(string userid, int cusIDSys)
        {
            var query = Repo.GetProjectByUserIDCusID(userid, cusIDSys);
            return query;
        }

        public Customer_MT GetCustomerByCusIDSys(int id)
        {
            return Repo.GetByID(id);
        }

        public CustomerDto GetCustomersInclude(int id, string[] tableNames)
        {
            var customer = Repo.GetByID(id);
            if (customer != null)
            {
                var query = Repo.GetByID(id);
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

                customer.CreatedDate = DateTime.Now;
                customer.UpdateDate = DateTime.Now;
                customer.UserUpdate = "1";
                try
                {
                    Repo.Insert(customer);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }

                return customer.CusIDSys;
            }
        }

        public bool UpdateCustomer(int id, Customer_MT customer)
        {
            using (var scope = new TransactionScope())
            {
                var existedCustomer = Repo.GetByID(id);
                existedCustomer.CusName = customer.CusName;
                existedCustomer.TaxID = customer.TaxID;
                existedCustomer.CompName = customer.CompName;
                existedCustomer.AddressBill = customer.AddressBill;
                existedCustomer.SubCity = customer.SubCity;
                existedCustomer.City = customer.City;
                existedCustomer.Province = customer.Province;
                existedCustomer.Zipcode = customer.Zipcode;
                existedCustomer.CountryCode = customer.CountryCode;
                existedCustomer.Email = customer.Email;
                existedCustomer.TelOffice = customer.TelOffice;
                existedCustomer.TelExt = customer.TelExt;
                existedCustomer.Mobile1 = customer.Mobile1;
                existedCustomer.Mobile2 = customer.Mobile2;
                existedCustomer.Mobile3 = customer.Mobile3;
                existedCustomer.UpdateDate = DateTime.Now;
                existedCustomer.UserUpdate = "1";
                
                try
                {
                    Repo.Update(existedCustomer);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                return true;
            }
        }

        public bool DeleteCustomer(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedCustomer = Repo.GetByID(id);
                existedCustomer.Active = 0;
                existedCustomer.UpdateDate = DateTime.Now;
                existedCustomer.UserUpdate = "1";
                Repo.Update(existedCustomer);
                scope.Complete();
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
