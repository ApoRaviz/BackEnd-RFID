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
using WMS.Master;

namespace WMS.Service
{
    public class CustomerService : ICustomerService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Customer_MT> Repo;

        public CustomerService()
        {
            Repo = new GenericRepository<Customer_MT>(db);
        }

        public object GetCustomerAll()
        {
            var query = from ctm in db.Customer_MT
                        where ctm.Active == 1
                        select new
                        {
                            ctm.CusID,
                            ctm.CusIDSys,
                            ctm.CusName
                        };

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
            var customer = GetCustomerByCusIDSys(id);
            if (customer != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ProcGetCustomerByCusIDSys_Result, CustomerDto>());
                CustomerDto customerDto = Mapper.Map<ProcGetCustomerByCusIDSys_Result, CustomerDto>(customer);

                var projects = db.ProcGetProjectsByCusIDSys(customerDto.CusIDSys).ToList();
                Mapper.Initialize(cfg => cfg.CreateMap<ProcGetProjectsByCusIDSys_Result, ProjectDto>());
                ICollection<ProjectDto> projectsDto = Mapper.Map<ICollection<ProcGetProjectsByCusIDSys_Result>, ICollection<ProjectDto>>(projects);
                customerDto.Project_MT = projectsDto;

                return customerDto;
            }
            return null;
        }

        public object GetCustomers(string userid)
        {
            var query = (from ctm in db.Customer_MT
                         join c in db.Project_MT on ctm.CusIDSys equals c.CusIDSys
                         join d in db.Roles on c.ProjectIDSys equals d.ProjectIDSys
                         join e in db.UserRoles on d.RoleID equals e.RoleID
                         where e.UserID == userid
                         select new
                         {
                             ctm.CusID,
                             ctm.CusIDSys,
                             ctm.CusName
                         }).Distinct();

            return query.ToList();
        }

        public object GetProjectByCustomer(string userid, int cusIDSys)
        {
            var query = from ctm in db.Customer_MT
                        join pm in db.Project_MT on ctm.CusIDSys equals pm.CusIDSys
                        join r in db.Roles on pm.ProjectIDSys equals r.ProjectIDSys
                        join ru in db.UserRoles on r.RoleID equals ru.RoleID
                        where ru.UserID == userid && pm.CusIDSys == cusIDSys
                        select new
                        {
                            pm.ProjectID,
                            pm.ProjectIDSys,
                            pm.ProjectName,
                        };
            return query.ToList();
        }

        public ProcGetCustomerByCusIDSys_Result GetCustomerByCusIDSys(int id)
        {
            return db.ProcGetCustomerByCusIDSys(id).FirstOrDefault();
        }

        public CustomerDto GetCustomersInclude(int id, string[] tableNames)
        {
            var customer = from cus in db.Customer_MT
                           where cus.CusIDSys == id && cus.Active == 1
                           select cus;
            if (customer != null)
            {
                var query = (from i in db.Customer_MT
                             where i.CusIDSys == id && i.Active == 1
                             select i);
                if (tableNames != null)
                {
                    foreach (string tableName in tableNames)
                    {
                        switch (tableName)
                        {
                            case "ProjectMT":
                                query = query.Include(pj => pj.Project_MT);
                                break;
                            default:
                                query = query.Include(tableName);
                                break;
                        }
                    }
                }
                CustomerDto customerDto = Mapper.Map<Customer_MT, CustomerDto>(query.SingleOrDefault());
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

                Repo.Insert(customer);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
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
                Repo.Update(existedCustomer);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
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
                db.SaveChanges();
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
