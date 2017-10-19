using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class CustomerRepository : IGenericRepository<Customer_MT>
    {
        private CoreDbContext Db { get; set; }

        public CustomerRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Customer_MT> Get()
        {
            var customers = from c in Db.Customer_MT
                            where c.Active == 1
                            select c;
            return customers.ToList();
        }


        public Customer_MT GetByID(object id)
        {
            var customer = (from c in Db.Customer_MT
                            where c.Active == 1 && c.CusIDSys == (int)id
                            select c).Include(b => b.Project_MT).SingleOrDefault();
            return customer;
        }

        public object GetByUserID(string userid)
        {
            var query = (from ctm in Db.Customer_MT
                         join c in Db.Project_MT on ctm.CusIDSys equals c.CusIDSys
                         join d in Db.Role on c.ProjectIDSys equals d.ProjectIDSys
                         join e in Db.UserRoles on d.RoleID equals e.RoleID
                         where e.UserID == userid
                         select new
                         {
                             ctm.CusID,
                             ctm.CusIDSys,
                             ctm.CusName
                         }).Distinct();
            return query.ToList();
        }

        public object GetProjectByUserIDCusID(string userid, int cusIDSys)
        {
            var query = from ctm in Db.Customer_MT
                        join pm in Db.Project_MT on ctm.CusIDSys equals pm.CusIDSys
                        join r in Db.Role on pm.ProjectIDSys equals r.ProjectIDSys
                        join ru in Db.UserRoles on r.RoleID equals ru.RoleID
                        where ru.UserID == userid && pm.CusIDSys == cusIDSys
                        select new
                        {
                            pm.ProjectID,
                            pm.ProjectIDSys,
                            pm.ProjectName,
                        };
            return query.ToList();
        }

        public void Insert(Customer_MT entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            Db.Customer_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedCustomer = (from c in Db.Customer_MT
                                   where c.CusIDSys== (int)id
                                   select c).SingleOrDefault();
            existedCustomer.Active = 0;
            Db.SaveChanges();
        }

        public void Delete(Customer_MT entityToDelete)
        {
            var existedCustomer = (from c in Db.Customer_MT
                                   where c.CusIDSys.Equals(entityToDelete.CusIDSys)
                                   select c).SingleOrDefault();
            existedCustomer.Active = 0;
            Db.SaveChanges();
        }

        public void Update(Customer_MT entityToUpdate)
        {
            var existedCustomer = (from c in Db.Customer_MT
                                   where c.CusIDSys.Equals(entityToUpdate.CusIDSys)
                                   select c).SingleOrDefault();
            existedCustomer.CusName = entityToUpdate.CusName;
            existedCustomer.TaxID = entityToUpdate.TaxID;
            existedCustomer.CompName = entityToUpdate.CompName;
            existedCustomer.AddressBill = entityToUpdate.AddressBill;
            existedCustomer.SubCity = entityToUpdate.SubCity;
            existedCustomer.City = entityToUpdate.City;
            existedCustomer.Province = entityToUpdate.Province;
            existedCustomer.Zipcode = entityToUpdate.Zipcode;
            existedCustomer.CountryCode = entityToUpdate.CountryCode;
            existedCustomer.Email = entityToUpdate.Email;
            existedCustomer.TelOffice = entityToUpdate.TelOffice;
            existedCustomer.TelExt = entityToUpdate.TelExt;
            existedCustomer.Mobile1 = entityToUpdate.Mobile1;
            existedCustomer.Mobile2 = entityToUpdate.Mobile2;
            existedCustomer.Mobile3 = entityToUpdate.Mobile3;
            existedCustomer.UpdateDate = DateTime.Now;
            existedCustomer.UserUpdate = "1";
            existedCustomer.Active = entityToUpdate.Active;
            Db.SaveChanges();
        }

        public IEnumerable<Customer_MT> GetMany(Func<Customer_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Customer_MT> GetManyQueryable(Func<Customer_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Customer_MT Get(Func<Customer_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Customer_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Customer_MT> GetWithInclude(Expression<Func<Customer_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Customer_MT GetSingle(Func<Customer_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Customer_MT GetFirst(Func<Customer_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
